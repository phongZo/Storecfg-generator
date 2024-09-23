using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace StorecfgGenerator
{
    /// <summary>
    /// Interaction logic for GeneralTab.xaml
    /// </summary>
    public partial class GeneralTab : UserControl
    {
        public static GeneralTab Instance { get; set; }

        public bool IsAllFieldsFilled { get; set; } = true;

        public GeneralTab()
        {
            this.InitializeComponent();
            GeneralTab.Instance = this;
        }

        private void Generate_Storecfg(object sender, RoutedEventArgs e)
        {
            if (!this.IsAllFieldsFilled)
            {
                int num1 = (int)MessageBox.Show("Please fill all the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else if (this.OutputPath.Text.Trim() == "")
            {
                int num2 = (int)MessageBox.Show("Please select output path", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                try
                {
                    bool? isChecked = this.MacMode.IsChecked;
                    if (isChecked.Value)
                    {
                        foreach (KeyValuePair<string, MarkerJson> marker in StoreCfg.Instance.CurrentStoreCfg.Profile.Markers)
                            marker.Value.LogoPosition = (object)this.ConvertBoolArrayToIntArray((bool[,])marker.Value.LogoPosition);
                    }
                    LocalSaveFile.Instance.SaveSettings(StoreCfg.Instance.CurrentStoreCfg, this.OutputPath.Text);
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

        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.IsFolderPicker = true;
            CommonFileDialogResult fileDialogResult = commonOpenFileDialog.ShowDialog();
            if (fileDialogResult != CommonFileDialogResult.Ok || !(fileDialogResult.ToString() != string.Empty))
                return;
            this.OutputPath.Text = commonOpenFileDialog.FileName;
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
                textBox.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.BorderBrush = (Brush)Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    this.IsAllFieldsFilled = false;
                }));
            else
                textBox.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.BorderBrush = (Brush)new BrushConverter().ConvertFrom((object)"#d9d9d9");
                    textBox.BorderThickness = new Thickness(1.0);
                    textBox.InvalidateVisual();
                    this.IsAllFieldsFilled = true;
                }));
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
