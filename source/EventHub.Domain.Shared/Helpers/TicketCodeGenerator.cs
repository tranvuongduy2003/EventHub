using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace EventHub.Domain.Shared.Helpers;

public static class TicketCodeGenerator
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    // Generate a ticket code with a prefix, timestamp, and random string
    public static string GenerateTicketCode(string prefix = "EVTICKET")
    {
        // Get the current timestamp in a formatted string
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

        // Generate a random string
        string randomString = GenerateCryptographicallySecureRandomString(6);

        // Combine prefix, timestamp, and random string to create the ticket code
        return $"{prefix}-{timestamp}-{randomString}";
    }

    // Helper method to generate a random string
    private static string GenerateCryptographicallySecureRandomString(int length)
    {
        var sb = new StringBuilder(length);
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                sb.Append(Characters[(int)(num % (uint)Characters.Length)]);
            }
        }

        return sb.ToString();
    }
}
