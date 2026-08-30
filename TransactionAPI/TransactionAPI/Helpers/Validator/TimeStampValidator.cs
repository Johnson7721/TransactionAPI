using System.Globalization;
using TransactionAPI.Models;

namespace TransactionAPI.Helpers.Validator
{
    public static class TimestampValidator
    {
        public static (bool IsValid, string ErrorMessage) ValidateNotExpired(TransactionRequest request)
        {
            if (!DateTimeOffset.TryParse(
                    ValidatorText.TrimText(request.Timestamp),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out var requestTime))
            {
                return (false, ValidationMessages.TimestampInvalidFormat);
            }

            var difference = Math.Abs((DateTimeOffset.UtcNow - requestTime).TotalMinutes);

            return difference <= ValidationConstants.Time.MaxTimestampDifferenceMinutes
                ? (true, string.Empty)
                : (false, ValidationMessages.Expired);
        }
    }
}
