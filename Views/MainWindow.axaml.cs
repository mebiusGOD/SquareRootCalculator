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
                    string resultText = "���������: ";

                    // ������ ��������
                    if (int.TryParse(this.FindControl<TextBox>("PrecisionTextBox").Text, out int precision) && precision >= 0)
                    {
                        // ����������� � ��������� ��������� (F ��� fixed-point)
                        resultText += sqrt.ToString($"F{precision}");
                    }
                    else
                    {
                        // �� ��������� ��� ��������������
                        resultText += sqrt.ToString();
                    }

                    this.FindControl<TextBlock>("ResultTextBlock").Text = resultText;
                }
                else
                {
                    this.FindControl<TextBlock>("ResultTextBlock").Text = "������: ����� ������ ���� ���������������";
                }
            }
            else
            {
                this.FindControl<TextBlock>("ResultTextBlock").Text = "������: ������� �����";
            }
        }
    }
}