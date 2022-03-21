
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
using V9AgentInfo.Models.Entities.AgentInfo;
using V9AgentInfo.Models.Entities.CRM;

namespace V9AgentInfo.Services
{
    public interface IInfoServices
    {
        Task<bool> Create( CreateInfoModel model);
        Task<Info> GetById(Guid InfoId);
        Task<bool> Update(Guid InfoId, UpdateContactInfoModel model);
        Task<bool> Delete(Guid Ids);
        Task<List<Info>> GetAll();
        Task<string> Authenticate(LoginModel model);
    }

    public class Infoservices : IInfoServices
    {
        private readonly V9Context _Context;
        private readonly ILogger<Infoservices> _logger;
        private readonly IConfiguration _config;
        private readonly UserTokenSettingModel _UserTokenSetting;

        public Infoservices(V9Context Context, ILogger<Infoservices> logger,IConfiguration config, IOptions<UserTokenSettingModel> UserTokenSetting)
        {
            _logger = logger;
            _Context = Context;
            _config = config;
            _UserTokenSetting = UserTokenSetting.Value;

        }
        public async Task<string> Authenticate( LoginModel model)
        {
            var user = await _Context.Infos.Where(x => x.UserName == model.UserName).FirstOrDefaultAsync();
            if (user == null) return "Tài khoản ko tồn tại";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.UserName)
            };
            /*var tokenHandler = new JwtSecurityTokenHandler();
            var certificateText =
        "-----BEGIN CERTIFICATE-----\n" +
        "MIIBljCCAUACCQCIDMpqK7WfWDANBgkqhkiG9w0BAQsFADBSMQswCQYDVQQGEwJV\n" +
        "UzETMBEGA1UECAwKU29tZS1TdGF0ZTESMBAGA1UECgwJTHV4b3R0aWNhMRowGAYD\n" +
        "VQQLDBFMdXhvdHRpY2EgZXllY2FyZTAeFw0xODA1MjMxNTE1MjdaFw0yODA1MjAx\n" +
        "NTE1MjdaMFIxCzAJBgNVBAYTAlVTMRMwEQYDVQQIDApTb21lLVN0YXRlMRIwEAYD\n" +
        "VQQKDAlMdXhvdHRpY2ExGjAYBgNVBAsMEUx1eG90dGljYSBleWVjYXJlMFwwDQYJ\n" +
        "KoZIhvcNAQEBBQADSwAwSAJBAKuMYcirPj81WBtMituJJenF0CG/HYLcAUOtWKl1\n" +
        "HchC0dM8VRRBI/HV+nZcweXzpjhX8ySa9s7kJneP0cuJiU8CAwEAATANBgkqhkiG\n" +
        "9w0BAQsFAANBAKEM8wQwlqKgkfqnNFcbsZM0RUxS+eWR9LvycGuMN7aL9M6GOmfp\n" +
        "QmF4MH4uvkaiZenqCkhDkyi4Cy81tz453tQ=\n" +
        "-----END CERTIFICATE-----";

            var key = Encoding.ASCII.GetBytes(certificateText);
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
            return "userToken"; */
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> Create(CreateInfoModel model)
        {
            var guidID = Guid.NewGuid();
            var guidComId = Guid.NewGuid();
            var item = new Info()
            {
                Id = guidID,
                CompanyId = guidComId,
                UserName = model.UserName,
                FullName = model.FullName,
                Image = model.Image,
                Department = model.Department,
                Extension = model.Extension,
                Module = model.Module,
            };
            
            await _Context.Infos.AddAsync(item);
            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Info>> GetAll()
        {
            return await _Context.Infos.ToListAsync();
        }

        public async Task<Info> GetById(Guid InfoId)
        {
            return await _Context.Infos.Where(x => x.Id == InfoId).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(Guid InfoId, UpdateContactInfoModel model)
        {
            try
            {

                var find = await _Context.Infos.Where(x => x.Id == InfoId).FirstOrDefaultAsync();
                find.Contacts = model.Contacts + 1;
                await _Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
        public async Task<bool> Delete(Guid Ids)
        {
            try
            {
                var item = await _Context.Infos.FindAsync(Ids);
                if (item != null)
                {
                    _Context.Infos.Remove(item);
                    await _Context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
