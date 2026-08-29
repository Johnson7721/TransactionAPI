using System.Text;
using TransactionAPI.Models;

namespace TransactionAPI.Helpers.Validator
{
    public static class PartnerValidator
    {
        private static readonly Dictionary<string, (string PartnerRefNo, string Password)> AllowedPartners =
               new(StringComparer.OrdinalIgnoreCase)
           {
            { "FAKEGOOGLE", ("FG-00001", "FAKEPASSWORD1234") },
            { "FAKEPEOPLE", ("FG-00002", "FAKEPASSWORD4578") }
           };

        public static (bool IsValid, string ErrorMessage) ValidatePartner(TransactionRequest request)
        {
            if (request is null)
                return Fail(ValidationMessages.AccessDenied);

            if (!IsAuthorizedPartner(request))
                return Fail(ValidationMessages.AccessDenied);

            return (true, string.Empty);
        }

        private static bool IsAuthorizedPartner(TransactionRequest request)
        {
            if (!AllowedPartners.TryGetValue(request.PartnerKey, out var partnerInfo))
                return false;

            string decodedPassword;
            try
            {
                decodedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(request.PartnerPassword));
            }
            catch (FormatException)
            {
                return false;
            }

            return string.Equals(request.PartnerRefNo, partnerInfo.PartnerRefNo, StringComparison.OrdinalIgnoreCase)
                && string.Equals(decodedPassword, partnerInfo.Password, StringComparison.Ordinal);
        }

             private static (bool IsValid, string ErrorMessage) Fail(string message)
            => (false, message);
    }
}
