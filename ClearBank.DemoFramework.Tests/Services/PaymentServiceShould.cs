using System;
using Xunit;
using Xunit.Abstractions;
using ClearBank.DemoFramework.Services;
using ClearBank.DemoFramework.Types;

namespace ClearBank.DemoFramework.Tests.Services
{
    public sealed class PaymentServiceShould : IDisposable
    {
        private DataStoreService _ds;
        private AccountService _as;
        private PaymentService _ps;
        private MakePaymentRequest _req;
        private MakePaymentResult _res;
        private ITestOutputHelper _output;

        public PaymentServiceShould(ITestOutputHelper output)
        {
            // Helper service objects 
            _ds = new DataStoreService();
            _as = new AccountService(_ds);
            _ps = new PaymentService(_as);

            // Helper objects
            _req = new MakePaymentRequest();
            _output = output;
            _output.WriteLine("Creating payment service...");
        }

        [Fact(Skip = "Only for prototype. This is not needed and safe to remove.") ]
        public void _StubTestCase()
        {
            _output.WriteLine("Running StubTestCase...");
            _res = _ps.MakePayment(_req);
            Assert.False(_res.Success);
        }

        [Fact]
        public void MakePaymentAccountMissingRequestReturnsFalse()
        {
            _res = _ps.MakePayment(null);
            Assert.False(_res.Success);
        }

        [Fact]
        public void MakePaymentBacsRequestProcessReturnsTrue()
        {
            var reqPay = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };
            _res = _ps.MakePayment(reqPay);
            Assert.True(_res.Success);
        }

        [Fact]
        public void MakePaymentChapsRequestProcessReturnsTrue()
        {
            var reqPay = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };
            _res = _ps.MakePayment(reqPay);
            Assert.True(_res.Success);
        }

        [Fact]
        public void MakePaymentFasterPaymentRequestProcessReturnsTrue()
        {
            var reqPay = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments };
            _res = _ps.MakePayment(reqPay);
            Assert.True(_res.Success);
        }

        //[Fact]
        //public void MakePaymentSuccessUpdateAccountReturnsTrue()
        //{
        //    var reqPay = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };
        //    _res = _ps.MakePayment(reqPay);

        //    Assert.True(_res.Success);
        //}

        public void Dispose()
        {
            _output.WriteLine("Disposing payment service!");
        }
    }
}
