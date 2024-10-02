using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StorecfgGenerator
{
    public partial class JsonPreviewTab : UserControl
    {
        public static JsonPreviewTab Instance { get; set; }

        private string _originalJsonContent;

        public JsonPreviewTab()
        {
            this.InitializeComponent();
            DataContext = this;
            Instance = this;
            _originalJsonContent = txtJsonPreview.Text;
            btnApply.IsEnabled = false;
        }

        private void txtJsonPreview_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnApply.IsEnabled = txtJsonPreview.Text != _originalJsonContent;
        }

        private void FormatJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var formattedJson = FormatJson(txtJsonPreview.Text);
                txtJsonPreview.Text = formattedJson;
                _originalJsonContent = txtJsonPreview.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error formatting JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DisplayStoreCfgJson(StoreCfgJson storeCfgJson)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(storeCfgJson, Formatting.Indented);
                txtJsonPreview.Text = jsonContent;
                btnApply.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error formatting JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string FormatJson(string json)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        private void ApplyJson_Click(object sender, RoutedEventArgs e)
        {
            string jsonContent = txtJsonPreview.Text;
            try
            {
                ApplyJsonSettings(jsonContent);
                _originalJsonContent = jsonContent;
                MessageBox.Show("JSON settings have been successfully applied!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while applying JSON settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyJsonSettings(string jsonContent)
        {
            if (string.IsNullOrEmpty(jsonContent))
            {
                MessageBox.Show("JSON content is empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                StoreCfgJson storeCfgJson = LoadSettingsFromJson(jsonContent);

                StoreCfg.Instance.CurrentStoreCfg = storeCfgJson;
                MainWindow.Instance.DataContext = storeCfgJson;
                MainWindow.Instance.GetListMarkerName();

                DisplayStoreCfgJson(storeCfgJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public StoreCfgJson LoadSettingsFromJson(string jsonContent)
        {
            if (string.IsNullOrEmpty(jsonContent))
            {
                throw new ArgumentException("JSON content cannot be null or empty.", nameof(jsonContent));
            }

            StoreCfgJson storeCfgJson;

            try
            {
                storeCfgJson = JsonConvert.DeserializeObject<StoreCfgJson>(jsonContent);
            }
            catch (Exception ex)
            {
                throw new JsonException("Failed to deserialize JSON content.", ex);
            }

            return storeCfgJson;
        }
    }
}
