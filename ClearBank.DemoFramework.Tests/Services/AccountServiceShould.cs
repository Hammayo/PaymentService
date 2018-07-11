using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using ClearBank.DemoFramework.Services;
using ClearBank.DemoFramework.Types;

namespace ClearBank.DemoFramework.Tests.Services
{
    public sealed class AccountServiceShould : IDisposable
    {
        private DataStoreService _ds;
        private AccountService _as;
        private ITestOutputHelper _output;

        public AccountServiceShould(ITestOutputHelper output)
        {
            // Helper service objects 
            _ds = new DataStoreService();
            _as = new AccountService(_ds);

            // Helper objects
            _output = output;
            _output.WriteLine("Creating account service...");
        }

        [Fact(Skip = "Only for prototype. This is not needed and safe to remove.")]
        public void _StubTestCase()
        {
            _output.WriteLine("Running StubTestCase...");
            Assert.True(true);
        }

        [Fact]
        public void GetAccountEmpty()
        {
            var sut = _as.GetAccount(string.Empty);
            Assert.Null(sut.AccountNumber);
        }

        [Fact]
        public void UpdateAccountDeducteBalance()
        {
            var acc = new Account { Balance = 5 };
            var req = new MakePaymentRequest { Amount = 2 };
            _as.UpdateAccount(acc, req);
            Assert.Equal(3, acc.Balance);
        }

        [Fact]
        public void UpdateAccountDeducteBalanceInvalid()
        {
            var acc = new Account { Balance = 8 };
            var req = new MakePaymentRequest { Amount = (decimal)12.68 };
            _as.UpdateAccount(acc, req);
            Assert.Equal(0, acc.Balance);
        }

        public void Dispose()
        {
            _output.WriteLine("Disposing account service!");
        }
    }   
}
