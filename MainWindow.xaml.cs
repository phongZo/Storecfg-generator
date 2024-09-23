using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public bool IsDevMode { get; set; } = false;

        public SetupConfigJson SetupConfig { get; set; } = new SetupConfigJson();

        public MainWindow()
        {
            InitializeComponent();
            MainWindow.Instance = this;
            StoreCfg.Instance.CurrentStoreCfg = new StoreCfgJson();
            this.SetupConfig = LocalSaveFile.Instance.LoadSetupConfig();
            MainWindow.Instance.IsDevMode = true;
            if (this.SetupConfig == null || !LocalSaveFile.Instance.EnforeSetupSettings(this.SetupConfig))
            {
                int num = (int)MessageBox.Show("Invalid or not found setup.cfg", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                Environment.Exit(0);
            }
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
            this.SetupTheDefaultConfig(this.SetupConfig);
            this.GetListMarkerName();
            this.DataContext = (object)StoreCfg.Instance.CurrentStoreCfg;
        }

        public void SetupTheDefaultConfig(SetupConfigJson setupConfig)
        {
            StoreCfg.Instance.CurrentStoreCfg.DevMode = this.IsDevMode;
            StoreCfg.Instance.CurrentStoreCfg.CustomerID = setupConfig.CustomerID;
            StoreCfg.Instance.CurrentStoreCfg.License = setupConfig.License;
            StoreCfg.Instance.CurrentStoreCfg.Profile.ServerSettings = setupConfig.ServerSettings;
            if (this.IsDevMode)
                return;
            StoreCfg.Instance.CurrentStoreCfg.IsServerMode = false;
            GeneralTab.Instance.ServerMode.IsChecked = false;
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
    }
}
