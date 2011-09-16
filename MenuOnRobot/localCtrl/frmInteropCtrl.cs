using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DevComponents.DotNetBar;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using SuperKeys;
using System.Net.Sockets;
using System.Net;

namespace MenuOnRobot
{
	delegate void DoAMouseEventHandler(ZigBeeData zbData);
	delegate void GameControlEventHandler(ZigBeeData zbData);
	delegate void GestureControlEventHandler();
    public partial class frmInteropCtrl : Form
    {
        #region 成员变量
        private const int N_BAUD_RATE = 38400;
        private const string STR_PORT_NAME = "COM7";
        // 串口数据包
        private frmLocalCtrl m_frmLocalCtrl;
        private bool m_bRecivingData;
        private ZigBeeData m_zbAMouse;
        // 弯曲传感器状态
        private bool m_bLeftDown  = false; // 左键按下（两传感器均弯曲）
        private bool m_bRightDown = false; // 右键按下（无名指传感器弯曲）
        // 手套状态（鼠标控制，游戏控制）
        private bool m_bGameMode; 		// 进入游戏模式后两传感器同时 弯曲/展开，代表 油门/刹车
        private byte m_byLastGesture; 	// 手套的上一个手势（AMouseAction中的一种）
        private byte m_byLastDirection; // 上一条指令中的前进方向
        // 游戏控制（状态转换函数组, 行标表示当前状态，列表表示上一状态(多一个初始状态)）
        private GameControlEventHandler[,] GameControl = new GameControlEventHandler[4,5];
        private System.Threading.Timer m_tmrChangeMode = null; // 计时器
        private bool m_bTiming; // 开始计时标志位
        private ManualResetEvent m_mreExiting = new ManualResetEvent(true);
        // 用于加载键盘驱动
        private WinIoSys m_myIoSys = new WinIoSys();
        // 用于向传感器控制程序发消息
        private UdpClient m_UdpSensorNet = null;
        private static readonly string strIPHead = "192.168.1";
        private readonly IPEndPoint SENSOR_NET_ADDRESS;
        // 从传感器接收投影跟随信息
        private UdpClient m_udpReciver = null;
        private readonly IPEndPoint LOCAL_ADDRESS; // 监听地址
        private volatile bool m_bDone = false;	   // 退出标记
        private event SensorDataRcvEventHandler DtcDataRcv;
        private TcpClient m_tcpSender = null;
        private readonly IPEndPoint ROBOT_ADDRESS; // 机器人控制地址
        private NetworkStream m_nsRobotControl = null;
        private byte[] ROBOT_STOP = {0x22,4,0,0,0,0};
        private byte[] ROBOT_GO_FORWARD = {0x22,4,1,0,0,0};
        private byte[] ROBOT_GO_BACKWARD = {0x22,4,0,1,0,0};

        // 外部程序
        private Process m_prcsCalculater  = null;
        private Process m_prcsPainter 	  = null;
        private Process m_prcsGame 		  = null;
        private Process m_prcsMediaPlayer = null;

        private event WriteLogEventHandler _WriteLog;
        public  event WriteLogEventHandler WriteLog
        {
            add { _WriteLog = value; }
            remove
            {
                if (_WriteLog != null) _WriteLog -= value;
            }
        }
        private event DoAMouseEventHandler _DoAMouse;
        private event DoAMouseEventHandler DoAMouse
        {
        	add {_DoAMouse = value;}
        	remove {if (_DoAMouse != null) _DoAMouse -= value;}
        }
        private event DoAMouseEventHandler _DoGesture;
        private event DoAMouseEventHandler DoGesture
        {
        	add { _DoGesture = value; }
        	remove { if (null != _DoGesture) _DoGesture -= value; }
        }
        
        
        protected const Int32 SW_RESTORE = 9;
        [DllImport("user32.dll")]
        protected static extern bool ShowWindow(IntPtr hWnd, Int32 flags);
        // 激活窗口（使窗口临时位于最上层，并获得焦点）
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        public frmInteropCtrl(frmLocalCtrl f)
        {
            m_frmLocalCtrl = f;

            InitializeComponent();

            IPAddress temp = _GetLocalIP();
            if (null != temp)
            {
                SENSOR_NET_ADDRESS = new IPEndPoint(temp, 1699); // new IPEndPoint(IPAddress.Parse("192.168.1.5"), 1699);
                LOCAL_ADDRESS = new IPEndPoint(temp, 1799);
                ROBOT_ADDRESS = new IPEndPoint(IPAddress.Parse("192.168.1.7"), 1599);
            }
            
            GameControl[0,0] = new GameControlEventHandler(GameStatusChange_0_0);
            GameControl[0,1] = new GameControlEventHandler(GameStatusChange_0_1);
            GameControl[0,3] = new GameControlEventHandler(GameStatusChange_0_3);
            GameControl[0,4] = new GameControlEventHandler(GameStatusChange_0_4);
            GameControl[1,0] = new GameControlEventHandler(GameStatusChange_1_0);
            GameControl[1,1] = new GameControlEventHandler(GameStatusChange_1_1);
            GameControl[1,3] = new GameControlEventHandler(GameStatusChange_1_3);
            GameControl[1,4] = new GameControlEventHandler(GameStatusChange_1_4);
            GameControl[3,0] = new GameControlEventHandler(GameStatusChange_3_0);
            GameControl[3,1] = new GameControlEventHandler(GameStatusChange_3_1);
            GameControl[3,3] = new GameControlEventHandler(GameStatusChange_3_3);
            GameControl[3,4] = new GameControlEventHandler(GameStatusChange_3_4);
            
            CurtainControl[0] = new GestureControlEventHandler(CurtainClose);
            CurtainControl[1] = new GestureControlEventHandler(CurtainOpen);
            CurtainControl[2] = new GestureControlEventHandler(CurtainHold);
            
            LightControl[0] = new GestureControlEventHandler(LightHold);
            LightControl[1] = new GestureControlEventHandler(LightShift);
            
            DtcDataRcv += new SensorDataRcvEventHandler(OnDtcDataRcv);
        }
                
