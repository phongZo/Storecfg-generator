// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.RuleJson
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StorecfgGenerator
{
  public class RuleJson
  {
    private string _data;
    private ObservableCollection<string> _items;
    private string _method;

    public string data
    {
      get => this._data;
      set
      {
        this._data = value;
        this.OnPropertyChanged(nameof (data));
      }
    }

    public ExtrasJson extras { get; set; }

    [JsonIgnore]
    public ObservableCollection<string> Items => this._items;

    public string method
    {
      get => this._method;
      set
      {
        this._method = value;
        this.OnPropertyChanged(nameof (method));
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

    [JsonIgnore]
    public string Domain
    {
      get
      {
        try
        {
          if (this.method == "HTTP" || this.method == "HTTPS")
            return new Uri(this.data).Host;
        }
        catch (Exception ex)
        {
        }
        return "";
      }
      set
      {
        string str = "http://";
        if (this.method == "HTTPS")
          str = "https://";
        this.data = str + value + ":" + this.Port;
      }
    }

    [JsonIgnore]
    public string Port
    {
      get
      {
        try
        {
          if (this.method == "HTTP" || this.method == "HTTPS")
            return new Uri(this.data).Port.ToString();
        }
        catch (Exception ex)
        {
        }
        return "";
      }
      set
      {
        string str = "http://";
        if (this.method == "HTTPS")
          str = "https://";
        this.data = str + this.Domain + ":" + value;
      }
    }

    public RuleJson()
    {
      ObservableCollection<string> observableCollection = new ObservableCollection<string>();
      observableCollection.Add("PING");
      observableCollection.Add("HTTP");
      observableCollection.Add("HTTPS");
      observableCollection.Add("DNS");
      observableCollection.Add("PROCESS");
      observableCollection.Add("WAIT");
      this._items = observableCollection;
      this._method = "PING";
    }
  }
}
