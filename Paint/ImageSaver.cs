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

        //созд изобрж в размер холста
        //Pbgra32 - 32-битный формат 
        RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
            width, height, 96d, 96d, PixelFormats.Pbgra32);
        
        //временный DrawingVisual для рендеринга содержимого канваса
        DrawingVisual visual = new DrawingVisual();
        using (DrawingContext dc = visual.RenderOpen())  //юз гарант закрытия после 
        {
            // Рисуем этой кистью прямоугольник размером с холст, заполняя его содержимым
            VisualBrush brush = new VisualBrush(canvas);
            dc.DrawRectangle(brush, null, new Rect(0, 0, width, height)); //у прямоуг нет обводки
        }

        //перенос нарисованного с временной в раст изобрж
        renderBitmap.Render(visual);
        
        BitmapEncoder encoder;
        string extension = Path.GetExtension(filePath).ToLower(); //пнг ПНГ

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

        //кодируем растр изображение в выбранный формат и сохраняем в файл
        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

        //отк файл для записи, если не существует создаем
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            encoder.Save(fileStream);
        }
    }
}
