namespace Paint;

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class ImageSaver
{
    public void SaveToFile(Canvas canvas, string filePath)
    {
        int width = (int)canvas.ActualWidth;
        int height = (int)canvas.ActualHeight;

        if (width <= 0) width = (int)canvas.Width;
        if (height <= 0) height = (int)canvas.Height;

        if (width <= 0 || height <= 0)
        {
            throw new InvalidOperationException("Размер холста должен быть больше нуля.");
        }

        RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
            width, height, 96d, 96d, PixelFormats.Pbgra32);
        
        DrawingVisual visual = new DrawingVisual();
        using (DrawingContext dc = visual.RenderOpen())
        {
            VisualBrush brush = new VisualBrush(canvas);
            dc.DrawRectangle(brush, null, new Rect(0, 0, width, height));
        }

        renderBitmap.Render(visual);

        BitmapEncoder encoder;
        string extension = Path.GetExtension(filePath).ToLower();

        switch (extension)
        {
            case ".jpg":
            case ".jpeg":
                encoder = new JpegBitmapEncoder();
                break;
            case ".bmp":
                encoder = new BmpBitmapEncoder();
                break;
            case ".png":
            default:
                encoder = new PngBitmapEncoder();
                break;
        }

        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            encoder.Save(fileStream);
        }
    }
}