        #region 交互控制
        /// <summary>
        /// 填写串口参数，如波特率，串口号等
        /// </summary>
        private void InitSerialPort()
        {
            spGlove.BaudRate = N_BAUD_RATE;
            spGlove.PortName = STR_PORT_NAME;
            
            // 收到至少一个完整包头后才触发数据事件（包头 6B） 
            spGlove.ReceivedBytesThreshold = (int)HEAD_LEN.ZIGBEE_HEAD_LEN;
            
            // 初始化标志
            m_bRecivingData = false;
        } // end of InitSerialPort()
        /// <summary>
        /// 处理手套数据包
        /// </summary>
        /// <param name="zbData"></param>
        private void GloveDataRecived(ZigBeeData zbData)
        {
            if (zbData.m_byData.Length == zbData.nDataLength)
            {
            	WriteLine(
            		string.Format("light[{0}], curtain[{1}]", zbData.m_byData[3].ToString(), zbData.m_byData[2].ToString()));
                switch (zbData.GloveMode)
                {
                    case (byte)GloveFunction.AMouse:
                        _DoAMouse(zbData);
                        break;
                    case (byte)GloveFunction.GestureCtrl:
                        _DoGesture(zbData);
                        break;
                    default:
                        break;
                } // end of switch
            } // end of if
            else
            {
                WriteLine(
                    String.Format(
                        "数据包接受错误（标识长度[{0}],实际长度[{1}]）", 
                        zbData.nDataLength, 
                        zbData.m_byData.Length));
            } // end of else
        }
        /// <summary>
        /// 完成空中鼠标操作
        /// </summary>
        /// <param name="zbData"></param>
        private void OnDoAMouse(ZigBeeData zbData)
        {
            switch (zbData.BendSensor)
            {
                case (byte)AMouseAction.Move:
                    {
                        // 若为两弯曲传感器恢复原状
                        if (m_bLeftDown)
                        {
                            m_bLeftDown = false;
                            MouseControl.LeftUp();
                        }

                        // 若为无名指弯曲传感器恢复原状
                        if (m_bRightDown)
                        {
                            m_bRightDown = false;
                            MouseControl.RightUp();
                        }

                        // 移动鼠标
                        MouseControl.MoveCursor(zbData.XMovement, zbData.YMovement);
                    } break;
                case (byte)AMouseAction.Click:
                    {
                        // 若两弯曲传感器未弯曲
                        if (!m_bLeftDown)
                            m_bLeftDown = true;

                        // 鼠标左键按下
                        MouseControl.LeftDown();

                        // 移动鼠标
                        MouseControl.MoveCursor(zbData.XMovement, zbData.YMovement);
                    } break;
                case (byte)AMouseAction.Gesture:
                    {
                        // 若无名指弯曲传感器未弯曲
                        if (!m_bRightDown)
                            m_bRightDown = true;
                        else
                        {
                            // 若为两弯曲传感器恢复原状
                            if (m_bLeftDown)
                            {
                                m_bLeftDown = false;
                                MouseControl.LeftUp();
                            }
                        } // end of else

                        // 鼠标右键按下
                        MouseControl.RightDown();

                        // 移动鼠标
                        MouseControl.MoveCursor(zbData.XMovement, zbData.YMovement);
                    } break;
            } // end of switch
        }
        /// <summary>
        /// 完成游戏控制操作
        /// </summary>
        /// <param name="zbData"></param>
        private void OnDoGameCtrl(ZigBeeData zbData)
        {
        	#region switch方式
//        	switch (zbData.BendSensor)
//        	{
//        		case (byte)AMouseAction.Move:
//        		{ // 收到 AMouseAction.Move 指令
//        			switch (m_byLastGesture)
//        			{
//        				case (byte)AMouseAction.Move:
//        				{
//        						
//        				}break;
//        				case (byte)AMouseAction.Gesture: 
//        				{
//        					m_myIoSys.KeyPress(WinIoSys.Key.VK_ESCAPE);
//        					tmrGame.Start();
//        					tmrGame.Tag = "2";
//        				}break;
//        				case (byte)AMouseAction.Click: break;
//        			} // end of switch
//        		}break;
//        		case (byte)AMouseAction.Gesture:
//        		{ // 收到 AMouseAction.Gesture 指令
//        			switch (m_byLastGesture)
//        			{
//        				case (byte)AMouseAction.Move: 
//        				{
//        					// 退出游戏模式
//        					m_bGameMode = false;
//        					if ("2" == (string)tmrGame.Tag)
//        					{
//        						tmrGame.Stop();
//        						tmrGame.Tag = "1";
//        					}
//        				}break;
//        				case (byte)AMouseAction.Gesture: 
//        				{ // 移动鼠标
//        					MouseControl.MoveCursor(zbData.XMovement, zbData.YMovement);
//        				}break;
//        				case (byte)AMouseAction.Click: break;
//        			} // end of switch
//        		}break;
//        		case (byte)AMouseAction.Click:
//        		{ // 收到 AMouseAction.Click 指令
//        			
//        		}break;
//        		default: m_byLastGesture = (byte)AMouseAction.Init; return;
//        	} // end of switch
        	#endregion
        	try
        	{
        		m_mreExiting.WaitOne();
        		if (zbData.BendSensor == 0x02) return;
        		// 执行状态转换函数
        		(GameControl[(int)zbData.BendSensor, m_byLastGesture])(zbData);
        		m_byLastGesture = zbData.BendSensor;
        		m_byLastDirection = zbData.GameContral;
        	}
        	catch (Exception ex)
        	{
        		WriteLine(ex.Message);
        	}        	
        }
        /// <summary>
        /// 完成手势控制操作
        /// </summary>
        /// <param name="zbData"></param>
        private void OnDoGesture(ZigBeeData zbData)
        {
        	WriteLine("doGesture");
        	try
        	{
        		if (zbData.m_byData[2] >= 0 && zbData.m_byData[2] <= 2)
        		{
        			(CurtainControl[(int)zbData.m_byData[2]])();
        		}
        		if (zbData.m_byData[3] >=0 && zbData.m_byData[3] <= 1)
        		{
        			(LightControl[(int)zbData.m_byData[3]])();
        		}
        	}
        	catch (Exception ex)
        	{
        		WriteLine(ex.Message);
        	}
        }
        
