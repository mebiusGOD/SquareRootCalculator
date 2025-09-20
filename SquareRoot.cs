using System;
using System.Collections.Generic;
using System.Linq;
// using System.Numerics;
// using System.Security;
using System.Text;
using System.Threading.Tasks;
// using BigFloatLibrary;
using Microsoft.CodeAnalysis.CSharp.Scripting;


internal sealed class SquareRoot
{
	public static async Task<string> CalculateRoot(string input, int precision)
	{
		try
		{
			input = input.Replace(" ", string.Empty);
			if (string.IsNullOrEmpty(input))
			{
				return "Input cannot be empty";
			}
			try
			{
				if (IsValidNumber(input))
				{
					double number = double.Parse(input);
					// number = double.SetPrecisionWithRound(number, precision);
					string sqrt = CalculateDoubleSquareRoot(number, precision);
					return sqrt;
				}
				else if (IsValidExpression(input))
				{
					var sqrt = CalculateAnaliticsSquareRoot(input, precision);
					return sqrt.Result;
				}
				else return "Bad string";
			}
			catch (Exception ex)
			{
				return $"Unexpected error: {ex.Message}";
			}
		}
		catch (Exception ex)
		{
			return $"Critical error: {ex.Message}";
		}
	}
	private static bool IsValidNumber(string input)
	{
		HashSet<char> usedChars = new HashSet<char>();
		foreach (char c in input)
		{
			if (!char.IsDigit(c))
				if ((c == '.' || c == 'e' || c == '-') && !usedChars.Contains(c))
				{
					usedChars.Add(c);
				}
				else
				{
					return false;
				}
		}
		return true;
	}
	private static bool IsValidExpression(string input)
	{
		HashSet<char> allowedChars = new HashSet<char> { '.', '-', '*', '/', '+', 'e', 'x', '(', ')', '^', 's', 'i', 'n', 'c', 'o', ' ' };
		foreach (char c in input)
		{
			if (!(char.IsDigit(c) || allowedChars.Contains(c)))
				return false;
		}
		return input.Contains("os ") ? false : true;
	}
	private static string CalculateDoubleSquareRoot(double number, int precision)
	{
		string output = "";
		if (number > 0)
		{
			double squareRoot = Math.Sqrt(number);
			// double squareRoot = double.Sqrt(number,prec);
			if (squareRoot.ToString().Contains('.'))
			{
				output = squareRoot.ToString($"F{precision}");
			}
			else
			{
				output = squareRoot.ToString();
			}
		}
		else if (number == 0)
		{
			output = "0";
		}
		else if (number == -1)
		{
			output = "i";
		}
		else
		{
			double squareRoot = Math.Sqrt(Math.Abs(number));
			// double squareRoot = double.Sqrt(double.Abs(number),prec);
			string parsedNumber = "";
			if (squareRoot.ToString().Contains('.'))
			{
				parsedNumber = squareRoot.ToString($"F{precision}");
			}
			else
			{
				parsedNumber = squareRoot.ToString();
			}
			output = parsedNumber + " * i\n";
			output += "-" + parsedNumber + " * i\n";
		}
		return output;
	}
	private static async Task<string> CalculateAnaliticsSquareRoot(string expression, int prec)
	{
		StringBuilder builder = new StringBuilder(expression);
		builder.Replace("**", "^");
		builder.Replace("+x", "+1*x");
		builder.Replace("(x", "(1*x");
		for (int i = 0; i < 10; i++)
		{
			builder.Replace($"{i}x", $"{i}*x");
		}
		expression = builder.ToString();
		Console.WriteLine(expression);
		if (expression[0] != '(')
		{
			expression = "1*" + expression;
		}
		if (expression.EndsWith("x^2"))
		{
			int charPos = expression.IndexOf("x");
			string subString = expression.Substring(0, charPos - 1);
			return double.Sqrt(await CSharpScript.EvaluateAsync<double>(subString)).ToString() + "*x";
		}
		else if (expression == "1*cos(1*x)^2")
		{
			return "cos(x)";
		}
		else if (expression == "1*sin(1*x)^2")
		{
			return "sin(x)";
		}
		else if (expression == "1*1-cos(1*x)^2" || expression == "(1*1-cos(1*x)^2)")
		{
			return "sin(x)^2";
		}
		else if (expression == "1*1-sin(1*x)^2" || expression == "(1*1-sin(1*x)^2)")
		{
			return "cos(x)^2";
		}
		else if (expression.EndsWith("^2") && IsValidNumber(expression.Substring(0, expression.IndexOf("^"))))
		{
			return Math.Sqrt(double.Parse(expression.Substring(0, expression.IndexOf("^")))).ToString();
		}
		else if (expression[0] == '(' && expression.EndsWith(")^2") && expression.Count(f => f == '(') == 1 && expression.Count(f => f == ')') == 1)
		{
			// return subString,charPos);;
			int charPos = expression.IndexOf("(");
			int charPosX = expression.IndexOf("x");
			string subString = expression.Substring(charPosX);
			string firstCoef = expression.Substring(charPos + 1, charPosX - charPos - 1);
			charPos = subString.IndexOf(")");
			subString = subString.Substring(0, charPos);
			return firstCoef + subString;
		}
		else
		{
			string[] subStrings = expression.Split('+');
			string[] sampleSubStr = expression.Replace('-', '+').Split('+');
			if (sampleSubStr.Length == 3)
			{
				double coeficcient = 0;
				double firstCoef = 0;
				double thirdCoef = 0;
				try
				{
					string subString = sampleSubStr[1];
					int charPos = subString.IndexOf("x");
					coeficcient = await CSharpScript.EvaluateAsync<double>(subString.Substring(0, charPos - 1)) / 2;

					subString = sampleSubStr[0];
					charPos = subString.IndexOf("x");
					firstCoef = await CSharpScript.EvaluateAsync<double>(subString.Substring(0, charPos - 1));

					subString = sampleSubStr[2];
					thirdCoef = await CSharpScript.EvaluateAsync<double>(subString);
				}
				catch (Exception ex)
				{
					return $"coefficient find error: {ex.Message}";
				}
				if ((double.Sqrt(firstCoef) * double.Sqrt(thirdCoef)) == coeficcient)
				{
					if (subStrings.Length == sampleSubStr.Length)
					{
						return $"{double.Sqrt(firstCoef)}*x+{double.Sqrt(thirdCoef)}";
					}
					else
					{
						return $"{double.Sqrt(firstCoef)}*x-{double.Sqrt(thirdCoef)}";
					}
				}
				else
				{
					return "(" + expression + ")^(1/2)";
				}
			}
			else
			{
				return "(" + expression + ")^(1/2)";
			}
		}
	}
}
