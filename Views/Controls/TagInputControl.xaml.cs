using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StorecfgGenerator.Views.Controls
{
    public partial class TagInputControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register("Tags", typeof(ObservableCollection<string>), typeof(TagInputControl),
                new FrameworkPropertyMetadata(new ObservableCollection<string>(), 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTagsChanged));

        public ObservableCollection<string> Tags
        {
            get { return (ObservableCollection<string>)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }

        private TextBox inputTextBox;
        private ItemsControl tagsContainer;

        public event PropertyChangedEventHandler PropertyChanged;

        public TagInputControl()
        {
            InitializeComponent();
        }

        private static void OnTagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TagInputControl control)
            {
                control.OnPropertyChanged(nameof(Tags));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            inputTextBox = FindName("PART_Input") as TextBox;
            tagsContainer = FindName("PART_Items") as ItemsControl;

            if (inputTextBox != null)
            {
                inputTextBox.KeyDown += InputTextBox_KeyDown;
                inputTextBox.LostFocus += InputTextBox_LostFocus;
            }
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddCurrentTag();
                e.Handled = true;
            }
            else if (e.Key == Key.Back && string.IsNullOrEmpty(inputTextBox.Text))
            {
                if (Tags.Count > 0)
                {
                    Tags.RemoveAt(Tags.Count - 1);
                    // Force the binding to update by setting the property again
                    SetValue(TagsProperty, Tags);
                }
                e.Handled = true;
            }
        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AddCurrentTag();
        }

        private void AddCurrentTag()
        {
            if (!string.IsNullOrWhiteSpace(inputTextBox.Text))
            {
                string tag = inputTextBox.Text.Trim();
                if (!Tags.Contains(tag))
                {
                    Tags.Add(tag);
                    // Force the binding to update by setting the property again
                    SetValue(TagsProperty, Tags);
                }
                inputTextBox.Text = string.Empty;
            }
        }

        private void RemoveTag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                Tags.Remove(tag);
                // Force the binding to update by setting the property again
                SetValue(TagsProperty, Tags);
            }
        }
    }
}


