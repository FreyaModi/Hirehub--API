using System;

namespace WebApplication1.Models
{
    public class TokenGenerator
    {
        public static string GenerateToken()
        {
            // Implement your token generation logic here
            // For example, you can generate a GUID and return it as a string
            return Guid.NewGuid().ToString();
        }
    }
}
