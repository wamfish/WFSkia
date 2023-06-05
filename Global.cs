//  Copyright (C) 2023 - Present John Roscoe Hamilton - All Rights Reserved
//  You may use, distribute and modify this code under the terms of the MIT license.
//  See the file License.txt in the root folder for full license details.

global using Godot;
global using SkiaSharp;
global using Svg.Skia;
global using System;
global using System.IO;
global using WFLib;
global using static WFSkia.Global;


namespace WFSkia;
public static partial class Global
{
    public static bool SvgToPng(string svgPath, int sizex = 0, int sizey = 0)
    {
        return SvgToPng(svgPath, SKColors.Transparent, sizex, sizey);
    }
    public static bool SvgToPng(string svgPath, SKColor background, int sizex = 0, int sizey = 0)
    {
        if (!File.Exists(svgPath)) return false;
        if (!svgPath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)) return false;
        using (var svg = new SKSvg())
        {
            if (svg == null) return false;
            using (var pic = svg.Load(svgPath))
            {
                if (pic == null) return false;
                string pngPath = svgPath.Replace(".svg", ".png");
                if (sizex < 1)
                {
                    sizex = (int)pic.CullRect.Width;
                }
                if (sizey < 1)
                {
                    sizey = (int)pic.CullRect.Height;
                }
                float scalex = (float)sizex / pic.CullRect.Width;
                float scaley = (float)sizey / pic.CullRect.Height;
                if (svg.Save(pngPath, background, SKEncodedImageFormat.Png, 100, scalex, scaley))
                    return true;
                return false;
            }
        }
    }
    public static bool SKImageToJpeg(SKImage img, string path, int quality = 90)
    {
        if (!path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) && !path.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) return false;
        var dirPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(dirPath)) return false;
        try
        {
            using (var data = img.Encode(SKEncodedImageFormat.Jpeg, quality))
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    data.SaveTo(fs);
                }
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public static ImageTexture ImageTextureFromFile(string path, int sizex = 0, int sizey = 0)
    {
        if (path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            return ImageTextureFromSvg(path, sizex, sizey);
        if (!File.Exists(path)) return null;
        using (SKBitmap bm = SKBitmap.Decode(path))
        {
            if (bm == null) return null;
            if (sizex < 1 || sizey < 1)
            {
                sizex = bm.Width;
                sizey = bm.Height;
            }
            float scalex = (float)sizex / (float)bm.Width;
            float scaley = (float)sizey / (float)bm.Height;
            using (var img = Image.CreateFromData(sizex, sizey, false, Image.Format.Rgba8, bm.Bytes))
            {
                if (img == null) return null;
                ImageTexture it = ImageTexture.CreateFromImage(img);
                return it;
            }
        }
    }
    public static ImageTexture ImageTextureFromSvg(string path, int sizex = 0, int sizey = 0)
    {
        if (!File.Exists(path)) return null;
        if (!path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)) return null;
        using (var svg = new SKSvg())
        {
            var pic = svg.Load(path);
            if (sizex == 0 || sizey == 0)
            {
                sizex = (int)pic.CullRect.Width;
                sizey = (int)pic.CullRect.Height;
            }
            if (pic != null)
            {
                float scalex = (float)sizex / pic.CullRect.Width;
                float scaley = (float)sizey / pic.CullRect.Height;
                SKColor bg = SKColors.Transparent;
                var cs = SKColorSpace.CreateSrgb();
                var src = pic.ToBitmap(bg, scalex, scaley, SKColorType.Rgba8888, SKAlphaType.Opaque, cs);
                var img = Image.CreateFromData(sizex, sizey, false, Image.Format.Rgba8, src.Bytes);
                ImageTexture it = ImageTexture.CreateFromImage(img);
                return it;
            }
        }
        return null;
    }
    /// <summary>
    /// Calculatates and assigns a value to "paint.TextSize" so that "text" will have the "desiredWidth" 
    /// if "text" is drawn with the "paint" structure.
    /// </summary>
    /// <param name="paint">The SKPaint structure to assign the adjusted TextSize to.</param>
    /// <param name="text">The text that needs to be drawn.</param>
    /// <param name="desiredWidth">The desired width of the text when drawn.</param>
    /// <returns>The height needed for "text" if "text" is drawn with the adjusted paint struct.</returns>
    public static float AdjustTextSizeForWidth(SKPaint paint, string text, float desiredWidth)
    {
        SKRect rect;
        float saveTextSize = paint.TextSize;
        rect = new SKRect();
        paint.MeasureText(text, ref rect);
        var dif = desiredWidth / rect.Width;
        paint.TextSize = saveTextSize * dif;
        paint.MeasureText(text, ref rect);
        //Height seems to be the most useful value returned from MeasureText
        //It was not obvious to me how the other values are meant to be used
        return rect.Height;
    }
    /// <summary>
    /// Calculatates and assigns a value to "paint.TextSize" so that "text" will have the "desiredHeight" 
    /// if "text" is drawn with the "paint" structure.
    /// </summary>
    /// <param name="paint">The SKPaint structure to assign the adjusted TextSize to.</param>
    /// <param name="text">The text that needs to be drawn.</param>
    /// <param name="desiredHeight">The desired height of the text when drawn.</param>
    /// <returns>The width needed for "text" if "text" is drawn with the adjusted paint struct.</returns>
    public static float AdjustTextSizeForHeight(SKPaint paint, string text, float desiredHeight)
    {
        SKRect rect;
        float saveTextSize = paint.TextSize;
        rect = new SKRect();
        paint.MeasureText(text, ref rect);
        var dif = desiredHeight / rect.Height;
        paint.TextSize = saveTextSize * dif;
        paint.MeasureText(text, ref rect);
        //Width seems to be the most useful value returned from MeasureText
        //It was not obvious to me how the other values are meant to be used.
        return rect.Width;
    }
}