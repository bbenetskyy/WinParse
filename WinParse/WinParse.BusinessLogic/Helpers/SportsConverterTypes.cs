using FormulasCollection.Models;
using System.Collections.Generic;
using System.Linq;
using FormulasCollection.Enums;
using ToolsPortable;

namespace FormulasCollection.Helpers
{
    public static class SportsConverterTypes
    {
        public static List<string> TypeParseAll(string typeEvent, SportType st)
        {
            string typeEventTrim = typeEvent.Trim();

            string typeCoefForPinnacle = string.Empty;
            string number = string.Empty;
            bool isTotal = false;
            bool isFora = false;
            if (typeEvent.Contains("TU"))
            {
                typeEventTrim = typeEvent.Split('(')[0];
                isTotal = true;
            }
            else if (typeEvent.Contains("TO"))
            {
                typeEventTrim = typeEvent.Split('(')[0];
                isTotal = true;
            }
            bool totalNum = typeEvent.Contains("(") && typeEvent.Contains(")");
            if (totalNum)
            {
                number = typeEvent.Split('(', ')')[1];
            }

            if (typeEvent.Contains("F1"))
            {
                typeEventTrim = typeEvent.Split('(')[0];
                isFora = true;
            }
            else if (typeEvent.Contains("F2"))
            {
                typeEventTrim = typeEvent.Split('(')[0];
                isFora = true;
            }
            if (isFora)
            {
                if (number[0] == '+')
                {
                    number = "-" + number.Substring(1);
                }
                else if (number[0] == '-')
                {
                    number = "+" + number.Substring(1);
                }
            }

            if (SportTypes.TypeCoefsAll.ContainsKey(typeEventTrim))
            {
                if (isTotal || isFora)
                {
                    return CheckAsiatType(SportTypes.TypeCoefsAll[typeEventTrim] + "(" + number + ")");
                }
                else
                    return CheckAsiatType(SportTypes.TypeCoefsAll[typeEventTrim]);
            }
            return null;
        }

        public static List<string> CheckAsiatType(string type)
        {
            // перевірити азіатскі типи на баскетбол const double delta = 0.25;

            List<string> result = new List<string>();
            result.Add(type);
            /*string znak = string.Empty;

            if (!(_type.Contains("(") && _type.Contains(")")))
                return result;
            string name = _type.Split('(', ')')[0];
            string number = _type.Split('(', ')')[1];
            if (!string.IsNullOrEmpty(number) && number.Contains("+") || number.Contains("-"))
            {
                znak = number[0].ToString();
                number = number.Substring(1);
            }
            if (!string.IsNullOrEmpty(number))
            {
                double num = number.ConvertToDoubleOrNull() ?? 0;
                double num1 = num - delta;
                double num2 = num + delta;

                string val1 = name.Trim() + "(" + (num1 == 0 ? "" : znak) + num1.ToString() + ")";
                string val2 = name.Trim() + "(" + (num1 == 0 ? "" : znak) + num2.ToString() + ")";
                result.Add(val1);
                result.Add(val2);
            }*/
            return result;
        }

        public static string MinimalizeValue(this string typeValue)
        {
            if (typeValue.IsBlank()) return typeValue;
            if (!typeValue.Contains(".")) return typeValue;
            if (typeValue.IndexOf('.') == typeValue.Length) return typeValue;
            if (typeValue[typeValue.IndexOf('.') + 1] == '0')
            {
                var firstpart = typeValue.Remove(typeValue.IndexOf('.'));
                var secondpart = typeValue.Split('0').Last();
                return firstpart + secondpart;
            }
            else
            {
                var firstpart = typeValue.Remove(typeValue.IndexOf('.') + 1);
                var secondpart = typeValue.Split('.').Last().Replace("0", string.Empty);
                return firstpart + secondpart;
            }
        }

        public static string InvertValue(this string typeValue)
        {
            if (typeValue.IsBlank()) return typeValue;

            var intValue = typeValue.ConvertToIntOrNull();
            if (intValue != null && intValue.Value == 0)
                return typeValue;

            return typeValue.StartsWith("-")
                ? typeValue.TrimStart('-')
                : "-" + typeValue;
        }

        public static string LocalizeToMarathon(this string typeValue)
        {
            return typeValue.StartsWith("-")
                ? typeValue
                : "+" + typeValue.TrimStart('-');
        }

        public static string ExtendType(this string typeValue, string extention, int period)
        {
            if (extention.IsBlank()) return typeValue;
            if (period <= 0) return typeValue;

            var brace = typeValue.IndexOf('(');
            if (brace != -1)
            {
                var leftPart = typeValue.Substring(0, brace);
                var rightPart = typeValue.Substring(brace, typeValue.Length - brace);
                return $"{leftPart}{extention}{period}{rightPart}";
            }
            else return $"{typeValue}{extention}{period}";
        }
    }
}