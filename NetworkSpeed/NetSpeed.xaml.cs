using CSDeskBand;
using System.Runtime.InteropServices;
using System.Threading;
using System.Management;

namespace NetworkSpeed
{
    [ComVisible(true)]
    [Guid("EDD99C62-F571-4C53-AD8A-B56BA0C786FA")]
    [CSDeskBandRegistration(Name = "NetworkSpeed")]
    public partial class NetSpeed
    {
        private int m_width = 86;
        private int m_height = 40;

        private NetSpeedChanged m_netSpeedChanged;
        private NetSpeedCalc m_netSpeedCalc;

        private Thread m_thread;
        private int m_interval = 1000;

        public NetSpeed()
        {
            InitializeComponent();
            var t = GetScale();
            float xScale = (float)t.Width / 96;
            float yScale = (float)t.Height / 96;
            Options.MinHorizontal = new Size((int)(this.m_width * xScale), (int)(this.m_height * yScale));
            Options.MinVertical = new Size((int)(this.m_width * xScale), (int)(this.m_height * yScale));

            this.Width = (int)(this.m_width * xScale);
            this.Height = (int)(this.m_height * yScale);
            up_arrow.FontSize = (int)(up_arrow.FontSize * xScale);
            down_arrow.FontSize = (int)(down_arrow.FontSize * xScale);
            tb_revSpeed.FontSize = (int)(tb_revSpeed.FontSize * xScale);
            tb_sendSpeed.FontSize = (int)(tb_sendSpeed.FontSize * xScale);

            this.m_netSpeedChanged = new NetSpeedChanged();
            this.m_netSpeedCalc = new NetSpeedCalc();

            this.tb_revSpeed.DataContext = this.m_netSpeedChanged;
            this.tb_sendSpeed.DataContext = this.m_netSpeedChanged;

            this.SetSpeed();

            this.m_thread = new Thread(new ThreadStart(() =>
            {

                while (true)
                {
                    Thread.Sleep(this.m_interval);
                    this.SetSpeed();
                }

            }));
            this.m_thread.Start();
        }

        private Size GetScale()
        {
            Size t = null;
            using (ManagementClass mc = new ManagementClass("Win32_DesktopMonitor"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    int x = 0;
                    int y = 0;
                    foreach (ManagementObject each in moc)
                    {
                        x = int.Parse((each.Properties["PixelsPerXLogicalInch"].Value.ToString()));
                        y = int.Parse((each.Properties["PixelsPerYLogicalInch"].Value.ToString()));
                        t = new Size(x, y);
                    }
                }
            }
            return t;
        }

        private void SetSpeed ()
        {
            this.m_netSpeedCalc.CalcSpeed();
            this.m_netSpeedChanged.SendSpeed = this.SpeedFormat(this.m_netSpeedCalc.SendSpeed);
            this.m_netSpeedChanged.RevSpeed = this.SpeedFormat(this.m_netSpeedCalc.RevSpeed);
        }

        private string SpeedFormat(long speed)
        {
            string formatSpeed;

            if (speed < 1024)
            {
                formatSpeed = speed.ToString() + "B/s";
            }
            else if (speed < 1024 * 1024)
            {
                formatSpeed = string.Format("{0:N2}KB/s", (double)speed / 1024);
            }
            else
            {
                formatSpeed = string.Format("{0:N2}MB/s", ((double)speed / 1024) / 1024);
            }

            return formatSpeed;
        }

    }

}
