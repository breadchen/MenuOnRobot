/*
 * Created by SharpDevelop.
 * User: cm
 * Date: 2011-3-8
 * Time: 11:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Drawing;

namespace MenuOnRobot
{
    /// <summary>
    /// 传感器网络控制命令
    /// </summary>
    public enum ZigBeeCMD : byte
    {
        CMD_SYSINFO           = 0x0000,
        CMD_GETRESLOC         = 0x0001,
        CMD_LOCBLD_CONFIG     = 0x0002,
        CMD_INFRGET_GETCODE   = 0x0003,
        CMD_INFRFIRE_FIRECODE = 0x0004,
        CMD_CTRL_ON           = 0x0005, // 开灯
        CMD_CTRL_OFF          = 0x0006, // 关灯
        CMD_CTRL_TOOGLE       = 0x0007, // 开关状态转换
        CMD_SENSOR            = 0x0008, // 传感器信息
        CMD_CTRL_UP			  = 0x0009, // 开窗帘
        CMD_CTRL_DOWN		  = 0x000A, // 关窗帘
        CMD_CTRL_HOLD		  = 0x000B, // 停止运行
        CMD_ROBOT             = 0x000C, // 标明当前地址为机器人
        CMD_USER              = 0x000D, // 标明当前地址为使用者
        CMD_CTRL              = 0x00FE, // 表明当前指令为控制指令
        CMD_QUERY             = 0x00F0  // 表明当前指令为查询指令
    }
    /// <summary>
    /// 手套的功能
    /// </summary>
    public enum GloveFunction : byte
    {
        AMouse      = 0, // 空中鼠标
        GestureCtrl = 1  // 手势控制
    }
    /// <summary>
    /// 空中鼠标的动作(空中鼠标模式下弯曲传感器状态)
    /// </summary>
    public enum AMouseAction : byte
    {
        Move    = 0, // 移动
        Gesture = 1, // 手势识别（右键单击）
        Click   = 3, // 左键单击
        Init	= 4  // 初始状态（未收到动作信息时用）
    }
    /// <summary>
    /// 游戏控制（主要用于NFS）
    /// </summary>
    public enum GameCtrl : byte
    {
        Forward = 0, // 直行
        Left 	= 1, // 左转
        Right 	= 2, // 右转
        Init 	= 3	 // 初始状态（未收到信息时使用）
    }
    /// <summary>
    /// 窗帘控制
    /// </summary>
    public enum Curtain : byte
    {
    	Close = 0,	// 关闭
    	Open  = 1,  // 打开
    	Hold  = 2	// 保持不变
    }
    /// <summary>
    /// 电灯控制
    /// </summary>
    public enum Light : byte
    {
    	Hold  = 0,	// 状态不变
    	Shift = 1	// 状态改变
    }
	/// <summary>
	/// zigbee 传出数据
	/// </summary>
	public class ZigBeeData : IDisposable
	{
        #region 成员变量
		private ZigBeeHeadStruct m_HeadData;
		private int m_nPosition = 0; // m_byData 写入位置标记
		public byte[] m_byData;
		public byte m_byFSC; // 校验字节
        #endregion

        #region 属性（在访问与数据段内容相关的属性时需要手动检查数据段长度是否正常，防止越界）
		public ZigBeeHeadStruct DataHead
		{
			get {return m_HeadData;}
		}
        /// <summary>
        /// 读、写当前写入位置
        /// </summary>
		public int nPosition
		{
			get {return m_nPosition;}
			set {int temp = value < m_byData.Length ? m_nPosition = value : m_nPosition = m_byData.Length;}			
		}
		/// <summary>
		/// 数据段的实际长度
		/// </summary>
		public int nDataLength
		{ // 包头标记的数据段长度中包含了 ENDPOINT 和 CLUSTID（一共两字节）
			get {return m_HeadData.DATA_LEN - 2;}
//			set {m_HeadData.DATA_LEN = }
		}
        /// <summary>
        /// 手套的工作模式
        /// </summary>
        public byte GloveMode
        {
            get { return m_byData[0]; }
        }
        /// <summary>
        /// 弯曲传感器状态
        /// </summary>
        public byte BendSensor
        {
            get { return m_byData[1]; }
        }
        /// <summary>
        /// 横向移动距离
        /// </summary>
        public int XMovement
        {
            get
            {
                int result = 0;
                result = (m_byData[2] < 40) ? (m_byData[2]) : (-(m_byData[2] - 40));
                return result;
            }
        }
        /// <summary>
        /// 纵向移动距离
        /// </summary>
        public int YMovement
        {
            get
            {
                int result = 0;
                result = (m_byData[3] < 40) ? (m_byData[3]) : (-(m_byData[3] - 40));
                return result;
            }
        }
        /// <summary>
        /// 游戏控制标志位
        /// </summary>
        public byte GameContral
        {
            get { return m_byData[4]; }
        }
        #endregion

		#region 构造函数
		public ZigBeeData(string str, byte[] by)
		{
			_ZigBeeData(str, by);
		}		
		public ZigBeeData(string str)
		{
			_ZigBeeData(str);
		}
		public ZigBeeData(byte[] byHeadData)
		{
			_ZigBeeData(byHeadData);
		}		
		public ZigBeeData(byte[] byHeadData, byte[] byData)
		{
			_ZigBeeData(byHeadData, byData);
		}			
		private void _ZigBeeData(string str)
		{
			m_HeadData = new ZigBeeHeadStruct(str);
			m_byData = new byte[nDataLength];
		} // end of _ZigBeeData()
		private void _ZigBeeData(string strHead, byte[] byData)
		{
			m_HeadData = new ZigBeeHeadStruct(strHead);
			m_byData = byData;
		} // end of _ZigBeeData()			
		private void _ZigBeeData(byte[] byHeadData, byte[] byData)
		{
			m_HeadData = new ZigBeeHeadStruct(byHeadData);
			m_byData = byData;
		} // end of _ZigBeeData()		
		private void _ZigBeeData(byte[] byHeadData)
		{
			m_HeadData = new ZigBeeHeadStruct(byHeadData);
			m_byData = new byte[nDataLength];
		} // end of _ZigBeeData()
		#endregion

        #region 输入方法
        /// <summary>
		/// 写入数据
		/// </summary>
		/// <param name="buf">要写入的内容</param>
		/// <param name="offset">写入内容在 buf 中的起始位置</param>
		/// <param name="length">写入长度</param>
		/// <returns>成功写入的字节数</returns>
		public int Write(byte[] buf, int offset, int length)
		{
			int result = 0;
			int i;
			for (i = 0; m_nPosition + i < m_byData.Length && i < length; i++)
			{
				m_byData[m_nPosition + i] = buf[offset + i];
				result++;
			} // end of for
			return result;
		} // end of Write()
		/// <summary>
		/// 写入某个字节的值
		/// </summary>
		/// <param name="by">写入的值</param>
		/// <param name="nPos">写入的位置</param>
		/// <returns>是否成功写入</returns>
		public bool SetByte(byte by, int nPos)
		{
			if (nPos < m_byData.Length)
			{
				m_byData[nPos] = by;
				return true;
			}
			else
			{
				return false;
			}
		} //end of SetByte()
        #endregion

        public void Dispose()
		{
			
		}
	}
}
