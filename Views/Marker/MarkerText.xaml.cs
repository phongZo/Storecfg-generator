using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for MarkerText.xaml
    /// </summary>
    public partial class MarkerText : UserControl
    {
        public MarkerText() => InitializeComponent();

        private static readonly string[] Tokens = new[]
        {
            "{Username}", "{MachineName}", "{FirstName}", "{LastName}", "{DisplayName}",
            "{Email}", "{ShortDate}", "{LongDate}", "{ShortTime}", "{LongTime}", "{CustomDateTime}"
        };

        private void ShowSuggestions()
        {
            if (SuggestionPopup == null || SuggestionList == null || TextToDisplayBox == null)
                return;
            SuggestionList.ItemsSource = Tokens;
            SuggestionList.SelectedIndex = 0;
            SuggestionPopup.IsOpen = true;
        }

        private void HideSuggestions()
        {
            if (SuggestionPopup == null)
                return;
            SuggestionPopup.IsOpen = false;
        }

        private void InsertSelectedToken()
        {
            if (SuggestionList?.SelectedItem == null || TextToDisplayBox == null)
                return;
            string token = SuggestionList.SelectedItem.ToString();
            int caret = TextToDisplayBox.CaretIndex;
            string text = TextToDisplayBox.Text ?? string.Empty;

            int insertIndex = caret;
            if (caret > 0 && text[caret - 1] == '{')
            {
                // Remove the just-typed '{' to avoid duplicate '{{'
                text = text.Remove(caret - 1, 1);
                insertIndex = caret - 1;
            }

            TextToDisplayBox.Text = text.Insert(insertIndex, token);
            TextToDisplayBox.CaretIndex = insertIndex + token.Length;
            HideSuggestions();
        }

        private void TextToDisplayBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox tb)) return;
            if (tb.Text?.EndsWith("{") == true)
                ShowSuggestions();
        }

        private void TextToDisplayBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (SuggestionPopup?.IsOpen != true)
                return;

            // Number key quick insert (1-9, 0)
            if (TryHandleDigitSelection(e.Key))
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Down)
            {
                e.Handled = true;
                if (SuggestionList.SelectedIndex < SuggestionList.Items.Count - 1)
                    SuggestionList.SelectedIndex++;
            }
            else if (e.Key == Key.Up)
            {
                e.Handled = true;
                if (SuggestionList.SelectedIndex > 0)
                    SuggestionList.SelectedIndex--;
            }
            else if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                e.Handled = true;
                InsertSelectedToken();
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                HideSuggestions();
            }
        }

        private bool TryHandleDigitSelection(Key key)
        {
            int? desiredIndex = null;
            if (key >= Key.D1 && key <= Key.D9)
                desiredIndex = key - Key.D1; // 1..9 -> 0..8
            else if (key == Key.D0)
                desiredIndex = 9; // 0 -> 10th
            else if (key >= Key.NumPad1 && key <= Key.NumPad9)
                desiredIndex = key - Key.NumPad1;
            else if (key == Key.NumPad0)
                desiredIndex = 9;

            if (desiredIndex.HasValue)
            {
                if (SuggestionList != null && desiredIndex.Value >= 0 && desiredIndex.Value < SuggestionList.Items.Count)
                {
                    SuggestionList.SelectedIndex = desiredIndex.Value;
                    InsertSelectedToken();
                }
                return true;
            }

            return false;
        }

        private void SuggestionList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure the item under mouse becomes selected before insertion
            if (sender is ListBox listBox)
            {
                var element = e.OriginalSource as FrameworkElement;
                if (element != null)
                {
                    var container = element.DataContext;
                    if (container != null)
                    {
                        listBox.SelectedItem = container;
                        InsertSelectedToken();
                        e.Handled = true;
                    }
                }
            }
        }

        private void TextToDisplayBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HideSuggestions();
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
