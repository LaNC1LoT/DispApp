using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DispApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DispetcherController : ControllerBase
    {
        private readonly IEnumerable<Disp> Disps;

        public DispetcherController()
        {
            Disps = from pr in Process.GetProcesses()
                    where (long)pr.MainWindowHandle != 0
                    select new Disp
                    {
                        Id = pr.Id,
                        Name = $"{pr.ProcessName}({pr.MainWindowTitle})" ,
                        Memory = Perm.GetMemory(pr.ProcessName)
                    };
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Disps);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Disp user = Disps.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(Disp disp)
        {
            if(string.IsNullOrWhiteSpace(disp.Name))
            {
                return BadRequest();
            }
            var s = Process.Start(disp.Name);

            var res = new Disp
            {
                Id = s.Id,
                Name = $"{s.ProcessName}({s.MainWindowTitle})",
                Memory = Perm.GetMemory(s.ProcessName)
            };

            return Ok(res);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var disp = (from pr in Process.GetProcesses(".")
                       where pr.Id == id
                       select pr).FirstOrDefault();

            if (disp == null)
            {
                return BadRequest();
            }

            disp.Kill();

            var d = new Disp
            { 
                Id = disp.Id,
                Name = $"{disp.ProcessName}({disp.MainWindowTitle})",
                Memory = ""
            };

            return Ok(d);
        }
    }

    public class Disp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Memory { get; set; }
    }
}
