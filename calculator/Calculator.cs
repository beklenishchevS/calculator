using System;
using System.Collections.Generic;
using System.Text;

namespace calculator
{
    public class Calculator
    {
        private static readonly Dictionary<string, int> priority = new Dictionary<string, int>()
        {
            {"*", 2 },
            {"/", 2 },
            {"+", 1 },
            {"-", 1 },
            {"(", 0 }
        };

        private static readonly Dictionary<string, Func<double, double, double>> operators = new Dictionary<string, Func<double, double, double>>()
        {
            {"*", (a, b) => a * b },
            {"/", (a, b) => a / b },
            {"+", (a, b) => a + b },
            {"-", (a, b) => a - b }
        };

        public static string Calculate(string expression)
        {
            var token = GetTokens(expression);
            var polishTokens = GetPolishArray(token);
            var result = GetResult(polishTokens);
            return result;
        }

        private static List<String> GetTokens(string expression)
        {
            var tokens = new List<String>();
            var cell = new StringBuilder();
            foreach (var symbol in expression)
            {
                if (char.IsDigit(symbol))
                {
                    cell.Append(symbol);
                    continue;
                }
                if (cell.Length != 0)
                {
                    tokens.Add(cell.ToString());
                }
                cell = new StringBuilder();
                tokens.Add(symbol.ToString());
            }
            if (cell.Length != 0)
            {
                tokens.Add(cell.ToString());
            }
            var avaibleTokens = new HashSet<string>(){ "*", "/", "+", "-", "(", ")" };
            foreach (var element in tokens)
            {
                if (!IsDigit(element) && !avaibleTokens.Contains(element))
                {
                    throw new InvalidOperationException("Unexpected Symbol");
                }
            }
            return tokens;
        }

        private static List<String> GetPolishArray(List<string> tokens)
        {
            var stack = new Stack<string>();

            var output = new List<string>();

            foreach (var symbol in tokens)
            {
                if (IsDigit(symbol))
                {
                    output.Add(symbol);
                    continue;
                }
                
                if ("(" == symbol)
                {
                    stack.Push(symbol);
                    continue;
                }
                if (")" == symbol)
                {
                    var last = stack.Peek();
                    while (last != "(")
                    {
                        output.Add(last);
                        stack.Pop();
                        last = stack.Peek();
                    }
                    stack.Pop();
                    continue;
                }
                if (stack.Count == 0)
                {
                    stack.Push(symbol);
                    continue;
                }
                var lastSymbol = stack.Peek();
                if (priority[lastSymbol] < priority[symbol])
                {
                    stack.Push(symbol);
                    continue;
                }
                while (priority[lastSymbol] >= priority[symbol])
                {
                    output.Add(lastSymbol);
                    stack.Pop();
                    if (stack.Count != 0)
                    {
                        lastSymbol = stack.Peek();
                    } else
                    {
                        break;
                    }
                }
                stack.Push(symbol);
            }
            while (stack.Count != 0)
            {
                output.Add(stack.Pop());
            }

            return output;
        }

        private static bool IsDigit(string str)
        {
            foreach (var e in str)
            {
                if (!char.IsDigit(e))
                {
                    return false;
                }
            }
            return true;
        }

        private static string GetResult(List<string> polishNotation)
        {
            var stack = new Stack<string>();
            foreach (var symbol in polishNotation)
            {

                if (IsDigit(symbol))
                {
                    stack.Push(symbol);
                    continue;
                }
                var val1 = stack.Pop();
                var val2 = stack.Pop();
                var result = CalculateStringsAsDigit(val1, val2, symbol);
                stack.Push(result.ToString());
            }
            return stack.Pop();
        }

        private static double CalculateStringsAsDigit(string val1, string val2, string operation)
        {
            var digit2 = Double.Parse(val1);
            var digit1 = Double.Parse(val2);
            return operators[operation](digit1, digit2);
        }

    }
}
