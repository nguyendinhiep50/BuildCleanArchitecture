using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text.Json;

namespace BuildCleanArchitecture.Infrastructure.Extensions
{
    public static class FluentEFStoredProcedureExtensions
    {
        /// <summary>
        /// Creates an initial DbCommand object based on a stored procedure name
        /// </summary>
        /// <param name="context">target database context</param>
        /// <param name="storedProcName">target procedure name</param>
        /// <param name="prependDefaultSchema">Prepend the default schema name to <paramref name="storedProcName"/> if explicitly defined in <paramref name="context"/></param>
        /// <param name="commandTimeout">Command timeout in seconds. Default is 30.</param>
        /// <returns></returns>
        public static DbCommand LoadStoredProc(this DbContext context, string storedProcName, bool prependDefaultSchema = true, short commandTimeout = 30)
        {

            var cmd = context.Database.GetDbConnection().CreateCommand();

            cmd.CommandTimeout = commandTimeout;

            if (prependDefaultSchema)
            {
                var schemaName = context.Model.GetDefaultSchema();

                if (schemaName != null)
                {
                    storedProcName = $"{schemaName}.{storedProcName}";
                }
            }

            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return cmd;
        }

        /// <summary>
        /// Creates a DbParameter object and adds it to a DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue, Action<DbParameter> configureParam = null)
        {
            if (string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandType != System.Data.CommandType.StoredProcedure)
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = (paramValue != null ? paramValue : DBNull.Value);
            configureParam?.Invoke(param);
            cmd.Parameters.Add(param);
            return cmd;
        }

        /// <summary>
        /// Creates a DbParameter object and adds it to a DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, Action<DbParameter> configureParam = null)
        {
            if (string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandType != System.Data.CommandType.StoredProcedure)
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            configureParam?.Invoke(param);
            cmd.Parameters.Add(param);
            return cmd;
        }

        /// <summary>
        /// Creates a DbParameter object based on the SqlParameter and adds it to a DbCommand.
        /// This enabled the ability to provide custom types for SQL-parameters.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, SqlParameter parameter)
        {
            if (string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandType != System.Data.CommandType.StoredProcedure)
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            cmd.Parameters.Add(parameter);

            return cmd;
        }

        public class SprocResults
        {

            private DbDataReader _reader;

            public SprocResults(DbDataReader reader)
            {
                _reader = reader;
            }

            public IList<T> ReadToList<T>()
            {
                return MapToList<T>(_reader);
            }

            public IList<T> ReadIncludeColumnPropToList<T>()
            {
                return MapToListIncludeColumnProps<T>(_reader);
            }

            public IList<object> ReadIncludeColumnPropToList(Type classType)
            {
                return MapToListIncludeColumnProps(_reader, classType);
            }

            public IList<object> ReadIncludeColumnPropToListAsObject(Type classType)
            {
                return MapToListIncludeColumnPropsAsObject(_reader, classType);
            }

            public IList<object> ToListDataDictionaryLower()
            {
                return ToListDataDictionaryLower(_reader);
            }

            public T? ReadToValue<T>() where T : struct
            {
                return MapToValue<T>(_reader);
            }

            public Task<bool> NextResultAsync()
            {
                return _reader.NextResultAsync();
            }

            public Task<bool> NextResultAsync(CancellationToken ct)
            {
                return _reader.NextResultAsync(ct);
            }

            public bool NextResult()
            {
                return _reader.NextResult();
            }

            /// <summary>
            /// Retrieves the column values from the stored procedure and maps them to <typeparamref name="T"/>'s properties
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="dr"></param>
            /// <returns>IList<<typeparamref name="T"/>></returns>
            private IList<T> MapToList<T>(DbDataReader dr)
            {
                var objList = new List<T>();
                var props = typeof(T).GetRuntimeProperties().ToList();

                var colMapping = dr.GetColumnSchema()
                    .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                    .ToDictionary(key => key.ColumnName.ToLower());

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        T obj = Activator.CreateInstance<T>();
                        foreach (var prop in props)
                        {
                            if (colMapping.ContainsKey(prop.Name.ToLower()))
                            {
                                var column = colMapping[prop.Name.ToLower()];

                                if (column?.ColumnOrdinal != null)
                                {
                                    var val = dr.GetValue(column.ColumnOrdinal.Value);

                                    if (prop.PropertyType == typeof(string))
                                    {
                                        val = val?.ToString()?.Trim() ?? string.Empty;
                                    }

                                    prop.SetValue(obj, val == DBNull.Value ? null : val);
                                }

                            }
                        }
                        objList.Add(obj);
                    }
                }
                return objList;
            }

