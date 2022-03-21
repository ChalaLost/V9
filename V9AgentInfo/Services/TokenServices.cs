using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using V9AgentInfo.Models.Config;
using V9AgentInfo.Models.CRM;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Models.Entities.CRM;
using V9Common;

namespace V9AgentInfo.Services
{
    public interface ITokenServices
    {
        bool VerifyAccessToken(string token);
        UserTokenDTO GetUserToken(V9_Account user);
        Task<UserTokenDTO> RefreshUserToken(string oldToken);
        ClaimsPrincipal GetClaimsPrincipalByToken(string token);
        UserTokenDTO Logon(UserLogonModel user);
    }

    public class TokenServices : ITokenServices
    {
        private readonly V9Context _Context;
        private readonly UserTokenSettingModel _UserTokenSetting;
        private readonly ILogger<TokenServices> _logger;
        public TokenServices(V9Context Context, IOptions<UserTokenSettingModel> UserTokenSetting, ILogger<TokenServices> logger)
        {
            _logger = logger;
            _Context = Context;
            _UserTokenSetting = UserTokenSetting.Value;
        }


        public UserTokenDTO GetUserToken(V9_Account user)
        {
            return this.GenUserToken(user);
        }

        public UserTokenDTO Logon(UserLogonModel user)
        {
            return this.GenUserToken(user);
        }

        public async Task<UserTokenDTO> RefreshUserToken(string oldToken)
        {
            try
            {
                var principal = this.GetClaimsPrincipalByToken(oldToken);
                var userData = JsonConvert.DeserializeObject<UserLogonModel>(principal.FindFirst("UserData")?.Value);
                if (principal != null && userData != null)
                {
                    var item = await _Context.Accounts.Where(x => x.Id == userData.Id)
                        .Select(s => new UserLogonModel()
                        {
                            Id = s.Id,
                            Email = s.Email,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            PhoneNumber = s.PhoneNo,
                            UserName = s.UserName,
                            RoleIds = s.AccountRole.Select(s => s.RoleId).ToList()
                        }).FirstOrDefaultAsync();

                    if (item.UserName == "admin")
                    {
                        item.Permissions = await _Context.Permissions.Select(x => x.Code).ToListAsync();
                    }
                    else
                    {
                        item.Permissions = await _Context.RolePermissions.Where(x => item.RoleIds.Contains(x.RoleId) && x.Role.IsActive)
                            .Select(s => s.Permission.Code).ToListAsync();
                    }
                    return this.Logon(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;

        }

        public ClaimsPrincipal GetClaimsPrincipalByToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_UserTokenSetting.Secret)),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var claim = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                var outToken = securityToken;
                return claim;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private UserTokenDTO GenUserToken(V9_Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_UserTokenSetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),

                Expires = DateTime.Now.AddHours(Convert.ToDouble(_UserTokenSetting.Expires)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserTokenDTO userToken = new()
            {
                Id = user.Id.ToString(),
                Accesstoken = tokenHandler.WriteToken(token),
                ExpiredAt = tokenDescriptor.Expires,
                Username = user.UserName
            };
            return userToken;
        }

        private UserTokenDTO GenUserToken(UserLogonModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var myClaims = new List<Claim>()
                {
                    new Claim("UserData", JsonConvert.SerializeObject(user))
                };
            // Add permission as multiple claims
            foreach (var role in user.Permissions)
            {
                myClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            //Mặc định giành cho quyền lấy thông tin tài khoản cá nhân
            myClaims.Add(new Claim(ClaimTypes.Role, PermissionConst.Default_account));


            var token = GetJwtToken(user.UserName, _UserTokenSetting.Secret, "", "",
                TimeSpan.FromHours(_UserTokenSetting.Expires), myClaims.ToArray());

            UserTokenDTO userToken = new()
            {
                Id = user.Id.ToString(),
                Accesstoken = tokenHandler.WriteToken(token),
                ExpiredAt = token.ValidTo,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Permissions = user.Permissions
            };
            return userToken;
        }



        JwtSecurityToken GetJwtToken(string username, string signingKey, string issuer, string audience,
            TimeSpan expiration, Claim[] additionalClaims = null)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (additionalClaims is object)
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.Add(expiration),
                claims: claims,
                signingCredentials: creds
            );
        }

        public bool VerifyAccessToken(string accesstoken)
        {
            var principal = this.GetClaimsPrincipalByToken(accesstoken);
            return principal != null;
        }
    }
}

