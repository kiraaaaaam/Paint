namespace Paint.Tools;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class EllipseTool : Tool
{
    private Point _startPoint;
    private Ellipse? _ellipse;

    public override void OnMouseDown(Point point, Canvas canvas)
    {
        _startPoint = ClampPoint(point, canvas);

        _ellipse = new Ellipse
        {
            Stroke = new SolidColorBrush(Color),
            StrokeThickness = Thickness
        };

        Canvas.SetLeft(_ellipse, _startPoint.X);
        Canvas.SetTop(_ellipse, _startPoint.Y);
        _ellipse.Width = 0;
        _ellipse.Height = 0;

        canvas.Children.Add(_ellipse);
    }

    public override void OnMouseMove(Point point, Canvas canvas)
    {
        if (_ellipse != null)
        {
            Point clamped = ClampPoint(point, canvas);

            double left = Math.Min(_startPoint.X, clamped.X);
            double top = Math.Min(_startPoint.Y, clamped.Y);
            double width = Math.Abs(clamped.X - _startPoint.X);
            double height = Math.Abs(clamped.Y - _startPoint.Y);

            Canvas.SetLeft(_ellipse, left);
            Canvas.SetTop(_ellipse, top);
            _ellipse.Width = width;
            _ellipse.Height = height;
        }
    }

    public override void OnMouseUp(Point point, Canvas canvas)
    {
        if (_ellipse != null)
        {
            Point clamped = ClampPoint(point, canvas);

            double left = Math.Min(_startPoint.X, clamped.X);
            double top = Math.Min(_startPoint.Y, clamped.Y);
            double width = Math.Abs(clamped.X - _startPoint.X);
            double height = Math.Abs(clamped.Y - _startPoint.Y);

            if (width < 1 && height < 1)
            {
                canvas.Children.Remove(_ellipse);
            }
            else
            {
                Canvas.SetLeft(_ellipse, left);
                Canvas.SetTop(_ellipse, top);
                _ellipse.Width = width;
                _ellipse.Height = height;
            }

            _ellipse = null;
        }
    }
}
