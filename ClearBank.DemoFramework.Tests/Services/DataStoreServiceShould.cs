using System;
using ClearBank.DemoFramework.Data;
using ClearBank.DemoFramework.Services;
using Xunit;

namespace ClearBank.DemoFramework.Tests.Services
{
    public class DataStoreServiceShould
    {
        private DataStoreService _ds;

        public DataStoreServiceShould()
        {
            _ds = new DataStoreService();
        }

        [Fact]
        public void CreateNormalAccountDataStore()
        {
            var acc = _ds.GetAccountDataStore("");
            Assert.IsType<AccountDataStore>(acc);
        }

        [Fact]
        public void CreateBackupAccountDataStore()
        {
            var acc = _ds.GetAccountDataStore("Backup");
            Assert.IsType<BackupAccountDataStore>(acc);
        }
    }
}
