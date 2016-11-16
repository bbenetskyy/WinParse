using DataParser.Enums;
using FormulasCollection.Models;

namespace FormulasCollection.Helpers
{
    public class SportsConverterTypes
    {
        public static string TypeParseAll(string typeEvent, SportType st)
        {
            string typeEventTrim = typeEvent.Trim();
            /*  if (typeEventTrim[0].Equals('F'))
              {
              string val = null;
              try
              {
                  val = typeEventTrim.Split('(', ')')[1].ToString();
              }
              catch(Exception ex)
              {

                  val = null;
              }
              if (val == null) return null;
                  return typeEventTrim[1].Equals('1')
                  ? "F2(" + (val[0].Equals('-') ? val.Substring(1) : "-" + val.Substring(1)) + ")"
                  : "F1(" + (val[0].Equals('-') ? val.Substring(1) : "-" + val.Substring(1)) + ")";
              }
              if (typeEventTrim[0].Equals('T'))
              {
              string val = null;
              try
              {
                  val = typeEventTrim.Split('(', ')')[1].ToString();
              }
              catch(Exception ex)
              {
                  val = null;
              }
              if (val == null) return null;
                  return typeEventTrim[1].Equals('U')
                  ? "TO(" + val + ")"
                  : "TU(" + val + ")";
              }*/
            string typeCoefForPinnacle = string.Empty;
            string number = string.Empty;
            bool isTotal = false;
            bool isFora = false;
            if (typeEvent.Contains("TU"))
            {
                typeEventTrim = "TU";
                isTotal = true;
            }
            else if (typeEvent.Contains("TO"))
            {
                typeEventTrim = "TO";
                isTotal = true;
            }
            bool totalNum = typeEvent.Contains("(") && typeEvent.Contains(")");
            if (totalNum)
            {
                number = typeEvent.Split('(', ')')[1];
            }

            if (typeEvent.Contains("F1"))
            {
                typeEventTrim = "F1";
                isFora = true;
            }
            else if (typeEvent.Contains("F2"))
            {
                typeEventTrim = "F2";
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

            if (SportTypes.TypeCoefsSoccer.ContainsKey(typeEventTrim) && st == SportType.Soccer)
            {
                if (isTotal || isFora)
                {
                    return SportTypes.TypeCoefsBasketBall[typeEventTrim] + "(" + number + ")";
                }
                else
                    return SportTypes.TypeCoefsSoccer[typeEventTrim];
            }
            if (SportTypes.TypeCoefsTennis.ContainsKey(typeEventTrim) && st == SportType.Tennis)
            {
                if (isTotal || isFora)
                {
                    return SportTypes.TypeCoefsBasketBall[typeEventTrim] + "(" + number + ")";
                }
                else
                    return SportTypes.TypeCoefsTennis[typeEventTrim];
            }
            if (SportTypes.TypeCoefsBasketBall.ContainsKey(typeEventTrim) && st == SportType.Basketball)
            {
                if (isTotal || isFora)
                {
                    return SportTypes.TypeCoefsBasketBall[typeEventTrim] + "(" + number + ")";
                }
                else
                    return SportTypes.TypeCoefsBasketBall[typeEventTrim];
            }
            if (SportTypes.TypeCoefsHockey.ContainsKey(typeEventTrim) && st == SportType.Hockey)
            {
                if (isTotal || isFora)
                {
                    return SportTypes.TypeCoefsBasketBall[typeEventTrim] + "(" + number + ")";
                }
                else
                    return SportTypes.TypeCoefsHockey[typeEventTrim];
            }
            if (SportTypes.TypeCoefsVolleyBall.ContainsKey(typeEventTrim) && st == SportType.Volleyball)
            {
                if (isTotal || isFora)
                {
                    return SportTypes.TypeCoefsBasketBall[typeEventTrim] + "(" + number + ")";
                }
                else
                    return SportTypes.TypeCoefsVolleyBall[typeEventTrim];
            }
            return string.Empty;
        }
    }
}
