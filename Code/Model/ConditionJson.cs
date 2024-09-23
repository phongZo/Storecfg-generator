// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.ConditionJson
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System.Collections.Generic;
using System.ComponentModel;

namespace StorecfgGenerator
{
  public class ConditionJson
  {
    private string _marker;

    public string name { get; set; }

    public string marker
    {
      get => this._marker;
      set
      {
        if (!(this._marker != value))
          return;
        this._marker = !(value.Trim() == "") ? value : StoreCfg.Instance.CurrentStoreCfg.MarkerListName[0];
        this.OnPropertyChanged(nameof (marker));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public List<RuleJson> rules { get; set; } = new List<RuleJson>();
  }
}
