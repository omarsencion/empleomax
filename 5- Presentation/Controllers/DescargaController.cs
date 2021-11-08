using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace empleoswebMax.Controllers
{
    public class DescargaController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public DescargaController(IWebHostEnvironment hostEnvironment)
        {

            webHostEnvironment = hostEnvironment;

        }
        public async Task<IActionResult> Descarga()
        {
            string uniqueFileName = "test.pdf";
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "documentos");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            var path = filePath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(path).ToLowerInvariant();

            return File(memory, GetMimeTypes()[ext], Path.GetFileName(path));

        }


        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string> {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxlmformats-officedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"},
            };
        }
    }
}