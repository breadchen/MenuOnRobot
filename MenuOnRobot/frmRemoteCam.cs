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
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using Microsoft.DirectX.DirectSound;

namespace MenuOnRobot
{
    public delegate void StopReciveImageEventHandler();
    public delegate void ImageReadyEventHandler(Image im);
    public delegate void VoiceReadyEventHandler(byte[] by);
    public partial class frmRemoteCam : Form
    {
    	#region 成员变量
        frmStarter m_frmStarter;
        public static int PORT = 399;
        public static string IP = "192.168.1.4";
        private int m_nSoundPort; // 远程音频服务程序端口
        private DoVoice m_doRemoteVoice;	// 接受远程音频
        private CircularBuffer m_objBuf;
        private VoicePlayer m_objVP;
        //private WavFile m_wav;
        private int m_nNextWritePos; // 获取声音的间隔

        DoImages doImages;
        static int m_Count;
        bool m_bRecivingPic = false;
        bool m_bMoving = false;
        int m_iX;
        #endregion

        public frmRemoteCam(frmStarter fs)
        {
            InitializeComponent();
            m_frmStarter = fs;
            try
            {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"\CamClient.cfg");
                IP = sr.ReadLine();
                PORT = int.Parse(sr.ReadLine());
                m_nSoundPort = PORT + 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StartRecivePic()
        {
            if (!m_bRecivingPic)
            {
                try
                {
                	// 视频
                    m_Count = 0;
                    doImages = new DoImages(this, IP, PORT);
                    doImages.StopReciveImage += new StopReciveImageEventHandler(StopRecivePic);
                    doImages.ImageReady += new ImageReadyEventHandler(doImages_ImageReady);
                    ThreadStart o = new ThreadStart(doImages.ThreadProc);
                    Thread thread = new Thread(o);
                    thread.Name = "Imaging";
                    thread.Start();                    
                    
                    // 音频
                    m_doRemoteVoice = new DoVoice(this, IP, m_nSoundPort);
                    m_doRemoteVoice.StopReciveVoice += new StopReciveImageEventHandler(StopRecivePic);
                	m_doRemoteVoice.VoiceReady += new VoiceReadyEventHandler(m_doRemoteVoice_VoiceReady);
                	ThreadStart s = new ThreadStart(m_doRemoteVoice.ThreadProc);
                	Thread t = new Thread(s);
                	t.IsBackground = true;
                	t.Name = "RemoteVoicing";
                    //m_wav = new WavFile(@"C:\test.wav");
                    //m_wav.BeginCreateFile();
                	
                	m_objBuf = new CircularBuffer(WavFile.GetDefalultWavFormat().AverageBytesPerSecond / 8);
                	m_objVP = new VoicePlayer(this.Handle, WavFile.GetDefalultWavFormat());
                	m_objVP.ReadyForNewData += new ReadyForNewDataEventHandler(m_objVP_ReadyForNewData);
                	m_objVP.Start();
                	m_nNextWritePos = m_objVP.nNotifySize * 2;
                	t.Start();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return;
                }
                
                m_bRecivingPic = true;
            } // end of if
        }

        void doImages_ImageReady(Image im)
        {
        	pictureBox1.Image = im;
        }

        private void StopRecivePic()
        {
            if (m_bRecivingPic)
            {
            	if (null != doImages)
            	{
	                doImages.Done = true;
	                doImages = null;
            	}

                m_bRecivingPic = false;

                pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\wall-e.jpg");
                
	            // 关闭音频线程
	            if (null != m_doRemoteVoice)
	            {
	            	m_doRemoteVoice.Done = true;
	            	m_doRemoteVoice = null;
	            }
	            if (null != m_objVP)
	            {
	            	m_objVP.Stop();
	        		m_objVP = null;
	            }
	            if (null != m_objBuf)
	            {	
	            	m_objBuf.Clear();
	        		m_objBuf = null;
	            }
            } // end of if
        }
        /// <summary>
        /// 将新收到的音频数据存入辅助播放缓冲区，准备播放
        /// </summary>
        /// <param name="secBuf">辅助播放缓冲区</param>
        void m_objVP_ReadyForNewData(SecondaryBuffer secBuf)
        {
            try
            {
                int curPlayPos, curWritePos;
                secBuf.GetCurrentPosition(out curPlayPos, out curWritePos);
                //                Write( "curWritePos[" + curWritePos.ToString() + "]"
                //                     + "curPlay[" 	  + curPlayPos.ToString()  + "]");
                // 写到下一个写入通知之前
                int lockSize;// = m_nNextWritePos - curWritePos;
                #region 测试代码
                //curWritePos = (curWritePos / m_objVP.nNotifySize) * m_objVP.nNotifySize;
                lockSize = m_objVP.nNotifySize;
                #endregion
                if (lockSize < 0) lockSize += m_objVP.nBufSize;
                if (0 != lockSize)
                {
                    byte[] data = new byte[lockSize];
                    if (m_objBuf.Read(data) > 0)
                    {
                        secBuf.Write(curWritePos, data, LockFlag.FromWriteCursor);
                        m_nNextWritePos = (m_nNextWritePos + m_objVP.nNotifySize) % m_objVP.nBufSize;
                        // Write( "curWritePos[" + curWritePos.ToString() + "]"
                        //      + "dataLength["  + data.Length.ToString() + "]");
                    } // end of if
                } // end of if
            }
            catch
            {
            }
        }
        /// <summary>
        /// 将受到的音频数据先存入本地缓存
        /// </summary>
        /// <param name="by"></param>
        void m_doRemoteVoice_VoiceReady(byte[] by)
        {
            //Write("reciveVoiceData[" + by.Length.ToString() + "]");
            m_objBuf.Write(by, 0, by.Length);
            //m_wav.Write(by, 0, by.Length);
        }

        private void frmRemoteCam_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (m_bRecivingPic)
            //    StopRecivePic();
            m_frmStarter.Show();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (m_bRecivingPic)
            {
                StopRecivePic();
            }
            this.Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_iX = e.X;
                m_bMoving = true;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            if (m_bMoving)
            {
                if (e.X > m_iX)
                {
                    StartRecivePic();
                }
                else
                {
                    StopRecivePic();
                }
                m_bMoving = false;
            } // end of if 
        }
        /// <summary>
        /// 接收图片
        /// </summary>
        public class DoImages
        {
            // Abort indicator
            public bool Done;

