using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSC.Number
{
    [Serializable]
    public class NumberElement
    {
        public NumberElement(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        [field: SerializeField] public int Numerator { get; private set; } = 1;
        [field: SerializeField] public int Denominator { get; private set; } = 1;

        public int IntegerPart => Numerator / Denominator;
        public int FractionPart => Numerator % Denominator;

        public override string ToString()
        {
            if (FractionPart == 0)
            {
                return IntegerPart.ToString();
            }
            else if (IntegerPart == 0)
            {
                return $"{FractionPart}/{Denominator}";
            }
            else
            {
                return $"{IntegerPart}({FractionPart}/{Denominator})";
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Numerator, Denominator);
        }

        public override bool Equals(object obj)
        {
            return obj is NumberElement element &&
                   Numerator == element.Numerator &&
                   Denominator == element.Denominator;
        }

        public static NumberElement Calculate(Operator op, NumberElement operand1, NumberElement operand2)
        {
            if (operand1 == null || op == Operator.None) return operand2;
            if (operand2 == null) return operand1;

            switch (op)
            {
                case Operator.Sum:
                    return Sum(operand1, operand2);
                case Operator.Subtract:
                    return Subtract(operand1, operand2);
                case Operator.Multiply:
                    return Multiply(operand1, operand2);
                case Operator.Divide:
                    return Divide(operand1, operand2);
                default:
                    return operand2;
            }
        }

        private static NumberElement Sum(NumberElement operand1, NumberElement operand2)
        {
            int denGCD = GCD(Mathf.Abs(operand1.Denominator), Mathf.Abs(operand2.Denominator));

            int num = operand1.Numerator * operand2.Denominator + operand2.Numerator * operand1.Denominator;
            num /= denGCD;
            int den = operand1.Denominator * operand2.Denominator / denGCD;

            return new NumberElement(num, den);
        }

        private static NumberElement Subtract(NumberElement operand1, NumberElement operand2)
        {
            int denGCD = GCD(Mathf.Abs(operand1.Denominator), Mathf.Abs(operand2.Denominator));

            int num = operand1.Numerator * operand2.Denominator - operand2.Numerator * operand1.Denominator;
            num /= denGCD;
            int den = operand1.Denominator * operand2.Denominator / denGCD;

            return new NumberElement(num, den);
        }

        private static NumberElement Multiply(NumberElement operand1, NumberElement operand2)
        {
            int num = operand1.Numerator * operand2.Numerator;
            int den = operand1.Denominator * operand2.Denominator;
            // invalid
            if (num == 0 && den == 0)
            {
                return new NumberElement(0, 0);
            }
            if (den < 0)
            {
                den = -den;
                num = -num;
            }

            int gcd = GCD(Mathf.Abs(num), Mathf.Abs(den));
            Debug.Log((int)(Mathf.Sign(num) * Mathf.Sign(den)));
            return new NumberElement(num / gcd, den / gcd);
        }

        private static NumberElement Divide(NumberElement operand1, NumberElement operand2)
        {
            int num = operand1.Numerator * operand2.Denominator;
            int den = operand1.Denominator * operand2.Numerator;
            // invalid
            if (num == 0 && den == 0)
            {
                return new NumberElement(0, 0);
            }
            if (den < 0)
            {
                den = -den;
                num = -num;
            }

            int gcd = GCD(Mathf.Abs(num), Mathf.Abs(den));
            return new NumberElement((int)(Mathf.Sign(num) * Mathf.Sign(den)) * num / gcd, den / gcd);
        }

        private static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }
    }
}