using TransactionAPI.Helpers.Validator;
using TransactionAPI.Helpers;
using TransactionAPI.Models;
using TransactionAPI.Helpers.Logger;

namespace TransactionAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IDiscountCalculator _discountCalculator;

        public TransactionService(IDiscountCalculator discountCalculator)
        {
            _discountCalculator = discountCalculator;
        }

        public TransactionResponse ProcessTransaction(TransactionRequest request)
        {
            try
            {
                var itemTotalValidation = ValidateItemTotal(request);
                if (!itemTotalValidation.IsValid)
                {
                    Logger.LogWarn(string.Format(LogMessages.InvalidItemTotal, request.PartnerKey, request.PartnerRefNo));
                    return TransactionResponse.Failure(itemTotalValidation.ErrorMessage);
                }

                var partnerValidation = PartnerValidator.ValidatePartner(request);
                if (!partnerValidation.IsValid)
                {
                    Logger.LogWarn(string.Format(LogMessages.PartnerValidationFailed, request.PartnerKey, partnerValidation.ErrorMessage));
                    return TransactionResponse.Failure(partnerValidation.ErrorMessage);
                }

                var signatureValidation = SignatureValidator.ValidateSignature(request);
                if (!signatureValidation.IsValid)
                {
                    Logger.LogWarn(string.Format(LogMessages.SignatureMismatch, request.PartnerKey, request.PartnerRefNo));
                    return TransactionResponse.Failure(signatureValidation.ErrorMessage);
                }

                var timestampValidation = TimestampValidator.ValidateNotExpired(request);
                if (!timestampValidation.IsValid)
                {
                    Logger.LogWarn(string.Format(LogMessages.RequestExpired, request.PartnerKey, request.Timestamp));
                    return TransactionResponse.Failure(timestampValidation.ErrorMessage);
                }

                var (totalDiscount, finalAmount) = _discountCalculator.CalculateDiscount(request.TotalAmount ?? 0);
                
                Logger.LogInfo(string.Format(LogMessages.TransactionProcessed, request.PartnerKey, request.PartnerRefNo, request.TotalAmount));

                return TransactionResponse.Success(
                    request.TotalAmount ?? 0,
                    totalDiscount,
                    finalAmount);
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format(LogMessages.TransactionError, request.PartnerKey), ex);
                return TransactionResponse.Failure(ValidationMessages.ProcessingError);
            }
        }

        private static (bool IsValid, string ErrorMessage) ValidateItemTotal(TransactionRequest request)
        {
            if (request.Items is null || request.Items.Count == 0)
                return (true, string.Empty);

            var itemTotal = request.Items.Sum(i => (long)i.Qty * i.UnitPrice);

            return itemTotal == request.TotalAmount
                ? (true, string.Empty)
                : (false, ValidationMessages.InvalidTotalAmount);
        }
    }
}
