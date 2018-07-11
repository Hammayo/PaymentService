using ClearBank.DemoFramework.Data;

namespace ClearBank.DemoFramework.Services
{
    public interface IDataStoreService
    {
        IAccountDataStore GetAccountDataStore(string dataStoreType);
    }
}