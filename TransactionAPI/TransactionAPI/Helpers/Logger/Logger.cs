using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using log4net;
using TransactionAPI.Models;

namespace TransactionAPI.Helpers.Logger
{
    /// <summary>
    /// logger helper for request/response/error logging via log4net.
    /// Passwords are encrypted (AES) before being written to the log file.
    /// </summary>
    public static class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Logger));

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF"); 
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("0123456789ABCDEF");                   

        public static void LogRequest(string url, TransactionRequest request)
        {
            var safeCopy = new
            {
                partnerkey = request.PartnerKey,
                partnerrefno = request.PartnerRefNo,
                partnerpassword = Encrypt(request.PartnerPassword),
                totalamount = request.TotalAmount,
                timestamp = request.Timestamp,
                sig = request.Sig,
                items = request.Items
            };
            Log.Info(string.Format(LogMessages.RequestBody, url, JsonSerializer.Serialize(safeCopy)));
        }

        public static void LogResponse(string url, TransactionResponse response)
        {
            var json = JsonSerializer.Serialize(response);
            if (response.Result == 1)
                Log.Info(string.Format(LogMessages.ResponseSuccess, url, json));
            else
                Log.Warn(string.Format(LogMessages.ResponseFailure, url, json));
        }

        public static void LogInfo(string message)
            => Log.Info(message);

        public static void LogWarn(string message)
            => Log.Warn(message);

        public static void LogError(string message, Exception? ex = null)
            => Log.Error(message, ex);

        private static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            var cipherBytes = aes.EncryptCbc(Encoding.UTF8.GetBytes(plainText ?? string.Empty), IV);
            return "ENC:" + Convert.ToBase64String(cipherBytes);
        }

        public static string Decrypt(string encryptedValue)
        {
            if (!encryptedValue.StartsWith("ENC:", StringComparison.Ordinal))
                return encryptedValue;

            using var aes = Aes.Create();
            aes.Key = Key;
            var cipherBytes = Convert.FromBase64String(encryptedValue["ENC:".Length..]);
            var plainBytes = aes.DecryptCbc(cipherBytes, IV);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
