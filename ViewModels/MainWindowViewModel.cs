using System;
using System.Reactive;
using ReactiveUI;

namespace SquareRootCalculator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _inputNumber;
        private string _result;

        public string InputNumber
        {
            get => _inputNumber;
            set => this.RaiseAndSetIfChanged(ref _inputNumber, value);
        }

        public string Result
        {
            get => _result;
            set => this.RaiseAndSetIfChanged(ref _result, value);
        }

        public ReactiveCommand<Unit, Unit> CalculateCommand { get; }

        public MainWindowViewModel()
        {
            CalculateCommand = ReactiveCommand.Create(Calculate);
        }

        private void Calculate()
        {
        
                if (double.TryParse(InputNumber, out double number) && number >= 0)
                {
                    double sqrt = Math.Sqrt(number);
                    Result = $"Квадратный корень: {sqrt}";
                }
                else
                {
                    Result = "Ошибка: Введите положительное число.";
                }
         
        }
    }
}