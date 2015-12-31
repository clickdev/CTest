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
        static int ExpParse(char[] exp)
        {
            Stack<int> numStack = new Stack<int>();
            Stack<char> opStack = new Stack<char>();

            opStack.Push('(');

            int index = 0;
            while (index < exp.Length)
            {
                if (index == exp.Length || exp[index] == ')')
                {
                    EndParenthesis(numStack, opStack);
                    index++;
                }
                else if (exp[index] >= '0' && exp[index] <= '9')
                {
                    index = ParseNumber(exp, index, numStack);
                }
                else
                {
                    ParseOperator(exp[index], numStack, opStack);
                    index++;
                }
            }

            return numStack.Pop();
        }

        
        
        static void ParseOperator(char op, Stack<int> numStack,
                                         Stack<char> opStack)
        {
            while (opStack.Count > 0 &&
                 OperatorEval(op, opStack.Peek()))
                Evaluate(numStack, opStack);

            opStack.Push(op);
        }

        static int ParseNumber(char[] exp, int index,
                                      Stack<int> numStack)
        {
            int value = 0;
            while (index < exp.Length &&
                    exp[index] >= '0' && exp[index] <= '9')
                value = value + (int)(exp[index++] - '0');

            numStack.Push(value);

            return index;
        }

        static void EndParenthesis(Stack<int> numStack,
                                              Stack<char> opStack)
        {
            while (opStack.Peek() != '(')
                Evaluate(numStack, opStack);

            opStack.Pop();
        }

        static bool OperatorEval(char op, char prevOp)
        {
            bool isOp = false;

            switch (op)
            {
                case '-':
                    isOp = (prevOp != '(');
                    break;
                case '/':
                    isOp = (prevOp == '*' || prevOp == '/');
                    break;
            }

            return isOp;
        }

        static void Evaluate(Stack<int> numStack, Stack<char> opStack)
        {
            int right = numStack.Pop();
            int left = numStack.Pop();
            char op = opStack.Pop();

            int result = 0;
            switch (op)
            {
                case '+':
                    result = left + right;
                    break;
                case '-':
                    result = left - right;
                    break;
                case '*':
                    result = left * right;
                    break;
                case '/':
                    result = left / right;
                    break;
            }

            numStack.Push(result);
        }

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter expression: ");
                string line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                char[] exp = line.ToCharArray();
                int result = ExpParse(exp);
                Console.WriteLine("{0}", result);

            }
        }
    }
}
