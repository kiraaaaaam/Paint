namespace Paint.Tools;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class PencilTool : Tool
{
    private Polyline? _polyline;

    public override void OnMouseDown(Point point, Canvas canvas)
    {
        Point clamped = ClampPoint(point, canvas);

        _polyline = new Polyline
        {
            Stroke = new SolidColorBrush(Color),
            StrokeThickness = Thickness,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round,
            StrokeLineJoin = PenLineJoin.Round
        };

        _polyline.Points.Add(clamped);
        canvas.Children.Add(_polyline);
    }

    public override void OnMouseMove(Point point, Canvas canvas)
    {
        if (_polyline != null)
        {
            Point clamped = ClampPoint(point, canvas);
            _polyline.Points.Add(clamped);
        }
    }

    public override void OnMouseUp(Point point, Canvas canvas)
    {
        if (_polyline != null)
        {
            Point clamped = ClampPoint(point, canvas);
            if (_polyline.Points.Count == 1)
            {
                _polyline.Points.Add(new Point(clamped.X + 0.1, clamped.Y + 0.1));
            }
            else
            {
                _polyline.Points.Add(clamped);
            }

            _polyline = null;
        }
    }
}
