// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.ConditionMethodToShowTimeoutVisibility
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StorecfgGenerator
{
  public class ConditionMethodToShowTimeoutVisibility : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is string str))
        return Binding.DoNothing;
      return targetType == typeof (Visibility) ? (str == "PROCESS" ? (object) Visibility.Collapsed : (object) Visibility.Visible) : (str == "WAIT" ? (object) "WAIT TIME" : (object) "TIMEOUT");
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