        #region 手势控制状态转换
        private GestureControlEventHandler[] CurtainControl = new GestureControlEventHandler[3];
        private GestureControlEventHandler[] LightControl   = new GestureControlEventHandler[2];
        /// <summary>
        /// 关闭窗帘
        /// </summary>
        private void CurtainClose()
        {
        	_CurtainCtrl((byte)ZigBeeCMD.CMD_CTRL_DOWN);
        }
        /// <summary>
        /// 打开窗帘
        /// </summary>
        private void CurtainOpen()
        {
        	_CurtainCtrl((byte)ZigBeeCMD.CMD_CTRL_UP);
        }
        /// <summary>
        /// 窗帘状态不变
        /// </summary>
        private void CurtainHold()
        {
        	_CurtainCtrl((byte)ZigBeeCMD.CMD_CTRL_HOLD);
        }
        /// <summary>
        /// 电灯状态保持不变
        /// </summary>
        private void LightHold()
        {
        	
        }
        /// <summary>
        /// 电灯状态改变
        /// </summary>
        private void LightShift()
        {
            _LightCtrl((byte)ZigBeeCMD.CMD_CTRL_TOOGLE);
        }

        private void _LightCtrl(byte data)
        {
            // 生成数据包
            byte[] temp = new byte[GestureControl.PackageHeadLen + 2];
            temp[0] = GestureControl.PackageHead;
            temp[1] = 2;
            temp[2] = (byte)ZIGBEE_CLUST_ID.CTRL_LED_ID;
            temp[3] = 0xFF;
            temp[4] = 0xFF;
            temp[5] = (byte)ZigBeeCMD.CMD_CTRL;
            temp[6] = data;
            GestureControl gc = new GestureControl(temp);
            
            // 发送数据
            GestureControl gcRcv = SendToSensorNet(gc);
            WriteLine("send message to[" + SENSOR_NET_ADDRESS.ToString() + "]");

            if (null != gcRcv)
                WriteLine("recive [" + gcRcv.Data[2].ToString() + "]");
        }

        private void _CurtainCtrl(byte data)
        {
            // 生成数据包
            byte[] temp = new byte[GestureControl.PackageHeadLen + 2];
            temp[0] = GestureControl.PackageHead;
            temp[1] = 2;
            temp[2] = (byte)ZIGBEE_CLUST_ID.CTRL_CURTAIN_ID;
            temp[3] = 0xFF;
            temp[4] = 0xFF;
            temp[5] = (byte)ZigBeeCMD.CMD_CTRL;
            temp[6] = data;
            GestureControl gc = new GestureControl(temp);

            // 发送数据
            GestureControl gcRcv = SendToSensorNet(gc);
            WriteLine("send message to[" + SENSOR_NET_ADDRESS.ToString() + "]");
            
//            if (null != gcRcv)
//            	WriteLine("recive [" + gcRcv.Data[2].ToString() + "]");
        }
        