            /// <summary>
            /// Retrieves the column values from the stored procedure and maps them to <typeparamref name="T"/>'s properties
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="dr"></param>
            /// <returns>IList<<typeparamref name="T"/>></returns>
            private IList<T> MapToListIncludeColumnProps<T>(DbDataReader dr)
            {
                var objList = new List<T>();
                var props = typeof(T).GetRuntimeProperties().ToList();

                Dictionary<PropertyInfo, DbColumn> colMappingDic = new Dictionary<PropertyInfo, DbColumn>();
                var columnSchemas = dr.GetColumnSchema();
                foreach (var property in props)
                {
                    var columnPropNames = GetColumnPropFieldStringNames(property).Select(x => x.ToLower()).ToList();
                    var key = columnPropNames.FirstOrDefault()?.ToLower() ?? property.Name.ToLower();

                    var matchingColumnSchema = columnSchemas.FirstOrDefault(x => x.ColumnName.ToLower() == key);
                    if (matchingColumnSchema != null)
                    {
                        colMappingDic.Add(property, matchingColumnSchema);
                    }
                }

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        T obj = Activator.CreateInstance<T>();

                        foreach (var colMapping in colMappingDic)
                        {
                            var prop = colMapping.Key;
                            var column = colMapping.Value;

                            if (column?.ColumnOrdinal != null)
                            {
                                var val = dr.GetValue(column.ColumnOrdinal.Value);

                                if (prop.PropertyType == typeof(string))
                                {
                                    val = val?.ToString()?.Trim() ?? string.Empty;
                                }

                                prop.SetValue(obj, val == DBNull.Value ? null : val);
                            }
                        }
                        objList.Add(obj);
                    }
                }
                return objList;
            }

            /// <summary>
            /// Retrieves the column values from the stored procedure and maps them to <typeparamref name="T"/>'s properties
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="dr"></param>
            /// <returns>IList<<typeparamref name="T"/>></returns>
            private IList<object> MapToListIncludeColumnProps(DbDataReader dr, Type classType)
            {
                var properties = classType.GetRuntimeProperties().ToList();

                var objList = new List<object>();

                Dictionary<PropertyInfo, DbColumn> colMappingDic = new Dictionary<PropertyInfo, DbColumn>();

                var columnSchemas = dr.GetColumnSchema();

                foreach (var property in properties)
                {
                    var columnPropNames = GetColumnPropFieldStringNames(property).Select(x => x.ToLower()).ToList();
                    var key = columnPropNames.FirstOrDefault()?.ToLower() ?? property.Name.ToLower();

                    var matchingColumnSchema = columnSchemas.FirstOrDefault(x => x.ColumnName.ToLower() == key);
                    if (matchingColumnSchema != null)
                    {
                        colMappingDic.Add(property, matchingColumnSchema);
                    }
                }

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var obj = Activator.CreateInstance(classType);

                        foreach (var colMapping in colMappingDic)
                        {
                            var prop = colMapping.Key;
                            var column = colMapping.Value;

                            if (column?.ColumnOrdinal != null)
                            {
                                var val = dr.GetValue(column.ColumnOrdinal.Value);

                                if (prop.PropertyType == typeof(string))
                                {
                                    val = val?.ToString()?.Trim() ?? string.Empty;
                                }

                                prop.SetValue(obj, val == DBNull.Value ? null : val);
                            }
                        }
                        objList.Add(obj!);
                    }
                }
                return objList;
            }

            private IList<object> MapToListIncludeColumnPropsAsObject(DbDataReader dr, Type classType)
            {
                var properties = classType.GetRuntimeProperties().ToList();

                var objList = new List<object>();

                Dictionary<PropertyInfo, DbColumn> colMappingDic = new Dictionary<PropertyInfo, DbColumn>();

                var columnSchemas = dr.GetColumnSchema();

                foreach (var property in properties)
                {
                    var columnPropNames = GetColumnPropFieldStringNames(property).Select(x => x.ToLower()).ToList();
                    var key = columnPropNames.FirstOrDefault()?.ToLower() ?? property.Name.ToLower();

                    var matchingColumnSchema = columnSchemas.FirstOrDefault(x => x.ColumnName.ToLower() == key);
                    if (matchingColumnSchema != null)
                    {
                        colMappingDic.Add(property, matchingColumnSchema);
                    }
                }

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var obj = new Dictionary<string, object>();

                        foreach (var colMapping in colMappingDic)
                        {
                            var prop = colMapping.Key;
                            var column = colMapping.Value;

                            if (column?.ColumnOrdinal != null)
                            {
                                var val = dr.GetValue(column.ColumnOrdinal.Value);

                                if (val is not null)
                                {
                                    if (prop.PropertyType == typeof(string))
                                    {
                                        val = val?.ToString()?.Trim() ?? string.Empty;
                                    }

                                    obj.Add(ToCamelCase(prop.Name), val);
                                }
                            }
                        }
                        objList.Add(obj!);
                    }
                }
                return objList;
            }

            private string ToCamelCase(string input)
            {
                if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
                    return input;

                char[] chars = input.ToCharArray();
                for (int i = 0; i < input.Length; i++)
                {
                    if (!char.IsUpper(input[i]))
                    {
                        break;
                    }

                    chars[i] = char.ToLowerInvariant(chars[i]);
                }

                return new string(chars);
            }

            private static List<object> ToListDataDictionaryLower(DbDataReader dr)
            {
                var objList = new List<object>();

                if (dr.HasRows)
                {
                    var columnSchemas = dr.GetColumnSchema();

                    while (dr.Read())
                    {
                        var obj = new Dictionary<string, object>();

                        foreach (var columnSchema in columnSchemas)
                        {
                            TypeCode typeCode = Type.GetTypeCode(columnSchema.DataType);

                            var columnName = columnSchema.ColumnName.ToLower();

                            if (typeCode == TypeCode.String)
                            {
                                var value = NullIfEmptyValues(dr.GetValue(columnSchema!.ColumnOrdinal!.Value)?.ToString()?.Trim() ?? string.Empty);
                                if (value is not null)
                                {
                                    obj.Add(columnName, value);
                                }
                            }
                            else
                            {
                                var value = NullIfEmptyValues(dr.GetValue(columnSchema!.ColumnOrdinal!.Value));

                                if (value is not null)
                                {
                                    obj.Add(columnName, dr.GetValue(columnSchema!.ColumnOrdinal!.Value));
                                }
                            }
                        }
                        objList.Add(obj!);
                    }
                }

                return objList;
            }

            private static object? NullIfEmptyValues(object obj)
            {
                return JsonSerializer.Serialize(obj) is "\"\"" or "[]" or "null" or "{}" ? null : obj;
            }

            private IEnumerable<string> GetColumnPropFieldStringNames(PropertyInfo propertyInfo)
            {
                return propertyInfo.CustomAttributes.SelectMany(x => x.ConstructorArguments).Select(x =>
                {
                    try
                    {
                        return (((string)x.Value!).ToString());
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }).Where(x => !string.IsNullOrEmpty(x));
            }

            /// <summary>
            ///Attempts to read the first value of the first row of the resultset.
            /// </summary>
            private T? MapToValue<T>(DbDataReader dr) where T : struct
            {
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        return dr.IsDBNull(0) ? new T?() : new T?(dr.GetFieldValue<T>(0));
                    }
                }
                return new T?();
            }
        }

        /// <summary>
        /// Executes a DbDataReader and returns a list of mapped column values to the properties of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public static void ExecuteStoredProc(this DbCommand command, Action<SprocResults> handleResults, System.Data.CommandBehavior commandBehaviour = System.Data.CommandBehavior.Default, bool manageConnection = true)
        {
            if (handleResults == null)
            {
                throw new ArgumentNullException(nameof(handleResults));
            }

            using (command)
            {
                if (manageConnection && command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader(commandBehaviour))
                    {
                        var sprocResults = new SprocResults(reader);
                        handleResults(sprocResults);
                    }
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Executes a DbDataReader asynchronously and returns a list of mapped column values to the properties of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public async static Task ExecuteStoredProcAsync(this DbCommand command, Func<SprocResults, Task> handleResults, System.Data.CommandBehavior commandBehaviour = System.Data.CommandBehavior.Default, CancellationToken ct = default(CancellationToken), bool manageConnection = true)
        {
            if (handleResults == null)
            {
                throw new ArgumentNullException(nameof(handleResults));
            }

            using (command)
            {
                if (manageConnection && command.Connection.State == System.Data.ConnectionState.Closed)
                    await command.Connection.OpenAsync(ct).ConfigureAwait(false);
                try
                {
                    using (var reader = await command.ExecuteReaderAsync(commandBehaviour, ct).ConfigureAwait(false))
                    {
                        var sprocResults = new SprocResults(reader);
                        await handleResults(sprocResults);
                    }
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Executes a non-query.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandBehaviour"></param>
        /// <param name="manageConnection"></param>
        /// <returns></returns>
        public static int ExecuteStoredNonQuery(this DbCommand command, System.Data.CommandBehavior commandBehaviour = System.Data.CommandBehavior.Default, bool manageConnection = true)
        {
            int numberOfRecordsAffected = -1;

            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                try
                {
                    numberOfRecordsAffected = command.ExecuteNonQuery();
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }

            return numberOfRecordsAffected;
        }

        /// <summary>
        /// Executes a non-query asynchronously.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandBehaviour"></param>
        /// <param name="ct"></param>
        /// <param name="manageConnection"></param>
        /// <returns></returns>
        public async static Task<int> ExecuteStoredNonQueryAsync(this DbCommand command, System.Data.CommandBehavior commandBehaviour = System.Data.CommandBehavior.Default, CancellationToken ct = default(CancellationToken), bool manageConnection = true)
        {
            int numberOfRecordsAffected = -1;

            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                {
                    await command.Connection.OpenAsync(ct).ConfigureAwait(false);
                }

                try
                {
                    numberOfRecordsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }

            return numberOfRecordsAffected;
        }
    }
}
