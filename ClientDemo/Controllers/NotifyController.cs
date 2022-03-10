using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;

namespace ClientDemo.Controllers
{
    [ApiController]
    [Route("api/calls")]
    public class NotifyController : Controller
    {
        private readonly V9Context _Context;

        public NotifyController(V9Context Context)
        {
            _Context = Context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var calls = await _Context.Notifys.ToListAsync();

            return View(calls);
        }

        /*[HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var call = await _ctx.Calls.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (call == null) return BadRequest();

                _ctx.Remove(call);
                if (await _ctx.SaveChangesAsync() > 0)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest("Database Error");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }*/
    }
}
