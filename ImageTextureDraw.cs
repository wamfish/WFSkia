//  Copyright (C) 2023 - Present John Roscoe Hamilton - All Rights Reserved
//  You may use, distribute and modify this code under the terms of the MIT license.
//  See the file License.txt in the root folder for full license details.

namespace WFSkia;
public class ImageTextureDraw
{
    public string Text { get; private set; } = string.Empty;
    public SKPaint LinePaint { get; private set; }
    public SKPaint TextPaint { get; private set; }
    public SKPaint BoxPaint { get; private set; }
    public int ImageWidth { get; private set; } = 0;
    public int ImageHeight { get; private set; } = 0;
    public SKColor TextColor { get; set; } = 0;
    public SKColor BackgroundColor { get; set; } = 0;
    public SKColor BorderColor { get; set; } = 0;
    public SKTypeface Typeface { get; set; } = null;
    public int TextHeight { get; private set; } = -1;
    public int TextWidth { get; private set; } = -1;
    public float TextSize { get; private set; } = 0;
    public int TopMargin { get; set; } = 3;
    public int LeftMargin { get; set; } = 5;
    public int RightMargin { get; set; } = 5;
    public int BottomMargin { get; set; } = 3;
    public ImageTextureDraw(string text, string typeface)
    {
        //"Fonts/NotoSans.ttf";
        if (!typeface.StartsWith("Fonts/"))
        {
            typeface = "Fonts/" + typeface;
        }
        if (!Asset.GetFullPath(typeface, out string fullPath))
        {
            Typeface = null;
            return;
        }
        Typeface = SKTypeface.FromFile(fullPath, 0);
        TextColor = SKColors.Black;
        BackgroundColor = SKColors.Green;
        BorderColor = SKColors.Black;

        Text = text;
        ImageWidth = 0;
        ImageHeight = 0;
        LinePaint = new SKPaint();
        TextPaint = new SKPaint();
        BoxPaint = new SKPaint();
        CalcImageSizeByHeight(20); //set some valid defaults
    }
    public void CalcImageSizeByHeight(int textHeight, int leftMargin = 5, int rightMargin = 5, int topMargin = 3, int botMargin = 3)
    {
        LeftMargin = leftMargin;
        RightMargin = rightMargin;
        TopMargin = topMargin;
        BottomMargin = botMargin;
        TextHeight = textHeight;
        float tw = AdjustTextSizeForHeight(TextPaint, Text, TextHeight);
        TextSize = TextPaint.TextSize;
        TextWidth = Mathf.CeilToInt(tw);
        ImageWidth = TextWidth + LeftMargin + RightMargin;
        ImageHeight = TextHeight + TopMargin + BottomMargin;
    }
    public void CalcImageSizeByWidth(int textWidth, int leftMargin = 5, int rightMargin = 5, int topMargin = 3, int botMargin = 3)
    {
        LeftMargin = leftMargin;
        RightMargin = rightMargin;
        TopMargin = topMargin;
        BottomMargin = botMargin;
        TextWidth = textWidth;
        float th = AdjustTextSizeForWidth(TextPaint, Text, TextWidth);
        TextSize = TextPaint.TextSize;
        TextHeight = Mathf.CeilToInt(th);
        ImageWidth = TextWidth + LeftMargin + RightMargin;
        ImageHeight = TextHeight + TopMargin + BottomMargin;
    }
    public void ResetDefaultValues()
    {
        if (Typeface == null) return;
        LinePaint.Reset();
        TextPaint.Reset();
        BoxPaint.Reset();
        SetLinePainter();
        SetTextPainter();
        SetBoxPainter();
        TextPaint.TextSize = TextSize;
    }
    void SetLinePainter()
    {
        var p = LinePaint;
        p.Style = SKPaintStyle.Stroke;
        p.IsAntialias = false;
        p.StrokeWidth = 1;
        p.Color = BorderColor;
        p.StrokeCap = SKStrokeCap.Square; //Must be square or you get a missing pixel at the end
    }
    void SetTextPainter()
    {
        var p = TextPaint;
        p.TextSize = 30;
        p.Style = SKPaintStyle.StrokeAndFill;
        p.StrokeWidth = 1;
        p.IsAntialias = true;
        p.Color = TextColor;
        p.StrokeCap = SKStrokeCap.Butt;
        p.Typeface = Typeface;
        p.TextAlign = SKTextAlign.Center;
    }
    void SetBoxPainter()
    {
        var p = BoxPaint;
        p.Style = SKPaintStyle.Fill;
        p.StrokeWidth = 1;
        p.Color = BackgroundColor;
        p.IsAntialias = false;
    }
    public ImageTexture DrawImage(Action<SKCanvas, ImageTextureDraw> draw, Action<ImageTextureDraw> setValues = null)
    {
        using var bm = new SKBitmap(ImageWidth, ImageHeight);
        using var Canvas = new SKCanvas(bm);
        ResetDefaultValues();
        setValues?.Invoke(this);
        draw(Canvas, this);
        using var img = Image.CreateFromData(bm.Width, bm.Height, false, Image.Format.Rgba8, bm.Bytes);
        ImageTexture it = ImageTexture.CreateFromImage(img);
        return it;
    }
}