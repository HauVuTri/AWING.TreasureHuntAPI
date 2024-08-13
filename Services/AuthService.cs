using AWING.TreasureHuntAPI.Interfaces;
using AWING.TreasureHuntAPI.Models.DTOs;
using AWING.TreasureHuntAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AWING.TreasureHuntAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly TreasureHuntDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(TreasureHuntDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> Register(UserRegisterDto userRegisterDto)
        {
            if (await UserExists(userRegisterDto.Username))
                return false;

            var hash = HashPassword(userRegisterDto.Password);

            var user = new User
            {
                Username = userRegisterDto.Username.ToLower(),
                Email = userRegisterDto.Email,
                PasswordHash = hash,
                CreatedAt = DateTime.Now
            };

            // Add the default "Member" role
            var memberRole = await _context.Roles.SingleOrDefaultAsync(r => r.RoleName == "Member");
            if (memberRole == null)
            {
                // Create the "Member" role if it doesn't exist
                memberRole = new Role { RoleName = "Member" };
                _context.Roles.Add(memberRole);
                await _context.SaveChangesAsync();
            }

            user.Roles.Add(memberRole);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<string> Login(UserLoginDto userLoginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == userLoginDto.Username.ToLower());

            if (user == null)
                return null;

            if (!VerifyPassword(userLoginDto.Password, user.PasswordHash))
                return null;

            return CreateJwtToken(user);
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }

        private string HashPassword(string password)
        {
            // Generate a random 16-byte salt
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using var hmac = new HMACSHA512(salt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Combine salt and hash
            var hashBytes = new byte[16 + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, hash.Length);

            // Convert to Base64 for storage
            return Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Convert stored hash from Base64
            var hashBytes = Convert.FromBase64String(storedHash);

            // Extract the salt (first 16 bytes)
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Hash the input password using the extracted salt
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Compare the computed hash with the stored hash (ignoring the first 16 bytes which is the salt)
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != hashBytes[i + 16]) return false;
            }

            return true;
        }

        private string CreateJwtToken(User user)
        {
            // Create a list of claims
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };

            // Add roles to the claims
            var roles = user.Roles.Select(role => role.RoleName).ToList();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create a security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSetting:Key"]));

            // Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenExpiresAfter = TimeSpan.Parse(_configuration["JwtSetting:TokenExpiresAfter"]);
            // Create the token descriptor
            var token = new JwtSecurityToken(
               issuer: _configuration["JwtSetting:Issuer"],
               audience: _configuration["JwtSetting:Audience"],
               claims: claims,
                expires: DateTime.Now.Add(tokenExpiresAfter),
               signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
