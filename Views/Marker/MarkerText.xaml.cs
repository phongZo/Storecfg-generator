using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for MarkerText.xaml
    /// </summary>
    public partial class MarkerText : UserControl
    {
        public MarkerText() => InitializeComponent();

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
                textBox.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.BorderBrush = (Brush)Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    DetailMarker.Instance.IsAllFieldsFilled = false;
                }));
            else
                textBox.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.BorderBrush = (Brush)new BrushConverter().ConvertFrom((object)"#d9d9d9");
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    DetailMarker.Instance.IsAllFieldsFilled = true;
                }));
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
