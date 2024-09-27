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
    /// Interaction logic for MarkerLogo.xaml
    /// </summary>
    public partial class MarkerLogo : UserControl
    {
        public static MarkerLogo Instance { get; set; }

        public MarkerLogo()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StoreCfgJson.GridItem dataContext;
            int num;
            if (sender is Border border)
            {
                dataContext = border.DataContext as StoreCfgJson.GridItem;
                num = dataContext != null ? 1 : 0;
                StoreCfgJson.GridItem gridItem = StoreCfg.Instance.CurrentStoreCfg.LogoPosition2.First(s => (s.ColumnIndex == dataContext.ColumnIndex && s.RowIndex == dataContext.RowIndex));
                StoreCfg.Instance.CurrentStoreCfg.LogoPosition2.First(s => (s.ColumnIndex == dataContext.ColumnIndex && s.RowIndex == dataContext.RowIndex)).Value = !gridItem.Value;
            }
            else
                num = 0;
            if (num == 0)
                return;
            StoreCfg.Instance.CurrentStoreCfg.CurrentEditingMarker.LogoPosition = (object)StoreCfg.Instance.CurrentStoreCfg.ConvertTo2DArray(StoreCfg.Instance.CurrentStoreCfg.LogoPosition2, 3, 3);
        }
    }
}
