using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;
using System.IO;
using System.Threading;

namespace MenuOnRobot
{
    public delegate void ReadyForNewDataEventHandler(SecondaryBuffer secBuf);
    class VoicePlayer
    {
        private Device m_playDev;   // 声音播放设备
        private IntPtr m_plHandle;  // 窗口句柄
        private WaveFormat m_wavFormat; // 声音播放格式
        private BufferDescription m_bufDesc; // 辅助缓冲区格式描述
        private Notify m_ntfNeedNewData;
        private int m_nNotifyNum = 16;       // 通知数量
        private int m_nNotifySize = 0;       // 通知触发间隔
        private int m_nBufSize = 0;          // 辅助缓冲区的大小
        private Thread m_trdNodify;
        private AutoResetEvent m_areNeedNewData;
        private bool m_bIsRunning;
        private SecondaryBuffer m_secBuf;
        //private MemoryStream m_msBuf;
        
        private event ReadyForNewDataEventHandler _ReadyForNewData;
        public event ReadyForNewDataEventHandler ReadyForNewData
        {
            add { _ReadyForNewData = value; }
            remove { if (_ReadyForNewData != null) _ReadyForNewData -= value; }
        }
        public int nNotifySize
        {
            get { return m_nNotifySize; }
        }
        public int nBufSize
        {
            get { return m_nBufSize; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pl">窗口句柄</param>
        public VoicePlayer(IntPtr pl)
        {
           _VoicePlayer(pl, WavFile.GetDefalultWavFormat());            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pl">窗口句柄</param>
        /// <param name="wf">声音播放格式</param>
        public VoicePlayer(IntPtr pl, WaveFormat wf)
        {
            _VoicePlayer(pl, wf);
        }
        private void _VoicePlayer(IntPtr pl, WaveFormat wf)
        {
            m_plHandle = pl;
            m_wavFormat = wf;
        }
        /// <summary>
        /// 创建用于播放的音频设备对象
        /// </summary>
        /// <returns>创建成功返回true</returns>
        private bool CreatePlayDevice()
        {
            DevicesCollection dc = new DevicesCollection();
            Guid g;
            if (dc.Count > 0)
            {
                g = dc[0].DriverGuid;
            }
            else
            { return false; }
            m_playDev = new Device(g);
            m_playDev.SetCooperativeLevel(m_plHandle, CooperativeLevel.Normal);
            return true;
        }
        /// <summary>
        /// 创建辅助缓冲区
        /// </summary>
        private void CreateSecondaryBuffer()
        {
            m_bufDesc = new BufferDescription();           
            
            m_nNotifySize   = m_wavFormat.AverageBytesPerSecond / m_nNotifyNum;
            //m_nNotifyNum 	= m_nNotifyNum / 4;
            m_nBufSize      = m_nNotifySize * m_nNotifyNum; // 缓存一秒的音频数据
			
            m_bufDesc.Format             = m_wavFormat;
            m_bufDesc.BufferBytes        = m_nBufSize;
            m_bufDesc.ControlPositionNotify = true;
            m_bufDesc.CanGetCurrentPosition = true;
            m_bufDesc.ControlPan         = true; // 允许控制左右声道
            m_bufDesc.ControlFrequency   = true;
            m_bufDesc.ControlVolume      = true;
            m_bufDesc.GlobalFocus        = true; // 使用全局缓存

            m_secBuf = new SecondaryBuffer(m_bufDesc, m_playDev);
            //byte[] bytMemory = new byte[100000];
            //m_msBuf = new MemoryStream(bytMemory, 0, 100000, true, true);
            //g729 = new G729();
            //g729.InitalizeEncode();
            //g729.InitalizeDecode();
        }
        /// <summary>
        /// 创建通知
        /// </summary>
        private void CreateNotification()
        {
            BufferPositionNotify[] bpn = new BufferPositionNotify[m_nNotifyNum];//设置缓冲区通知个数
            //设置通知事件
            m_areNeedNewData = new AutoResetEvent(false);
            m_trdNodify = new Thread(PlayData);
            m_trdNodify.IsBackground = true;
            m_trdNodify.Start();
            for (int i = 0; i < m_nNotifyNum; i++)
            {
                bpn[i].Offset = m_nNotifySize + i * m_nNotifySize - 1;//设置具体每个的位置
                bpn[i].EventNotifyHandle = m_areNeedNewData.SafeWaitHandle.DangerousGetHandle();
            }
            m_ntfNeedNewData = new Notify(m_secBuf);
            m_ntfNeedNewData.SetNotificationPositions(bpn);
        } // end of CreateNotification()
        /// <summary>
        /// 线程中的事件
        /// </summary>
        private void PlayData()
        {
            while (m_bIsRunning)
            {
                // 等待缓冲区的通知消息
                m_areNeedNewData.WaitOne(Timeout.Infinite, true);
                
                if (m_bIsRunning)
                {// 写新数据	                
	                _ReadyForNewData(m_secBuf);
                }
                //Thread.Sleep(200);
            }
        } // end of RecoData()
        

        public void Start()
        {
            m_bIsRunning = true;
            CreatePlayDevice();
            CreateSecondaryBuffer();
            CreateNotification();
            m_secBuf.Play(0, BufferPlayFlags.Looping);
        }

        public void Stop()
        {
            m_bIsRunning = false;
            m_secBuf.Stop();
            if (null != m_areNeedNewData)
                m_areNeedNewData.Set();
            if (null != m_trdNodify)
                m_trdNodify.Abort();
        }
    }
}
