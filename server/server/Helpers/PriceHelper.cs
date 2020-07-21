using System;

namespace Server.Helpers
{
  public static class PriceHelper
  {
    public static decimal IntToDecimal(int price)
    {
      return (decimal)price / 100;
    }

    public static int StrToInt(string price)
    {
      return Convert.ToInt32(Convert.ToDouble(price) * 100);
    }
  }
}
