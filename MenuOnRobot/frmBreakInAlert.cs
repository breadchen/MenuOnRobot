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

namespace MenuOnRobot
{
	class frmBreakInAlert : frmShowTime
	{
		private SoundPlayer m_spPlayer = null;
		private string MusicPath = @"D:\robotDancing.wav";
		private string PicPath = @"D:\110.jpg";
		
		public frmBreakInAlert() : base()
		{			
			
		}
		
		
	}
}
