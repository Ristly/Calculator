using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculator.Logic;

public class ProccessEval : IProccessEval
{
    public int Timer { get; set; } = 0;

    private readonly DataTable _evalService;
    private readonly char[] _highPriorityOperations;
    private readonly char[] _lowPriorityOperations;
    private const string ErrorMessage = "Некорректное выражение";
    public ProccessEval()
    {
        _evalService = new DataTable();
        _highPriorityOperations = new char[] { '*', '/' };
        _lowPriorityOperations = new char[] { '+', '-' };
    }
    public string Eval(string expression)
    {

        var timer = Task.Delay(Timer * 1000);

        var res = CalculateFromString(expression);
        Task.WaitAll(timer);

        return res;
    }


    private string CalculateFromString(string expression)
    {

        if (float.TryParse(expression, out var _))
            return expression;

        string firstNumber = string.Empty;
        string secondNumber = string.Empty;

        var highOperationIndex = expression.IndexOf(expression.Skip(1).FirstOrDefault(x => _highPriorityOperations.Any(y => y == x)), 1);

        if (highOperationIndex == 0 || highOperationIndex == expression.Length - 1)
            return ErrorMessage;

        if (highOperationIndex != -1)
        {

            firstNumber = GetLeft(expression, highOperationIndex);
            secondNumber = GetRight(expression, highOperationIndex);

            try
            {
                var first = Convert.ToSingle(firstNumber.ToString());
                var second = Convert.ToSingle(secondNumber.ToString());
                var res = Calulate(first, second, expression[highOperationIndex]);

                expression = expression.Replace(firstNumber + expression[highOperationIndex].ToString() + secondNumber, res);
            }
            catch (DivideByZeroException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        else
        {

            var lowOperationIndex = expression.IndexOf(expression.Skip(1).FirstOrDefault(x => _lowPriorityOperations.Any(y => y == x)),1);

            if (lowOperationIndex == 0 || lowOperationIndex == expression.Length - 1)
                return ErrorMessage;

            firstNumber = GetLeft(expression, lowOperationIndex);
            secondNumber = GetRight(expression, lowOperationIndex);

            if(firstNumber == ErrorMessage || secondNumber == ErrorMessage)
                return ErrorMessage;

            try
            {
                var first = Convert.ToSingle(firstNumber.ToString());
                var second = Convert.ToSingle(secondNumber.ToString());
                var res = Calulate(first, second, expression[lowOperationIndex]);
                expression = expression.Replace(firstNumber + expression[lowOperationIndex].ToString() + secondNumber, res);
            }
            catch (DivideByZeroException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        return CalculateFromString(expression);
    }

    private string GetLeft(string expression, int ind)
    {
        StringBuilder number = new StringBuilder();
        bool hasComma = false;
        for (var i = ind - 1; i >= 0; i--)
        {
            if (char.IsDigit(expression[i]))
            {
                number.Insert(0, expression[i]);

            }
            else
            {
                if (number.Length == 0)
                    return ErrorMessage;

                if (expression[i] == ',')
                {
                    if (hasComma)
                        return ErrorMessage;
                    else
                    {
                        number.Insert(0, expression[i]);
                        hasComma = true;
                        continue;
                    }
                }
                else if (expression[i] == '-' && i == 0)
                {
                    number.Insert(0, expression[i]);
                    continue;
                }
                break;
            }

        }
        return number.ToString();
    }

    private string GetRight(string expression, int ind)
    {
        StringBuilder number = new StringBuilder();
        bool hasComma = false;
        for (var i = ind + 1; i < expression.Length; i++)
        {
            if (char.IsDigit(expression[i]))
            {
                number.Append(expression[i]);
            }
            else
            {
                if (number.Length == 0)
                    return ErrorMessage;

                if (expression[i] == ',')
                {
                    if (hasComma)
                        return ErrorMessage;
                    else
                    {
                        number.Append(expression[i]);
                        hasComma = true;
                        continue;
                    }
                }
                break;
            }

        }
        return number.ToString();
    }

    private string Calulate(float firstNumber, float secondNumber, char operation)
    => operation switch
    {
        '+' => (firstNumber + secondNumber).ToString(),
        '-' => (firstNumber - secondNumber).ToString(),
        '*' => (firstNumber * secondNumber).ToString(),
        '/' => secondNumber == 0 ? throw new DivideByZeroException("Нельзя делить на ноль") : (firstNumber / secondNumber).ToString()
    };
}
