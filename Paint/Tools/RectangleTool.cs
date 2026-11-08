namespace Paint.Tools;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class RectangleTool : Tool
{
    private Point _startPoint;
    private Rectangle? _rectangle;

    public override void OnMouseDown(Point point, Canvas canvas)
    {
        _startPoint = ClampPoint(point, canvas);

        _rectangle = new Rectangle
        {
            Stroke = new SolidColorBrush(Color),
            StrokeThickness = Thickness
        };

        Canvas.SetLeft(_rectangle, _startPoint.X);
        Canvas.SetTop(_rectangle, _startPoint.Y);
        _rectangle.Width = 0;
        _rectangle.Height = 0;

        canvas.Children.Add(_rectangle);
    }

    public override void OnMouseMove(Point point, Canvas canvas)
    {
        if (_rectangle != null)
        {
            Point clamped = ClampPoint(point, canvas);

            double left = Math.Min(_startPoint.X, clamped.X);
            double top = Math.Min(_startPoint.Y, clamped.Y);
            double width = Math.Abs(clamped.X - _startPoint.X);
            double height = Math.Abs(clamped.Y - _startPoint.Y);

            Canvas.SetLeft(_rectangle, left);
            Canvas.SetTop(_rectangle, top);
            _rectangle.Width = width;
            _rectangle.Height = height;
        }
    }

    public override void OnMouseUp(Point point, Canvas canvas)
    {
        if (_rectangle != null)
        {
            Point clamped = ClampPoint(point, canvas);

            double left = Math.Min(_startPoint.X, clamped.X);
            double top = Math.Min(_startPoint.Y, clamped.Y);
            double width = Math.Abs(clamped.X - _startPoint.X);
            double height = Math.Abs(clamped.Y - _startPoint.Y);

            if (width < 1 && height < 1)
            {
                canvas.Children.Remove(_rectangle);
            }
            else
            {
                Canvas.SetLeft(_rectangle, left);
                Canvas.SetTop(_rectangle, top);
                _rectangle.Width = width;
                _rectangle.Height = height;
            }

            _rectangle = null;
        }
    }
}
