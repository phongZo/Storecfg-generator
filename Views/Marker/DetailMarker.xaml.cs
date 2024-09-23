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
    /// Interaction logic for DetailMarker.xaml
    /// </summary>
    public partial class DetailMarker : UserControl
    {
        public int currentEditMarkerIndex;
        public string EditingMarkerName;

        public static DetailMarker Instance { get; set; }

        public bool IsAllFieldsFilled { get; set; } = true;

        public DetailMarker()
        {
            this.InitializeComponent();
            DetailMarker.Instance = this;
        }

        private void Cancel_Create_New_Marker(object sender, RoutedEventArgs e)
        {
            if (MarkerTab.Instance.action.Equals(1))
                StoreCfg.Instance.CurrentStoreCfg.Profile.ObservedMarkers[this.currentEditMarkerIndex] = new KeyValuePair<string, MarkerJson>(this.EditingMarkerName, MarkerTab.Instance.StoredMarker);
            MarkerTab.Instance.BackToListMarkerView();
        }

        private void Save_New_Marker(object sender, RoutedEventArgs e)
        {
            StoreCfgJson dataContext = (StoreCfgJson)DetailMarker.Instance.DataContext;
            if (dataContext.CurrentMarkerName.Trim().Equals(""))
            {
                int num1 = (int)MessageBox.Show("Please fill marker's name.", "Missing Marker Name", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else if (!this.IsAllFieldsFilled)
            {
                int num2 = (int)MessageBox.Show("Please fill all the required fields in marker.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                if (MarkerTab.Instance.action.Equals(2))
                    StoreCfg.Instance.CurrentStoreCfg.Profile.Markers.Add(dataContext.CurrentMarkerName, MarkerTab.Instance.StoredMarker);
                else
                    StoreCfg.Instance.CurrentStoreCfg.Profile.ObservedMarkers[this.currentEditMarkerIndex] = new KeyValuePair<string, MarkerJson>(dataContext.CurrentMarkerName, dataContext.CurrentEditingMarker);
                MainWindow.Instance.GetListMarkerName();
                MarkerTab.Instance.BackToListMarkerView();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox))
                return;
            this.ValidateTextBox(textBox);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox))
                return;
            this.ValidateTextBox(textBox);
        }

        private void ValidateTextBox(TextBox textBox)
        {
            if (textBox.Text.Trim().Equals(""))
                textBox.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.BorderBrush = (Brush)Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    this.IsAllFieldsFilled = false;
                }));
            else
                textBox.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.BorderBrush = (Brush)new BrushConverter().ConvertFrom((object)"#d9d9d9");
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    this.IsAllFieldsFilled = true;
                }));
        }
    }
}
