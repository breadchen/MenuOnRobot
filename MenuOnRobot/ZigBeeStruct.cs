/*
 * Created by SharpDevelop.
 * User: cm
 * Date: 2011-3-7
 * Time: 15:16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace ServiceTest
{	
	public enum ZIGBEE_CLUST_ID : byte
	{		
		ROBOT_BIND_ID 	= 0x0001, 	  // 机器人地址绑定
		TEMP_ID      	= 0x0002,     // 温度
		TPHU_ID       	= 0x0003,     // 温湿度
		LIGHT_ID      	= 0x0004,     // 光敏传感器节点
		SMOKE_ID      	= 0x0005,     // 烟雾传感器节点
		INFR_ID       	= 0x0006,     // 学习型红外遥控器节点
		XY_RSSI_REQ		= 0x0011,	  // 参考点坐标请求（所有）
		XY_RSSI_RES		= 0x0012,	  // 参考点坐标应答
		BIND_NODE_FIND_REQ 	= 0x0014, // 盲节点查找
		REF_NODE_CFG	= 0x0015,	  // 参考点配置请求
		BIND_NODE_CFG	= 0x0016,	  // 盲节点配置请求
		REF_NODE_REQ_CFG 	= 0x0017, // 参考节点信息请求
		BIND_NODE_CFG_REQ 	= 0x0018, // 盲节点信息请求
		RSSI_BLAST		= 0x0019,	  // 广播
		HANDMOTE_ID   	= 0x00e0      // 姿态测试手套
	}
	#region 过时的的接收包格式
//	/// <summary>
//	/// 接收包包头的数据格式
//	/// </summary>
//	[Serializable]
//	public struct ZigBeeRcvHeadStruct
//	{
//		private readonly byte _SOP;			// 包头标识 （默认为 0x02）
//		private readonly UInt16 _CMD;		// 同上	（默认为 0x0018）
//		private readonly byte _CLUST;		// 功能 ID（值在 enum ZIGBEE_CLUST_ID 中）
//		private readonly byte _DATA_LEN;	// 数据段长度（CLUST不计算在数据段中）
//		public byte SOP {get {return _SOP;}}
//		public UInt16 CMD {get {return _CMD;}}
//		public byte CLUST {get {return _CLUST;}}
//		public byte DATA_LEN {get {return _DATA_LEN;}}		
//		
//		public ZigBeeRcvHeadStruct(byte[] temp)
//		{
//			_SOP = temp[0];
//			_CMD = (UInt16)(temp[1] * 256 + temp[2]);
//			_CLUST = temp[3];
//			_DATA_LEN = temp[4];
//		} // end of zigBeeHeadStruct()
//		
//		public override string ToString()
//		{
//			return string.Format("sop[{0}] cmd[{1}] clust[{2}] dataLenth[{3}]"
//			                     , _SOP, _CMD, _CLUST, _DATA_LEN);
//		}
//	} // end of struct ZigBeeRcvHeadStruct
	#endregion
	
	/// <summary>
	/// 发送包包头的数据格式 
	/// </summary>
	[Serializable]
	public struct ZigBeeHeadStruct
	{
		public byte SOP;		// 包头标识 （默认为 0x02）
		public UInt16 CMD;		// 同上	（默认为 0x0018）	
		public byte DATA_LEN;	// 收发包时的数据段长度（ENDPOINT、CLUST算在数据段中, FSC不算在内）
		public byte ENDPOINT;	// 目标端口（默认为 10）
		public byte CLUST;		// 功能 ID（值在 enum ZIGBEE_CLUST_ID 中）
		
		public ZigBeeHeadStruct(string str)
		{ // 将传入字符串按','分割，分别赋给每个参数
			string[] temp = str.Split(',');
			if (temp.Length > 4)
			{
				SOP = byte.Parse(temp[0]);
				CMD = UInt16.Parse(temp[1]);
				DATA_LEN = byte.Parse(temp[2]);
				ENDPOINT = byte.Parse(temp[3]);
				CLUST = byte.Parse(temp[4]);
			} // end of if
			else
			{
				MessageBox.Show("传入参数数量不足");
				SOP = 0x02;
				CMD = 0x0018;
				DATA_LEN = 0;
				ENDPOINT = 10;
				CLUST = (byte)ZIGBEE_CLUST_ID.ROBOT_BIND_ID;
			} // end of else
		} // end of ZigBeeSendHeadStruct()
		
		public ZigBeeHeadStruct(byte[] by)
		{
				SOP = by[0];
				CMD = (UInt16)(by[1] * 256 + by[2]);
				DATA_LEN = by[3];
				ENDPOINT = by[4];
				CLUST = by[5];
		} // end of ZigBeeHeadStruct()
		/// <summary>
		/// 把结构体转换成 byte[] 格式
		/// </summary>
		/// <returns>转换得到的 byte[]</returns>
		public byte[] ToArray()
		{
			byte[] result = new byte[(int)HEAD_LEN.ZIGBEE_HEAD_LEN];
			#region 复杂的方法
//			MemoryStream msTemp = new MemoryStream((int)HEAD_LEN.ZIGBEE_SEND_HEAD_LEN);
//			BinaryWriter bwTemp = new BinaryWriter(msTemp);
//			bwTemp.Write(SOP);
//			bwTemp.Write((byte)(CMD / 256));
//			bwTemp.Write((byte)(CMD % 256));
//			bwTemp.Write(DATA_LEN);
//			bwTemp.Write(ENDPOINT);
//			bwTemp.Write(CLUST);
//			result = msTemp.ToArray();
			#endregion
			result[0] = SOP;
			result[1] = (byte)(CMD / 256);
			result[2] = (byte)(CMD % 256);
			result[3] = DATA_LEN;
			result[4] = ENDPOINT;
			result[5] = CLUST;
			return result;
		} // end of ToByte()
		
		public byte GetFSC()
		{
			byte result = (byte)(CMD / 256);
			result ^= (byte)(CMD % 256);
			result ^= DATA_LEN;
			result ^= ENDPOINT;
			result ^= CLUST;
			return result;
		} // end of GetFSC()
	} // end of struct ZigBeeSendHeadStruct
}
