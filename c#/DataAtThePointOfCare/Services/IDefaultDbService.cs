namespace DataAtThePointOfCare.Services
{
    public interface IDefaultDbService
    {
        void CreateServiceDb(string dbName);
        void DeleteServiceDb(string dbName);
    }
}