            // Form to write to
            private frmRemoteCam m_f;

            // Client connection to server
            private TcpClient tcpClient;

            // stream to read from
            private NetworkStream networkStream;
        
	        // 停止接收图像
	        public event StopReciveImageEventHandler StopReciveImage;
	        // 收到图像
	        public event ImageReadyEventHandler ImageReady;

            public DoImages(frmRemoteCam f, string hostName, int nPort)
            {
                Done = false;
                m_f = f;

                // Connect to the server and get the stream
                tcpClient = new TcpClient(hostName, nPort);
                tcpClient.NoDelay = false;
                tcpClient.ReceiveTimeout = 2000;
                tcpClient.ReceiveBufferSize = 20000;
                networkStream = tcpClient.GetStream();
            }

            public void ThreadProc()
            {
                string s;
                int iBytesComing, iBytesRead, iOffset;
                byte[] byLength = new byte[10];
                byte[] byImage = new byte[1000];
                MemoryStream m = new MemoryStream(byImage);

                do
                {
                    try
                    {
                        // Read the fixed length string that
                        // tells the image size
                        iBytesRead = 0;
                        int offset = 0;
	                    // 收到全部数据长度部分
	                    while (offset != (iBytesRead += networkStream.Read(byLength, iBytesRead, 10 - iBytesRead))
	                          && iBytesRead < 10)
	                    	offset = iBytesRead;

                        if (iBytesRead != 10)
                        {
                        	Debug.WriteLine("No response from host");
                            StopReciveImage();
                            break;
                        }
                        s = Encoding.ASCII.GetString(byLength);
                        iBytesComing = Convert.ToInt32(s);

                        // Make sure our buffer is big enough
                        if (iBytesComing > byImage.Length)
                        {
                            byImage = new byte[iBytesComing];
                            m = new MemoryStream(byImage);
                            tcpClient.ReceiveBufferSize = iBytesComing + 10;
                        }
                        else
                        {
                            m.Position = 0;
                        }

                        // Read the image
                        iOffset = 0;

                        do
                        {
                            iBytesRead = networkStream.Read(byImage, iOffset, iBytesComing - iOffset);
                            if (iBytesRead != 0)
                            {
                                iOffset += iBytesRead;
                            }
                            else
                            {
                            	Debug.WriteLine("No response from host");
                                StopReciveImage();
                            }
                        } while ((iOffset != iBytesComing) && (!Done));


                        if (!Done)
                        {
                            // Write back a byte
                            networkStream.Write(byImage, 0, 1);

                            // Put the image on the screen
                            ImageReady(new System.Drawing.Bitmap(m));

                            // Increment the frame count
                            m_Count++;
                        }
                    }
                    catch (Exception e)
                    {
                        // If we get out of sync, we're probably toast, since
                        // there is currently no resync mechanism
                        Debug.WriteLine(e.Message);
                        m_f.StopRecivePic();
                    }

                } while (!Done);

                networkStream.Close();
                tcpClient.Close();
                networkStream = null;
                tcpClient = null;
            }
        }
	    /// <summary>
	    /// 接收声音
	    /// </summary>
	    public class DoVoice
	    {
	    	// Abort indicator
	        public volatile bool Done;
	        // Form to write to
	        private frmRemoteCam m_f;
	        // Client connection to server
	        private TcpClient tcpClient;
	        // stream to read from
	        private NetworkStream networkStream;
	        
