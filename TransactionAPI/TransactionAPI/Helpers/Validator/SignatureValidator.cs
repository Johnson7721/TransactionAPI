using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using TransactionAPI.Models;
using AppLogger = TransactionAPI.Helpers.Logger.Logger;

namespace TransactionAPI.Helpers.Validator
{
    public static class SignatureValidator
    {
        public static (bool IsValid, string ErrorMessage) ValidateSignature(TransactionRequest request)
        {
            try
            {
                var expectedSignature = GenerateSignature(
                    request.Timestamp,
                    request.PartnerKey,
                    request.PartnerRefNo,
                    request.TotalAmount ?? 0,
                    request.PartnerPassword);

                if (!string.Equals(request.Sig?.Trim(), expectedSignature, StringComparison.Ordinal))
                    return Fail(ValidationMessages.AccessDenied);

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                AppLogger.LogError(LogMessages.SignatureException, ex);
                return Fail(ValidationMessages.AccessDenied);
            }
        }

        private static string GenerateSignature(
            string timestamp,
            string partnerKey,
            string partnerRefNo,
            long totalAmount,
            string partnerPasswordBase64)
        {
            if (!DateTimeOffset.TryParse(
                    timestamp?.Trim(),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out var parsedTimestamp))
            {
                throw new FormatException("Invalid timestamp format.");
            }

            var formattedTimestamp = parsedTimestamp.ToUniversalTime().ToString("yyyyMMddHHmmss");

            var payload = $"{formattedTimestamp}{partnerKey}{partnerRefNo}{totalAmount}{partnerPasswordBase64}";

            // SHA-256 hash (lowercase hexadecimal)
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
            var hex = Convert.ToHexString(hashBytes).ToLowerInvariant();

            // Convert hex to Base64
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(hex));
        }

        private static (bool IsValid, string ErrorMessage) Fail(string message)
            => (false, message);
    }
}
