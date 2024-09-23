﻿// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.BooleanToVisibilityConverter
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StorecfgGenerator
{
  public class BooleanToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value is bool flag ? (object) (Visibility) (flag ? 0 : 2) : (object) Visibility.Collapsed;
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
