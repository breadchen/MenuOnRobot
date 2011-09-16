using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MenuOnRobot
{
    delegate void SensorDataRcvEventHandler(SensorData sd);
    public partial class frmStatus : Office2007Form
    {
        #region 成员变量
        frmLocalCtrl m_frmLocalCtrl;

        // 用于接收传感器信息
        private UdpClient m_UdpSensorData = null;
        private static readonly string strIPHead = "192.168.1";
        private readonly IPEndPoint LOCAL_ADDRESS;

        private Thread m_trdListener = null;
        private bool Done = false;

        private int m_nRobotAddress = -1;
        private int m_nUserAddress = -1;
        
        // 传感器信息标志
        private int _nBrightnessLevel;
        /// <summary>
        /// 亮度级别分为 5 档（从零开始）, 亮度值每跳变 20 为一档，99以上均视为 4 档
        /// </summary>
        private int m_nBrightnessLevel
        {
        	get {return _nBrightnessLevel;}
        	set {_nBrightnessLevel = (value / 20) < 5 ? (value / 20) : 4;}
        }
        // 环境温度
        private int _nTemperature;
        private int m_nTemperature
        {
        	get {return _nTemperature;}
        	set {_nTemperature = value;}
        }
		// 环境湿度
		private int _nHumidity;
		private int m_nHumidity
		{
			get {return _nHumidity;}
			set {_nHumidity = value;}
		}
        
        private event SensorDataRcvEventHandler SensorDataRcv;

        private event WriteLogEventHandler _WriteLog;
        public event WriteLogEventHandler WriteLog
        {
            add { _WriteLog = value; }
            remove
            {
                if (null != _WriteLog) _WriteLog -= value;
            }
        }
        
        //private bool m_bDoorIsOpen = false;
        #endregion

        public frmStatus(frmLocalCtrl flc)
        {
            InitializeComponent();

            m_frmLocalCtrl = flc;

            LOCAL_ADDRESS = new IPEndPoint(_GetLocalIP(), 1799);
        }
        /// <summary>
        /// 获取本机ip
        /// </summary>
        /// <returns></returns>
        private IPAddress _GetLocalIP()
        {
            IPAddress result = null;

            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] aryLocalAddresser = ipEntry.AddressList;
            foreach (IPAddress a in aryLocalAddresser)
            {
                if (a.ToString().Contains(strIPHead))
                {
                    result = a;
                    break;
                }
            } // end of foreach

            return result;
        }
        /// <summary>
        /// UDP 监听线程
        /// </summary>
        private void threadListener()
        {
            byte[] byRcv;
            IPEndPoint ipRcv = null;
            while (!Done)
            {
                byRcv = m_UdpSensorData.Receive(ref ipRcv);

                if (byRcv != null && byRcv.Length >= 0)
                {
                    SensorDataRcv(new SensorData(byRcv));
                }
            } // end of while
        }

        #region 字符串常量
        private readonly string[] strEnvironmentInfoCaption = 
        {
            "平均温度：",
            "平均湿度：",
            "平均亮度：",
            "建议：",
            "可燃性气体水平：",
            "门禁状态："
        };
        private const string strCelsius = "C";
        private const string strHumidity = "%";
        private readonly string[] strBrightness = 
        {
            "暗",
            "较暗",
            "适中",
            "较亮",
            "亮",
        };
        private readonly string[] strSmoke = 
        {
            "正常",
            "超标"
        };
        private readonly string[] strGuard = 
        {
            "设防",
            "解防"
        };
        #endregion 字符串常量

        private void OnSensorDataRcv(SensorData sd)
        {
            switch ((byte)sd.ClustID)
            {
                case (byte)ZIGBEE_CLUST_ID.LIGHT_ID:
                { // 光照强度传感器
            		m_nBrightnessLevel = (int)sd.Data[2];
                    lblAverageBrightness.Text = 
                    	strEnvironmentInfoCaption[2] + 
                    	strBrightness[m_nBrightnessLevel];
                }break;
            	case (byte)ZIGBEE_CLUST_ID.TEMP_ID:
            	{ // 温度传感器
            		m_nTemperature = ((int)sd.Data[3] * 256 + sd.Data[2]) / 10;
            		lblAverageTemp.Text = 
            			strEnvironmentInfoCaption[0] + 
            			m_nTemperature.ToString() + strCelsius;            			
            	}break;
            	case (byte)ZIGBEE_CLUST_ID.TPHU_ID:
           		{ // 温湿度传感器
            		m_nHumidity = (int)sd.Data[2];
            		lblAverageHumidity.Text = 
            			strEnvironmentInfoCaption[1] +
            			m_nHumidity.ToString() + strHumidity;
           		}break;
            	case (byte)ZIGBEE_CLUST_ID.SMOKE_ID:
           		{ // 烟雾传感器
           			
           		}break;
                case (byte)ZIGBEE_CLUST_ID.LOCATION_INFO:
                { 
                    #region 定位信息
                    int x = sd.Data[2] + sd.Data[3] * 256;
                    int y = sd.Data[4] + sd.Data[5] * 256;

                    if (-1 == m_nRobotAddress && sd.CMD == (byte)ZigBeeCMD.CMD_ROBOT)
                    {
                        m_nRobotAddress = sd.Address;
                        MoveRobot(x, y);
                    }
                    else
                    {
                        if (sd.Address == m_nUserAddress)
                            MoveRobot(x, y);
                    }

                    if (-1 == m_nUserAddress && sd.CMD == (byte)ZigBeeCMD.CMD_USER)
                    {
                        m_nUserAddress = sd.Address;
                        MoveUser(x, y);
                    }
                    else
                    {
                        if (sd.Address == m_nRobotAddress)
                            MoveUser(x, y);
                    }
                    #endregion 定位信息
                } break;
            	case (byte)ZIGBEE_CLUST_ID.SECURITY_ID:
           		{
           			
           		}break;
                default: break;
            } // end of switch
        }

        private void MoveRobot(int x, int y)
        {
            if (x > 40 || y > 40) return;

            float f_x = (float)x / 4;
            float f_y = (float)y / 4;

            int col = (int)f_x > 5 ? 5 : (int)f_x;
            int row = (int)f_y > 5 ? 5 : (int)f_y;

            WriteLine(string.Format("robot position x[{0}] y[{1}]", f_x.ToString(), f_y.ToString()));
            MovePos(row, col, btnRobotPos);
        }

        private void MoveUser(int x, int y)
        {
            if (x > 40 || y > 40) return;

            float f_x = (float)x / 4;
            float f_y = (float)y / 4;

            int col = (int)f_x > 5 ? 5 : (int)f_x;
            int row = (int)f_y > 5 ? 5 : (int)f_y;

            WriteLine(string.Format("user position x[{0}] y[{1}]", f_x.ToString(), f_y.ToString()));
            MovePos(row, col, btnCurrentPos);
        }

        private void MovePos(int to_i, int to_j, PulseButton.PulseButton btn)
        {
            if (to_i >= tlpLocation.RowCount || to_j >= tlpLocation.ColumnCount)
                return;
            MoveButton(
                tlpLocation.GetRow(btn), 
                tlpLocation.GetColumn(btn),
                to_i, 
                to_j);
        }
        /// <summary>
        /// 移动第 from_i 行 from_j 列的控件，到第 to_i 行 to_j 列
        /// </summary>
        /// <param name="from_i"></param>
        /// <param name="from_j"></param>
        /// <param name="to_i"></param>
        /// <param name="to_j"></param>
        private void MoveButton(int from_i, int from_j, int to_i, int to_j)
        {
            Control c = tlpLocation.GetControlFromPosition(from_j, from_i);
            if (null != c)
                tlpLocation.Controls.Add(c, to_j, to_i);
        }
        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="str"></param>
        private void WriteLine(string str)
        {
            if (_WriteLog != null)
                _WriteLog(str);
        }

        #region 窗体事件响应
        private void frmStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_frmLocalCtrl.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Done = true;
            Thread.Sleep(1);
            if (null != m_trdListener) m_trdListener.Abort();
            if (null != m_UdpSensorData) m_UdpSensorData.Close();
            this.Close();
        }

        private void btnCurrentPos_Click(object sender, EventArgs e)
        {

        }

        private void frmStatus_Load(object sender, EventArgs e)
        {
            try
            {
                // 允许在不同线程中使用控件
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

                m_UdpSensorData = new UdpClient(LOCAL_ADDRESS);

                SensorDataRcv = new SensorDataRcvEventHandler(OnSensorDataRcv);

                ThreadStart starter = new ThreadStart(threadListener);
                m_trdListener = new Thread(starter);
                m_trdListener.Start();
            }
            catch(Exception ex)
            {
                WriteLine(ex.Message);
            }
        }
        #endregion

        private void btnInfoControl_Click(object sender, EventArgs e)
        {
            frmRemoteControl frc = new frmRemoteControl();
            frc.ShowDialog();
        }
    }
}
