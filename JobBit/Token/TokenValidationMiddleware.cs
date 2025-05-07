using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JobBit_Business;

namespace JobBit.Token
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "").Trim();

                // 1️⃣ ✅ التحقق مما إذا كان التوكن في القائمة السوداء
                if (await BlackList.IsTokenExistAsync(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token has been blacklisted.");
                    return;
                }

                // 2️⃣ ✅ التحقق من صحة التوكن باستخدام JwtSecurityTokenHandler
                if (!ValidateJwtToken(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid or expired token.");
                    return;
                }
            }

            await _next(context);
        }

        private bool ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out _);

                return true;
            }
            catch
            {
                return false; // ❌ التوكن غير صالح أو منتهي الصلاحية
            }
        }
    }
}
