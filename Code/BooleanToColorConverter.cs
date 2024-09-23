// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.BooleanToColorConverter
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StorecfgGenerator
{
  public class BooleanToColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value is bool flag && targetType == typeof (Brush) ? (flag ? (object) new SolidColorBrush(Colors.Yellow) : (object) new SolidColorBrush(Colors.White)) : Binding.DoNothing;
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
