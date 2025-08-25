using System.Collections.Generic;
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
        private static readonly string[] Tokens = new[]
        {
            "{Username}", "{MachineName}", "{FirstName}", "{LastName}", "{DisplayName}",
            "{Email}", "{ShortDate}", "{LongDate}", "{ShortTime}", "{LongTime}", "{CustomDateTime}"
        };

        private readonly List<TextBox> _registeredLocal = new List<TextBox>();

        public MarkerText()
        {
            InitializeComponent();
            Loaded += MarkerText_Loaded;
            Unloaded += MarkerText_Unloaded;
        }

        private void MarkerText_Loaded(object sender, RoutedEventArgs e)
        {
            ScanAndRegisterRequired(this);
        }

        private void MarkerText_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DetailMarker.Instance != null)
            {
                foreach (var tb in _registeredLocal)
                    DetailMarker.Instance.UnregisterRequired(tb);
            }
            _registeredLocal.Clear();
        }
        /// <summary>
        /// Find TextBox in visual tree has Tag = "required:Label"
        /// và register in DetailMarker.Instance.
        /// </summary>
        private void ScanAndRegisterRequired(DependencyObject parent)
        {
            if (parent == null || DetailMarker.Instance == null) return;

            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is TextBox tb && tb.Tag is string tag && tag.StartsWith("required:"))
                {
                    string label = tag.Substring("required:".Length).Trim();
                    DetailMarker.Instance.RegisterRequired(tb, label);
                    _registeredLocal.Add(tb);
                }

                ScanAndRegisterRequired(child);
            }
        }

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

            DetailMarker.Instance?.TouchRequired(TextToDisplayBox);
        }

        private void TextToDisplayBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (tb.Text?.EndsWith("{") == true)
                    ShowSuggestions();

                DetailMarker.Instance?.TouchRequired(tb);
            }
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
            if (sender is ListBox listBox && e.OriginalSource is FrameworkElement element && element.DataContext != null)
            {
                listBox.SelectedItem = element.DataContext;
                InsertSelectedToken();
                e.Handled = true;
            }
        }

        private void TextToDisplayBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HideSuggestions();
            if (sender is TextBox tb)
                DetailMarker.Instance?.TouchRequired(tb);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (int.TryParse(e.Text, out _)) return;
            e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
                DetailMarker.Instance?.TouchRequired(tb);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                DetailMarker.Instance?.TouchRequired(tb);
        }
    }
}
