using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using System.IO;
using System.Threading;

namespace DirectShow
{
    public delegate void VoiceDataReadyEventHandler(byte[] byVoiceData);
    public delegate void VoiceDataFinishEventHandler();
    class VoiceRecorder
    {
        public VoiceRecorder()
        {
            m_wavFormat = WavFile.GetDefalultWavFormat();
        }

        public VoiceRecorder(WaveFormat wf)
        {
            m_wavFormat = wf;
        } // end of VoiceRecorder()

        #region 成员变量
        private string m_strSaveFileName = string.Empty;
        private Notify m_ntfBufIsFull = null;   // 缓冲区被填满时触发通知
        private int m_nNotifyNum = 16;          // 通知数量
        private int m_nNotifySize = 0;          // 通知触发间隔
        private int m_nBufSize = 0;
        private int m_nBufOffset = 0;           // 本次写入的起点
        private int m_nSampleSize = 0;          // 采集到的数据大小
        Capture m_objCapture = null;            // 声音采集设备
        CaptureBuffer m_objCaptureBuf = null;   // 声音采集使用的缓冲区
        AutoResetEvent m_areNotifyEvent = null; // 收到通知时释放信号量
        Thread m_trdNodify = null;              // 收到通知时执行
        WaveFormat m_wavFormat;                 // 音频采集格式信息（同 wav 文件格式设置）        
        #endregion

        private event VoiceDataReadyEventHandler _VoiceDataReady;
        public event VoiceDataReadyEventHandler VoiceDataReady
        {
            add { _VoiceDataReady = value; }
            remove { if (_VoiceDataReady != null) _VoiceDataReady -= value; }
        }
        private event VoiceDataFinishEventHandler _VoiceDataFinish;
        public event VoiceDataFinishEventHandler VoiceDataFinish
        {
            add { _VoiceDataFinish = value; }
            remove { if (_VoiceDataFinish != null) _VoiceDataFinish -= value; }
        }
        #region 已转移到 WavFile 中
        /// <summary>
        /// 设置 wav 文件格式
        /// </summary>
        /// <returns>文件格式</returns>
        //private WaveFormat SetWaveFormat()
        //{
        //    WaveFormat format = new WaveFormat();
        //    format.FormatTag = WaveFormatTag.Pcm;   //设置音频类型
        //    format.SamplesPerSecond = 22050;        //采样率（单位：赫兹）典型值：11025、22050、44100Hz
        //    format.BitsPerSample = 16;              //采样位数
        //    format.Channels = 1;                    //声道
        //    format.BlockAlign = (short)(format.Channels * (format.BitsPerSample / 8));  //单位采样点的字节数
        //    format.AverageBytesPerSecond = format.BlockAlign * format.SamplesPerSecond;
        //    return format;
        //    //按照以上采样规格，可知采样1秒钟的字节数为22050*2=55100B 约为 53K
        //} // end of  SetWaveFormat()
        /// <summary>
        /// 创建 wav 文件，并写入文件头
        /// </summary>
        /// <param name="strFileName">文件名</param>
        //private void CreateWaveFile(BinaryWriter bwWav)
        //{
        //    //m_fsWav = new FileStream(strFileName, FileMode.CreateNew);
        //    //m_bwWav = new BinaryWriter(m_fsWav);

