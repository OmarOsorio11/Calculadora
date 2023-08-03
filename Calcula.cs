using System;
using System.Collections.Generic;

namespace Calculadora
{
    /// <summary>
    /// A static class that provides methods to evaluate mathematical expressions using postfix notation.
    /// </summary>
    public static class Calcula
    {
        /// <summary>
        /// Evaluates a mathematical expression in postfix notation and returns the result as a string.
        /// </summary>
        /// <param name="expresion">The mathematical expression to be evaluated.</param>
        /// <returns>The result of the expression evaluation as a string.</returns>
        public static string EvaluarExpresion(string expresion)
        {
            try
            {
                // Convert the expression to a list of tokens and store it in a queue.
                Queue<string> cola = new Queue<string>(ExpresionALista(expresion));
                // Stacks to hold numbers and operators during evaluation.
                Stack<double> pilaNumeros = new Stack<double>();
                Stack<string> pilaOperadores = new Stack<string>();
                int aux = 0;
                string Mensaje = "";

                // Process each token in the queue.
                while (cola.Count > 0)
                {
                    string token = cola.Dequeue();
                    double numero;

                    // Special handling for the first token in the expression.
                    if (aux == 0)
                    {
                        if (EsOperador(token))
                        {
                            switch (token)
                            {
                                case "-":
                                    Mensaje = "COMIENZA CON MENOS";
                                    continue;
                                case "+":
                                    continue;
                                case "*":
                                    throw new Exception("No se puede comenzar con *");
                                case "/":
                                    throw new Exception("No se puede comenzar con /");
                                default:
                                    break;
                            }
                        }
                        aux = 1;
                    }

                    // If the token is a number, push it to the numbers stack.
                    if (double.TryParse(token, out numero))
                    {
                        if (Mensaje == "COMIENZA CON MENOS")
                        {
                            Mensaje = "";
                            pilaNumeros.Push(-numero);
                        }
                        else
                        {
                            pilaNumeros.Push(numero);
                        }
                    }
                    // If the token is an operator, process it.
                    else if (EsOperador(token))
                    {
                        // Apply operators with higher precedence first.
                        while (pilaOperadores.Count > 0 && Precedencia(token) <= Precedencia(pilaOperadores.Peek()))
                        {
                            double resultadoParcial = AplicarOperacion(pilaNumeros.Pop(), pilaOperadores.Pop(), pilaNumeros.Pop());
                            pilaNumeros.Push(resultadoParcial);
                        }
                        // Push the current operator onto the operators stack.
                        pilaOperadores.Push(token);
                    }
                    // Invalid token (neither number nor operator).
                    else
                    {
                        throw new ArgumentException("Carácter inválido en la expresión: " + token);
                    }
                }

                // After processing all tokens, apply any remaining operators in the stack.
                while (pilaOperadores.Count > 0)
                {
                    double resultadoParcial = AplicarOperacion(pilaNumeros.Pop(), pilaOperadores.Pop(), pilaNumeros.Pop());
                    pilaNumeros.Push(resultadoParcial);
                }

                // The final result will be the remaining number in the numbers stack.
                return pilaNumeros.Pop().ToString();
            }
            catch (DivideByZeroException ex)
            {
                return "Math Error";
            }
            catch (Exception ex)
            {
                return "Syntax Error";
            }
        }

        // Converts the expression into a list of tokens (numbers and operators).
        static List<string> ExpresionALista(string expresion)
        {
            List<string> lista = new List<string>();
            string numeroActual = "";
            string operadorActual = "";

            foreach (char c in expresion)
            {
                // If the character is a digit or a decimal point, add it to the current number token.
                if (char.IsDigit(c) || c == '.')
                {
                    if (!string.IsNullOrEmpty(operadorActual))
                    {
                        lista.Add(operadorActual);
                        operadorActual = "";
                    }
                    numeroActual += c;
                }
                // If the character is an operator, add the current number token (if any) and then add the operator token.
                else
                {
                    if (!string.IsNullOrEmpty(numeroActual))
                    {
                        lista.Add(numeroActual);
                        numeroActual = "";
                    }

                    if (!char.IsWhiteSpace(c))
                    {
                        operadorActual += c;
                    }
                    else
                    {
                        lista.Add(operadorActual);
                    }
                }
            }

            if (!string.IsNullOrEmpty(numeroActual))
            {
                lista.Add(numeroActual);
            }

            return lista;
        }

        // Checks if the token is an operator.
        static bool EsOperador(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "*+" || token == "*-" || token == "/+" || token == "/-";
        }

        // Assigns precedence values to operators for the evaluation order.
        static int Precedencia(string operador)
        {
            switch (operador)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                case "*-":
                case "*+":
                case "/-":
                case "/+":
                    return 2;
                default:
                    return 0;
            }
        }

        // Performs the arithmetic operations based on the operator and two operands.
        static double AplicarOperacion(double b, string operador, double a)
        {
            switch (operador)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    if (b == 0)
                        throw new DivideByZeroException("No se puede dividir entre cero.");
                    return a / b;

                case "*-":
                    return a * -b; ;
                case "*+":
                    return a * b; ;
                case "/-":
                    if (-b == 0)
                        throw new DivideByZeroException("No se puede dividir entre cero.");
                    return a / -b;
                case "/+":
                    if (b == 0)
                        throw new DivideByZeroException("No se puede dividir entre cero.");
                    return a / b;
                default:
                    throw new ArgumentException("Operador inválido: " + operador);
            }
        }
    }
}
