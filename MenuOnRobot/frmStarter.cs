using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Net;
using System.Net.Sockets;

using System.Media;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SpeechLib;

namespace MenuOnRobot
{
    public delegate void WriteLogEventHandler(string str);
    public delegate void ShowAlertFormEventHandler();

    public partial class frmStarter : Office2007Form
    {
    	#region 成员变量
        frmRemoteCam m_frmRemoteCam;
        frmLocalCtrl m_frmLocalCtrl;
        
        private UdpClient m_udpClient;
        private readonly IPEndPoint LOCAL_ADDRESS;
        private const int PORT = 1899;
        private const string strIPHead = "192.168.1";
        private volatile bool m_bUdpDone = false;
        
        string exePath = @"D:\ZigbeeHoset.exe";
        System.Diagnostics.Process m_exePrcs = null;
        
        // 安保
        private bool m_bDoorIsOpen = false;
        private frmBreakIn m_frmBreak = null;
        private byte lastData = 0;
        private System.Threading.Timer m_tmrAlert;
        private bool IsTiming = false;
        private bool BreakInHappened = false;
        private ShowAlertFormEventHandler ShowAlertForm;
        private ShowAlertFormEventHandler CloseAlertForm;
        
        private event SensorDataRcvEventHandler BreakInAlert;
        
        // 语音
        SpeechVoiceSpeakFlags m_spFlags;
        SpVoice m_voice;
//        Thread m_trdSpeak;
        
        // 激活窗口（使窗口临时位于最上层，并获得焦点）
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);
        
        #endregion 成员变量
        public frmStarter()
        {
            InitializeComponent();
            this.EnableGlass = false;
            
            LOCAL_ADDRESS = new IPEndPoint(_GetLocalIP(), PORT);
            m_udpClient = new UdpClient(LOCAL_ADDRESS);
            
            BreakInAlert += new SensorDataRcvEventHandler(OnBreakInAlert);
            ShowAlertForm = new ShowAlertFormEventHandler(OnShowAlertForm);
            CloseAlertForm = new ShowAlertFormEventHandler(OnCloseAlertForm);
            
            m_tmrAlert = new System.Threading.Timer(
            	TmrAlertTick, "", Timeout.Infinite,	Timeout.Infinite);
            
            m_spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            m_voice = new SpVoice();
        }
        
        #region 接收报警信息        
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
        
        private void UdpCallBack(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)(ar.AsyncState);
            IPEndPoint e = null;

            Byte[] receiveBytes = u.EndReceive(ar, ref e);
            
            if (receiveBytes.Length > 5)
            	BreakInAlert(new SensorData(receiveBytes));
            
            if (!m_bUdpDone)
            {
            	u.BeginReceive(UdpCallBack, u);
            }
        }
        /// <summary>
        /// 执行小偷驱逐动作~
        /// </summary>
        /// <param name="sd"></param>
        private void OnBreakInAlert(SensorData sd)
        {
        	System.Diagnostics.Debug.WriteLine(
        		string.Format(
        			"data[2] = {0}, data[3] = {1}", 
        			sd.Data[2].ToString(), 
        			sd.Data[3].ToString()
        		));
        	if (sd.Data[2] == (byte)0)
        	{
        		m_bDoorIsOpen = false;
        		lastData = 0;
        		
        		BreakInHappened = false;
        		((Control)this).Invoke(CloseAlertForm);
        	}
        	else
        	{
        		m_bDoorIsOpen = true;
        		
        		if (AlertJudge(sd.Data[3]))
        		{ // 当前状态为 入侵报警
        			if (!BreakInHappened)
        			{ // 上一状态为 安全        				
	        			if (null == m_frmBreak)
	        			{ // 实际执行的状态为 安全
	        				StartTimer();
	        			} // end of if
	        			else
	        			{ // 实际执行的状态为 报警
	        				StopTimer();
	        			}
	        			// 更新上一状态
	        			BreakInHappened = true;
        			} // end of if
        			else
        			{ // 若上一状态为报警，则什么也不做
        			} // end of else
        		} // end of if
        		else
        		{ // 当前状态为 安全
        			if (BreakInHappened)
        			{ // 上一状态为 报警	        			
	        			if (null != m_frmBreak)
	        			{ // 实际执行的状态为 报警
	        				StartTimer();
	        			} // end of if
	        			else
	        			{ // 实际执行的状态为 安全
	        				StopTimer();
	        			}
	        			BreakInHappened = false;
        			} // end of if
        			else
        			{ // 若上一状态为安全，则什么也不做
        			} // end of else
        		} // end of else
        	} // end of else        	
        }
        
        private bool AlertJudge(int s)
        {
        	int temp = s;
        	
        	if (s < lastData && lastData - s > 50)
        		temp = lastData;
        	else
        		lastData = (byte)temp;
        	
        	if (temp < 60)
        		return true;
        	else
        		return false;
        }
        
        private void StartTimer()
        {
        	if (!IsTiming)
        	{
        		m_tmrAlert.Change(1500,1500);
        		IsTiming = true;
        	}
        }
        
        private void StopTimer()
        {
        	if (IsTiming)
        	{
        		m_tmrAlert.Change(Timeout.Infinite, Timeout.Infinite);
        		IsTiming = false;
        	}
        }
        
        private void OnShowAlertForm()
        {
        	if (null != m_frmBreak)
        		return;
        	
        	m_frmBreak = new frmBreakIn();
        	m_frmBreak.Show();
        }
        
        private void OnCloseAlertForm()
        {
        	if (null == m_frmBreak)
        		return;
        	
        	m_frmBreak.Close();
	       	m_frmBreak.Dispose();
	       	m_frmBreak = null;
        }
        
        private void TmrAlertTick(object state)
        {
        	StopTimer();
        	
        	// 转换状态
        	if (null == m_frmBreak)
        	{
        		((Control)this).Invoke(ShowAlertForm);
	        	//frmStarter m_frmBreak.Show();
	        	// SetForegroundWindow(m_frmBreak.Handle);
	        	System.Diagnostics.Debug.WriteLine("alert!!!");
        	}
        	else
        	{
	        	((Control)this).Invoke(CloseAlertForm);
	        	System.Diagnostics.Debug.WriteLine("stop alert~~~");
        	} // end of else
        }
        #endregion 接收报警信息
        
