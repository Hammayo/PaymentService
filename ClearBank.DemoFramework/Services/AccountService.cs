using System.Configuration;
using ClearBank.DemoFramework.Types;

namespace ClearBank.DemoFramework.Services
{
    public class AccountService : IAccountService
    {
        private IDataStoreService _dataStoreService;
        private string _dataStoreConfigKey;

        public AccountService(IDataStoreService dataStoreService)
        {
            // See constructor test case GetAccountEmpty()
            _dataStoreService = dataStoreService;
            _dataStoreConfigKey = GetAppSettingForKey("DataStoreType");
        }

        public Account GetAccount(string accountNumber)
        {
            // This requires more test cases when real data store is wired up
            // At the moment this is wired to GetAccountEmpty() test case
            var accountDataStore = _dataStoreService.GetAccountDataStore(_dataStoreConfigKey);
            return accountDataStore.GetAccount(accountNumber);
        }

        public void UpdateAccount(Account account, MakePaymentRequest request)
        {
            // This method is wired to UpdateAccountDeducteBalance() test case
            var accountDataStore = _dataStoreService.GetAccountDataStore(_dataStoreConfigKey);
            account.Balance = GetDeductedBalance(account.Balance, request.Amount);
            accountDataStore.UpdateAccount(account);
        }

        private decimal GetDeductedBalance(decimal existingBalance, decimal paymentAmount)
        {
            if (paymentAmount <= existingBalance)
                return existingBalance - paymentAmount;
            else
                return 0;
        }

        private string GetAppSettingForKey(string dataStoreType)
        {
            return ConfigurationManager.AppSettings[dataStoreType];
        }
    }
}