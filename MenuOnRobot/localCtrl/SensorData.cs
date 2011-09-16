using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MenuOnRobot
{
    class SensorData : SensorNet
    {
        #region 构造函数
        public SensorData(byte[] byData)
        {
            if (byData.Length < PackageHeadLen) return;
            _SensorData(byData);
        }

        private void _SensorData(byte[] by)
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
