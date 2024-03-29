using Microsoft.AspNetCore.Mvc;
using System;
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
            string scriptPath = "C:\\Users\\CWP2020\\HelloWorld\\test.ps1";

            using (PowerShell powerShell = PowerShell.Create())
            {
				powerShell.AddScript($"Set-ExecutionPolicy Bypass -Scope Process");
                powerShell.AddScript(scriptPath);
                var results = powerShell.Invoke();

                if (powerShell.HadErrors)
                {
                    foreach (var error in powerShell.Streams.Error)
                    {
                        Console.WriteLine("Error: " + error.ToString());
                    }
                }
                else
                {
                    foreach (var result in results)
                    {
                        Console.WriteLine(result.ToString());
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
