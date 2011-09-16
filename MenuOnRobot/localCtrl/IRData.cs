using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MenuOnRobot
{
    public enum IR_Returns : byte
    {
        IR_Init    = (byte)0,
        IR_Success = (byte)1, // 操作成功
        IR_Fail    = (byte)2  // 操作失败
    }

    class IRData : SensorNet
    {
        #region 构造函数
        public IRData(byte[] byData)
        {
            if (byData.Length < PackageHeadLen) return;
            _RemoteController(byData);
        }

        private void _RemoteController(byte[] by)
        {
            byLen = by[1];
            byClustID = by[2];
            dbAdd[0] = by[3];
            dbAdd[1] = by[4];
            if (by.Length > PackageHeadLen)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(by, PackageHeadLen, byLen);
                byData = ms.ToArray();
            }
        }
        #endregion
    }
}
