using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MenuOnRobot
{
    class SensorNet
    {
        #region 成员变量
        public const int PackageHeadLen = 5; // 包头的固定长度
        public const byte PackageHead = 0x26; // 包头

        protected byte byLen = 0;    // 数据段长度
        protected byte byClustID = 0;
        protected byte[] dbAdd = { 0, 0 };// 地址位   
        protected byte[] byData = null; // 数据段内容

        public UInt16 Address
        {
            get { return (UInt16)(dbAdd[0] + 256 * dbAdd[1]); }
            set
            {
                dbAdd[0] = (byte)(value % 256);
                dbAdd[1] = (byte)(value / 256);
            }
        }

        public byte ClustID
        {
            get { return byClustID; }
            set { byClustID = value; }
        }

        public byte[] Data
        {
            get { return byData; }
        }

        public byte CMD
        {
            get { return Data[1]; }
        }
        #endregion

        public void WriteData(byte[] by, int offset, int count)
        {
            if (offset < 0 || offset + count > by.Length) return;

            MemoryStream ms = new MemoryStream();
            ms.Write(by, offset, count);

            byData = ms.ToArray();
            byLen = (byte)byData.Length;
        }

        public virtual byte[] ToArray()
        {
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(PackageHead);
            ms.WriteByte(byLen);
            ms.WriteByte(byClustID);
            ms.WriteByte(dbAdd[0]);
            ms.WriteByte(dbAdd[1]);
            ms.Write(byData, 0, byData.Length);

            return ms.ToArray();
        }
    }
}
