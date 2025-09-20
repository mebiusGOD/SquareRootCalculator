using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using static SquareRoot;
using System.Threading.Tasks;

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

        private async void CalculateButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // string resultText = "Результат: ";
            if (int.TryParse(this.FindControl<TextBox>("PrecisionTextBox").Text, out int precision) && precision >= 0)
            {
                string output = await SquareRoot.CalculateRoot(this.FindControl<TextBox>("InputTextBox").Text, precision);
                this.FindControl<TextBlock>("ResultTextBlock").Text = $"Результат: {output}";
            }
            else
            {
                string output = await SquareRoot.CalculateRoot(this.FindControl<TextBox>("InputTextBox").Text, 16);
                this.FindControl<TextBlock>("ResultTextBlock").Text = $"Результат: {output}";
            }
        
            

            // if (double.TryParse(this.FindControl<TextBox>("InputTextBox").Text, out double number))
            // {
            //     if (number >= 0)
            //     {
            //         double sqrt = Math.Sqrt(number);
            //         string resultText = "Результат: ";

            //         // ������ ��������
            //         if (int.TryParse(this.FindControl<TextBox>("PrecisionTextBox").Text, out int precision) && precision >= 0)
            //         {
            //             // ����������� � ��������� ��������� (F ��� fixed-point)
            //             resultText += sqrt.ToString($"F{precision}");
            //         }
            //         else
            //         {
            //             // �� ��������� ��� ��������������
            //             resultText += sqrt.ToString();
            //         }

            //         this.FindControl<TextBlock>("ResultTextBlock").Text = resultText;
            //     }
            //     else
            //     {
            //         this.FindControl<TextBlock>("ResultTextBlock").Text = "������: ����� ������ ���� ���������������";
            //     }
            // }
            // else
            // {
            //     this.FindControl<TextBlock>("ResultTextBlock").Text = "������: ������� �����";
            // }
        }
    }
}