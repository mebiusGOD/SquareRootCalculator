using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace SquareRootCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CalculateButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (double.TryParse(this.FindControl<TextBox>("InputTextBox").Text, out double number))
            {
                if (number >= 0)
                {
                    double sqrt = Math.Sqrt(number);
                    string resultText = "Результат: ";

                    // Парсим точность
                    if (int.TryParse(this.FindControl<TextBox>("PrecisionTextBox").Text, out int precision) && precision >= 0)
                    {
                        // Форматируем с указанной точностью (F для fixed-point)
                        resultText += sqrt.ToString($"F{precision}");
                    }
                    else
                    {
                        // По умолчанию без форматирования
                        resultText += sqrt.ToString();
                    }

                    this.FindControl<TextBlock>("ResultTextBlock").Text = resultText;
                }
                else
                {
                    this.FindControl<TextBlock>("ResultTextBlock").Text = "Ошибка: число должно быть неотрицательным";
                }
            }
            else
            {
                this.FindControl<TextBlock>("ResultTextBlock").Text = "Ошибка: введите число";
            }
        }
    }
}