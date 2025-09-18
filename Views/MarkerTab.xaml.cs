using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for MarkerTab.xaml
    /// </summary>
    public partial class MarkerTab : UserControl
    {
        public MarkerJson StoredMarker = new MarkerJson();
        public int action;

        public static MarkerTab Instance { get; set; }

        public MarkerTab()
        {
            this.InitializeComponent();
            Instance = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BackToListMarkerView();
        }

        private void MarkerTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                action = 0;
                BackToListMarkerView();
            }
        }

        private void Add_Marker_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.action = 2;
            StoreCfgJson dataContext = (StoreCfgJson)MarkerTab.Instance.DataContext;
            dataContext.CurrentMarkerName = "";
            this.StoredMarker = new MarkerJson();
            dataContext.CurrentEditingMarker = this.StoredMarker;
            dataContext.CurrentGridShapeIndex = 0;
            dataContext.LogoPosition2 = StoreCfg.Instance.ConvertToGridItems((bool[,])this.StoredMarker.LogoPosition);
            MarkerTab.Instance.ListMarkers.Visibility = Visibility.Collapsed;
            MarkerTab.Instance.DetailMarker.Visibility = Visibility.Visible;
        }

        public void BackToListMarkerView()
        {
            ListMarkers.Visibility = Visibility.Visible;
            DetailMarker.Visibility = Visibility.Collapsed;
        }

        private void Cancel_Edit_Marker(object sender, RoutedEventArgs e)
        {
            StoreCfg.Instance.CurrentStoreCfg.Profile.Conditions.Insert(0, new ConditionJson()
            {
                rules = { new RuleJson() }
            });
        }

        private void DeleteMarker_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe?.DataContext is System.Collections.Generic.KeyValuePair<string, MarkerJson> kv)
            {
                string markerName = kv.Key;

                if (IsMarkerUsedInConditions(markerName))
                {
                    string usedIn = string.Join(", ",
                        StoreCfg.Instance.CurrentStoreCfg?.Profile?.Conditions?
                            .Select((c, i) => new { c, i })
                            .Where(x => x.c != null &&
                                        !string.IsNullOrEmpty(x.c.marker) &&
                                        string.Equals(x.c.marker, markerName, StringComparison.OrdinalIgnoreCase))
                            .Select(x => string.IsNullOrWhiteSpace(x.c.name) ? $"Condition #{x.i + 1}" : x.c.name)
                        ?? Enumerable.Empty<string>());

                    if (string.IsNullOrWhiteSpace(usedIn)) usedIn = "some conditions";

                    MessageBox.Show(
                        $"Cannot delete marker \"{markerName}\" because it is being used in condition \"{usedIn}\".",
                        "Marker is in use",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                var confirm = MessageBox.Show(
                    $"Delete marker \"{markerName}\"?",
                    "Confirm delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
                if (confirm != MessageBoxResult.Yes) return;

                var list = StoreCfg.Instance.CurrentStoreCfg.Profile.ObservedMarkers;
                var removeIndex = list.ToList().FindIndex(p => string.Equals(p.Key, markerName, StringComparison.Ordinal));
                if (removeIndex >= 0) list.RemoveAt(removeIndex);

                try
                {
                    var dict = StoreCfg.Instance.CurrentStoreCfg.Profile.Markers;
                    if (dict != null && dict.ContainsKey(markerName))
                        dict.Remove(markerName);
                }
                catch
                {
                }
            }
        }

        private bool IsMarkerUsedInConditions(string markerName)
        {
            var conditions = StoreCfg.Instance.CurrentStoreCfg?.Profile?.Conditions;
            if (conditions == null) return false;

            foreach (var cond in conditions)
            {
                // cond.marker : string at Condition
                if (!string.IsNullOrEmpty(cond?.marker) &&
                    string.Equals(cond.marker, markerName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
