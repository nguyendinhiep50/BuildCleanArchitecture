using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildCleanArchitecture.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IUser _user;

    public AuditableEntityInterceptor(
        IUser user)
    {
        _user = user;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        //foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        //{
        //    if (entry.State == EntityState.Added)
        //    {
        //        entry.Entity.CreatedBy = _user.Id;
        //        entry.Entity.Created = DateTimeOffset.UtcNow;
        //    }

        //    if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
        //    {
        //        entry.Entity.LastModifiedBy = _user.Id;
        //        entry.Entity.LastModified = DateTimeOffset.UtcNow;
        //    }
        //}

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _user.UserId;
                entry.Entity.CreatedDate = DateTime.Now.Date;
                entry.Entity.CreatedSpanTime = DateTime.UtcNow.AddHours(7).ToString("HH:mm:ss");
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.UpdatedBy = _user.UserId;
                entry.Entity.UpdatedDate = DateTime.Now.Date;
                entry.Entity.UpdatedSpanTime = DateTime.UtcNow.AddHours(7).ToString("HH:mm:ss");
            }
        }

        foreach (var entry in context.ChangeTracker.Entries<AuditableWithUIdEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.UId = entry.Entity.UId == Guid.Empty
                                        ? Guid.NewGuid()
                                        : entry.Entity.UId;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
