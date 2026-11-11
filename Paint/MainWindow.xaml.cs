namespace Paint;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using Paint.Tools;

public partial class MainWindow : Window
{
    private readonly ToolManager _toolManager;

    private readonly ImageSaver _imageSaver;

    public MainWindow()
    {
        InitializeComponent();

        _toolManager = new ToolManager();
        _imageSaver = new ImageSaver();

        UpdateColorDisplay();
        UpdateThicknessDisplay();
    }

    private void ToolRadioButton_Checked(object sender, RoutedEventArgs e) 
    {
        if (_toolManager == null) return;

        if (sender is RadioButton rb && rb.Tag is string toolType)
        {
            switch (toolType)
            {
                case "Pencil":
                    _toolManager.CurrentTool = new PencilTool();
                    if (StatusToolText != null) StatusToolText.Text = "Инструмент: Карандаш";
                    break;
                case "Line":
                    _toolManager.CurrentTool = new LineTool();
                    if (StatusToolText != null) StatusToolText.Text = "Инструмент: Линия";
                    break;
                case "Rectangle":
                    _toolManager.CurrentTool = new RectangleTool();
                    if (StatusToolText != null) StatusToolText.Text = "Инструмент: Прямоугольник";
                    break;
                case "Ellipse":
                    _toolManager.CurrentTool = new EllipseTool();
                    if (StatusToolText != null) StatusToolText.Text = "Инструмент: Эллипс";
                    break;
            }
        }
    }

    private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DrawingCanvas.CaptureMouse();
            Point point = e.GetPosition(DrawingCanvas);
            _toolManager.OnMouseDown(point, DrawingCanvas);
        }
    }

    private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        Point point = e.GetPosition(DrawingCanvas);

        double clampedX = Math.Clamp(Math.Round(point.X), 0, DrawingCanvas.ActualWidth);
        double clampedY = Math.Clamp(Math.Round(point.Y), 0, DrawingCanvas.ActualHeight);
        StatusCoordsText.Text = $"X: {clampedX}, Y: {clampedY}";

        if (DrawingCanvas.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
        {
            _toolManager.OnMouseMove(point, DrawingCanvas);
        }
    }

    private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (DrawingCanvas.IsMouseCaptured)
        {
            Point point = e.GetPosition(DrawingCanvas);
            _toolManager.OnMouseUp(point, DrawingCanvas);
            DrawingCanvas.ReleaseMouseCapture();
        }
    }

    private void DrawingCanvas_LostMouseCapture(object sender, MouseEventArgs e)
    {
        Point point = Mouse.GetPosition(DrawingCanvas);
        _toolManager.OnMouseUp(point, DrawingCanvas);
    }

    private void QuickColor_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string hex)
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(hex);
                _toolManager.CurrentColor = color;
                UpdateColorDisplay();
            }
            catch
            {
            }
        }
    }

    private void UpdateColorDisplay()
    {
        if (CurrentColorBorder != null)
        {
            CurrentColorBorder.Background = new SolidColorBrush(_toolManager.CurrentColor);
        }
        if (ThicknessPreviewLine != null)
        {
            ThicknessPreviewLine.Stroke = new SolidColorBrush(_toolManager.CurrentColor);
        }
    }
    private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_toolManager == null) return;

        double thickness = Math.Round(ThicknessSlider.Value);
        _toolManager.CurrentThickness = thickness;
        UpdateThicknessDisplay();
    }

    private void UpdateThicknessDisplay()
    {
        if (ThicknessText != null)
        {
            ThicknessText.Text = $"{(int)_toolManager.CurrentThickness} пкс";
        }
        if (ThicknessPreviewLine != null)
        {
            ThicknessPreviewLine.StrokeThickness = _toolManager.CurrentThickness;
        }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Вы уверены, что хотите полностью очистить холст?",
            "Очистить холст",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            DrawingCanvas.Children.Clear();
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Title = "Сохранить изображение",
            Filter = "PNG файлы (*.png)|*.png|JPEG файлы (*.jpg;*.jpeg)|*.jpg;*.jpeg|BMP файлы (*.bmp)|*.bmp",
            DefaultExt = ".png",
            FileName = "Рисунок"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                _imageSaver.SaveToFile(DrawingCanvas, saveFileDialog.FileName);

                MessageBox.Show(
                    "Изображение успешно сохранено!",
                    "Сохранение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось сохранить изображение: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

}
