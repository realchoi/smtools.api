using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Core.Helpers;
using SpringMountain.Api.Exceptions.Contracts.Exceptions;

namespace SmTools.Api.Controllers;

[ApiController]
[Route("image")]
public class ImageController
{
    /// <summary>
    /// 压缩图片
    /// </summary>
    /// <param name="imageFile">图片文件</param>
    /// <param name="maxWidth">最大宽度（像素）</param>
    /// <param name="quality">压缩后的图片质量（0-100）</param>
    /// <returns></returns>
    /// <exception cref="ApiBaseException"></exception>
    [HttpPost("compress")]
    public IActionResult Compress([Required, FromForm] IFormFile imageFile,
        [Required, FromQuery] decimal maxWidth, [FromQuery] int quality = 100)
    {
        var stream = imageFile.OpenReadStream();
        var res = ImageHelper.Compress(stream, maxWidth, quality);
        if (res == null)
        {
            throw new ApiBaseException("图片压缩失败");
        }

        return new FileStreamResult(res, "image/jpg");
    }
}