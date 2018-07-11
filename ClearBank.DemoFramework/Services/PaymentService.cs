using System.ComponentModel;
using ClearBank.DemoFramework.Types;

namespace ClearBank.DemoFramework.Services
{
    public class PaymentService : IPaymentService
    {
        private IAccountService _as;

        public PaymentService(IAccountService accountService)
        {
            _as = accountService;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            Account account = request != null ? _as.GetAccount(request.DebtorAccountNumber) : null;
            if (account == null)
            {
                // "Request Invalid: Unable to find details for supplied account number!"
                // TODO: Ask how to catch this?
                return new MakePaymentResult { Success = false };
            }

            var result = TakePayment(request, account);

            if (result.Success)
            {
                _as.UpdateAccount(account, request);
                result.Success = true;
            }

            return result;
        }

        // Abstract away following private helper methods into seprate sub-classes based and
        // may consider into Factories and write test cases accordingly, BUT for now I decided 
        // not to do this at these stage - kept is simple for the purpose of readability.
        // NOTE: The account.AllowedPaymentSchemes.HasFlag(xxx) can be refactored into Is<T>Valid()
        private static MakePaymentResult TakePayment(MakePaymentRequest paymentRequest, Account accountInfo)
        {
            var paymentResult = new MakePaymentResult();
            // TODO: Factory pattern for each type of payment!
            // For now just call appropriate helper methods
            switch (paymentRequest.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    BacsPaymentProcess(accountInfo, paymentResult);
                    break;

                case PaymentScheme.Chaps:
                    ChapsPaymentProcess(accountInfo, paymentResult);
                    break;

                case PaymentScheme.FasterPayments:
                    FasterPaymentProcess(paymentRequest, accountInfo, paymentResult);
                    break;

                default:
                    throw new InvalidEnumArgumentException("Invalid PaymentScheme enum");
            }

            return paymentResult;
        }

        private static void BacsPaymentProcess(Account account, MakePaymentResult result)
        {
            if (account == null) return;
            result.Success = !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }

        private static void ChapsPaymentProcess(Account account, MakePaymentResult result)
        {
            if (account == null) return;
            var isChaps = !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps);
            if (!isChaps)
                isChaps = (account.Status == AccountStatus.Live);

            result.Success = isChaps;
        }

        private static void FasterPaymentProcess(MakePaymentRequest request, Account account, MakePaymentResult result)
        {
            if (account == null) return;
            var isFasterPayments = !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments);
            if (!isFasterPayments)
                isFasterPayments = (account.Balance < request.Amount) ? true : false;

            result.Success = isFasterPayments;
        }
    }
}