        private GestureControl SendToSensorNet(GestureControl gc)
        {
        	byte[] byData = gc.ToArray();
        	byte[] byRcv = null;
        	IPEndPoint ipRcv = null;
        	
        	try
        	{
	        	m_UdpSensorNet.Send(byData, byData.Length, SENSOR_NET_ADDRESS);
	        	//byRcv = m_UdpSensorNet.Receive(ref ipRcv);
        	}
        	catch (Exception ex)
        	{
        		WriteLine(ex.Message);
        	}
        	
        	if (null != ipRcv && 
        	    null != byRcv &&
        	    ipRcv.Equals(SENSOR_NET_ADDRESS) && 
        	    byRcv.Length >= byData.Length)
        		return new GestureControl(byRcv);
        	else
        		return null;
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
        #endregion
        
        #region 游戏控制状态转换函数
        private WinIoSys.Key[] DirectionKeyMap = 
	        {
	        	WinIoSys.Key.VK_UP,		// 占位，没用
	        	WinIoSys.Key.VK_LEFT,	// 左转
	        	WinIoSys.Key.VK_RIGHT	// 右转
	        };
        /// <summary>
        /// 调试用
        /// </summary>
        private string[] debugKeyMap = 
	        {
	        	"^",
	        	"<-",
	        	"->"
	        };
        /// <summary>
        /// 开启计时器
        /// </summary>
        private void StartTimer()
        {
        	if (!m_bTiming)
        	{
        		m_tmrChangeMode.Change(1500, 1500);
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
        		m_tmrChangeMode.Change(Timeout.Infinite, Timeout.Infinite);
        		m_bTiming = false;
        	}
        }
        
        /// <summary>
        /// 移动状态->移动状态
        /// </summary>
        private void GameStatusChange_0_0(ZigBeeData zbData)
        { // 如果没进入游戏模式就什么也不做~  
        	WriteLine("0 -> 0, gameMode[" + m_bGameMode.ToString() + "]");
        	if (m_bGameMode)
        	{ // 若已经进入游戏模式
        		// 弹起上次的按键
        		if (zbData.GameContral != m_byLastDirection && 
        		    (byte)GameCtrl.Forward != m_byLastDirection &&
        		    (byte)GameCtrl.Init != m_byLastDirection)
        		{
        			m_myIoSys.KeyUpEx(DirectionKeyMap[(int)m_byLastDirection], 1);
        		}
        		
        		// 后退
        		//m_myIoSys.KeyDown(WinIoSys.Key.VK_DOWN);
        		m_myIoSys.KeyDownEx(WinIoSys.Key.VK_DOWN, 1);
        		
        		// 根据当前状态转向
        		if (zbData.GameContral != (byte)GameCtrl.Forward)
        		{
        			//m_myIoSys.KeyDown(DirectionKeyMap[(int)zbData.GameContral]);
        			m_myIoSys.KeyDownEx(DirectionKeyMap[(int)zbData.GameContral],1);
        			WriteLine("v + " + debugKeyMap[(int)zbData.GameContral]);
        		}
        	} // end of if
        }
        /// <summary>
        /// 移动状态->手势状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_1_0(ZigBeeData zbData)
        {
        	WriteLine("0 -> 1");
        	if (!m_bGameMode)
        	{ // 若未进入游戏模式，停止计时
        		StopTimer();
        	} // end of if
        	else
        	{ // 若已进入游戏模式
        		StartTimer();
        	} // end of else
        }
        /// <summary>
        /// 2 状态暂时无用
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_2_0(ZigBeeData zbData)
        {
        	
        }
        /// <summary>
        /// 移动状态->单击状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_3_0(ZigBeeData zbData)
        { // 若未进入游戏状态，则什么也不做~
        	WriteLine("0 -> 3");
        	if (m_bGameMode)
        	{
        		// 弹起上次的按键
        		if (zbData.GameContral != m_byLastDirection && 
        		    (byte)GameCtrl.Forward != m_byLastDirection &&
        		    (byte)GameCtrl.Init != m_byLastDirection)
        		{
        			m_myIoSys.KeyUpEx(DirectionKeyMap[(int)m_byLastDirection], 1);
        		}
        		
        		// 弹起后退键
        		m_myIoSys.KeyUpEx(WinIoSys.Key.VK_DOWN, 1);
        		
        		// 前进
        		m_myIoSys.KeyDownEx(WinIoSys.Key.VK_UP, 1);
        		
        		// 根据当前状态转向 
        		if (zbData.GameContral != (byte)GameCtrl.Forward)
        			m_myIoSys.KeyDownEx(DirectionKeyMap[(int)zbData.GameContral], 1);
        	}
        }
        /// <summary>
        /// 手势状态->移动状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_0_1(ZigBeeData zbData)
        {
        	WriteLine("1 -> 0");
        	if (m_bGameMode)
        	{ // 游戏模式下，停止计时
        		StopTimer();
        	} // end of if
        	else
        	{ // 控制模式下, 按一次 esc键 ，开始计时
        		// 按 esc键
        		m_myIoSys.KeyPressEx(WinIoSys.Key.VK_ESCAPE, 100);
        		
        		StartTimer();
        	} // end of else
        }
        /// <summary>
        /// 手势状态->手势状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_1_1(ZigBeeData zbData)
        { // 若在游戏模式下，则什么也不做~
        	WriteLine("1 -> 1");
        	if (!m_bGameMode)
        	{ // 移动鼠标
        		MouseControl.MoveCursor(zbData.XMovement, zbData.YMovement);
        	}
        }
        /// <summary>
        /// 2 状态暂时无用
        /// </summary>
        /// <param name="str"></param>
        private void GameStatusChange_2_1(ZigBeeData zbData)
        {
        	
        }
        /// <summary>
        /// 手势状态->单击状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_3_1(ZigBeeData zbData)
        {
        	WriteLine("1 -> 3");
        	if (m_bGameMode)
        	{ // 游戏模式下，停止计时
        		StopTimer();
        	} // end of if
        	else
        	{ // 控制模式下，单击左键，开始计时
        		MouseControl.LeftClick();
        		WriteLine("left click");
        		StartTimer();
        	} // end of else
        }
        /// <summary>
        /// 单击状态->移动状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_0_3(ZigBeeData zbData)
        { // 若在控制模式下，则什么也不做~
        	WriteLine("3 -> 0");
        	if (m_bGameMode)
        	{
        		// 弹起上次的按键
        		if (zbData.GameContral != m_byLastDirection && 
        		    (byte)GameCtrl.Forward != m_byLastDirection &&
        		    (byte)GameCtrl.Init != m_byLastDirection)
        		{
        			m_myIoSys.KeyUpEx(DirectionKeyMap[(int)m_byLastDirection], 1);
        		}
        		
        		// 弹起前进键
        		m_myIoSys.KeyUpEx(WinIoSys.Key.VK_UP, 1);
        		
        		// 后退
        		m_myIoSys.KeyDownEx(WinIoSys.Key.VK_DOWN, 1);
        		
        		// 根据当前状态转向 
        		if (zbData.GameContral != (byte)GameCtrl.Forward)
        			m_myIoSys.KeyDownEx(DirectionKeyMap[(int)zbData.GameContral], 1);
        	}
        }
        /// <summary>
        /// 单击状态->手势状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_1_3(ZigBeeData zbData)
        {
        	WriteLine("3 -> 1");
        	if (!m_bGameMode)
        	{ // 若未进入游戏模式，停止计时
        		StopTimer();
        	} // end of if
        	else
        	{ // 若已进入游戏模式
        		StartTimer();
        	} // end of else
        }
        /// <summary>
        /// 单击状态->单击状态
        /// </summary>
        /// <param name="str"></param>
        private void GameStatusChange_3_3(ZigBeeData zbData)
        { // 若在控制模式下, 则什么也不做~
        	WriteLine("3 -> 3");
        	if (m_bGameMode)
        	{
        		// 弹起上次的按键
        		if (zbData.GameContral != m_byLastDirection && 
        		    (byte)GameCtrl.Forward != m_byLastDirection &&
        		    (byte)GameCtrl.Init != m_byLastDirection)
        		{
        			m_myIoSys.KeyUpEx(DirectionKeyMap[(int)m_byLastDirection], 1);
        		}
        		
        		// 前进
        		m_myIoSys.KeyDownEx(WinIoSys.Key.VK_UP, 1);
        		
        		// 根据当前状态转向
        		if (zbData.GameContral != (byte)GameCtrl.Forward)
        		{
        			m_myIoSys.KeyDownEx(DirectionKeyMap[(int)zbData.GameContral], 1);        			
        			WriteLine("^ + " + debugKeyMap[(int)zbData.GameContral]);
        		}
        	} // end of if
        }
        /// <summary>
        /// 初始状态->移动状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_0_4(ZigBeeData zbData)
        {
        	WriteLine("4 -> 0");
        	if (m_bGameMode)
        	{ // 游戏模式下，停止计时
        		StopTimer();
        	} // end of if
        	else
        	{ // 控制模式下, 按一次 esc键 ，开始计时
        		// 按 esc键
        		m_myIoSys.KeyPress(WinIoSys.Key.VK_ESCAPE, 100);
        		
        		StartTimer();
        		WriteLine("40 开始计时");
        	} // end of else
        }
        /// <summary>
        /// 初始状态->手势状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_1_4(ZigBeeData zbData)
        {
        	WriteLine("4 -> 1");
        	if (m_bGameMode)
        	{ // 游戏模式下，开始计时
        		StartTimer();
        	} // end of if
        	else
        	{ // 控制模式下，单击左键，停止计时
        		StopTimer();
        	} // end of else
        	MouseControl.MoveCursor(zbData.XMovement, zbData.YMovement);
        }
        /// <summary>
        /// 初始状态->单击状态状态
        /// </summary>
        /// <param name="zbData"></param>
        private void GameStatusChange_3_4(ZigBeeData zbData)
        {
        	WriteLine("4 -> 3");
        	if (m_bGameMode)
        	{ // 游戏模式下，停止计时
        		StopTimer();
        	} // end of if
        	else
        	{ // 控制模式下, 按一次 esc键 ，开始计时
        		// 按 esc键
        		m_myIoSys.KeyPressEx(WinIoSys.Key.VK_ESCAPE, 100);
        		
        		StartTimer();
        	} // end of else
        }
        #endregion
        
        #region 投影跟随
        private void UdpCallBack(IAsyncResult ar)
        {
        	
	       	UdpClient u = (UdpClient)(ar.AsyncState);
	       	IPEndPoint e = null;
	       	
	       	byte[] byDataRcv = null;
	       	try
        	{
	        	byDataRcv = u.EndReceive(ar, ref e);
        	}
        	catch (Exception ex)
        	{
        		WriteLine(ex.Message);
        	}
	       	
	       	if (null != DtcDataRcv && 
	       	    null != byDataRcv &&
	       	    5 < byDataRcv.Length
	       	   )
	       		DtcDataRcv(new SensorData(byDataRcv));
	       	
	       	if (!m_bDone)
	       	{
	       		u.BeginReceive(UdpCallBack, u);
	       	}
        }
        
        private void OnDtcDataRcv(SensorData sd)
        {
        	switch (sd.Data[2])
        	{
        		case 60:
        			{
        				//m_nsRobotControl.Write(ROBOT_GO_FORWARD, 0, ROBOT_GO_FORWARD.Length);
        				WriteLine("60");
        			}break;
        		case 61:
        			{
        				//m_nsRobotControl.Write(ROBOT_STOP, 0, ROBOT_GO_FORWARD.Length);
        				WriteLine("61");
        			}break;
        		case 62:
        			{
        				//m_nsRobotControl.Write(ROBOT_GO_BACKWARD, 0, ROBOT_GO_BACKWARD.Length);
        				WriteLine("62");
        			}break;
        		default:break;
        	} // end of switch
        }
        #endregion 投影跟随
        #endregion 交互控制

        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="str"></param>
        private void WriteLine(string str)
        {
            if (_WriteLog != null)
                _WriteLog(str);
        }

        #region 窗口事件处理
        private void frmInteropCtrl_Load(object sender, EventArgs e)
        {        	
            // 允许在不同线程中使用控件
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            
        	// 填写事件
        	DoAMouse += new DoAMouseEventHandler(OnDoAMouse);
        	DoGesture += new DoAMouseEventHandler(OnDoGesture);
        	
        	// 加载键盘驱动
        	m_myIoSys.InitSuperKeys();
        	
            // 初始化串口
            InitSerialPort();

            // 准备与传感器控制程序通信
            m_UdpSensorNet = new UdpClient();
            m_UdpSensorNet.Client.ReceiveTimeout = 1000;
            try
            {
	            m_udpReciver = new UdpClient(LOCAL_ADDRESS);
//	            m_tcpSender = new TcpClient(ROBOT_ADDRESS.Address.ToString(), ROBOT_ADDRESS.Port);
//	            m_tcpSender.NoDelay = false;
//	            m_tcpSender.ReceiveTimeout = 5000;
	            m_nsRobotControl = m_tcpSender.GetStream();
            }
            catch(Exception ex)
            {
            	WriteLine(ex.Message);
            }
            if (null != m_udpReciver)
            	m_udpReciver.BeginReceive(UdpCallBack, m_udpReciver);
            
            // 打开串口
            try
            {
                if (!spGlove.IsOpen) spGlove.Open();
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private void spGlove_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!m_bRecivingData)
                {
                    byte[] bysTemp = new byte[10];
                    spGlove.Read(bysTemp, 0, 1);

                    m_bRecivingData = true;

                    #region 检验包头
                    while (true)
                    { // 向后寻找直到找到 头部为 “0x02 0x00 0x18”的包 
                        if (bysTemp[0] == 0x02)
                        {
                            spGlove.Read(bysTemp, 1, 1);
                            if (bysTemp[1] == 0x00)
                            {
                                spGlove.Read(bysTemp, 2, 1);
                                if (bysTemp[2] == 0x18)
                                    break;
                                else
                                {
                                    bysTemp[0] = bysTemp[2];
                                }
                            }
                            else
                            {
                                bysTemp[0] = bysTemp[1];
                                WriteLine("wrong follow message[" + bysTemp[1].ToString() + "]");
                            }
                        } // end of if
                        else
                        {
                            WriteLine("wrong message[" + bysTemp[0].ToString() + "]");
                            spGlove.Read(bysTemp, 0, 1);
                        }
                    } // end of while
                    #endregion
                    int i = spGlove.Read(bysTemp, 3, ((int)HEAD_LEN.ZIGBEE_HEAD_LEN - 3));
                    while (i < ((int)HEAD_LEN.ZIGBEE_HEAD_LEN - 3))
                    {
                        i += spGlove.Read(bysTemp, 3 + i, ((int)HEAD_LEN.ZIGBEE_HEAD_LEN - 3 - i));
                        //WriteLine("reading");
                    }
                    if (bysTemp[3] <= 0)
                    {
                        return;
                    }
                    m_zbAMouse = new ZigBeeData(bysTemp);
                    if (m_zbAMouse.DataHead.CLUST == 0)
                    {
                        //Thread.Sleep(200);
                    }
                } // end of if
                int nRecived = 0;
                while (nRecived < m_zbAMouse.DataHead.DATA_LEN - 2)
                { // 继续访问缓冲区直到读取全部数据包
                    nRecived += spGlove.Read(m_zbAMouse.m_byData
                                              , nRecived
                                              , m_zbAMouse.nDataLength - nRecived);
                } // end of while
                // 读取校验位
                m_zbAMouse.m_byFSC = (byte)spGlove.ReadByte();
                //WriteLine("fsc[" + m_RcvData.m_byFSC.ToString() + "]");
                
                // 调整标记准备接受下个数据包
                m_bRecivingData = false;
                //zigbeeDataRecived(this);
                GloveDataRecived(m_zbAMouse);
            } // end of try
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }

        private void frmInteropCtrl_FormClosed(object sender, FormClosedEventArgs e)
        {
        	// 退出异步接收
        	m_bDone = true;
        	byte[] byTemp = {0};
        	try
        	{
        		m_udpReciver.Send(byTemp, 1, LOCAL_ADDRESS);
        		Thread.Sleep(10);
        	}
        	catch{}
        	// 退出 tcp 客户端
        	if (null != m_tcpSender)
        		m_tcpSender.Close();
        	// 卸载键盘驱动
        	m_myIoSys.CloseSuperKeys();
            // 关闭 udp 客户端
            m_UdpSensorNet.Close();
        	m_udpReciver.Close();

            if (m_frmLocalCtrl != null) 
            	m_frmLocalCtrl.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (spGlove.IsOpen) 
                	spGlove.Close();
                if (null != m_prcsCalculater &&
                   !m_prcsCalculater.HasExited)
                	m_prcsCalculater.CloseMainWindow();
                if (null != m_prcsPainter && 
                    !m_prcsPainter.HasExited)
                	m_prcsPainter.CloseMainWindow();
                if (null != m_prcsMediaPlayer && 
                    !m_prcsMediaPlayer.HasExited)
                	m_prcsMediaPlayer.CloseMainWindow();
                if (null != m_prcsGame && 
                    !m_prcsGame.HasExited)
                	m_prcsGame.Kill();
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
            this.Close();
        }
        
