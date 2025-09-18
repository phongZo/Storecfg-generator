using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for GeneralTab.xaml
    /// </summary>
    public partial class GeneralTab : UserControl
    {
        public static GeneralTab Instance { get; set; }

        public static MainWindow mainWindow;
        public bool IsAllFieldsFilled { get; set; } = true;

        private string outputLocation;

        private readonly HashSet<TextBox> _requiredBoxes = new HashSet<TextBox>();
        private readonly Dictionary<TextBox, string> _requiredLabels = new Dictionary<TextBox, string>();
        private readonly HashSet<TextBox> _mustPositiveInt = new HashSet<TextBox>();
        private readonly HashSet<TextBox> _attachedRealtime = new HashSet<TextBox>();

        private readonly Brush _defaultBorder = (Brush)new BrushConverter().ConvertFrom("#d9d9d9");
        private readonly Brush _errorBorder = Brushes.Red;
        private readonly Thickness _borderThickness = new Thickness(1);

        public GeneralTab()
        {
            InitializeComponent();
            Instance = this;

            this.Loaded += (s, e) => HookNewRequiredInputs();
            this.LayoutUpdated += (s, e) => HookNewRequiredInputs();
        }

        public void Generate_Storecfg()
        {
            try
            {
                if (!SelectOutputPath()) return;

                bool isMac = this.MacMode != null && this.MacMode.IsChecked == true;

                if (isMac)
                {
                    foreach (var marker in StoreCfg.Instance.CurrentStoreCfg.Profile.Markers)
                        marker.Value.LogoPosition =
                            (object)this.ConvertBoolArrayToIntArray((bool[,])marker.Value.LogoPosition);
                }

                LocalSaveFile.Instance.SaveSettings(StoreCfg.Instance.CurrentStoreCfg, outputLocation);
                MessageBox.Show("Generate store.cfg successful!");

                if (isMac)
                {
                    foreach (var marker in StoreCfg.Instance.CurrentStoreCfg.Profile.Markers)
                        marker.Value.LogoPosition =
                            (object)this.ConvertIntArrayToBoolArray((int[,])marker.Value.LogoPosition);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error when save file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private bool SelectOutputPath()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Config files (*.cfg)|*.cfg|All files (*.*)|*.*",
                Title = "Save store.cfg",
                FileName = "store.cfg"
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                outputLocation = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        private void HookNewRequiredInputs()
        {
            DependencyObject root = Window.GetWindow(this) as DependencyObject ?? this;

            var q = new Queue<DependencyObject>();
            q.Enqueue(root);

            while (q.Count > 0)
            {
                var d = q.Dequeue();

                int count = VisualTreeHelper.GetChildrenCount(d);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(d, i);
                    if (child != null) q.Enqueue(child);
                }

                var fe = d as FrameworkElement;
                if (fe == null) continue;

                var tb = fe as TextBox;
                if (tb == null) continue;

                string label;
                bool mustPosInt;
                if (!TryParseRequiredTag(fe.Tag, out label, out mustPosInt)) continue;

                _requiredBoxes.Add(tb);
                if (!_requiredLabels.ContainsKey(tb))
                    _requiredLabels.Add(tb, string.IsNullOrWhiteSpace(label) ? tb.Name : label);
                if (mustPosInt) _mustPositiveInt.Add(tb);

                if (!_attachedRealtime.Contains(tb))
                {
                    tb.TextChanged += RealtimeRequired_Changed;
                    tb.LostFocus += RealtimeRequired_Changed;
                    _attachedRealtime.Add(tb);

                    ValidateOne(tb);
                }
            }
        }

        private void RealtimeRequired_Changed(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;
            ValidateOne(tb);
        }

        private void ValidateOne(TextBox tb)
        {
            if (tb == null) return;

            bool ok = !string.IsNullOrWhiteSpace(tb.Text);
            if (ok && _mustPositiveInt.Contains(tb))
            {
                int v;
                ok = int.TryParse(tb.Text, out v) && v > 0;
            }

            tb.BorderBrush = ok ? _defaultBorder : _errorBorder;
            tb.BorderThickness = _borderThickness;
            tb.ToolTip = ok ? null : (_mustPositiveInt.Contains(tb) ? "Required (number > 0)" : "Required");
            tb.InvalidateVisual();
        }

        public bool ValidateAllRequired(out string message)
        {
            HookNewRequiredInputs();

            var missing = new List<string>();

            foreach (var tb in _requiredBoxes)
            {
                bool ok = tb != null && !string.IsNullOrWhiteSpace(tb.Text);
                if (ok && _mustPositiveInt.Contains(tb))
                {
                    int v;
                    ok = int.TryParse(tb.Text, out v) && v > 0;
                }

                ValidateOne(tb);

                if (!ok)
                {
                    string label;
                    if (!_requiredLabels.TryGetValue(tb, out label) || string.IsNullOrWhiteSpace(label))
                        label = tb != null ? tb.Name : "(unknown)";
                    if (_mustPositiveInt.Contains(tb))
                        label += " (number > 0)";
                    missing.Add(label);
                }
            }

            IsAllFieldsFilled = missing.Count == 0;
            message = IsAllFieldsFilled ? string.Empty
                                        : "Missing/invalid fields:\n- " + string.Join("\n- ", missing);
            return IsAllFieldsFilled;
        }

        private bool TryParseRequiredTag(object tagObj, out string label, out bool mustPositiveInt)
        {
            label = null;
            mustPositiveInt = false;

            var s = tagObj as string;
            if (string.IsNullOrWhiteSpace(s)) return false;

            s = s.Trim();
            if (!s.StartsWith("required", StringComparison.OrdinalIgnoreCase)) return false;

            var parts = s.Split('|');
            if (parts.Length > 1) label = parts[1].Trim();

            var head = parts[0];
            if (head.IndexOf("int>0", StringComparison.OrdinalIgnoreCase) >= 0)
                mustPositiveInt = true;

            return true;
        }

        public bool[,] ConvertIntArrayToBoolArray(int[,] intArray)
        {
            int rows = intArray.GetLength(0);
            int cols = intArray.GetLength(1);
            var boolArray = new bool[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    boolArray[i, j] = intArray[i, j] == 1;
            return boolArray;
        }

        public int[,] ConvertBoolArrayToIntArray(bool[,] boolArray)
        {
            int rows = boolArray.GetLength(0);
            int cols = boolArray.GetLength(1);
            var intArray = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    intArray[i, j] = boolArray[i, j] ? 1 : 0;
            return intArray;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.IsNumeric(e.Text))
                return;
            e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e) { }
        private bool IsNumeric(string text) => int.TryParse(text, out int _);
    }
}
