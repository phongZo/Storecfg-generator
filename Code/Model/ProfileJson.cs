// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.ProfileJson
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StorecfgGenerator
{
  public class ProfileJson : INotifyPropertyChanged
  {
    private ObservableCollection<KeyValuePair<string, MarkerJson>> _observedMarkers = new ObservableCollection<KeyValuePair<string, MarkerJson>>();
    private ObservableCollection<ConditionJson> _conditions;

    public Dictionary<string, MarkerJson> Markers { get; set; }

    [JsonIgnore]
    public ObservableCollection<KeyValuePair<string, MarkerJson>> ObservedMarkers
    {
      get => this._observedMarkers;
      set
      {
        this._observedMarkers = value;
        this.OnPropertyChanged(nameof (ObservedMarkers));
      }
    }

    public ObservableCollection<ConditionJson> Conditions
    {
      get => this._conditions;
      set
      {
        if (this._conditions == value)
          return;
        this._conditions = value;
        this.OnPropertyChanged(nameof (Conditions));
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

    public List<string> ServerSettings { get; set; } = new List<string>(2);

    public bool EncryptStoreFile { get; set; } = false;

    public bool EncryptionLockEnforced { get; set; } = false;

    public bool ShowSystemTray { get; set; } = false;

    public bool FastMode { get; set; } = false;

    public int FastRefreshDuration { get; set; } = 600000;

    public int FastRefreshInterval { get; set; } = 5000;

    public int SlowRefreshInterval { get; set; } = 1800000;

    public bool ScreenCaptureWithWatermark { get; set; } = false;

    public int WaitTime { get; set; } = 1000;

    public bool MessageOfTheDay { get; set; } = false;

    public bool ScreenSharingWithWatermark { get; set; } = false;

    public bool PresentationModeWithWatermark { get; set; } = false;

    public bool BypassWatermark { get; set; } = false;

    public string LastTimeBypass { get; set; } = "";

    public int BypassDuration { get; set; } = 0;
  }
}
