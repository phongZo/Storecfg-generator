// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.ArrayHelper
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace StorecfgGenerator
{
  public class ArrayHelper
  {
    public static readonly DependencyProperty ArrayProperty = DependencyProperty.RegisterAttached("Array", typeof (Array), typeof (ArrayHelper), new PropertyMetadata((object) null, new PropertyChangedCallback(ArrayHelper.OnArrayChanged)));
    public static readonly DependencyProperty FlattenedArrayProperty = DependencyProperty.RegisterAttached("FlattenedArray", typeof (IEnumerable), typeof (ArrayHelper));

    public static Array GetArray(DependencyObject obj)
    {
      return (Array) obj.GetValue(ArrayHelper.ArrayProperty);
    }

    public static void SetArray(DependencyObject obj, Array value)
    {
      obj.SetValue(ArrayHelper.ArrayProperty, (object) value);
    }

    private static void OnArrayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(e.NewValue is Array newValue))
        return;
      ObservableCollection<LogoCell> observableCollection = ArrayHelper.FlattenArray(newValue);
      ArrayHelper.SetFlattenedArray(d, (IEnumerable) observableCollection);
    }

    private static ObservableCollection<LogoCell> FlattenArray(Array array)
    {
      ObservableCollection<LogoCell> observableCollection = new ObservableCollection<LogoCell>();
      foreach (object obj in array)
        observableCollection.Add(new LogoCell()
        {
          Value = (bool) obj
        });
      return observableCollection;
    }

    public static IEnumerable GetFlattenedArray(DependencyObject obj)
    {
      return (IEnumerable) obj.GetValue(ArrayHelper.FlattenedArrayProperty);
    }

    public static void SetFlattenedArray(DependencyObject obj, IEnumerable value)
    {
      obj.SetValue(ArrayHelper.FlattenedArrayProperty, (object) value);
    }
  }
}
