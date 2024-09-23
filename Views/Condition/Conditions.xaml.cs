using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for Conditions.xaml
    /// </summary>
    public partial class Conditions : UserControl
    {
        public static Conditions Instance { get; set; }
        private string[] enteredStrings;
        public Conditions()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Add_Condition_Top_Btn_Click(object sender, RoutedEventArgs e)
        {
            StoreCfg.Instance.CurrentStoreCfg.Profile.Conditions.Insert(0, new ConditionJson()
            {
                rules = {
          new RuleJson()
        }
            });
        }

        private void Add_Condition_Bottom_Btn_Click(object sender, RoutedEventArgs e)
        {
            StoreCfg.Instance.CurrentStoreCfg.Profile.Conditions.Add(new ConditionJson()
            {
                rules = {
          new RuleJson()
        }
            });
        }

        private void Condition_Remove_Btn_Click(object sender, MouseButtonEventArgs e)
        {
            StoreCfg.Instance.CurrentStoreCfg.Profile.Conditions.RemoveAt(StoreCfg.Instance.CurrentStoreCfg.Profile.Conditions.IndexOf((ConditionJson)((FrameworkElement)sender).DataContext));
        }

        private void ProcessInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return || !(sender is TextBox textBox))
                return;
            this.enteredStrings = textBox.Text.Split('|');
            textBox.Clear();
            foreach (string enteredString in enteredStrings)
            {
                if (!(enteredString.Trim() == "") && !textBox.Text.Contains(enteredString + "|"))
                    textBox.AppendText(enteredString + "|");
            }
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
        }

        private void DnsInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return || !(sender is TextBox textBox))
                return;
            enteredStrings = textBox.Text.Split('|');
            textBox.Clear();
            foreach (string enteredString in enteredStrings)
            {
                if (!(enteredString.Trim() == "") && !textBox.Text.Contains(enteredString + "|"))
                    textBox.AppendText(enteredString + "|");
            }
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox))
                return;
            ValidateTextBox(textBox);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox))
                return;
            ValidateTextBox(textBox);
        }

        private void ValidateTextBox(TextBox textBox)
        {
            if (textBox.Text.Trim().Equals(""))
                textBox.Dispatcher.Invoke(() =>
                {
                    textBox.BorderBrush = Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    GeneralTab.Instance.IsAllFieldsFilled = false;
                });
            else
                textBox.Dispatcher.Invoke(() =>
                {
                    textBox.BorderBrush = (Brush)new BrushConverter().ConvertFrom((object)"#d9d9d9");
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    GeneralTab.Instance.IsAllFieldsFilled = false;
                });
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumeric(e.Text))
                return;
            e.Handled = true;
        }

        private bool IsNumeric(string text) => int.TryParse(text, out int _);
    }
}