        private void BtnGameClick(object sender, EventArgs e)
        {
        	// 问题：如何得知游戏已经退出~
        	// 解答：Process 类的 Exited 事件
        	
        	// 改变处理手套数据的方式
        	DoAMouse += new DoAMouseEventHandler(OnDoGameCtrl);
        	// 清理标志位
        	m_bGameMode = false;
        	m_byLastGesture = (byte)AMouseAction.Init; // 设为初始状态
        	m_byLastDirection = (byte)GameCtrl.Init;   // 设为初始方向
        	m_tmrChangeMode = new System.Threading.Timer(
        		TmrGameTick, "", Timeout.Infinite, Timeout.Infinite);
        	m_bTiming = false;
        	// 游戏路径
            string gamePath = @"D:\game\nfs6";
        	string fileName = gamePath + "\\RunNfs.bat";
        	try
        	{
        		// 若游戏已经打开，则将其激活
	        	if (null != m_prcsGame && !m_prcsGame.HasExited)
	        	{
	        		SetForegroundWindow(m_prcsGame.MainWindowHandle);
	        		return;
	        	} // end of if
	        	
	        	// 运行 .bat 问件，打开游戏
	        	Process tempPrcs = new Process();
	        	tempPrcs.StartInfo.FileName = fileName;
	        	// 不显示窗口
	        	tempPrcs.StartInfo.CreateNoWindow = true;
	        	tempPrcs.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
	        	// 启动进程
	        	tempPrcs.Start();
	        	
	        	// 等待游戏启动
	        	Thread.Sleep(300);
	        	
	        	// 获得游戏进程	        
	        	m_prcsGame = (Process.GetProcessesByName("NFSHP2"))[0];
	        	
	        	// 设置该属性以使 Exited 事件能够被触发
	        	m_prcsGame.EnableRaisingEvents = true;
	        	m_prcsGame.Exited += new EventHandler(m_prcsGame_Exited);

                Thread.Sleep(14000); // 等待14秒开始自动进入游戏
                //SetForegroundWindow(m_prcsGame.MainWindowHandle);
                WriteLine(m_prcsGame.MainWindowHandle.ToString());
                int i;
                WriteLine("sending enter");
                for (i = 0; i < 4; i++ )
                {
                    m_myIoSys.KeyPressEx(WinIoSys.Key.VK_RETURN, 100);
                    Thread.Sleep(500);
                }
                Thread.Sleep(800);
                WriteLine("sending enter2");
                for (i = 0; i < 3; i++ )
                {
                    m_myIoSys.KeyPressEx(WinIoSys.Key.VK_RETURN, 100);
                    Thread.Sleep(500);
                }
                Thread.Sleep(2000);
                WriteLine("sending tab");  
                for(i = 0; i < 5; i++)
                {
                	m_myIoSys.KeyPress(WinIoSys.Key.VK_TAB, 200);
                	Thread.Sleep(500);
                }
                Thread.Sleep(500);
                WriteLine("sending enter");
                m_myIoSys.KeyPressEx(WinIoSys.Key.VK_RETURN, 100);
        	}
        	catch (Exception ex)
        	{
        		try
        		{
	        		if (null != m_prcsGame && !m_prcsGame.HasExited)
	        			m_prcsGame.Kill();
	        		m_prcsGame = null;
        		}
        		catch { }
        		WriteLine(ex.Message);
        	}
        }
		/// <summary>
		/// 游戏退出时执行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        void m_prcsGame_Exited(object sender, EventArgs e)
        {        	
        	DoAMouse += new DoAMouseEventHandler(OnDoAMouse);
        	m_prcsGame = null;
        	m_bLeftDown  = false;
        	m_bRightDown = false;
        	if (null != m_tmrChangeMode)
        		m_tmrChangeMode.Dispose();
        }
        
        void BtnCalculatorClick(object sender, EventArgs e)
        {
        	try
        	{
	        	if (null != m_prcsCalculater && !m_prcsCalculater.HasExited)
	        	{
	        		SetForegroundWindow(m_prcsCalculater.MainWindowHandle);
	        		return;
	        	}
	        	m_prcsCalculater = new Process();
	        	m_prcsCalculater.StartInfo = new ProcessStartInfo(@"calc.exe");
	        	m_prcsCalculater.Start();
        	}
        	catch (Exception ex)
        	{
        		m_prcsCalculater = null;
        		WriteLine(ex.Message);
        	}
        }
        
        void BtnPainterClick(object sender, EventArgs e)
        {
//            _CurtainCtrl((byte)ZigBeeCMD.CMD_CTRL_UP);
//            _LightCtrl((byte)ZigBeeCMD.CMD_CTRL_TOOGLE);
            try
            {
                if (null != m_prcsPainter && !m_prcsPainter.HasExited)
                {
                    SetForegroundWindow(m_prcsPainter.MainWindowHandle);
                    return;
                }
                m_prcsPainter = new Process();
                m_prcsPainter.StartInfo = new ProcessStartInfo(@"D:\软件\knKan\knKan.exe");
                m_prcsPainter.Start();
            }
            catch (Exception ex)
            {
                m_prcsPainter = null;
                WriteLine(ex.Message);
            }
        }
        
        void BtnPlayerClick(object sender, EventArgs e)
        {
        	try
        	{
	        	if (null != m_prcsMediaPlayer && !m_prcsMediaPlayer.HasExited)
	        	{
	        		SetForegroundWindow(m_prcsMediaPlayer.MainWindowHandle);
	        		return;
	        	}
	        	//if (ofdOpenMedia.ShowDialog() == DialogResult.OK)
	        	{
		        	m_prcsMediaPlayer = new Process();
		        	m_prcsMediaPlayer.StartInfo = 
		        		new ProcessStartInfo(
		        			@"D:\软件\Kmplayerbeta\KMPlayer.exe", 
		        			@"D:\生活大爆炸.The.Big.Bang.Theory.S04E08.Chi_Eng.HDTVrip.624X352-YYeTs人人影视.rmvb");
		        	m_prcsMediaPlayer.Start();
	        	}
        	}
        	catch (Exception ex)
        	{
        		m_prcsMediaPlayer = null;
        		WriteLine(ex.Message);
        	}
        }
        
