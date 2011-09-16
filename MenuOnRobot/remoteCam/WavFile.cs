using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.DirectSound;
using System.IO;

namespace MenuOnRobot
{
    class WavFile
    {
        private WaveFormat m_wavFormat;     // 文件格式
        private string m_strFileName;       // 录音文件名
        private FileStream m_fsWavFile;
        private BinaryWriter m_bwWavWriter;
        private int m_nDataLength;          // 录音数据长度
        private bool m_bReadyToWrite;       // 文件打开标志位
        public WaveFormat FileFormat
        {
            get { return m_wavFormat; }
            set { m_wavFormat = value; }
        }

        public WavFile(string fileName)
        {             
            //WaveFormat format = new WaveFormat();
            #region 载入默认配置
            //format.FormatTag = WaveFormatTag.Pcm;   // 设置音频类型
            //format.SamplesPerSecond = 22050;        // 采样率（单位：赫兹）典型值：11025、22050、44100Hz
            //format.BitsPerSample = 16;              // 采样位数
            //format.Channels = 1;                    // 声道
            //// 单位采样点的字节数
            //format.BlockAlign = (short)(format.Channels * (format.BitsPerSample / 8));  
            //// 平均数据率
            //format.AverageBytesPerSecond = format.BlockAlign * format.SamplesPerSecond;
            #endregion
            _WavFile(WavFile.GetDefalultWavFormat(), fileName);
        }

        public WavFile(WaveFormat wf, string fileName)
        {
            _WavFile(wf, fileName);
        }

