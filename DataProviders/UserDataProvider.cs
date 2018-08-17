using Api.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Api.DataProviders
{
    public class UserDataProvider
    {
        private readonly ConfidentialInfo _confidentialInfo;

        public UserDataProvider(IOptions<ConfidentialInfo> myConfiguration, IConfiguration configuration)
        {
            _confidentialInfo = myConfiguration.Value;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            using (var sqlConnection = new SqlConnection(_confidentialInfo.ConnectionString))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<User>(
                    "usp_GetUsersList",
                    null,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public bool CheckUserName(UserForSingIn user)
        {
            using (var sqlConnection = new SqlConnection(_confidentialInfo.ConnectionString))
            {
                sqlConnection.Open();

                LoginUserResponse responseUser = sqlConnection.Query<LoginUserResponse>("usp_GetUserDataForLogin", new { @username = user.Username }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                string passwordHashed = String.Concat(user.Password, responseUser.Salt);

                string hashedPasswordAndSalt = GetSwcSHA1(passwordHashed);

                if (!hashedPasswordAndSalt.Equals(responseUser.Password))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static string GetSwcSHA1(string value)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sh1;
        }

        public string GenerateJwtToken(string userName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_confidentialInfo.TokenSecretKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var userClaim = new[] {
                new Claim(ClaimTypes.Name,"jbknowledge")
            };

            var token = new JwtSecurityToken(
                issuer: "www.pablogramajo.com",
                audience: "www.pablogramajo.com",
                expires: DateTime.Now.AddMinutes(10),
                claims: userClaim,
                signingCredentials: credential
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

    }
}
