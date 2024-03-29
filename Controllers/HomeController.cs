using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Management.Automation;

namespace MyWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RunPowerShellScript()
        {
            string scriptPath = "C:\\Users\\CWP2020\\Documents\\test.ps1";

            List<string> output = new List<string>();

            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript($"Set-ExecutionPolicy Bypass -Scope Process");
                powerShell.AddScript(scriptPath);
                var results = powerShell.Invoke();

                if (powerShell.HadErrors)
                {
                    foreach (var error in powerShell.Streams.Error)
                    {
                        output.Add("Error: " + error.ToString());
                    }
                }
                else
                {
                    foreach (var result in results)
                    {
                        output.Add(result.ToString());
                    }
                }
            }

            ViewBag.Output = output;

            return View("Index");
        }
    }
}
