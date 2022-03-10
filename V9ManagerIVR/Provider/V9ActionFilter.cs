using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Linq;
using V9ManagerIVR.Models.CRM;

namespace V9ManagerIVR.Provider
{
    public class V9ActionFilter : V9ActionAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            try
            {
                if (string.IsNullOrEmpty(Permission)) return;
                var myPermiss = JsonConvert.DeserializeObject<string[]>(context.HttpContext.Request.Headers["Permissions"]);
                if (myPermiss != null && myPermiss.Length > 0)
                {
                    var data = myPermiss.Select(s => new PermissionModel
                    {
                        Data = s
                    }).ToList();


                    if (!data.Any(x => x.Permission.Contains(Permission)))
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    else
                    {
                        var controller = context.Controller as V9Controller;
                        if (controller == null) return;
                        controller.SubActionPermission = data.First(x => x.Permission.Contains(Permission)).SubActionPermission;
                    }
                }
                return;
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }


    public class V9ActionAttribute : ActionFilterAttribute
    {
        public string Permission { get; set; }
        public V9ActionAttribute(params string[] permission) : base()
        {
            Permission = string.Join(",", permission);
        }
    }
}
