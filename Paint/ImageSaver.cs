namespace Paint;

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class ImageSaver
{
    // Преобразует содержимое канвас и сохраняет его в файл
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

        //Буфер для итогового изображения
        //Pbgra32 - 32-битный формат 
        RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
            width, height, 96d, 96d, PixelFormats.Pbgra32);
        
        //содержимое в отдельный битмап, не добавляя временные элементы в интерфейс
        DrawingVisual visual = new DrawingVisual();
        using (DrawingContext dc = visual.RenderOpen())
        {
            // Рисуем этой кистью прямоугольник размером с холст, заполняя его содержимым
            VisualBrush brush = new VisualBrush(canvas);
            dc.DrawRectangle(brush, null, new Rect(0, 0, width, height));
        }

        // рендер DrawingVisual в растровый буфер
        renderBitmap.Render(visual);

        // Кодировщик преобразует битмап в формат файла
        // Расширение сравнивается без учёта регистра благодаря ToLower()
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

        // Кодировщик принимает кадры; для этого изображения добавляем один кадр.
        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

        // файлмод кр создаёт файл или перезаписывает сущ-й
        // using гарантирует закрытие потока даже в случае ошибки при сохранении
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            encoder.Save(fileStream);
        }
    }
}
