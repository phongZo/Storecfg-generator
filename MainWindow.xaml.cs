using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LocalSaveFile LocalSaveFile = new LocalSaveFile();

        public StoreCfg StoreCfg = new StoreCfg();

        public static MainWindow Instance { get; set; }

        public GeneralTab generalTab = new GeneralTab();

        public bool IsDevMode { get; set; } = false;

        public SetupConfigJson SetupConfig { get; set; } = new SetupConfigJson();

        private string currentFilePath;

        public MainWindow()
        {
            InitializeComponent();
            MainWindow.Instance = this;
            StoreCfg.Instance.CurrentStoreCfg = new StoreCfgJson();
            this.SetupConfig = LocalSaveFile.Instance.LoadSetupConfig();
            MainWindow.Instance.IsDevMode = true;
            AutoMapperConfig.Configure();
            StoreCfg.Instance.CurrentStoreCfg.Profile.Markers = new Dictionary<string, MarkerJson>();
            StoreCfg.Instance.CurrentStoreCfg.Profile.Markers.Add("basemarker", new MarkerJson());
            RuleJson ruleJson = new RuleJson();
            ruleJson.data = "127.0.0.1";
            ruleJson.method = "PING";
            ruleJson.extras = new ExtrasJson();
            ConditionJson conditionJson = new ConditionJson();
            conditionJson.rules.Add(ruleJson);
            conditionJson.name = "Out of Office";
            conditionJson.marker = "basemarker";
            StoreCfg.Instance.CurrentStoreCfg.Profile.Conditions = new ObservableCollection<ConditionJson>();
            StoreCfg.Instance.CurrentStoreCfg.Profile.Conditions.Add(conditionJson);
            this.GetListMarkerName();
            this.DataContext = (object)StoreCfg.Instance.CurrentStoreCfg;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenAndLoadConfiguration();
        }


        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                generalTab.Generate_Storecfg();
            }
            else
            {
                LocalSaveFile.Instance.SaveSettings(StoreCfg.Instance.CurrentStoreCfg, currentFilePath);
                MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            GeneralTab.Instance.Generate_Storecfg();
        }

        public void OpenAndLoadConfiguration()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Configuration Files (*.cfg)|*.cfg|All Files (*.*)|*.*",
                Title = "Select a Store Configuration File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                currentFilePath = openFileDialog.FileName;

                StoreCfgJson SS_new = LocalSaveFile.Instance.LoadSettings(currentFilePath);

                if (SS_new == null)
                {
                    MessageBox.Show("Invalid store.cfg", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    StoreCfgJson storeCfgJson = LocalSaveFile.Instance.EnforceValidStoredSetting(SS_new);
                    StoreCfg.Instance.CurrentStoreCfg = storeCfgJson;
                    this.DataContext = storeCfgJson;
                    this.GetListMarkerName();
                }
            }
        }

        public void GetListMarkerName()
        {
            List<string> stringList = new List<string>();
            foreach (KeyValuePair<string, MarkerJson> marker in StoreCfg.Instance.CurrentStoreCfg.Profile.Markers)
                stringList.Add(marker.Key);
            StoreCfg.Instance.CurrentStoreCfg.MarkerListName = stringList;
            StoreCfg.Instance.CurrentStoreCfg.Profile.ObservedMarkers = new ObservableCollection<KeyValuePair<string, MarkerJson>>((IEnumerable<KeyValuePair<string, MarkerJson>>)StoreCfg.Instance.CurrentStoreCfg.Profile.Markers);
        }

        public int GetProtocol(int index)
        {
            if (!(this.DataContext is StoreCfgJson dataContext) || dataContext.Profile.ServerSettings.Count <= index)
                return 0;
            try
            {
                return new Uri(dataContext.Profile.ServerSettings[index]).Scheme == "http" ? 0 : 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string GetDomain(int index)
        {
            if (!(this.DataContext is StoreCfgJson dataContext) || dataContext.Profile.ServerSettings.Count <= index)
                return string.Empty;
            try
            {
                return new Uri(dataContext.Profile.ServerSettings[index]).Host;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string GetPort(int index)
        {
            if (!(this.DataContext is StoreCfgJson dataContext) || dataContext.Profile.ServerSettings.Count <= index)
                return string.Empty;
            try
            {
                return new Uri(dataContext.Profile.ServerSettings[index]).Port.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public int GetConditionMethod(int index)
        {
            if (!(this.DataContext is StoreCfgJson dataContext) || dataContext.Profile.Conditions.Count <= index)
                return 0;
            try
            {
                return new Uri(dataContext.Profile.ServerSettings[index]).Scheme == "http" ? 0 : 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void ToolBar_ToolTipClosing(object sender, System.Windows.Controls.ToolTipEventArgs e)
        {

        }
    }
}
