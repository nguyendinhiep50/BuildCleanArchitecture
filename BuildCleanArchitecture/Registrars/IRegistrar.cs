namespace BuildCleanArchitecture.Registrars
{
    public interface IRegistrar
    {
        void RegisterServices(WebApplicationBuilder builder);
    }
}
