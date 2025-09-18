using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace StorecfgGenerator
{
    public class RuleJson : INotifyPropertyChanged
    {
        private string _data;
        private readonly ObservableCollection<string> _items;
        private string _method;

        public RuleJson()
        {
            _items = new ObservableCollection<string>
            {
                "PING", "HTTP", "HTTPS", "DNS", "PROCESS", "WAIT"
            };
            _method = "PING";

            extras = new ExtrasJson
            {
                timeout = 0,
                tries = 0,
                interval = 0,
                dns = null,
                flowThrough = false
            };
        }

        public string data
        {
            get => _data;
            set
            {
                if (_data == value) return;
                _data = value;
                OnPropertyChanged(nameof(data));
                OnPropertyChanged(nameof(Domain));
                OnPropertyChanged(nameof(Port));
            }
        }

        public ExtrasJson extras { get; set; }

        [JsonIgnore]
        public ObservableCollection<string> Items => _items;

        public string method
        {
            get => _method;
            set
            {
                if (_method == value) return;
                _method = value;
                OnPropertyChanged(nameof(method));

                // HTTP/HTTPS
                if (IsHttpLike())
                {
                    var host = Domain;
                    var port = Port;
                    data = ComposeUrl(host, port);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool IsHttpLike() => method == "HTTP" || method == "HTTPS";
        private string Scheme() => method == "HTTPS" ? "https" : "http";

        private string ComposeUrl(string host, string port)
        {
            if (string.IsNullOrWhiteSpace(host)) return string.Empty;

            if (string.IsNullOrWhiteSpace(port))
                return $"{Scheme()}://{host}";
            return $"{Scheme()}://{host}:{port}";
        }

        private static string StripScheme(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (s.StartsWith("http://", true, CultureInfo.InvariantCulture))
                return s.Substring("http://".Length);
            if (s.StartsWith("https://", true, CultureInfo.InvariantCulture))
                return s.Substring("https://".Length);
            return s;
        }

        [JsonIgnore]
        public string Domain
        {
            get
            {
                if (!IsHttpLike()) return string.Empty;

                var s = StripScheme(_data?.Trim());
                if (string.IsNullOrEmpty(s)) return string.Empty;

                var slash = s.IndexOf('/');
                if (slash >= 0) s = s.Substring(0, slash);

                var colon = s.LastIndexOf(':');
                if (colon > 0) s = s.Substring(0, colon);

                return s;
            }
            set
            {
                if (!IsHttpLike()) return;

                var host = (value ?? string.Empty).Trim();
                var port = Port;
                data = ComposeUrl(host, port);
            }
        }

        [JsonIgnore]
        public string Port
        {
            get
            {
                if (!IsHttpLike()) return string.Empty;

                var s = StripScheme(_data?.Trim());
                if (string.IsNullOrEmpty(s)) return string.Empty;

                var slash = s.IndexOf('/');
                if (slash >= 0) s = s.Substring(0, slash);

                var colon = s.LastIndexOf(':');
                if (colon > 0 && colon < s.Length - 1)
                {
                    var maybePort = s.Substring(colon + 1).Trim();
                    if (int.TryParse(maybePort, out _)) return maybePort;
                }
                return string.Empty;
            }
            set
            {
                if (!IsHttpLike()) return;

                var newPort = (value ?? string.Empty).Trim();
                if (newPort.Length > 0 && !int.TryParse(newPort, out _))
                {
                    return;
                }

                var host = Domain;
                data = ComposeUrl(host, newPort);
            }
        }
    }
}
