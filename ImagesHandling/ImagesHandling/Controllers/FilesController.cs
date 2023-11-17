using ImagesHandling.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImagesHandling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost]
        public ActionResult<UploadFileDto>Upload(IFormFile file)
        {
            #region Checking Extension
            var extension = Path.GetExtension(file.FileName);
            //allowed extensions its better to be part of configuration ex app setting .json
            var allowedExtensions = new string[]
            {
                ".png",
                ".jpg",
                ".svg"
            };
            bool isExtensionAllowed = allowedExtensions.Contains(extension, StringComparer.InvariantCultureIgnoreCase);
            if (!isExtensionAllowed)
            {
                return BadRequest(new UploadFileDto(false, "Extension is not valid"));
            }
            #endregion

            #region Checking Size
            bool isSizeAllowed = file.Length is > 0 and <= 4_000_000;
            if (!isSizeAllowed)
            {
                return BadRequest(new UploadFileDto(false, "Size is not Allowed"));
            }
            #endregion

            #region Storing The Image
            var newFileName = $"{Guid.NewGuid()}{extension}";
            var imagesPath =Path.Combine(Environment.CurrentDirectory,"Images");
            var fullFilePath= Path.Combine(imagesPath,newFileName);
           
            using var stream = new FileStream(fullFilePath, FileMode.Create);
            file.CopyTo(stream);
            #endregion

            #region Generating URL
            var url = $"{Request.Scheme}://{Request.Host}/Images/{newFileName}";
            #endregion

            return new UploadFileDto(true, "Image has been added successfully", url);
        }
    }
}
