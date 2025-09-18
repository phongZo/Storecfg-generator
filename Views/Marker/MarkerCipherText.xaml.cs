using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for MarkerCipherText.xaml
    /// </summary>
    public partial class MarkerCipherText : UserControl
    {
        public MarkerCipherText()
        {
            InitializeComponent();
            Loaded += MarkerCipherText_Loaded;
        }

        private void MarkerCipherText_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterRequiredBoxesByTag(this);
        }
        /// <summary>
        /// Find TextBox in visual tree has Tag = "required:Label"
        /// và register in DetailMarker.Instance.
        /// </summary>
        private static void RegisterRequiredBoxesByTag(DependencyObject root)
        {
            if (root == null || DetailMarker.Instance == null) return;

            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                if (child is TextBox tb)
                {
                    if (tb.Tag is string tag && tag.StartsWith("required:", StringComparison.OrdinalIgnoreCase))
                    {
                        var label = tag.Substring("required:".Length).Trim();
                        if (string.IsNullOrWhiteSpace(label))
                            label = tb.Name;
                        DetailMarker.Instance.RegisterRequired(tb, label);
                    }
                }
                RegisterRequiredBoxesByTag(child);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
                DetailMarker.Instance?.TouchRequired(textBox);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
                DetailMarker.Instance?.TouchRequired(textBox);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumeric(e.Text))
                return;
            e.Handled = true;
        }

        private bool IsNumeric(string text) => int.TryParse(text, out _);
    }
}
