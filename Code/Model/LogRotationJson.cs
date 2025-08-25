using System.ComponentModel;

namespace StorecfgGenerator
{
    public class LogRotationJson : INotifyPropertyChanged
    {
        private bool _enable = true;
        private int _size = 10 * 1024 * 1024; // 10MB default
        private int _rotate = 5;

        public bool enable
        {
            get => _enable;
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                    OnPropertyChanged(nameof(enable));
                }
            }
        }

        public int size
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged(nameof(size));
                }
            }
        }

        public int rotate
        {
            get => _rotate;
            set
            {
                if (_rotate != value)
                {
                    _rotate = value;
                    OnPropertyChanged(nameof(rotate));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


