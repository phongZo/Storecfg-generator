using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for DetailMarker.xaml
    /// </summary>
    public partial class DetailMarker : UserControl
    {
        public int currentEditMarkerIndex;
        public string EditingMarkerName;
        private int _selectedTabIndex = 0;

        public static DetailMarker Instance { get; set; }

        private readonly HashSet<TextBox> _requiredBoxes = new HashSet<TextBox>();
        private readonly Dictionary<TextBox, string> _requiredLabels = new Dictionary<TextBox, string>();
        public bool AreRequiredFieldsFilled { get; private set; } = true;

        public DetailMarker()
        {
            InitializeComponent();
            Instance = this;
            UpdateSelectedTabVisual();
            UpdateTabContent();

            if (FindName("MarkerNameBox") is TextBox nameBox)
            {
                RegisterRequired(nameBox, "Marker Name");
            }
        }

        public void RegisterRequired(TextBox box, string label)
        {
            if (box == null || string.IsNullOrWhiteSpace(label)) return;

            if (_requiredBoxes.Add(box))
            {
                _requiredLabels[box] = label;
                box.TextChanged += RequiredBox_TextChanged;
                box.LostFocus += RequiredBox_LostFocus;
                UpdateOneRequired(box);
                ReevaluateRequired();
            }
            else
            {
                _requiredLabels[box] = label;
            }
        }

        public void UnregisterRequired(TextBox box)
        {
            if (box == null) return;
            if (_requiredBoxes.Remove(box))
            {
                _requiredLabels.Remove(box);
                box.TextChanged -= RequiredBox_TextChanged;
                box.LostFocus -= RequiredBox_LostFocus;
                ReevaluateRequired();
            }
        }

        public void TouchRequired(TextBox box)
        {
            if (box == null) return;
            if (_requiredBoxes.Contains(box))
            {
                UpdateOneRequired(box);
                ReevaluateRequired();
            }
        }

        private void RequiredBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                UpdateOneRequired(tb);
                ReevaluateRequired();
            }
        }

        private void RequiredBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                UpdateOneRequired(tb);
                ReevaluateRequired();
            }
        }

        private void UpdateOneRequired(TextBox box)
        {
            bool isVisible = box.IsVisible && box.Visibility == Visibility.Visible;
            bool ok = !string.IsNullOrWhiteSpace(box.Text);

            if (isVisible && !ok)
            {
                box.BorderBrush = Brushes.Red;
                box.BorderThickness = new Thickness(1.0);
            }
            else
            {
                box.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#d9d9d9");
                box.BorderThickness = new Thickness(1.0);
            }
            box.InvalidateVisual();
        }

        private void ReevaluateRequired()
        {
            bool allOk = true;
            foreach (var tb in _requiredBoxes)
            {
                if (!(tb.IsVisible && tb.Visibility == Visibility.Visible)) continue;

                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    allOk = false;
                    break;
                }
            }
            AreRequiredFieldsFilled = allOk;
        }

        private List<string> GetMissingRequiredLabels()
        {
            var list = new List<string>();
            foreach (var tb in _requiredBoxes)
            {
                if (!(tb.IsVisible && tb.Visibility == Visibility.Visible)) continue;

                if (string.IsNullOrWhiteSpace(tb.Text) && _requiredLabels.TryGetValue(tb, out var label))
                    list.Add(label);
            }
            return list;
        }

        private void TabButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tagString && int.TryParse(tagString, out int tabIndex))
            {
                _selectedTabIndex = tabIndex;
                UpdateSelectedTabVisual();
                UpdateTabContent();
            }
        }

        private void UpdateSelectedTabVisual()
        {
            // Reset
            TabOverall.BorderBrush = Brushes.Transparent;
            TabOverall.Foreground = (Brush)new BrushConverter().ConvertFrom("#6B7280");
            TabOverall.FontWeight = FontWeights.Normal;

            TabText.BorderBrush = Brushes.Transparent;
            TabText.Foreground = (Brush)new BrushConverter().ConvertFrom("#6B7280");
            TabText.FontWeight = FontWeights.Normal;

            // Set selected tab style with bottom border indicator
            Button selectedButton;
            if (_selectedTabIndex == 0)
                selectedButton = TabOverall;
            else if (_selectedTabIndex == 1)
                selectedButton = TabText;
            else
                selectedButton = TabOverall;

            selectedButton.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#3B82F6");
            selectedButton.Foreground = (Brush)new BrushConverter().ConvertFrom("#1F2937");
            selectedButton.FontWeight = FontWeights.SemiBold;
        }

        private void UpdateTabContent()
        {
            UserControl content;
            if (_selectedTabIndex == 0)
                content = new MarkerOverall();
            else if (_selectedTabIndex == 1)
                content = new MarkerText();
            else
                content = new MarkerOverall();

            TabContentPresenter.Content = content;
        }

        private void Cancel_Create_New_Marker(object sender, RoutedEventArgs e)
        {
            if (MarkerTab.Instance.action.Equals(1))
            {
                StoreCfg.Instance.CurrentStoreCfg.Profile.ObservedMarkers[this.currentEditMarkerIndex] =
                    new KeyValuePair<string, MarkerJson>(this.EditingMarkerName, MarkerTab.Instance.StoredMarker);
            }
            MarkerTab.Instance.BackToListMarkerView();
        }

        private void Save_New_Marker(object sender, RoutedEventArgs e)
        {
            foreach (var tb in _requiredBoxes)
                tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            if (FindName("MarkerNameBox") is TextBox nameBox)
                nameBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            ReevaluateRequired();
            if (!AreRequiredFieldsFilled)
            {
                var missing = GetMissingRequiredLabels();
                string msg = "Please fill all required fields.\nMissing: " + string.Join(", ", missing);
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            var dataContext = DetailMarker.Instance.DataContext as StoreCfgJson;
            var profile = StoreCfg.Instance.CurrentStoreCfg.Profile;
            var newName = (dataContext?.CurrentMarkerName ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Please fill marker's name.", "Missing Marker Name",
                                MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            // Create
            if (MarkerTab.Instance.action.Equals(2))
            {
                var marker = MarkerTab.Instance.StoredMarker ?? new MarkerJson();
                profile.Markers[newName] = marker;
            }
            // Edit
            else
            {
                var oldKey = this.EditingMarkerName;
                var markerObj = dataContext?.CurrentEditingMarker
                                ?? MarkerTab.Instance.StoredMarker
                                ?? new MarkerJson();

                if (!string.Equals(oldKey, newName, StringComparison.Ordinal))
                {
                    if (profile.Markers.ContainsKey(oldKey))
                        profile.Markers.Remove(oldKey);
                    profile.Markers[newName] = markerObj;
                }
                else
                {
                    profile.Markers[newName] = markerObj;
                }

                profile.ObservedMarkers[this.currentEditMarkerIndex] =
                    new KeyValuePair<string, MarkerJson>(newName, profile.Markers[newName]);

                this.EditingMarkerName = newName;
            }

            MainWindow.Instance.GetListMarkerName();
            MarkerTab.Instance.BackToListMarkerView();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb) TouchRequired(tb);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb) TouchRequired(tb);
        }
    }
}
