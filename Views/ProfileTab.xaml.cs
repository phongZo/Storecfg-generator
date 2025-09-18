using System.Windows.Controls;
using System.Windows.Input;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for ProfileTab.xaml
    /// </summary>
    public partial class ProfileTab : UserControl
    {
        public ProfileTab() => this.InitializeComponent();

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.IsNumeric(e.Text))
                return;
            e.Handled = true;
        }

        private bool IsNumeric(string text) => int.TryParse(text, out int _);
    }
}