        //    /**************************************************************************
        //       Here is where the file will be created. A
        //       wave file is a RIFF file, which has chunks
        //       of data that describe what the file contains.
        //       A wave RIFF file is put together like this:
        //       The 12 byte RIFF chunk is constructed like this:
        //       Bytes 0 - 3 :  'R' 'I' 'F' 'F'
        //       Bytes 4 - 7 :  Length of file, minus the first 8 bytes of the RIFF description.
        //                         (4 bytes for "WAVE" + 24 bytes for format chunk length +
        //                         8 bytes for data chunk description + actual sample data size.)
        //        Bytes 8 - 11: 'W' 'A' 'V' 'E'
        //        The 24 byte FORMAT chunk is constructed like this:
        //        Bytes 0 - 3 : 'f' 'm' 't' ' '
        //        Bytes 4 - 7 : The format chunk length. This is always 16.
        //        Bytes 8 - 9 : File padding. Always 1.
        //        Bytes 10- 11: Number of channels. Either 1 for mono,  or 2 for stereo.
        //        Bytes 12- 15: Sample rate.
        //        Bytes 16- 19: Number of bytes per second.
        //        Bytes 20- 21: Bytes per sample. 1 for 8 bit mono, 2 for 8 bit stereo or
        //                        16 bit mono, 4 for 16 bit stereo.
        //        Bytes 22- 23: Number of bits per sample.
        //        The DATA chunk is constructed like this:
        //        Bytes 0 - 3 : 'd' 'a' 't' 'a'
        //        Bytes 4 - 7 : Length of data, in bytes.
        //        Bytes 8 -: Actual sample data.
        //      ***************************************************************************/
        //    char[] ChunkRiff = { 'R', 'I', 'F', 'F' };
        //    char[] ChunkType = { 'W', 'A', 'V', 'E' };
        //    char[] ChunkFmt = { 'f', 'm', 't', ' ' };
        //    char[] ChunkData = { 'd', 'a', 't', 'a' };
        //    short shPad = 1;                // File padding
        //    int nFormatChunkLength = 0x10;  // Format chunk length.
        //    int nLength = 0;                // File length, minus first 8 bytes of RIFF description. This will be filled in later.
        //    short shBytesPerSample = 0;     // Bytes per sample.
        //    // 一个样本点的字节数目
        //    if (8 == m_wavFormat.BitsPerSample && 1 == m_wavFormat.Channels)
        //        shBytesPerSample = 1;
        //    else if ((8 == m_wavFormat.BitsPerSample && 2 == m_wavFormat.Channels) || (16 == m_wavFormat.BitsPerSample && 1 == m_wavFormat.Channels))
        //        shBytesPerSample = 2;
        //    else if (16 == m_wavFormat.BitsPerSample && 2 == m_wavFormat.Channels)
        //        shBytesPerSample = 4;
        //    // RIFF 块
        //    bwWav.Write(ChunkRiff);
        //    bwWav.Write(nLength);
        //    bwWav.Write(ChunkType);
        //    // WAVE块
        //    bwWav.Write(ChunkFmt);
        //    bwWav.Write(nFormatChunkLength);
        //    bwWav.Write(shPad);
        //    bwWav.Write(m_wavFormat.Channels);
        //    bwWav.Write(m_wavFormat.SamplesPerSecond);
        //    bwWav.Write(m_wavFormat.AverageBytesPerSecond);
        //    bwWav.Write(shBytesPerSample);
        //    bwWav.Write(m_wavFormat.BitsPerSample);
        //    // 数据块
        //    bwWav.Write(ChunkData);
        //    bwWav.Write((int)0);   // The sample length will be written in later.
        //} // end of CreateWaveFile()
        #endregion
        /// <summary>
        /// 创建音频捕捉设备对象
        /// </summary>
        /// <returns>创建是否成功</returns>
        private bool CreateCaptureDevice()
        {
            CaptureDevicesCollection captureDevs = new CaptureDevicesCollection();
            Guid guidDev;
            if (captureDevs.Count > 0)
            {
                guidDev = captureDevs[0].DriverGuid;
            } // end of if
            else
            {
                MessageBox.Show("没有找到音频捕捉设备，请确认麦克风是否插好");
                return false;
            } // end of else
            m_objCapture = new Capture(guidDev);
            return true;
        } // end of CreateCaputerDevice()
        /// <summary>
        /// 创建缓冲区
        /// </summary>
        private void CreateCaptureBuffer()
        { // 想要创建一个捕捉缓冲区必须要两个参数：缓冲区信息（描述这个缓冲区中的格式等），缓冲设备。
            CaptureBufferDescription cbdBufDescription = new CaptureBufferDescription();
            cbdBufDescription.Format = m_wavFormat; // 缓冲区捕捉的数据格式
            // 1秒的数据量/设置的通知数 得到的每个通知大小小于0.2s的数据量，话音延迟小于200ms为优质话音
            m_nNotifySize = m_wavFormat.AverageBytesPerSecond / m_nNotifyNum;
            m_nBufSize = m_nNotifyNum * m_nNotifySize;
            cbdBufDescription.BufferBytes = m_nBufSize;
            m_objCaptureBuf = new CaptureBuffer(cbdBufDescription, m_objCapture);
        } // end of CreateCaptureBuffer()
        /// <summary>
        /// 创建通知
        /// </summary>
        private void CreateNotification()
        {
            BufferPositionNotify[] bpn = new BufferPositionNotify[m_nNotifyNum];//设置缓冲区通知个数
            //设置通知事件
            m_areNotifyEvent = new AutoResetEvent(false);
            m_trdNodify = new Thread(RecoData);
            m_trdNodify.IsBackground = true;
            m_trdNodify.Start();
            for (int i = 0; i < m_nNotifyNum; i++)
            {
                bpn[i].Offset = m_nNotifySize + i * m_nNotifySize - 1;//设置具体每个的位置
                bpn[i].EventNotifyHandle = m_areNotifyEvent.SafeWaitHandle.DangerousGetHandle();
            }
            m_ntfBufIsFull = new Notify(m_objCaptureBuf);
            m_ntfBufIsFull.SetNotificationPositions(bpn);

        } // end of CreateNotification()
        /// <summary>
        /// 线程中的事件
        /// </summary>
        private void RecoData()
        {
            while (true)
            {
                // 等待缓冲区的通知消息
                m_areNotifyEvent.WaitOne(Timeout.Infinite, true);
                // 录制数据
                RecordCapturedData();
                //Thread.Sleep(200);
            }
        } // end of RecoData()
        /// <summary>
        /// 真正转移数据的事件，其实就是执行 VoiceDataReady 事件
        /// </summary>
        private void RecordCapturedData()
        {
            byte[] capturedata = null;
            int readpos = 0, capturepos = 0, locksize = 0;
            m_objCaptureBuf.GetCurrentPosition(out capturepos, out readpos);
            locksize = readpos - m_nBufOffset;  // 这个大小就是我们可以安全读取的大小
            if (locksize == 0)
            {
                return;
            }
            try
            {
                capturedata = (byte[])m_objCaptureBuf.Read(m_nBufOffset, typeof(byte), LockFlag.FromWriteCursor, (locksize + m_nBufSize) % m_nBufSize);
                _VoiceDataReady(capturedata);
            }
            catch (Exception ex)
            {
            }

            m_nSampleSize += capturedata.Length;
            m_nBufOffset += capturedata.Length;
            m_nBufOffset %= m_nBufSize; //取模是因为缓冲区是循环的。
        } // end of RecordCapturedData()
        /// <summary>
        /// 开始录制声音
        /// </summary>
        public bool StartRecord()
        {
            #region 为成员变量附初值(调用顺序不可随意改变)
            if(!CreateCaptureDevice()) return false;
            CreateCaptureBuffer();
            CreateNotification();
            #endregion
            m_objCaptureBuf.Start(true);
            return true;
        } // end of StartRecord()
        /// <summary>
        /// 停止录音
        /// </summary>
        public void StopRecord()
        {
            m_objCaptureBuf.Stop();
            if (m_ntfBufIsFull != null)
                m_areNotifyEvent.Set();
            if (m_trdNodify != null && m_trdNodify.IsAlive)
                m_trdNodify.Abort();
            RecordCapturedData();
            _VoiceDataFinish();
        } // end of StopRecord()
    }
}