	        public event VoiceReadyEventHandler VoiceReady;
	        public event StopReciveImageEventHandler StopReciveVoice;
	        
	        public DoVoice(frmRemoteCam fm, string hostName, int port)
	        {
	        	m_f = fm;
	        	tcpClient = new TcpClient(hostName, port);
	        	tcpClient.NoDelay = false;
	            tcpClient.ReceiveTimeout = 2000;
	            tcpClient.ReceiveBufferSize = 20000;
	            networkStream = tcpClient.GetStream();
	        }
	        
	        public void ThreadProc()
	        {
	        	int iBytesComing, iBytesRead, iOffset;
	            byte[] byLength = new byte[10];
	            byte[] byVoice = new byte[1000];
	            MemoryStream m = new MemoryStream(byVoice);
	        	do
	        	{
	        		try
	        		{
	        			// Read the fixed length string that
	                    // tells the voice data size
	                    iBytesRead = 0;
                        int offset = 0;
	                    // 收到全部数据长度部分
	                    while (offset != (iBytesRead += networkStream.Read(byLength, iBytesRead, 10 - iBytesRead))
	                          && iBytesRead < 10)
	                    	offset = iBytesRead;
	
	                    if (iBytesRead != 10)
	                    {
	                        Debug.WriteLine( @"No response from host @ " + DateTime.Now.ToString());
	                        StopReciveVoice();
	                        break;
	                    }
	                    string s = Encoding.ASCII.GetString(byLength);
	                    iBytesComing = Convert.ToInt32(s);
	                    
	                    // Make sure our buffer is big enough
	                    if (iBytesComing > byVoice.Length)
	                    {
	                        byVoice = new byte[iBytesComing];
	                        m = new MemoryStream(byVoice);
	                        tcpClient.ReceiveBufferSize = iBytesComing + 10;
	                    }
	                    else
	                    {
	                        m.Position = 0;
	                    }
	
	                    // Read the image
	                    iOffset = 0;
	
	                    do
	                    {
	                        iBytesRead = networkStream.Read(byVoice, iOffset, iBytesComing - iOffset);
	                        if (iBytesRead != 0)
	                        {
	                            iOffset += iBytesRead;
	                        }
	                        else
	                        {
	                            Debug.WriteLine(@"No response from host @ " + DateTime.Now.ToString());
	                            StopReciveVoice();
	                        }
	                    } while ((iOffset != iBytesComing) && (!Done));
	
	
	                    if (!Done)
	                    {
	                        // Write back a byte
	                        networkStream.Write(byVoice, 0, 1);
	
	                        // 将声音数据写入循环缓冲区
	                        VoiceReady(m.ToArray());
	
	                        // Increment the frame count
	                        //frmRemoteCam.m_Count++;
	                    }
	        		}
	        		catch (Exception e)
	        		{
	        			// If we get out of sync, we're probably toast, since
	                    // there is currently no resync mechanism
	                    Debug.WriteLine(DateTime.Now.ToString());
	                    Debug.WriteLine(e.Message);
	                    StopReciveVoice();
	        		}
	        	} while (!Done);
	        	
	        	networkStream.Close();
	            tcpClient.Close();
	            networkStream = null;
	            tcpClient = null;
	        } // end of ThreadProc()
	    } // end of class DoVoice
    } // end of class frmRemoteCam
}
