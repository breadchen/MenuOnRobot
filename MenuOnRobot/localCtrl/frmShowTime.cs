/*
 * Created by SharpDevelop.
 * User: cm
 * Date: 2011-5-18
 * Time: 9:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Media;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace MenuOnRobot
{
	/// <summary>
	/// Description of frmShowTime.
	/// </summary>
	public partial class frmShowTime : Form
	{
		#region 成员变量
		private frmLocalCtrl m_frmLocalCtrl;
		// 音频播放
		private SoundPlayer m_soundPlayer = null;
		private readonly string MusicPath = @"D:\robotDancing.wav";
		// 控制机器人
		private const string StrArm = "PL0SQ0ONCE\r";
		private byte[] ByWheel = {0x22,4,2,2,2,2};
		private TcpClient m_tcpSender = null;
		private string m_RobotCtrlIP = null;
		private int m_RobotCtrlPort = 1599;
		// 定时退出
		private System.Threading.Timer m_tmrQuit = null;
		private bool m_bTiming = false;
		
		private string[] strArmCmd = 
		{
			"#0P2500#2P1343#4P1777#6P1500#8P2500T100\r","#0P2500#2P1343#4P1777#6P1500#8P2500T7200\r","#0P2500#2P500#4P1777#6P1500#8P2500T7500\r","#0P1970#2P500#4P861#6P1500#8P2500T200\r","#0P1970#2P500#4P861#6P1500#8P2500T1000\r","#0P1970#2P500#4P1753#6P1500#8P2500T100\r","#0P1970#2P500#4P1753#6P1500#8P2500T800\r","#0P2500#2P1295#4P1753#6P1500#8P2500T400\r","#0P2500#2P1295#4P1753#6P1500#8P2500T200\r","#0P2500#2P1319#4P1295#6P1536#8P2500T200\r","#0P2500#2P789#4P2235#6P1500#8P2500T200\r","#0P2500#2P789#4P2500#6P1500#8P2500T200\r","#0P2500#2P1295#4P2235#6P1500#8P2500T200\r","#0P2500#2P1801#4P1295#6P1500#8P2500T200\r","#0P2500#2P1801#4P934#6P1500#8P2500T200\r","#0P2500#2P1247#4P1801#6P1500#8P2500T200\r","#0P2500#2P1247#4P1801#6P1500#8P2500T1000\r","#0P2500#2P1295#4P1753#6P1500#8P2500T200\r","#0P2500#2P1319#4P1295#6P1536#8P2500T200\r","#0P2500#2P789#4P2235#6P1500#8P2500T200\r","#0P2500#2P789#4P2500#6P1500#8P2500T200\r","#0P2500#2P1295#4P2235#6P1500#8P2500T200\r","#0P2500#2P1801#4P1295#6P1500#8P2500T200\r","#0P2500#2P1801#4P934#6P1500#8P2500T200\r","#0P2500#2P1247#4P1801#6P1500#8P2500T200\r","#0P1970#2P500#4P861#6P1500#8P2500T100\r","#0P1970#2P500#4P861#6P1500#8P2500T700\r","#0P1970#2P500#4P1753#6P1500#8P2500T200\r","#0P1970#2P500#4P1753#6P1500#8P2500T800\r","#0P2500#2P1295#4P1753#6P1500#8P2500T200\r","#0P2500#2P1319#4P1295#6P1536#8P2500T200\r","#0P2500#2P789#4P2235#6P1500#8P2500T200\r","#0P2500#2P789#4P2500#6P1500#8P2500T200\r","#0P2500#2P1295#4P2235#6P1500#8P2500T200\r","#0P2500#2P1801#4P1295#6P1500#8P2500T200\r","#0P2500#2P1801#4P934#6P1500#8P2500T200\r","#0P2500#2P1247#4P1801#6P1500#8P2500T200\r","#0P2500#2P1295#4P1753#6P1500#8P2500T200\r","#0P2500#2P1319#4P1295#6P1536#8P2500T200\r","#0P2500#2P789#4P2235#6P1500#8P2500T200\r","#0P2500#2P789#4P2500#6P1500#8P2500T200\r","#0P2500#2P1295#4P2235#6P1500#8P2500T200\r","#0P2500#2P1801#4P1295#6P1500#8P2500T200\r","#0P2500#2P1801#4P934#6P1500#8P2500T200\r","#0P2500#2P1247#4P1801#6P1500#8P2500T200\r","#0P2500#2P1247#4P1801#6P1500#8P2500T800\r","#0P2500#2P1295#4P1753#6P1500#8P2500T200\r","#0P2500#2P1319#4P1295#6P1536#8P2500T200\r","#0P2500#2P789#4P2235#6P1500#8P2500T200\r","#0P2500#2P789#4P2500#6P1500#8P2500T200\r","#0P2500#2P1295#4P2235#6P1500#8P2500T200\r","#0P2500#2P1801#4P1295#6P1500#8P2500T200\r","#0P2500#2P1801#4P934#6P1500#8P2500T200\r","#0P2500#2P1247#4P1801#6P1500#8P2500T200\r","#0P2500#2P500#4P1801#6P1500#8P2500T100\r","#0P2500#2P500#4P1801#6P1500#8P2500T300\r","#0P2500#2P500#4P1801#6P1500#8P500T100\r","#0P2500#2P500#4P1801#6P1500#8P500T300\r","#0P2500#2P500#4P1801#6P693#8P500T100\r","#0P2500#2P500#4P1801#6P693#8P500T300\r","#0P1416#2P500#4P1801#6P693#8P2500T200\r","#0P1416#2P500#4P1801#6P693#8P2500T1000\r"
		};
		#endregion
		public frmShowTime(frmLocalCtrl frmParent)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			m_frmLocalCtrl = frmParent;
			
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
//			ControlArm(null);
		}
		
		public frmShowTime ()
		{
			InitializeComponent();
		}
		
		private void ControlArm(NetworkStream ns)
		{
			int i = 0;
			for (; i < strArmCmd.Length; i++)
			{
				byte[] bysTemp = GetArmData(strArmCmd[i]);
				//ns.Write(bysTemp, 0, bysTemp.Length);
				int s = int.Parse((strArmCmd[i].Split('T'))[1]);
				Thread.Sleep(s);
			}
			
		}
		
		private byte[] GetArmData(string str)
		{
			// 初始化手臂控制信息
			MemoryStream ms = new MemoryStream();
			ms.WriteByte(0x16);
			byte[] bysArm = UniConvertToAsc(str);
			ms.WriteByte((byte)bysArm.Length);
			ms.Write(bysArm, 0, bysArm.Length);
			return ms.ToArray();
		}
		
		/// <summary>
        /// 将 unicode 编码格式的字符串转换为 ascii 格式
        /// </summary>
        /// <param name="strSource">unicode 编码格式的字符</param>
        /// <returns>ascii 格式编码的 byte 数组</returns>
        private byte[] UniConvertToAsc(string strSource)
        {
            //        	string strDst;
            Encoding uniEncoder = Encoding.Unicode;
            Encoding ascEncoder = Encoding.ASCII;
            byte[] byUni = uniEncoder.GetBytes(strSource);
            byte[] byAsc = Encoding.Convert(uniEncoder, ascEncoder, byUni);
            //        	char[] chAsc = new char[ascEncoder.GetCharCount(byAsc)];
            //        	chAsc = ascEncoder.GetChars(byAsc);
            //        	strDst = new String(chAsc);
            return byAsc;
        } // end of EncodingConvert()
        
        public static IPAddress GetAddresses()
        {
            IPAddress [] aryLocalAddr = null;
            IPAddress ipResult;
            string strHostName = "";

            // NOTE: DNS lookups are nice and all but quite time consuming.
            strHostName = Dns.GetHostName();
#if USING_NET11
            IPHostEntry ipEntry = Dns.GetHostByName( strHostName );
#else
            IPHostEntry ipEntry = Dns.GetHostEntry( strHostName );
#endif
            aryLocalAddr = ipEntry.AddressList;

            // Verify we got an IP address.
            if (aryLocalAddr == null || aryLocalAddr.Length < 1)
            {
                throw new Exception("Unable to get local address");
            }
            else
            {
                ipResult = aryLocalAddr[0];
                foreach (IPAddress a in aryLocalAddr)
                {
                    if (a.ToString().Contains("192.168.1."))
                    {
                        ipResult = a;
                        break;
                    }
                } // end of foreach
            }

            return ipResult;
        }        
        /// <summary>
        /// 开启计时器
        /// </summary>
        private void StartTimer()
        {
        	if (!m_bTiming)
        	{
        		m_tmrQuit.Change(32000, 32000);
        		m_bTiming = true;
        	}
        }
        /// <summary>
        /// 关闭计时器
        /// </summary>
        private void StopTimer()
        {
        	if (m_bTiming)
        	{
        		m_tmrQuit.Change(Timeout.Infinite, Timeout.Infinite);
        		m_bTiming = false;
        	}
        }
        
        void TmrQuitTick(object state)
        {
        	StopTimer();
        	
        	this.Close();
        }
        
        void Trd()
        {
        	try
			{
				// 初始化 tcp 客户端
				m_tcpSender = new TcpClient();
				m_tcpSender.Connect(m_RobotCtrlIP, m_RobotCtrlPort);
				m_tcpSender.NoDelay = false;
				m_tcpSender.ReceiveTimeout = 5000;
				m_tcpSender.SendBufferSize = 2000;
				NetworkStream ns = m_tcpSender.GetStream();
				// 发送开始信息
				m_soundPlayer.Play(); // 播放音乐
				ns.Write(ByWheel, 0, ByWheel.Length); // 轮子开始表演
				StartTimer();
				ControlArm(ns); // 手臂开始表演
				ns.Close();
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}			
        }
        
		void FrmShowTimeLoad(object sender, EventArgs e)
		{
			// 允许在不同线程中使用控件
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
			
			if (File.Exists(MusicPath))
				m_soundPlayer = new SoundPlayer(MusicPath);
			else
				MessageBox.Show("文件[" + MusicPath + "]不存在");
			
			m_RobotCtrlIP = GetAddresses().ToString();//"192.168.1.7";
			
			// 初始化计时器
			m_tmrQuit = new System.Threading.Timer(
				TmrQuitTick, "", Timeout.Infinite, Timeout.Infinite);
			
			ThreadStart s = new ThreadStart(Trd);
			Thread t = new Thread(s);
			t.IsBackground = true;
			t.Start();
			
			StartTimer();
		}
		
		void FrmShowTimeFormClosed(object sender, FormClosedEventArgs e)
		{
			m_tcpSender.Close();
			m_frmLocalCtrl.Show();
		}
	}
}
