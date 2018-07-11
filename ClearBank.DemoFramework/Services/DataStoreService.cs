using ClearBank.DemoFramework.Data;

namespace ClearBank.DemoFramework.Services
{
    public class DataStoreService : IDataStoreService
    {
        public IAccountDataStore GetAccountDataStore(string dataStoreType)
        {
            if (dataStoreType == "Backup")
            {
                return new BackupAccountDataStore();
            }

            return new AccountDataStore();
        }
    }
}
