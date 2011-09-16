using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace MenuOnRobot
{
    delegate void StartLearnEventHandler();
    delegate void LearnFinishEventHandler(byte[] data);
    public partial class frmInfo : Form
    {
    	#region 成员变量
        private frmLocalCtrl m_frmLocalCtrl;

        private event WriteLogEventHandler _WriteLog;
        public event WriteLogEventHandler WriteLog
        {
            add { _WriteLog = value; }
            remove
            {
                if (null != _WriteLog) _WriteLog -= value;
            }
        }

        // 用于向传感器控制程序发消息
        private UdpClient m_UdpSensorNet = null;
        private static readonly string strIPHead = "192.168.1";
        private readonly IPEndPoint SENSOR_NET_ADDRESS;

        // 用于将组合框中的序号 映射到 组号
        private const int CATEGORY_COUNT = 8;
        private const int NAME_COUNT     = 8;
        private int[] CategoryMap = new int[CATEGORY_COUNT];
        private int[] NameMap     = new int[NAME_COUNT];
        private readonly int[,] GroupMap = 
        {
            { 1, 2, 3, 4, 5, 6, 7, 8},
            { 9,10,11,12,13,14,15,16},
            {17,18,19,20,21,22,23,24},
            {25,26,27,28,29,30,31,32},
            {33,34,35,36,37,38,39,40},
            {41,42,43,44,45,46,47,48},
            {49,50,51,52,53,54,55,56},
            {57,58,59,60,61,62,63,64}
        };

        // 存储正在使用的编号
        private ArrayList m_IDUsing = new ArrayList();
        // 存储未使用的编号（若为空则在已使用编号最后添加新编号）
        private ArrayList m_IDCanUse = new ArrayList();

        // 标记是否在修改模式下
        private bool m_bIsModifying;
        // 提示信息
        private readonly string[] StrTips = 
        {
            "点击需要修改的按钮，开始学习；点击修改按钮，返回使用模式",
            "正在学习，请稍等...",
            "学习成功！点击其他按钮继续学习",
            "学习失败，请重试"
        };

        private event StartLearnEventHandler StartLearn;
        private event LearnFinishEventHandler LearnFinish;
        #endregion

        public frmInfo(frmLocalCtrl f)
        {
            m_frmLocalCtrl = f;

            InitializeComponent();

            m_UdpSensorNet = new UdpClient();
            SENSOR_NET_ADDRESS = new IPEndPoint(_GetLocalIP(), 1699);//new IPEndPoint(IPAddress.Parse("192.168.1.4"), 1699);

            StartLearn = new StartLearnEventHandler(OnStartLearn);
            LearnFinish = new LearnFinishEventHandler(OnLearnFinish);
            #region 写映射表
            int counter = 0;
            for (; counter < CATEGORY_COUNT; counter++)
            {
                CategoryMap[counter] = counter;
            }
            for (counter = 0; counter < NAME_COUNT; counter++)
            {
                NameMap[counter] = counter;
            }
            #endregion
        }

        #region 管理遥控器
        /// <summary>
        /// 删除 ID（将 ID 移动到可使用区）
        /// </summary>
        /// <param name="ID"></param>
        private void RemoveID(int ID)
        {
            if (m_IDUsing.Contains(ID))
            {
                m_IDCanUse.Add(ID);
                m_IDUsing.Remove(ID);
            }
        }
        /// <summary>
        /// 获得新编号(同时保存新编号)
        /// </summary>
        /// <returns></returns>
        private int GetNewID()
        {
            int newID;
            if (0 == m_IDCanUse.Count)
            {
                newID = m_IDUsing.Count;
                m_IDUsing.Add(newID);
            }
            else
            {
                newID = (int)m_IDCanUse[0];
                m_IDCanUse.RemoveAt(0);
                m_IDUsing.Add(newID);
            }
            return newID;
        }
        #endregion

        #region 使用遥控器
        /// <summary>
        /// 获得当前的组号
        /// </summary>
        private int GetCurrentGroupID()
        {
            int result = -1;

            if (cbxCategory.SelectedIndex != -1 && cbxName.SelectedIndex != -1)
            {
                result = GroupMap[
                    CategoryMap[cbxCategory.SelectedIndex], 
                    NameMap[cbxName.SelectedIndex]];
            }

            return result;
        }
        /// <summary>
        /// 使用红外遥控器
        /// </summary>
        /// <param name="ID"></param>
        private void UseIRCode(byte ID)
        {
            // 生成数据包
            byte[] tempData = new byte[IRData.PackageHeadLen + 4];
            tempData[0] = IRData.PackageHead;
            tempData[1] = 4;
            tempData[2] = (byte)ZIGBEE_CLUST_ID.INFR_ID;
            tempData[3] = 0xFF;
            tempData[4] = 0xFF;
            tempData[5] = (byte)ZigBeeCMD.CMD_CTRL;
            tempData[6] = (byte)ZigBeeCMD.CMD_INFRFIRE_FIRECODE;
            tempData[7] = (byte)GetCurrentGroupID();
            tempData[8] = ID;
            IRData ird = new IRData(tempData);

            // 发送数据
            SendToSensorNet(ird);
        }
        /// <summary>
        /// 学习红外遥控器
        /// </summary>
        /// <param name="ID"></param>
        private void LearnIRCode(byte ID)
        {
            // 生成数据包
            byte[] tempData = new byte[IRData.PackageHeadLen + 4];
            tempData[0] = IRData.PackageHead;
            tempData[1] = 4;
            tempData[2] = (byte)ZIGBEE_CLUST_ID.INFR_ID;
            tempData[3] = 0xFF;
            tempData[4] = 0xFF;
            tempData[5] = (byte)ZigBeeCMD.CMD_CTRL;
            tempData[6] = (byte)ZigBeeCMD.CMD_INFRGET_GETCODE;
            tempData[7] = (byte)GetCurrentGroupID();
            tempData[8] = ID;
            IRData ird = new IRData(tempData);

            // 发送数据
            SendToSensorNet(ird);
        }
        /// <summary>
        /// 想传感器网络发送指令
        /// </summary>
        /// <param name="rm">指令的数据段</param>
        /// <returns></returns>
        private void SendToSensorNet(IRData rm)
        {
            byte[] byData = rm.ToArray();

            m_UdpSensorNet.Send(byData, byData.Length, SENSOR_NET_ADDRESS);
            WriteLine("send IRData to[" + SENSOR_NET_ADDRESS.ToString() + "]");

            m_UdpSensorNet.BeginReceive(UdpCallBack, m_UdpSensorNet);
        }

        private void UdpCallBack(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)(ar.AsyncState);
            IPEndPoint e = null;

            Byte[] receiveBytes = u.EndReceive(ar, ref e);
            WriteLine("recive data from[" + e.ToString() + "]");

            if (e.Equals(SENSOR_NET_ADDRESS) && receiveBytes.Length > 0)
            {
                LearnFinish(receiveBytes);
            }            
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

        private void OnStartLearn()
        {
            lblTips.Text = StrTips[1];
        }

        private void OnLearnFinish(byte[] data)
        {
            if ((byte)IR_Returns.IR_Success == data[data.Length - 1])
            {
                lblTips.Text = StrTips[2];
            }
            else
            {
                lblTips.Text = StrTips[3];
            }
        }
        #endregion

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
        private void frmInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_frmLocalCtrl != null) m_frmLocalCtrl.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            // 允许在不同线程中使用控件
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            lblTips.Text = null;

            #region 测试代码
            btnSwitch.Tag = (byte)1;
            #endregion
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        { // 重选组号后需要改变界面上的控件集合
            
        }

        private void RemoteControllerButton_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ButtonX btnTemp = (DevComponents.DotNetBar.ButtonX)sender;

            if (m_bIsModifying)
            {
                StartLearn();

                LearnIRCode((byte)btnTemp.Tag);
            } // end of if
            else
            {
                UseIRCode((byte)btnTemp.Tag);
            } // end of else
        }

        private void btnModifyButton_Click(object sender, EventArgs e)
        {
            if (!m_bIsModifying)
            { // 若未进入修改模式
                m_bIsModifying = true;
                // 显示操作提示
                lblTips.Text = StrTips[0];
            }
            else
            {
                m_bIsModifying = false;
                // 清空提示
                lblTips.Text = null;
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            SensorNet snTemp = new SensorNet();
            byte[] byData = { 1 };

            snTemp.ClustID = 0xFF;
            snTemp.Address = 0xFFFE;
            snTemp.WriteData(byData, 0, 1);

            m_UdpSensorNet.Send(snTemp.ToArray(), snTemp.ToArray().Length, SENSOR_NET_ADDRESS);
        }
        #endregion
    }
}