        void TmrGameTick(object state)
        {
        	StopTimer();
        	
        	WriteLine("tick in");
        	if (m_bGameMode)
        	{ // 弹起按下的按键
        		m_mreExiting.Reset();
        		Thread.Sleep(100);
        		m_myIoSys.KeyUp(WinIoSys.Key.VK_UP);
        		Thread.Sleep(100);
        		m_myIoSys.KeyUp(WinIoSys.Key.VK_DOWN);
        		m_mreExiting.Set();
        	}
        	m_bGameMode = !m_bGameMode;

            if (m_bGameMode)
                WriteLine("进入游戏模式");
            else
            {
                WriteLine("退出游戏模式");
                m_mreExiting.Reset();                
                m_myIoSys.KeyPress(WinIoSys.Key.VK_ESCAPE, 100);
                WriteLine("esc");
                Thread.Sleep(200);
                SetForegroundWindow(m_prcsGame.MainWindowHandle);
                m_myIoSys.KeyPressEx(WinIoSys.Key.VK_UP, 100);
                WriteLine("up");
                Thread.Sleep(200);
                SetForegroundWindow(m_prcsGame.MainWindowHandle);
                m_myIoSys.KeyPressEx(WinIoSys.Key.VK_RETURN, 100);
                WriteLine("enter");
                Thread.Sleep(200);
                SetForegroundWindow(m_prcsGame.MainWindowHandle);
                m_myIoSys.KeyPressEx(WinIoSys.Key.VK_UP, 100);
                WriteLine("up");
                Thread.Sleep(200);
                SetForegroundWindow(m_prcsGame.MainWindowHandle);
                m_myIoSys.KeyPressEx(WinIoSys.Key.VK_RETURN, 100);
                WriteLine("enter");
                Thread.Sleep(200);
                m_mreExiting.Set();
            }
        	WriteLine("tick out");
        }
        #endregion
    }

    /// <summary>
    /// 控制鼠标
    /// </summary>
    class MouseControl
    {
        /// <summary>
        /// 鼠标控制参数
        /// </summary>
        const int MOUSEEVENTF_LEFTDOWN   = 0x2;
        const int MOUSEEVENTF_LEFTUP     = 0x4;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const int MOUSEEVENTF_MIDDLEUP   = 0x40;
        const int MOUSEEVENTF_MOVE       = 0x1;
        const int MOUSEEVENTF_ABSOLUTE   = 0x8000;
        const int MOUSEEVENTF_RIGHTDOWN  = 0x8;
        const int MOUSEEVENTF_RIGHTUP    = 0x10;

        /// <summary>
        /// 鼠标的位置
        /// </summary>
        public struct PONITAPI
        {
            public int x, y;
        }

        [DllImport("user32.dll")]
        public static extern int GetCursorPos(ref PONITAPI p);

        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        /// <summary>
        /// 左键单击
        /// </summary>
        public static void LeftClick()
        {
            PONITAPI location = new PONITAPI();
            GetCursorPos(ref location);
            mouse_event(MOUSEEVENTF_LEFTDOWN, location.x, location.y, 0, 0);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTUP, location.x, location.y, 0, 0);
        }

        public static void LeftDown()
        {
            PONITAPI location = new PONITAPI();
            GetCursorPos(ref location);
            mouse_event(MOUSEEVENTF_LEFTDOWN, location.x, location.y, 0, 0);
        }

        public static void LeftUp()
        {
            PONITAPI location = new PONITAPI();
            GetCursorPos(ref location); 
            mouse_event(MOUSEEVENTF_LEFTUP, location.x, location.y, 0, 0);
        }

