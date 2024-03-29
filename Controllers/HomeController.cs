using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RunPowerShellScript()
        {
            string scriptPath = "C:\\Users\\CWP2020\\Documents\\test.ps1";

            var output = new List<string>();

            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"-NoProfile -ExecutionPolicy unrestricted -File \"{scriptPath}\""
            };

            using (var process = new Process { StartInfo = psi })
            {
                process.OutputDataReceived += (sender, e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        output.Add(e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        output.Add($"Error: {e.Data}");
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();
            }

            return Json(output);
        }
    }
}
