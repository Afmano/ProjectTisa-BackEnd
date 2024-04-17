using Microsoft.IdentityModel.Tokens;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Responses;
using ProjectTisa.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProjectTisa.Libs
{
    /// <summary>
    /// Class with static methods to provide auth.
    /// </summary>
    public static class AuthTools
    {
        /// <summary>
        /// Create JWT token using <see cref="User"/>'s data.
        /// </summary>
        /// <param name="user"><see cref="User"/> entity.</param>
        /// <param name="authData">Configuration data for auth.</param>
        /// <returns>JWT token.</returns>
        public static TokenResponse CreateToken(User user, AuthData authData)
        {
            SymmetricSecurityKey symKey = new(Encoding.UTF8.GetBytes(authData.IssuerSigningKey));
            SigningCredentials credentials = new(symKey, SecurityAlgorithms.HmacSha256);
            DateTime expireDateTime = DateTime.UtcNow.Add(authData.ExpirationTime);
            JwtSecurityToken token = new(
                issuer: authData.Issuer, audience: authData.Audience,
                claims: [new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.Role, user.Role.ToString())],
                expires: expireDateTime, signingCredentials: credentials);
            return new(new JwtSecurityTokenHandler().WriteToken(token), expireDateTime);
        }
        /// <summary>
        /// Create random salt form hashing password.
        /// </summary>
        /// <param name="size">Size of salt.</param>
        /// <returns>Salt in byte array.</returns>
        public static string CreateSalt(int size)
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(size));
        }
        /// <summary>
        /// Hash password using auth configuration and salt.
        /// <para>See <seealso cref="CreateSalt"/></para>
        /// </summary>
        /// <param name="password">Password to hash.</param>
        /// <param name="salt">Salt added to password.</param>
        /// <param name="authData">Configuration data for auth.</param>
        /// <returns>Password in hash format.</returns>
        public static string HashPasword(string password, string salt, AuthData authData)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), Convert.FromHexString(salt), authData.IterationCount, authData.HashAlgorithm, salt.Length);
            return Convert.ToHexString(hash);
        }
        /// <summary>
        /// Verify equals of password hash and provided password.
        /// </summary>
        /// <param name="password">Password to check.</param>
        /// <param name="hash">Hash to check.</param>
        /// <param name="salt">Salt added to password.</param>
        /// <param name="authData">Configuration data for auth.</param>
        /// <returns>Result of verifying: <c>true</c> - password and salt belong to hash, <c>false</c> - password and salt doesn't belong to hash.</returns>
        public static bool VerifyPassword(string password, string hash, string salt, AuthData authData)
        {
            byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, Convert.FromHexString(salt), authData.IterationCount, authData.HashAlgorithm, salt.Length);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
        /// <summary>
        /// Generate verification code for send to email.
        /// </summary>
        /// <returns>Code with N(default 6) digits</returns>
        public static string GenerateCode(int digits = 6)
        {
            return Random.Shared.Next(0, (int)Math.Pow(10, digits)).ToString("D" + digits);
        }
    }
}
