using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MenuOnRobot
{
    class GestureControl : SensorNet
    {       
        #region 构造函数
        public GestureControl(byte[] byData)
        {
            if (byData.Length < PackageHeadLen) return;
            _GestureControl(byData);
        }

        private void _GestureControl(byte[] by)
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

        public void SetData(byte by, int pos)
        {
            if (pos < 0 || pos >= byData.Length) return;

            byData[pos] = by;
        }
    }
}
