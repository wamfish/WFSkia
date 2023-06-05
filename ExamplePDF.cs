//  Copyright (C) 2023 - Present John Roscoe Hamilton - All Rights Reserved
//  You may use, distribute and modify this code under the terms of the MIT license.
//  See the file License.txt in the root folder for full license details.

namespace WFSkia;
public static class ExamplePDF
{
    //ToDo: Drawing Images/Bitmaps is broken
    //Use PdfSharp for now. Would be nice to use Skia. Track if fix is ever applied.
    public static void Create()
    {
        var metadata = new SKDocumentPdfMetadata
        {
            Author = "Cool Developer",
            Creation = DateTime.Now,
            Creator = "Cool Developer Library",
            Keywords = "SkiaSharp, Sample, PDF, Developer, Library",
            Modified = DateTime.Now,
            Producer = "SkiaSharp",
            Subject = "SkiaSharp Sample PDF",
            Title = "Sample PDF",
        };
        var pdfPath = Path.Combine(Directories.UserData, "Documents");
        if (!Directory.Exists(pdfPath)) Directory.CreateDirectory(pdfPath);
        pdfPath = Path.Combine(pdfPath, "my.pdf");
        //metadata.RasterDpi = 400;
        metadata.EncodingQuality = 101;
        using var document = SKDocument.CreatePdf(pdfPath, metadata);

        if (document == null)
            throw new WamfishException();


        using var paint = new SKPaint
        {
            TextSize = 64.0f,
            IsAntialias = true,
            Color = 0xFF9CAFB7,
            IsStroke = true,
            StrokeWidth = 3,
            TextAlign = SKTextAlign.Center
        };

        var pageWidth = 72 * 8.5f;
        var pageHeight = 72 * 11.0f;

        // draw page 1
        using (var pdfCanvas = document.BeginPage(pageWidth, pageHeight))
        {
            pdfCanvas.Clear(SKColors.Transparent);
            // draw button
            using var nextPagePaint = new SKPaint
            {
                IsAntialias = true,
                TextSize = 16,
                Color = SKColors.OrangeRed
            };
            ExampleImageTexture.DrawStuff(pdfCanvas);
            var nextText = "Next Page >>";
            var btn = new SKRect(pageWidth - nextPagePaint.MeasureText(nextText) - 24, 0, pageWidth, nextPagePaint.TextSize + 24);
            pdfCanvas.DrawText(nextText, btn.Left + 12, btn.Bottom - 12, nextPagePaint);
            // make button link
            pdfCanvas.DrawLinkDestinationAnnotation(btn, "next-page");
            ExampleImageTexture.DrawStuff(pdfCanvas);


            // draw contents
            // pdfCanvas.DrawText("...PDF 1/2...", pageWidth / 2, pageHeight / 4, paint);

            document.EndPage();
        }

        // draw page 2
        using (var pdfCanvas = document.BeginPage(pageWidth, pageHeight))
        {
            // draw link destintion
            var r = new SKRect(10, 10, 256 + 10, 256 + 10);
            pdfCanvas.DrawUrlAnnotation(r, "https://www.bing.com");
            pdfCanvas.DrawNamedDestinationAnnotation(SKPoint.Empty, "next-page");
            var bm = ExampleImageTexture.CreateBitmap(256);
            pdfCanvas.DrawBitmap(bm, r);
            pdfCanvas.DrawText("...PDF 2/2...", pageWidth / 2, pageHeight / 4, paint);
            document.EndPage();
        }

        // end the doc
        document.Close();
    }

}
