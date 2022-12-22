namespace Shared.DAL.Postgres;

public interface IDataInitializer
{
    Task InitAsync();
}