        public static void RightDown()
        {
            PONITAPI location = new PONITAPI();
            GetCursorPos(ref location);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, location.x, location.y, 0, 0);
        }

        public static void RightUp()
        {
            PONITAPI location = new PONITAPI();
            GetCursorPos(ref location);
            mouse_event(MOUSEEVENTF_RIGHTUP, location.x, location.y, 0, 0);
        }
        /// <summary>
        /// 移动鼠标
        /// </summary>
        /// <param name="xDisplacement">横向移动距离（右正左负）</param>
        /// <param name="yDisplacement">纵向移动距离（下正上负）</param>
        public static void MoveCursor(int xDisplacement, int yDisplacement)
        {
            PONITAPI location = new PONITAPI();
            GetCursorPos(ref location);
            SetCursorPos(location.x + xDisplacement, location.y + yDisplacement);
        }
    }
    
    /// <summary>
    /// 控制键盘(暂时没用~)
    /// </summary>
    class KeyboardControl
    {
        #region 虚拟按键(Windows 使用的256个虚拟键码)
		public const int VK_LBUTTON = 0x1;
		public const int VK_RBUTTON = 0x2;
		public const int VK_CANCEL  = 0x3;
		public const int VK_MBUTTON = 0x4;
		public const int VK_BACK 	= 0x8;
		public const int VK_TAB 	= 0x9;
		public const int VK_CLEAR 	= 0xC;
		public const int VK_RETURN 	= 0xD;
		public const int VK_SHIFT 	= 0x10;
		public const int VK_CONTROL = 0x11;
		public const int VK_MENU 	= 0x12;
		public const int VK_PAUSE 	= 0x13;
		public const int VK_CAPITAL = 0x14;
		public const int VK_ESCAPE 	= 0x1B;
		public const int VK_SPACE 	= 0x20;
		public const int VK_PRIOR 	= 0x21;
		public const int VK_NEXT 	= 0x22;
		public const int VK_END 	= 0x23;
		public const int VK_HOME 	= 0x24;
		public const int VK_LEFT 	= 0x25;
		public const int VK_UP 		= 0x26;
		public const int VK_RIGHT 	= 0x27;
		public const int VK_DOWN 	= 0x28;
		public const int VK_SELECT 	= 0x29;
		public const int VK_PRINT 	= 0x2A;
		public const int VK_EXECUTE = 0x2B;
		public const int VK_SNAPSHOT = 0x2C;
		public const int VK_INSERT 	= 0x2D;
		public const int VK_DELETE 	= 0x2E;
		public const int VK_HELP 	= 0x2F;
		public const int VK_0 = 0x30;
		public const int VK_1 = 0x31;
		public const int VK_2 = 0x32;
		public const int VK_3 = 0x33;
		public const int VK_4 = 0x34;
		public const int VK_5 = 0x35;
		public const int VK_6 = 0x36;
		public const int VK_7 = 0x37;
		public const int VK_8 = 0x38;
		public const int VK_9 = 0x39;
		public const int VK_A = 0x41;
		public const int VK_B = 0x42;
		public const int VK_C = 0x43;
		public const int VK_D = 0x44;
		public const int VK_E = 0x45;
		public const int VK_F = 0x46;
		public const int VK_G = 0x47;
		public const int VK_H = 0x48;
		public const int VK_I = 0x49;
		public const int VK_J = 0x4A;
		public const int VK_K = 0x4B;
		public const int VK_L = 0x4C;
		public const int VK_M = 0x4D;
		public const int VK_N = 0x4E;
		public const int VK_O = 0x4F;
		public const int VK_P = 0x50;
		public const int VK_Q = 0x51;
		public const int VK_R = 0x52;
		public const int VK_S = 0x53;
		public const int VK_T = 0x54;
		public const int VK_U = 0x55;
		public const int VK_V = 0x56;
		public const int VK_W = 0x57;
		public const int VK_X = 0x58;
		public const int VK_Y = 0x59;
		public const int VK_Z = 0x5A;
		public const int VK_STARTKEY 	= 0x5B;
		public const int VK_CONTEXTKEY 	= 0x5D;
		public const int VK_NUMPAD0 = 0x60;
		public const int VK_NUMPAD1 = 0x61;
		public const int VK_NUMPAD2 = 0x62;
		public const int VK_NUMPAD3 = 0x63;
		public const int VK_NUMPAD4 = 0x64;
		public const int VK_NUMPAD5 = 0x65;
		public const int VK_NUMPAD6 = 0x66;
		public const int VK_NUMPAD7 = 0x67;
		public const int VK_NUMPAD8 = 0x68;
		public const int VK_NUMPAD9 = 0x69;
		public const int VK_MULTIPLY 	= 0x6A;
		public const int VK_ADD 		= 0x6B;
		public const int VK_SEPARATOR 	= 0x6C;
		public const int VK_SUBTRACT    = 0x6D;
		public const int VK_DECIMAL 	= 0x6E;
		public const int VK_DIVIDE 		= 0x6F;
		public const int VK_F1 = 0x70;
		public const int VK_F2 = 0x71;
		public const int VK_F3 = 0x72;
		public const int VK_F4 = 0x73;
		public const int VK_F5 = 0x74;
		public const int VK_F6 = 0x75;
		public const int VK_F7 = 0x76;
		public const int VK_F8 = 0x77;
		public const int VK_F9 = 0x78;
		public const int VK_F10 = 0x79;
		public const int VK_F11 = 0x7A;
		public const int VK_F12 = 0x7B;
		public const int VK_F13 = 0x7C;
		public const int VK_F14 = 0x7D;
		public const int VK_F15 = 0x7E;
		public const int VK_F16 = 0x7F;
		public const int VK_F17 = 0x80;
		public const int VK_F18 = 0x81;
		public const int VK_F19 = 0x82;
		public const int VK_F20 = 0x83;
		public const int VK_F21 = 0x84;
		public const int VK_F22 = 0x85;
		public const int VK_F23 = 0x86;
		public const int VK_F24 = 0x87;
		public const int VK_NUMLOCK 	= 0x90;
		public const int VK_OEM_SCROLL 	= 0x91;
		public const int VK_OEM_1 		= 0xBA;
		public const int VK_OEM_PLUS 	= 0xBB;
		public const int VK_OEM_COMMA 	= 0xBC;
		public const int VK_OEM_MINUS 	= 0xBD;
		public const int VK_OEM_PERIOD 	= 0xBE;
		public const int VK_OEM_2 = 0xBF;
		public const int VK_OEM_3 = 0xC0;
		public const int VK_OEM_4 = 0xDB;
		public const int VK_OEM_5 = 0xDC;
		public const int VK_OEM_6 = 0xDD;
		public const int VK_OEM_7 = 0xDE;
		public const int VK_OEM_8 = 0xDF;
		public const int VK_ICO_F17 	= 0xE0;
		public const int VK_ICO_F18 	= 0xE1;
		public const int VK_OEM102 		= 0xE2;
		public const int VK_ICO_HELP 	= 0xE3;
		public const int VK_ICO_00 		= 0xE4;
		public const int VK_ICO_CLEAR 	= 0xE6;
		public const int VK_OEM_RESET 	= 0xE9;
		public const int VK_OEM_JUMP 	= 0xEA;
		public const int VK_OEM_PA1 = 0xEB;
		public const int VK_OEM_PA2 = 0xEC;
		public const int VK_OEM_PA3 = 0xED;
		public const int VK_OEM_WSCTRL 	= 0xEE;
		public const int VK_OEM_CUSEL 	= 0xEF;
		public const int VK_OEM_ATTN 	= 0xF0;
		public const int VK_OEM_FINNISH = 0xF1;
		public const int VK_OEM_COPY 	= 0xF2;
		public const int VK_OEM_AUTO 	= 0xF3;
		public const int VK_OEM_ENLW 	= 0xF4;
		public const int VK_OEM_BACKTAB = 0xF5;
		public const int VK_ATTN 	= 0xF6;
		public const int VK_CRSEL 	= 0xF7;
		public const int VK_EXSEL 	= 0xF8;
		public const int VK_EREOF 	= 0xF9;
		public const int VK_PLAY 	= 0xFA;
		public const int VK_ZOOM 	= 0xFB;
		public const int VK_NONAME 	= 0xFC;
		public const int VK_PA1 	= 0xFD;
		public const int VK_OEM_CLEAR =	 0xFE;
        #endregion
        
        #region wMsg参数常量值
        // WM_KEYDOWN 按下一个键
        public static int WM_KEYDOWN = 0x0100;
        // 释放一个键
        public static int WM_KEYUP = 0x0101;
        // 按下某键，并已发出WM_KEYDOWN， WM_KEYUP消息
        public static int WM_CHAR = 0x102;
        #endregion
        // 向窗口发送消息
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        
        // 释放按键的常量（dwFlags）
        private const int KEYEVENTF_KEYUP = 2;  
        // 模拟键盘事件
        [DllImport("User32.dll")]
        public static extern void keybd_event(Byte bVk, Byte bScan, Int32 dwFlags, Int32 dwExtraInfo);
        
        // 临时使窗口位于最上层并获得焦点
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
