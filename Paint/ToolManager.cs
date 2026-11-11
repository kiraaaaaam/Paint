namespace Paint;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Paint.Tools;

public class ToolManager
{
    private Tool _currentTool;
    private Color _currentColor = Colors.Black;
    private double _currentThickness = 2.0;

    public ToolManager()
    {
        _currentTool = new PencilTool();
        ApplySettingsToTool(_currentTool);
    }

    public Tool CurrentTool
    {
        get => _currentTool;
        set
        {
            _currentTool = value;
            ApplySettingsToTool(_currentTool);
        }
    }

    public Color CurrentColor
    {
        get => _currentColor;
        set
        {
            _currentColor = value;
            if (_currentTool != null)
            {
                _currentTool.Color = _currentColor;
            }
        }
    }

    public double CurrentThickness
    {
        get => _currentThickness;
        set
        {
            _currentThickness = value;
            if (_currentTool != null)
            {
                _currentTool.Thickness = _currentThickness;
            }
        }
    }

    private void ApplySettingsToTool(Tool tool) // применяем настройки к текущему инструменту
    {
        if (tool != null)
        {
            tool.Color = _currentColor;
            tool.Thickness = _currentThickness;
        }
    }

    public void OnMouseDown(Point point, Canvas canvas)
    {
        _currentTool.OnMouseDown(point, canvas);
    }

    public void OnMouseMove(Point point, Canvas canvas)
    {
        _currentTool.OnMouseMove(point, canvas);
    }

    public void OnMouseUp(Point point, Canvas canvas)
    {
        _currentTool.OnMouseUp(point, canvas);
    }
}
