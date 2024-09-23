// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.StoreCfgJson
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StorecfgGenerator
{
  public class StoreCfgJson : INotifyPropertyChanged
  {
    private bool _isServerMode = true;
    private ICommand _editCommand;
    private ICommand _deleteCommand;
    private int _currentGridShapeIndex;
    private MarkerJson _currentEditingMarker = new MarkerJson();
    private StoreCfgJson.GridItem[] _logoPosition2;
    private List<string> _markerListName;
    private string _currentMarkerName = "";
    private List<string> ShapeOfGrid = new List<string>()
    {
      "Square",
      "Circle",
      "Logo"
    };

    [JsonIgnore]
    public bool DevMode { get; set; } = false;

    [JsonIgnore]
    public int Protocol1
    {
      get => MainWindow.Instance.GetProtocol(0);
      set
      {
        if (this.Profile.ServerSettings.Count == 0)
          this.Profile.ServerSettings.Add(string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox1.Items[value]).Content, (object) this.Domain1, (object) this.Port1));
        else
          this.Profile.ServerSettings[0] = string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox1.Items[value]).Content, (object) this.Domain1, (object) this.Port1);
      }
    }

    [JsonIgnore]
    public string Domain1
    {
      get => MainWindow.Instance.GetDomain(0);
      set
      {
        if (this.Profile.ServerSettings.Count == 0)
          this.Profile.ServerSettings.Add(string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox1.Items[this.Protocol1]).Content, (object) value, (object) this.Port1));
        else
          this.Profile.ServerSettings[0] = string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox1.Items[this.Protocol1]).Content, (object) value, (object) this.Port1);
      }
    }

    [JsonIgnore]
    public string Port1
    {
      get => MainWindow.Instance.GetPort(0);
      set
      {
        if (this.Profile.ServerSettings.Count == 0)
          this.Profile.ServerSettings.Add(string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox1.Items[this.Protocol1]).Content, (object) this.Domain1, (object) value));
        else
          this.Profile.ServerSettings[0] = string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox1.Items[this.Protocol1]).Content, (object) this.Domain1, (object) value);
      }
    }

    [JsonIgnore]
    public int Protocol2
    {
      get => MainWindow.Instance.GetProtocol(1);
      set
      {
        if (this.Profile.ServerSettings.Count == 1)
          this.Profile.ServerSettings.Add(string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox2.Items[value]).Content, (object) this.Domain2, (object) this.Port2));
        else
          this.Profile.ServerSettings[1] = string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox2.Items[value]).Content, (object) this.Domain2, (object) this.Port2);
      }
    }

    [JsonIgnore]
    public string Domain2
    {
      get => MainWindow.Instance.GetDomain(1);
      set
      {
        if (this.Profile.ServerSettings.Count == 1)
          this.Profile.ServerSettings.Add(string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox2.Items[this.Protocol2]).Content, (object) value, (object) this.Port2));
        else
          this.Profile.ServerSettings[1] = string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox2.Items[this.Protocol2]).Content, (object) value, (object) this.Port2);
      }
    }

    [JsonIgnore]
    public string Port2
    {
      get => MainWindow.Instance.GetPort(1);
      set
      {
        if (this.Profile.ServerSettings.Count == 1)
          this.Profile.ServerSettings.Add(string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox2.Items[this.Protocol2]).Content, (object) this.Domain2, (object) value));
        else
          this.Profile.ServerSettings[1] = string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox2.Items[this.Protocol2]).Content, (object) this.Domain2, (object) value);
      }
    }

    public string CurrentHash { get; set; } = "";

    public string CustomerID { get; set; } = "";

    public ProfileJson Profile { get; set; } = new ProfileJson();

    public long ClientID { get; set; } = 1;

    public string ClientGroupsHash { get; set; } = "";

    [JsonIgnore]
    public bool IsServerMode
    {
      get
      {
        this._isServerMode = this.DeployType == "server";
        return this._isServerMode;
      }
      set
      {
        if (this._isServerMode == value)
          return;
        this._isServerMode = value;
        this.DeployType = this._isServerMode ? "server" : "serverless";
        this.OnPropertyChanged(nameof (IsServerMode));
      }
    }

    public string DeployType { get; set; } = "server";

    public MotdJson MotdSettings { get; set; } = new MotdJson();

    public string License { get; set; } = "";

    private bool Validate()
    {
      return this.Profile.ServerSettings.Count > 0 && this.Profile.Markers.Count > 0;
    }

    public static bool IsValid(StoreCfgJson ss) => ss != null && ss.Validate();

    [JsonIgnore]
    public ICommand EditCommand
    {
      get
      {
        if (this._editCommand == null)
          this._editCommand = (ICommand) new RelayCommand(new Action<object>(this.ExecuteEdit), new Predicate<object>(this.CanExecuteEdit));
        return this._editCommand;
      }
    }

    [JsonIgnore]
    public ICommand DeleteCommand
    {
      get
      {
        if (this._deleteCommand == null)
          this._deleteCommand = (ICommand) new RelayCommand(new Action<object>(this.ExecuteDelete), new Predicate<object>(this.CanExecuteDelete));
        return this._deleteCommand;
      }
    }

    [JsonIgnore]
    public string CurrentGridShape
    {
      get => MainWindow.Instance.GetPort(1);
      set
      {
        this.Profile.ServerSettings[1] = string.Format("{0}{1}:{2}", ((ContentControl) GeneralTab.Instance.ComboBox2.Items[this.Protocol2]).Content, (object) this.Domain2, (object) value);
      }
    }

    [JsonIgnore]
    public int CurrentGridShapeIndex
    {
      get => this._currentGridShapeIndex;
      set
      {
        if (this._currentGridShapeIndex == value)
          return;
        this._currentGridShapeIndex = value;
        Application.Current.Dispatcher.Invoke((Action) (() => this.OnPropertyChanged(nameof (CurrentGridShapeIndex))));
      }
    }

    [JsonIgnore]
    public MarkerJson CurrentEditingMarker
    {
      get => this._currentEditingMarker;
      set
      {
        if (this._currentEditingMarker == value)
          return;
        this._currentEditingMarker = value;
        Application.Current.Dispatcher.Invoke((Action) (() => this.OnPropertyChanged(nameof (CurrentEditingMarker))));
      }
    }

    [JsonIgnore]
    public StoreCfgJson.GridItem[] LogoPosition2
    {
      get => this._logoPosition2;
      set
      {
        this._logoPosition2 = value;
        this.OnPropertyChanged(nameof (LogoPosition2));
      }
    }

    public bool[,] ConvertTo2DArray(StoreCfgJson.GridItem[] flatArray, int rows, int columns)
    {
      bool[,] flagArray = new bool[rows, columns];
      if (flatArray.Length != rows * columns)
        throw new ArgumentException("Invalid dimensions for the provided flat array.");
      for (int index1 = 0; index1 < rows; ++index1)
      {
        for (int index2 = 0; index2 < columns; ++index2)
          flagArray[index1, index2] = flatArray[index1 * columns + index2].Value;
      }
      return flagArray;
    }

    [JsonIgnore]
    public List<string> MarkerListName
    {
      get => this._markerListName;
      set
      {
        if (this._markerListName == value)
          return;
        this._markerListName = value;
        this.OnPropertyChanged(nameof (MarkerListName));
      }
    }

    [JsonIgnore]
    public string CurrentMarkerName
    {
      get => this._currentMarkerName;
      set
      {
        if (!(this._currentMarkerName != value))
          return;
        this._currentMarkerName = value;
        Application.Current.Dispatcher.Invoke((Action) (() => this.OnPropertyChanged(nameof (CurrentMarkerName))));
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

    private void ExecuteEdit(object parameter)
    {
      string key = parameter as string;
      if (key == null)
        return;
      StoreCfgJson dataContext = (StoreCfgJson) MainWindow.Instance.DataContext;
      KeyValuePair<string, MarkerJson> keyValuePair = dataContext.Profile.ObservedMarkers.FirstOrDefault<KeyValuePair<string, MarkerJson>>((Func<KeyValuePair<string, MarkerJson>, bool>) (item => item.Key == key));
      MarkerJson source = keyValuePair.Value;
      MarkerJson markerJson = Mapper.Map<MarkerJson, MarkerJson>(source);
      MarkerTab.Instance.action = 1;
      DetailMarker.Instance.currentEditMarkerIndex = dataContext.Profile.ObservedMarkers.IndexOf(keyValuePair);
      MarkerTab.Instance.StoredMarker = markerJson;
      DetailMarker.Instance.EditingMarkerName = key;
      dataContext.CurrentEditingMarker = source;
      dataContext.CurrentMarkerName = key;
      dataContext.CurrentGridShapeIndex = this.ShapeOfGrid.IndexOf(dataContext.CurrentEditingMarker.GridShape);
      dataContext.LogoPosition2 = StoreCfg.Instance.ConvertToGridItems((bool[,]) dataContext.CurrentEditingMarker.LogoPosition);
      MarkerTab.Instance.ListMarkers.Visibility = Visibility.Collapsed;
      MarkerTab.Instance.DetailMarker.Visibility = Visibility.Visible;
    }

    private bool CanExecuteEdit(object parameter) => true;

    private void ExecuteDelete(object parameter)
    {
      string key = parameter as string;
      if (key == null)
        return;
      if (StoreCfg.Instance.CurrentStoreCfg.Profile.Markers.Count == 1)
      {
        int num = (int) MessageBox.Show("Cannot remove all markers");
      }
      else
      {
        StoreCfgJson dataContext = (StoreCfgJson) MainWindow.Instance.DataContext;
        KeyValuePair<string, MarkerJson> keyValuePair = dataContext.Profile.ObservedMarkers.FirstOrDefault<KeyValuePair<string, MarkerJson>>((Func<KeyValuePair<string, MarkerJson>, bool>) (item => item.Key == key));
        dataContext.Profile.ObservedMarkers.Remove(keyValuePair);
        StoreCfg.Instance.CurrentStoreCfg.Profile.Markers.Remove(key);
        MainWindow.Instance.GetListMarkerName();
      }
    }

    private bool CanExecuteDelete(object parameter) => true;

    public class GridItem : INotifyPropertyChanged
    {
      private bool _value;

      public bool Value
      {
        get => this._value;
        set
        {
          this._value = value;
          this.OnPropertyChanged(nameof (Value));
        }
      }

      public int RowIndex { get; set; }

      public int ColumnIndex { get; set; }

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
}
