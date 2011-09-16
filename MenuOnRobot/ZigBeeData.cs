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

namespace ServiceTest
{
	/// <summary>
	/// zigbee 传出数据
	/// </summary>
	public class ZigBeeData : IDisposable
	{
		private ZigBeeHeadStruct m_HeadData;
		private int m_nPosition = 0; // m_byData 写入位置标记
		public byte[] m_byData;
		public byte m_byFSC; // 校验字节
		public ZigBeeHeadStruct DataHead
		{
			get {return m_HeadData;}
		}
		public int nPosition
		{
			get {return m_nPosition;}
			set {int temp = value < m_byData.Length ? m_nPosition = value : m_nPosition = m_byData.Length;}			
		}
		// 数据段的实际长度
		public int nDataLength
		{ // 包头标记的数据段长度中包含了 ENDPOINT 和 CLUSTID（一共两字节）
			get {return m_HeadData.DATA_LEN - 2;}
//			set {m_HeadData.DATA_LEN = }
		}
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
		/// <summary>
		/// 转换为字节数组
		/// </summary>
		/// <returns>生成的字节数组</returns>
		public byte[] ToByte()
		{
			// m_byData.Length 中不包含 CLUST，末尾有一位 FSC
			byte[] result;
			MemoryStream msTemp = new MemoryStream((int)HEAD_LEN.ZIGBEE_HEAD_LEN + m_byData.Length + 1);
			BinaryWriter bwTemp = new BinaryWriter(msTemp);
			bwTemp.Write(m_HeadData.ToArray(), 0, (int)HEAD_LEN.ZIGBEE_HEAD_LEN);
			bwTemp.Write(m_byData, 0, m_byData.Length);
			// 求 FSC
			byte byFSC = m_HeadData.GetFSC();
			int i;
			for (i = 0; i < m_byData.Length; i++)
			{
				byFSC ^=m_byData[i];
			} // end of for
			bwTemp.Write(byFSC);
			result = msTemp.ToArray();
			return result;
		} // end of ToByte()
		/// <summary>
		/// 生成发给 wheel 的字符串
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string strDst;
			switch (m_HeadData.CLUST & 0x1f)
			{
				case 0x01:
					{
						strDst = string.Format("#0P{0:D4}T250\r", m_byData[0] * 256 + m_byData[1]);
					}break;
				case 0x02:
					{
						strDst = string.Format("#1P{0:D4}T250\r", m_byData[0] * 256 + m_byData[1]);
					}break;
				case 0x1c:
					{
						strDst = string.Format("#3P{0:D4}#5P{1:D4}#7P{2:D4}T250\r"
						                       , m_byData[0] * 256 + m_byData[1]
						                       , m_byData[2] * 256 + m_byData[3]
						                       , m_byData[4] * 256 + m_byData[5]);
					}break;
				default :
					strDst = null;
					break;
			} // end of switch
			return strDst;
		}
		public void Dispose()
		{
			
		}
	}
}
