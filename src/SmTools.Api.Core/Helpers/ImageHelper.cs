using SkiaSharp;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;

namespace SmTools.Api.Core.Helpers;

/// <summary>
/// 图像处理工具类
/// </summary>
public class ImageHelper
{
    /// <summary>
    /// 压缩图片
    /// </summary>
    /// <param name="source">原文件流</param>
    /// <param name="maxWidth">最大宽度，根据此宽度计算是否需要缩放，计算新高度</param>
    /// <param name="quality">图片质量，范围 0-100</param>
    public static Stream? Compress(Stream source, decimal maxWidth, int quality)
    {
        if (source.Length == 0)
        {
            throw new InvalidParameterException("待处理的图片流为空");
        }

        if (maxWidth <= 0)
        {
            throw new InvalidParameterException("图片宽度需要大于 0");
        }

        if (quality is < 0 or > 100)
        {
            throw new InvalidParameterException("图片质量需要在 0-100 范围内");
        }

        using var fileStream = new SKManagedStream(source);
        using var bitmap = SKBitmap.Decode(fileStream);
        var width = (decimal)bitmap.Width;
        var height = (decimal)bitmap.Height;
        var newWidth = width;
        var newHeight = height;
        if (width > maxWidth)
        {
            newWidth = maxWidth;
            newHeight = height / width * maxWidth;
        }

        using var resized = bitmap.Resize(new SKImageInfo((int)newWidth, (int)newHeight), SKFilterQuality.Medium);
        if (resized == null) return null;
        using var image = SKImage.FromBitmap(resized);
        // using var writeStream = File.OpenWrite(target);
        // image.Encode(SKEncodedImageFormat.Jpeg, quality).SaveTo(writeStream);
        var res = image.Encode(SKEncodedImageFormat.Jpeg, quality).AsStream();
        return res;
    }
}