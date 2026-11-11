namespace Paint.Tools;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public abstract class Tool 
{
    private Color _color = Colors.Black;
    private double _thickness = 2.0;

    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    public double Thickness
    {
        get => _thickness;
        set => _thickness = value;
    }

    protected Point ClampPoint(Point point, Canvas canvas)
    {
        double x = Math.Clamp(point.X, 0, Math.Max(0, canvas.ActualWidth));
        double y = Math.Clamp(point.Y, 0, Math.Max(0, canvas.ActualHeight));
        return new Point(x, y);
    }

    public abstract void OnMouseDown(Point point, Canvas canvas);
    public abstract void OnMouseMove(Point point, Canvas canvas);
    public abstract void OnMouseUp(Point point, Canvas canvas);
}
