using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using DevComponents.DotNetBar;
using System.Windows.Forms;

namespace MenuOnRobot
{
    /// <summary>
    /// 遥控器类（包括 标题 按钮等）
    /// </summary>
    class RemoteController
    {
        #region 成员变量
        /// <summary>
        /// 遥控器名
        /// </summary>
        public string m_strCaption;
        /// <summary>
        /// 已经保存的按钮数目
        /// </summary>
        public int ButtonCount
        {
            get { return m_Buttons.Count; }
        }

        // 遥控器按钮集合
        private ArrayList m_Buttons = new ArrayList();
        
        #endregion

        public RemoteController()
        {
        }

        public RemoteController(string strCaption)
        {
            m_strCaption = strCaption;
        }

        public ButtonX[] GetButtons()
        {
            ButtonX[] btns;
            if (0 >= m_Buttons.Count)
            {
                btns = null;
            }
            else
            {
                btns = new ButtonX[m_Buttons.Count];
                int i = 0;
                for (; i < m_Buttons.Count; i++)
                {
                    btns[i] = (ButtonX)m_Buttons[i];
                }
            } // end of else

            return btns;
        }

        public void AddButton(ButtonX btnx)
        {
            m_Buttons.Add(btnx);
        }
        /// <summary>
        /// 暂时无用
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        private Control.ControlCollection GetControls(Control owner)
        {
            Control.ControlCollection tempCollection;

            if (0 >= m_Buttons.Count)
            {
                tempCollection = null;
            } // end of if
            else
            {
                tempCollection = new Control.ControlCollection(owner);

                foreach (object o in m_Buttons)
                {
                    tempCollection.Add((ButtonX)o);
                }
            } // end of else

            return tempCollection;
        }
    }
}
