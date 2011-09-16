using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevComponents.DotNetBar;
using System.IO;
using System.Diagnostics;
using SpeechLib;

namespace MenuOnRobot
{
    public partial class frmLocalCtrl : Form
    {
        #region 成员变量
        private frmStarter      m_frmStarter;       // 上级窗口
        // 下级窗口
        private frmStatus       m_frmStatus;        // 状态信息
        private frmInteropCtrl  m_frmInteropCtrl;   // 交互体感控制
        private frmInfo         m_frmInfo;          // 智能家居
        private frmShowTime		m_frmShowTime;		// 表演时刻
        // 日志文件写入器
        private StreamWriter m_swStatus;
        private StreamWriter m_swInteropCtrl;
        private StreamWriter m_swInfo;
        // 语音控制
        SpVoice m_voice;
        #endregion

        public frmLocalCtrl(frmStarter fs)
        {
            InitializeComponent();
            m_frmStarter = fs;
            m_voice = new SpVoice();
        }

        private void frmLocalCtrl_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_frmStarter.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
        	m_voice.Speak("返回", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        	#region 关闭打开的日志文件
        	if (null != m_swStatus)
        		m_swStatus.Close();
        	if (null != m_swInteropCtrl)
        		m_swInteropCtrl.Close();
        	if (null != m_swInfo)
        		m_swInfo.Close();
        	#endregion
            this.Close();
        }

        private string m_strInteropCtrlCaption = "体感交互控制";
        private string m_strInteropCtrlText    = "使用本功能请佩戴体感交互手套及身份识别模块";
        private string m_strInfoCaption        = "家电中央控制器";
        private string m_strInfoText           = "使用本功能可以代替红外遥控器";
        private string m_strStatusCaption      = "室内信息监控";
        private string m_strStatusText         = "查看室内的环境信息，以及控制服务机器人";

        private const string STR_LOG_STATUS       = @"\logStatus.log";
        private const string STR_LOG_INTEROP_CTRL = @"\logInteropCtrl.log";
        private const string STR_LOG_INFO         = @"\logInfo.log";

        private void frmLocalCtrl_Load(object sender, EventArgs e)
        {
            #region 填写气球控件信息
            balloonTips.SetBalloonCaption(btnInteropCtrl, m_strInteropCtrlCaption);
            balloonTips.SetBalloonText(btnInteropCtrl, m_strInteropCtrlText);
            balloonTips.SetBalloonCaption(btnInfo, m_strInfoCaption);
            balloonTips.SetBalloonText(btnInfo, m_strInfoText);
            balloonTips.SetBalloonCaption(btnStatus, m_strStatusCaption);
            balloonTips.SetBalloonText(btnStatus, m_strStatusText);
            #endregion

            #region 打开日志文件
            m_swStatus = File.AppendText(Directory.GetCurrentDirectory() + STR_LOG_STATUS);
            m_swInteropCtrl = File.AppendText(Directory.GetCurrentDirectory() + STR_LOG_INTEROP_CTRL);
            m_swInfo = File.AppendText(Directory.GetCurrentDirectory() + STR_LOG_INFO);
            #endregion
        }

        #region 日志文件相关
        private void WriteLogStatus(string str)
        {
            WriteLog(str, m_swStatus);
        } // end of WriteLogStatus()

        private void WriteLogInteropCtrl(string str)
        {
            WriteLog(str, m_swInteropCtrl);
        } // end of WriteLogInteropCtrl()

        private void WriteLogInfo(string str)
        {
            WriteLog(str, m_swInfo);
        } // end of WriteLogInfo()

        private void WriteLog(string str, StreamWriter sw)
        {
            string strTemp = str + " " + DateTime.Now.ToString();
            // 测试代码（这里必须制定所有者为 当前窗口this，否则有可能会看不到消息框）
            //MessageBox.Show(this, strTemp);
            try
            {
                sw.WriteLine(strTemp);
                sw.Flush();
#if DEBUG
                Debug.WriteLine(strTemp);
#endif
            }
            catch { }
        } // end of WriteLog()
		#endregion
        
        private void btnInteropCtrl_Click(object sender, EventArgs e)
        {
            this.Hide();
            m_voice.Speak(m_strInteropCtrlCaption, 
                          SpeechVoiceSpeakFlags.SVSFlagsAsync);
            m_frmInteropCtrl = new frmInteropCtrl(this);
            m_frmInteropCtrl.WriteLog += new WriteLogEventHandler(WriteLogInteropCtrl);
            m_frmInteropCtrl.Show();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            this.Hide();
            m_voice.Speak(m_strInfoCaption, 
                          SpeechVoiceSpeakFlags.SVSFlagsAsync);
            m_frmInfo = new frmInfo(this);
            m_frmInfo.WriteLog += new WriteLogEventHandler(WriteLogInfo);
            m_frmInfo.Show();
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            this.Hide();
            m_voice.Speak(m_strStatusCaption, 
                          SpeechVoiceSpeakFlags.SVSFlagsAsync);
            m_frmStatus = new frmStatus(this);
            m_frmStatus.WriteLog += new WriteLogEventHandler(WriteLogStatus);
            m_frmStatus.Show();
        }
        
        void BtnShowTimeClick(object sender, EventArgs e)
        {
        	this.Hide();
        	m_frmShowTime = new frmShowTime(this);
        	m_frmShowTime.Show();
//        	frmBreakIn frmtemp = new frmBreakIn();
//        	frmtemp.Show();
        }
    }
}
