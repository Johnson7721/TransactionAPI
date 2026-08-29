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

        public async Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var partnerValidation = PartnerValidator.ValidatePartner(request);
                if (!partnerValidation.IsValid)
                {
                    Logger.LogWarn(string.Format(LogMessages.PartnerValidationFailed, request.PartnerKey, partnerValidation.ErrorMessage));
                    return TransactionResponse.Failure(partnerValidation.ErrorMessage);
                }

                cancellationToken.ThrowIfCancellationRequested();

                var signatureValidation = SignatureValidator.ValidateSignature(request);
                if (!signatureValidation.IsValid)
                {
                    Logger.LogWarn(string.Format(LogMessages.SignatureMismatch, request.PartnerKey, request.PartnerRefNo));
                    return TransactionResponse.Failure(signatureValidation.ErrorMessage);
                }

                // Calculate discount
                var (totalDiscount, finalAmount) = _discountCalculator.CalculateDiscount(request.TotalAmount);



                Logger.LogInfo(string.Format(LogMessages.TransactionProcessed, request.PartnerKey, request.PartnerRefNo, request.TotalAmount));

                return await Task.FromResult(TransactionResponse.Success(
                    request.TotalAmount,
                    totalDiscount,
                    finalAmount));
            }
            catch (OperationCanceledException)
            {
                Logger.LogWarn(string.Format(LogMessages.TransactionCancelled, request.PartnerKey));
                throw; 
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format(LogMessages.TransactionError, request.PartnerKey), ex);
                return TransactionResponse.Failure(ValidationMessages.ProcessingError);
            }
        }
    }
}
