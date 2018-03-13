using System;
using XStore.Common;

/// <summary>
/// Utils 扩展
/// </summary>
public static class Utils_Ex
{
    //Object
    public static DateTime ObjToDateTime(this object str)
    {
        return Utils.ObjectToDateTime(str, DateTime.MinValue);
    }
    public static string ObjToStr(this object str)
    {
        return Utils.ObjectToStr(str);
    }
    public static decimal ObjToDecimal(this object str, decimal defValue)
    {
        return Utils.ObjToDecimal(str, defValue);
    }
    public static float ObjToFloat(this object str, float defValue)
    {
        return Utils.ObjToFloat(str, defValue);
    }
    public static int ObjToInt(this object str, int defValue)
    {
        return Utils.ObjToInt(str, defValue);
    }
  
    public static string[] ObjToArray(this object str, string strSplit, int count)
    {
        return Utils.SplitString(Utils.ObjectToStr(str), strSplit, count);
    }
    //String
    public static int StrToInt(this string str, int defValue)
    {
        return Utils.StrToInt(str, defValue);
    }
    public static DateTime StrToDateTime(this string str, DateTime defValue)
    {
        return Utils.StrToDateTime(str, defValue);
    }
    public static decimal StrToDecimal(this string str, decimal defValue)
    {
        return Utils.StrToDecimal(str, defValue);
    }
    public static float StrToFloat(this string str, float defValue)
    {
        return Utils.StrToFloat(str, defValue);
    }
    public static string[] StrToArray(this string str, string strSplit, int count)
    {
        return Utils.SplitString(str, strSplit, count);
    }
    public static Decimal CentToRMB(this int cent, decimal defValue) {
        return cent.ObjToDecimal(defValue)/100;
    }
}