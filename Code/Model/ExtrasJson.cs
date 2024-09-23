// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.ExtrasJson
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace StorecfgGenerator
{
  public class ExtrasJson
  {
    private string _dnsString;

    public int timeout { get; set; } = 8000;

    [JsonIgnore]
    public int ConditionTimeout
    {
      get => this.timeout / 1000;
      set
      {
        this.timeout = value * 1000;
        this.OnPropertyChanged(nameof (ConditionTimeout));
      }
    }

    public int tries { get; set; }

    public int interval { get; set; }

    [JsonIgnore]
    public string dnsString
    {
      get
      {
        string str = "";
        if (this.dns != null)
        {
          foreach (string dn in this.dns)
            str = str + dn + "|";
        }
        this._dnsString = str;
        return this._dnsString;
      }
      set
      {
        this._dnsString = value;
        if (this.dns == null)
          this.dns = new List<string>();
        this.dns.Clear();
        string dnsString = this._dnsString;
        char[] chArray = new char[1]{ '|' };
        foreach (string str in dnsString.Split(chArray))
        {
          if (str.Trim() != "")
            this.dns.Add(str);
        }
      }
    }

    public List<string> dns { get; set; }

    public bool flowThrough { get; set; } = false;

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
