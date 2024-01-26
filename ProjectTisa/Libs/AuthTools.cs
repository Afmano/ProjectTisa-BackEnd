using Microsoft.IdentityModel.Tokens;
using ProjectTisa.Controllers.GeneralData;
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
        /// Create JWT token using user's data.
        /// </summary>
        /// <param name="user"><b>User</b> entity.</param>
        /// <param name="authData">Configuration data for auth.</param>
        /// <returns>JWT token.</returns>
        public static string CreateToken(User user, AuthData authData)
        {
            SymmetricSecurityKey symKey = new(Encoding.UTF8.GetBytes(authData.IssuerSigningKey));
            SigningCredentials credentials = new(symKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                issuer: authData.Issuer, audience: authData.Audience,
                claims: [new Claim(nameof(user.Username), user.Username), new Claim(nameof(user.PasswordHash), user.PasswordHash!)],
                expires: DateTime.Now.Add(authData.ExpirationTime), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// Create random salt form hashing password.
        /// </summary>
        /// <param name="size">Size of salt.</param>
        /// <returns>Salt in byte array.</returns>
        public static byte[] CreateSalt(int size)
        {
            return RandomNumberGenerator.GetBytes(size);
        }
        /// <summary>
        /// Hash password using auth configuration and salt.
        /// <para>See <seealso cref="CreateSalt"/></para>
        /// </summary>
        /// <param name="password">Password to hash.</param>
        /// <param name="salt">Salt added to password.</param>
        /// <param name="authData">Configuration data for auth.</param>
        /// <returns>Password in hash format.</returns>
        public static string HashPasword(string password, byte[] salt, AuthData authData)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, authData.IterationCount, authData.HashAlgorithm, salt.Length);
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
        public static bool VerifyPassword(string password, string hash, byte[] salt, AuthData authData)
        {
            byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, authData.IterationCount, authData.HashAlgorithm, salt.Length);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

    }
}
