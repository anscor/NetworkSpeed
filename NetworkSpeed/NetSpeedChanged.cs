using System.ComponentModel;

namespace NetworkSpeed
{
    public class NetSpeedChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_revSpeed;
        private string m_sendSpeed;

        public string RevSpeed
        {
            get { return m_revSpeed; }
            set
            {
                m_revSpeed = value;
                OnPropertyChanged("RevSpeed");
            }
        }

        public string SendSpeed
        {
            get { return m_sendSpeed; }
            set
            {
                m_sendSpeed = value;
                OnPropertyChanged("SendSpeed");
            }
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}