//        private void TrdSpeak()
//        {
//        	m_voice.Resume();
//        	m_voice.Speak(strIntroduce, m_spFlags);
//        	this.btnIntroduce.Tag = "1";
//        }

        #region 窗口事件处理
        private void btnExit_Click(object sender, EventArgs e)
        {
        	m_voice.Speak("退出", SpeechVoiceSpeakFlags.SVSFlagsAsync);
            this.Close();
        }

        private void btnRemoteCam_Click(object sender, EventArgs e)
        {
            this.Hide();
            m_voice.Speak("远程聊天功能", SpeechVoiceSpeakFlags.SVSFlagsAsync);
            m_frmRemoteCam = new frmRemoteCam(this);
            m_frmRemoteCam.Show();
        }
        
        private void btnLocalCtrl_Click(object sender, EventArgs e)
        {
            this.Hide();
            m_voice.Speak("本地控制功能", SpeechVoiceSpeakFlags.SVSFlagsAsync);
            m_frmLocalCtrl = new frmLocalCtrl(this);
            m_frmLocalCtrl.Show();
        }

        string strWelcome = "欢迎使用新概念网络型室内体感服务系统";

        private void frmStarter_Load(object sender, EventArgs e)
        {
        	try
        	{
        		m_voice.Speak(strWelcome, m_spFlags);
        		
        		m_exePrcs = new System.Diagnostics.Process();
        		m_exePrcs.StartInfo.FileName = exePath;
        		// 不显示窗口
        		m_exePrcs.StartInfo.CreateNoWindow = true;
        		m_exePrcs.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        		m_exePrcs.Start();
        		
        		System.Threading.Thread.Sleep(100);
        		
        	}
        	catch (Exception ex)
        	{
        		
        	}
        	
        	m_udpClient.BeginReceive(UdpCallBack, m_udpClient);
        }
        
        void FrmStarterFormClosing(object sender, FormClosingEventArgs e)
        {
        	try
        	{
        		m_bUdpDone = true;
        		byte[] by = {0};
        		m_udpClient.Send(by, by.Length, LOCAL_ADDRESS);    
        		
        		m_exePrcs.Kill();
        	}
        	catch{}
        }
        
        string strIntroduce = "各位观众，大家好！我是扎克，新概念网络型室内体感服务系统的中央机器人。" + 
        	"我可以作为您的服务管家，为您管理室内环境。您可以了解当前环境温度，湿度，光照强度。" + 
        	"也可以通过我开关电灯和窗帘。同时我也为室内的安全保驾护航。" + 
        	"如果您不在房屋内，您也可以通过互联网登录，使用我的替身机器人功能。" + 
        	"我竟成为您的替身，我的面部将会显示您计算机摄像头采集到的实时图像，" + 
        	"您也可以通过软件查看房间环境，并同房间内其他人交流。" + 
        	"同时，使用体感手套，我的手臂也将模仿您右手的动作。我的顺利运行离不开外围设备的支持。" + 
        	"体感手套除了模仿功能外还可以开关电灯和窗帘，也可以当做空中鼠标或游戏控制器使用。" + 
        	"外围的z个b传感器网络将分布在室内各个角落的传感器信息发送到我这里集中处理。所以我不是一个孤独的机器人o";
        
        void BtnIntroduceClick(object sender, EventArgs e)
        {
        	PulseButton.PulseButton btnSender = (PulseButton.PulseButton)sender;
        	if ((string)btnSender.Tag == "1")
        	{
        		btnSender.Tag = "2";
        		m_voice.Speak(strIntroduce, m_spFlags);

//				ThreadStart s = new ThreadStart(TrdSpeak);
//				m_trdSpeak = new Thread(s);
//				m_trdSpeak.IsBackground = true;
//				m_trdSpeak.Start();
        	}
        	else
        	{
        		btnSender.Tag = "1";
        		m_voice.Speak(string.Empty, SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
//				m_trdSpeak.Abort();
        	}
        }
        #endregion 窗口事件处理
        
        void BtnRecognitionClick(object sender, EventArgs e)
        {
        	PulseButton.PulseButton btn = (PulseButton.PulseButton)sender;
        	if ("1" == (string)btn.Tag)
        	{
        		btn.Tag = "2";
        		btn.Text = "关闭语音识别";
        	}
        	else
        	{
        		btn.Tag = "1";
        		btn.Text = "开启语音识别";
        	}
        }
    }  

}
