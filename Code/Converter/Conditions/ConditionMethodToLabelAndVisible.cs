// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.ConditionMethodToLabelAndVisible
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StorecfgGenerator
{
  public class ConditionMethodToLabelAndVisible : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is string str1)
      {
        if (targetType == typeof (string))
        {
          switch (str1)
          {
            case "PING":
            case "HTTP":
            case "HTTPS":
              return (object) "IP OR DOMAIN";
            case "PROCESS":
              return (object) "PROCESS NAME";
            case "DNS":
              return (object) "DOMAIN";
          }
        }
        else
        {
          string str = str1;
          return str == "HTTP" || str == "HTTPS" ? (object) Visibility.Visible : (object) Visibility.Collapsed;
        }
      }
      return Binding.DoNothing;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
