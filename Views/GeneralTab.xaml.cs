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
        public GeneralTab()
        {
            this.InitializeComponent();
            GeneralTab.Instance = this;
        }

        public void Generate_Storecfg()
        {
            if (!Instance.IsAllFieldsFilled)
            {
                int num1 = (int)MessageBox.Show("Please fill all the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }

            else
            {
                try
                {
                    if (!SelectOutputPath())
                    {
                        return;
                    }
                    bool? isChecked = this.MacMode.IsChecked;
                    if (isChecked.Value)
                    {
                        foreach (KeyValuePair<string, MarkerJson> marker in StoreCfg.Instance.CurrentStoreCfg.Profile.Markers)
                            marker.Value.LogoPosition = (object)this.ConvertBoolArrayToIntArray((bool[,])marker.Value.LogoPosition);
                    }
                    LocalSaveFile.Instance.SaveSettings(StoreCfg.Instance.CurrentStoreCfg, outputLocation);
                    int num3 = (int)MessageBox.Show("Generate store.cfg successful!");
                    isChecked = this.MacMode.IsChecked;
                    if (!isChecked.Value)
                        return;
                    foreach (KeyValuePair<string, MarkerJson> marker in StoreCfg.Instance.CurrentStoreCfg.Profile.Markers)
                        marker.Value.LogoPosition = (object)this.ConvertIntArrayToBoolArray((int[,])marker.Value.LogoPosition);
                }
                catch (Exception ex)
                {
                    int num4 = (int)MessageBox.Show("There is an error when save file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        private bool SelectOutputPath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Config files (*.cfg)|*.cfg|All files (*.*)|*.*",
                Title = "Save store.cfg",
                FileName = "store.cfg" // Tên mặc định cho file
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                outputLocation = saveFileDialog.FileName;
                return true;
            }

            return false;
        }

        public bool[,] ConvertIntArrayToBoolArray(int[,] intArray)
        {
            int length1 = intArray.GetLength(0);
            int length2 = intArray.GetLength(1);
            bool[,] boolArray = new bool[length1, length2];
            for (int index1 = 0; index1 < length1; ++index1)
            {
                for (int index2 = 0; index2 < length2; ++index2)
                    boolArray[index1, index2] = intArray[index1, index2] == 1;
            }
            return boolArray;
        }

        public int[,] ConvertBoolArrayToIntArray(bool[,] boolArray)
        {
            int length1 = boolArray.GetLength(0);
            int length2 = boolArray.GetLength(1);
            int[,] intArray = new int[length1, length2];
            for (int index1 = 0; index1 < length1; ++index1)
            {
                for (int index2 = 0; index2 < length2; ++index2)
                    intArray[index1, index2] = boolArray[index1, index2] ? 1 : 0;
            }
            return intArray;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox))
                return;
            this.ValidateTextBox(textBox);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox))
                return;
            this.ValidateTextBox(textBox);
        }

        private void ValidateTextBox(TextBox textBox)
        {
            if (textBox.Text.Trim().Equals(""))
            {
                textBox.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.BorderBrush = (Brush)Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    Instance.IsAllFieldsFilled = false;
                }));

            }
            else
            {
                textBox.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.BorderBrush = (Brush)new BrushConverter().ConvertFrom((object)"#d9d9d9");
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    Instance.IsAllFieldsFilled = true;
                }));
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.IsNumeric(e.Text))
                return;
            e.Handled = true;
        }

        private bool IsNumeric(string text) => int.TryParse(text, out int _);
    }
}
