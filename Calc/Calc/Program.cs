using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Calc
{
    class Program
    {
        private static void Main()
        {
            Calculator calculator = new Calculator();
            EquationParser Expr = new EquationParser();
            Console.Write("Please enter an expression (Type 'end' to terminate):");

            bool doLoop = true;
            bool isFirstLoop = true;

            do
            {
                if (!isFirstLoop)
                {
                    Console.Write(calculator.Result);
                }

                string UserExp = Console.ReadLine();
                if (UserExp == "end")
                {
                    doLoop = false;
                }
                else
                {
                    ParseResult parsedInput = Expr.Parse(UserExp);

                    long firstArgumentForOutput = (isFirstLoop ? parsedInput.FirstArgument : calculator.Result);

                    calculator.Process(parsedInput);

                    Console.WriteLine("\n{0}\t{1}\t{2}\t= {3}", firstArgumentForOutput, parsedInput.Operator, parsedInput.SecondArgument, calculator.Result);
                }

                isFirstLoop = false;
            }
            while (doLoop);
            Console.Read();
        }
    }

    public class Calculator
    {
        private bool _isInitialized;

        public long Result { get; private set; }

        public void Process(ParseResult parsedInput)
        {
            if (!this._isInitialized)
            {
                this.Result = parsedInput.FirstArgument;

                this._isInitialized = true;
            }

            switch (parsedInput.Operator)
            {
                case "+":
                    this.Result += parsedInput.SecondArgument;
                    break;

                case "-":
                    this.Result -= parsedInput.SecondArgument;
                    break;

                case "*":
                    this.Result *= parsedInput.SecondArgument;
                    break;

                case "/":
                    this.Result /= parsedInput.SecondArgument;
                    break;

                case "%":
                    this.Result %= parsedInput.SecondArgument;
                    break;

                default:
                    break;
            }
        }
    }

    public class ParseResult
    {
        public long FirstArgument { get; set; }

        public string Operator { get; set; }

        public long SecondArgument { get; set; }
    }

    public class EquationParser
    {
        public ParseResult Parse(string input)
        {
            long firstArgument = 0;
            Match firstMatch = Regex.Match(input, "^\\d+");
            if (firstMatch.Success)
            {
                firstArgument = Convert.ToInt64(firstMatch.Value);
            }

            string operatorString = Regex.Match(input, "\\D+").Value;

            Match secondMatch = Regex.Match(input, "\\d+$");
            long secondArgument = Convert.ToInt64(secondMatch.Value);

            return new ParseResult()
            {
                FirstArgument = firstArgument,
                Operator = operatorString,
                SecondArgument = secondArgument
            };
        }
    }
}
