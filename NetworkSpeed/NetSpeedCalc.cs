using System.Net.NetworkInformation;

namespace NetworkSpeed
{
    public class NetSpeedCalc
    {

        private long m_lastRevCount;
        private long m_lastSendCount;
        private long m_revCount;
        private long m_sendCount;

        private long m_revSpeed;
        private long m_sendSpeed;

        private NetworkInterface[] adapters;

        public long SendSpeed
        {
            get { return this.m_sendSpeed; }
        }

        public long RevSpeed
        {
            get { return this.m_revSpeed; }
        }

        public NetSpeedCalc()
        {
            this.adapters = NetworkInterface.GetAllNetworkInterfaces();

            this.m_sendCount = 0;
            this.m_revCount = 0;
            this.m_lastRevCount = 0;
            this.m_lastSendCount = 0;
            this.m_revSpeed = 0;
            this.m_sendSpeed = 0;
        }

        public void CalcSpeed()
        {
            this.m_revCount = this.m_sendCount = 0;

            IPv4InterfaceStatistics ipv4Statistics;

            foreach (var adapter in adapters)
            {
                ipv4Statistics = adapter.GetIPv4Statistics();

                this.m_revCount += ipv4Statistics.BytesReceived;
                this.m_sendCount += ipv4Statistics.BytesSent;
            }

            //this.m_revSpeed = this.m_lastRevCount == 0 ? 0 : this.m_revCount - this.m_lastRevCount;
            //this.m_sendSpeed = this.m_lastSendCount == 0 ? 0 : this.m_sendCount - this.m_lastSendCount;

            if (this.m_lastRevCount == 0 || 
                this.m_revCount - this.m_lastRevCount < 0)
                this.m_revSpeed = 0;
            else this.m_revSpeed = this.m_revCount - this.m_lastRevCount;

            if (this.m_lastSendCount == 0 ||
                this.m_sendCount - this.m_lastSendCount < 0)
                this.m_sendSpeed = 0;
            else this.m_sendSpeed = this.m_sendCount - this.m_lastSendCount;

            this.m_lastSendCount = this.m_sendCount;
            this.m_lastRevCount = this.m_revCount;
        }

    }
}
