using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/CronJobApi")]
    [ApiController]
    public class CronJobApiController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public CronJobApiController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [Route("DeleteViewFiles")]
        public IActionResult DeleteViewFiles()
        {
            var path= _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\ViewFile\\";
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            return Ok("success");
        }

    }
}
