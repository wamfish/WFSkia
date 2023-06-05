//  Copyright (C) 2023 - Present John Roscoe Hamilton - All Rights Reserved
//  You may use, distribute and modify this code under the terms of the MIT license.
//  See the file License.txt in the root folder for full license details.

namespace WFSkia;
public static class ExampleImageTexture
{
    public static ImageTexture Create(int sizex = 0, int sizey = 0)
    {
        if (sizex == 0) sizex = 128;
        if (sizey == 0) sizey = 128;
        using var bm = new SKBitmap(sizex, sizey);
        using var Canvas = new SKCanvas(bm);
        //Canvas.Translate(0, Bitmap.Height); //run first, this moves the image up so when we flip it, the image flips back in place.
        //Canvas.Scale(1, -1); //run after translate. This flips the image.
        DrawStuff(Canvas);
        using var img = Image.CreateFromData(bm.Width, bm.Height, false, Image.Format.Rgba8, bm.Bytes);
        ImageTexture it = ImageTexture.CreateFromImage(img);
        return it;
    }
    public static SKBitmap CreateBitmap(int sizex = 0, int sizey = 0)
    {
        if (sizex == 0) sizex = 128;
        if (sizey == 0) sizey = 128;

        var bm = new SKBitmap(sizex, sizey);
        using var Canvas = new SKCanvas(bm);
        //Canvas.Translate(0, Bitmap.Height); //run first, this moves the image up so when we flip it, the image flips back in place.
        //Canvas.Scale(1, -1); //run after translate. This flips the image.
        DrawStuff(Canvas);
        return bm;
    }
    public static void DrawStuff(SKCanvas c)
    {
        SKTypeface TypeFace;
        if (!Asset.GetFullPath("Fonts/LuckiestGuy.ttf", out string fullPath)) return;
        TypeFace = SKTypeface.FromFile(fullPath, 0);

        var bgc = SKColors.Beige;
        var fgc = SKColors.Blue;
        c.Clear(bgc);

        using var paint = new SKPaint();
        paint.TextSize = 30;
        paint.IsAntialias = true;
        paint.Color = fgc;
        paint.StrokeCap = SKStrokeCap.Round;
        paint.Typeface = TypeFace;

        float tw = paint.MeasureText("SKIA");
        float w = c.LocalClipBounds.Width;
        w = w - 20;
        while (tw * 3 > w)
        {
            paint.TextSize = paint.TextSize - 1;
            tw = paint.MeasureText("SKIA");
        }
        tw = tw * 3;
        tw += 10;
        float x0 = 10;
        float x1 = 10 + (tw / 2);
        float x2 = 10 + tw;
        float ypos = 30;
        paint.Color = new SKColor(0x42, 0x81, 0xA4);
        paint.IsStroke = false;
        paint.TextAlign = SKTextAlign.Left;
        c.DrawText("Skia", x0, ypos, paint);

        paint.Color = new SKColor(0x9C, 0xAF, 0xB7);
        paint.IsStroke = true;
        paint.StrokeWidth = 3;
        paint.TextAlign = SKTextAlign.Center;
        c.DrawText("Skia", x1, ypos, paint);

        paint.IsAntialias = true;
        paint.Color = new SKColor(0xE6, 0xB8, 0x9C);
        paint.TextAlign = SKTextAlign.Right;
        c.DrawText("Skia", x2, ypos, paint);
        DrawButton(c, paint);
    }
    private static void DrawButton(SKCanvas c, SKPaint paint)
    {
        int w = c.DeviceClipBounds.Width;
        int h = c.DeviceClipBounds.Height;
        //Log.Message($"ClipBounds: {c.DeviceClipBounds.ToString()}");
        float top = h - 100;
        float left = 20;
        float tw = w - 40;
        float th = 64;
        float ts = 20;

        paint.TextSize = ts;
        var tb = new SKRect(left, top, left + tw, top + th);
        var tb2 = new SKRect();
        string str = "Hello World!";
        paint.IsAntialias = true;
        paint.IsStroke = false;
        paint.Color = SKColors.Chocolate;
        paint.TextAlign = SKTextAlign.Center;
        //tb2 returns width and height, not sure what top and left are
        paint.MeasureText(str, ref tb2);
        //to make text fit the width we need to change TextSize
        var dif = tw / tb2.Width;
        paint.TextSize = ts * dif;
        paint.MeasureText(str, ref tb2);
        th = tb2.Height;
        top = h - (th + 12);
        float ypos = (top + th) - 1;
        float xpos = left + (tw / 2);

        SKRect frameRect = new SKRect(left, top, left + tw, top + th);
        frameRect.Inflate(10, 10);
        using SKPaint framePaint = new SKPaint();
        framePaint.Style = SKPaintStyle.Fill;
        framePaint.StrokeWidth = 3;
        framePaint.Color = SKColors.Blue;
        framePaint.IsAntialias = true;
        c.DrawRoundRect(frameRect, 10, 10, framePaint);

        framePaint.Color = SKColors.Black;
        framePaint.Style = SKPaintStyle.Stroke;
        c.DrawRoundRect(frameRect, 10, 10, framePaint);

        c.DrawText(str, xpos, ypos, paint);
    }
}
