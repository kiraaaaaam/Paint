namespace Paint.Tools;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class LineTool : Tool
{
    private Point _startPoint;
    private Line? _line;

    public override void OnMouseDown(Point point, Canvas canvas)
    {
        _startPoint = ClampPoint(point, canvas);

        _line = new Line
        {
            X1 = _startPoint.X,
            Y1 = _startPoint.Y,
            X2 = _startPoint.X,
            Y2 = _startPoint.Y,
            Stroke = new SolidColorBrush(Color),
            StrokeThickness = Thickness,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round
        };

        canvas.Children.Add(_line);
    }

    public override void OnMouseMove(Point point, Canvas canvas)
    {
        if (_line != null)
        {
            Point clamped = ClampPoint(point, canvas);
            _line.X2 = clamped.X;
            _line.Y2 = clamped.Y;
        }
    }

    public override void OnMouseUp(Point point, Canvas canvas)
    {
        if (_line != null)
        {
            Point clamped = ClampPoint(point, canvas);
            _line.X2 = clamped.X;
            _line.Y2 = clamped.Y;
            _line = null;
        }
    }
}