        private void _WavFile(WaveFormat wf, string fileName)
        {
            m_wavFormat = wf;
            m_strFileName = fileName;
        }
        /// <summary>
        /// 开始创建 wav 文件（ 创建文件、写入文件头、初始化文件长度）
        /// </summary>
        public void BeginCreateFile()
        {
            try
            {
                m_fsWavFile = new FileStream(m_strFileName, FileMode.Create);
                m_bwWavWriter = new BinaryWriter(m_fsWavFile);
                m_nDataLength = 0;
                m_bReadyToWrite = true;

                #region wav 文件头格式
                /**************************************************************************
               Here is where the file will be created. A
               wave file is a RIFF file, which has chunks
               of data that describe what the file contains.
               A wave RIFF file is put together like this:
               The 12 byte RIFF chunk is constructed like this:
               Bytes 0 - 3 :  'R' 'I' 'F' 'F'
               Bytes 4 - 7 :  Length of file, minus the first 8 bytes of the RIFF description.
                                 (4 bytes for "WAVE" + 24 bytes for format chunk length +
                                 8 bytes for data chunk description + actual sample data size.)
                Bytes 8 - 11: 'W' 'A' 'V' 'E'
                The 24 byte FORMAT chunk is constructed like this:
                Bytes 0 - 3 : 'f' 'm' 't' ' '
                Bytes 4 - 7 : The format chunk length. This is always 16.
                Bytes 8 - 9 : File padding. Always 1.
                Bytes 10- 11: Number of channels. Either 1 for mono,  or 2 for stereo.
                Bytes 12- 15: Sample rate.
                Bytes 16- 19: Number of bytes per second.
                Bytes 20- 21: Bytes per sample. 1 for 8 bit mono, 2 for 8 bit stereo or
                                16 bit mono, 4 for 16 bit stereo.
                Bytes 22- 23: Number of bits per sample.
                The DATA chunk is constructed like this:
                Bytes 0 - 3 : 'd' 'a' 't' 'a'
                Bytes 4 - 7 : Length of data, in bytes.
                Bytes 8 -: Actual sample data.
              ***************************************************************************/
                #endregion

                #region 写入文件头
                char[] ChunkRiff = { 'R', 'I', 'F', 'F' };
                char[] ChunkType = { 'W', 'A', 'V', 'E' };
                char[] ChunkFmt = { 'f', 'm', 't', ' ' };
                char[] ChunkData = { 'd', 'a', 't', 'a' };
                short shPad = 1;                // File padding
                int nFormatChunkLength = 0x10;  // Format chunk length.
                int nLength = 0;                // File length, minus first 8 bytes of RIFF description. This will be filled in later.
                short shBytesPerSample = 0;     // Bytes per sample.
                // 一个样本点的字节数目
                if (8 == m_wavFormat.BitsPerSample && 1 == m_wavFormat.Channels)
                    shBytesPerSample = 1;
                else if ((8 == m_wavFormat.BitsPerSample && 2 == m_wavFormat.Channels) || (16 == m_wavFormat.BitsPerSample && 1 == m_wavFormat.Channels))
                    shBytesPerSample = 2;
                else if (16 == m_wavFormat.BitsPerSample && 2 == m_wavFormat.Channels)
                    shBytesPerSample = 4;
                // RIFF 块
                m_bwWavWriter.Write(ChunkRiff);
                m_bwWavWriter.Write(nLength);
                m_bwWavWriter.Write(ChunkType);
                // WAVE块
                m_bwWavWriter.Write(ChunkFmt);
                m_bwWavWriter.Write(nFormatChunkLength);
                m_bwWavWriter.Write(shPad);
                m_bwWavWriter.Write(m_wavFormat.Channels);
                m_bwWavWriter.Write(m_wavFormat.SamplesPerSecond);
                m_bwWavWriter.Write(m_wavFormat.AverageBytesPerSecond);
                m_bwWavWriter.Write(shBytesPerSample);
                m_bwWavWriter.Write(m_wavFormat.BitsPerSample);
                // 数据块
                m_bwWavWriter.Write(ChunkData);
                m_bwWavWriter.Write((int)0);   // The sample length will be written in later.
                #endregion
            }
            catch
            {
                m_bReadyToWrite = false;
                m_fsWavFile = null;
                m_bwWavWriter = null;
            }
        }
        /// <summary>
        /// 写入文件数据
        /// </summary>
        /// <param name="buf">待写入的数据</param>
        /// <param name="offset">待写入数据的起始偏移量</param>
        /// <param name="length">待写入数据的长度</param>
        /// <returns>成功写入的字节数</returns>
        public int Write(byte[] buf, int offset, int length)
        {
            // 若文件未打开，返回 0
            if (!m_bReadyToWrite) return 0;
            int nCount = length;
            if (offset > buf.Length) return 0;
            if (offset + length > buf.Length) nCount = buf.Length - offset;
            m_nDataLength += nCount;
            m_bwWavWriter.Write(buf, offset, nCount);
            return nCount;
        }
        /// <summary>
        /// 写入文件尾，关闭文件
        /// </summary>
        public void EndCreateFile()
        {
            if (!m_bReadyToWrite) return;
            m_bwWavWriter.Seek(4, SeekOrigin.Begin);
            m_bwWavWriter.Write((int)(m_nDataLength + 36));   // 写文件长度
            m_bwWavWriter.Seek(40, SeekOrigin.Begin);
            m_bwWavWriter.Write(m_nDataLength);               // 写数据长度

            m_bwWavWriter.Close();
            m_fsWavFile.Close();
            m_fsWavFile = null;
            m_bwWavWriter = null;
            m_bReadyToWrite = false;
        }
        /// <summary>
        /// 获得默认 wav 文件设置
        /// </summary>
        /// <returns>wav 文件格式</returns>
        public static WaveFormat GetDefalultWavFormat()
        {
            WaveFormat format = new WaveFormat();
            #region 载入默认配置
            format.FormatTag = WaveFormatTag.Pcm;   // 设置音频类型
            format.SamplesPerSecond = 11025;        // 采样率（单位：赫兹）典型值：11025、22050、44100Hz
            format.BitsPerSample = 16;              // 采样位数
            format.Channels = 1;                    // 声道
            // 单位采样点的字节数
            format.BlockAlign = (short)(format.Channels * (format.BitsPerSample / 8));
            // 平均数据率
            format.AverageBytesPerSecond = format.BlockAlign * format.SamplesPerSecond;
            #endregion
            return format;
        }
    } // end of WavFile
}
