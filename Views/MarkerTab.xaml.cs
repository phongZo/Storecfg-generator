using System;
using System.Collections.Generic;
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
    /// Interaction logic for MarkerTab.xaml
    /// </summary>
    public partial class MarkerTab : UserControl
    {
        public MarkerJson StoredMarker = new MarkerJson();
        public int action;

        public static MarkerTab Instance { get; set; }

        public MarkerTab()
        {
            this.InitializeComponent();
            Instance = this;
        }

        private void Add_Marker_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.action = 2;
            StoreCfgJson dataContext = (StoreCfgJson)MarkerTab.Instance.DataContext;
            dataContext.CurrentMarkerName = "";
            this.StoredMarker = new MarkerJson();
            dataContext.CurrentEditingMarker = this.StoredMarker;
            dataContext.CurrentGridShapeIndex = 0;
            dataContext.LogoPosition2 = StoreCfg.Instance.ConvertToGridItems((bool[,])this.StoredMarker.LogoPosition);
            MarkerTab.Instance.ListMarkers.Visibility = Visibility.Collapsed;
            MarkerTab.Instance.DetailMarker.Visibility = Visibility.Visible;
        }

        public void BackToListMarkerView()
        {
            MarkerTab.Instance.ListMarkers.Visibility = Visibility.Visible;
            MarkerTab.Instance.DetailMarker.Visibility = Visibility.Collapsed;
        }

        private void Cancel_Edit_Marker(object sender, RoutedEventArgs e)
        {
            StoreCfg.Instance.CurrentStoreCfg.Profile.Conditions.Insert(0, new ConditionJson()
            {
                rules = {
          new RuleJson()
        }
            });
        }
    }
}
