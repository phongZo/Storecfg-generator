// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.MethodToProtocolConverter
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace StorecfgGenerator
{
  public class MethodToProtocolConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is string && value is string str)
      {
        switch (str)
        {
          case "HTTP":
            return (object) "http://";
          case "HTTPS":
            return (object) "https://";
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
