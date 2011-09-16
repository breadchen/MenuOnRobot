/*
 * Created by SharpDevelop.
 * User: cm
 * Date: 2011-6-11
 * Time: 19:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using System.Media;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

namespace MenuOnRobot
{
	/// <summary>
	/// Description of frmBreakIn.
	/// </summary>
	public partial class frmBreakIn : Form
	{
		private SoundPlayer m_spPlayer = null;
		private string MusicPath = @"D:\alert.wav";
		private string PicPath = @"D:\110.jpg";
		
		private TcpClient m_tcpClient = null;
		private string m_RobotControlIP = null;
		private int m_RobotControlPort = 1599;
		private byte[] bysStart = {0x22,4,3,3,3,3};
		private byte[] bysStop = {0x22,4,3,3,3,0};
		
		 // 激活窗口（使窗口临时位于最上层，并获得焦点）
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);
        		
		public frmBreakIn()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			try
			{
				m_RobotControlIP = frmShowTime.GetAddresses().ToString();
				IPEndPoint ep = new IPEndPoint(
					frmShowTime.GetAddresses(), 
					m_RobotControlPort);
				m_tcpClient = new TcpClient();
				m_tcpClient.Connect(ep);
				m_tcpClient.NoDelay = false;
				m_tcpClient.ReceiveBufferSize = 5000;
				m_tcpClient.SendBufferSize = 2000;				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		NetworkStream ns = null;
		void FrmBreakInLoad(object sender, EventArgs e)
		{
			ns = m_tcpClient.GetStream();
			ns.Write(bysStart, 0, bysStart.Length);
			
			pictureBox1.Image = new Bitmap(PicPath);
			m_spPlayer = new SoundPlayer(MusicPath);
			m_spPlayer.PlayLooping();
			SetForegroundWindow(this.Handle);
		}
		
		void FrmBreakInFormClosing(object sender, FormClosingEventArgs e)
		{
//			NetworkStream ns = m_tcpClient.GetStream();
			ns.Write(bysStop, 0, bysStop.Length);
			
			ns.Close();
			
			m_spPlayer.Stop();
		}
	}
}
