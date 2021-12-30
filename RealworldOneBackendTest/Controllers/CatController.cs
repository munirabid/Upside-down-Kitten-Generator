using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealworldOneBackendTest.Services;

namespace RealworldOneBackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly ICatService catService;

        public CatController(ICatService catService)
        {
            this.catService = catService;
        }

        /// <summary>
        /// Gets the random cat image and returns upsidedown image.
        /// </summary>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]

        [Route("GetRandomCat")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await catService.GetCat();

                if (response.Length == 0)
                {
                    return NotFound("Image could not be fetched");
                }

                var image = byteArrayToImage(response);

                image.RotateFlip(RotateFlipType.Rotate180FlipNone);

                return File(imgToByteArray(image), "image/jpeg");
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable to process request at the moment");
            }
        }

        #region Private Methods

        //convert bytearray to image
        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }
        }

        //convert image to bytearray
        private byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, ImageFormat.Jpeg);
                return mStream.ToArray();
            }
        }

        #endregion
    }
}