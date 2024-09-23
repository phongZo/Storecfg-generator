using System.Windows.Controls;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for MarkerOverall.xaml
    /// </summary>
    public partial class MarkerOverall : UserControl
    {
        public static MarkerOverall Instance { get; set; }

        public MarkerOverall()
        {
            InitializeComponent();
            Instance = this;
        }

        public string TimeFormat => timeFormatTextBox.Text;
    }
}
