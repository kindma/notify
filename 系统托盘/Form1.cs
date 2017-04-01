using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace 系统托盘
{
    public partial class Form1 : Form
    {
        public System.Timers.Timer m_Timer;

        public Form1()
        {
            InitializeComponent();
        }
        private void ShowWindows()
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowWindows();
        }

        private void 显示窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindows();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //在窗体上点击关闭按钮程序不会退出，需自己添加退出方法 
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_Timer = new System.Timers.Timer(); // explicit namespace (Timer also in System.Threading) 

            m_Timer.Elapsed += new ElapsedEventHandler(OnTimerKillPopup);

            m_Timer.Interval = 2000; // for instance 3000 milliseconds 

            m_Timer.Enabled = true; // start timer 
        }

        protected void OnTimerKillPopup(Object source, ElapsedEventArgs e)
        {
            var stopTime = false;

            m_Timer.Enabled = false; // pause the timer 

            

                IntPtr hParent = IntPtr.Zero;
                IntPtr hNext = IntPtr.Zero;
                String sClassNameFilter = "IEFrame"; // 所有IE窗口的类 
                do
                {
                    hNext = NativeWIN32.FindWindowEx(hParent, hNext, sClassNameFilter, IntPtr.Zero);
                    // we've got a hwnd to play with 
                    if (!hNext.Equals(IntPtr.Zero))
                    {
                        // get window caption 
                        NativeWIN32.STRINGBUFFER sLimitedLengthWindowTitle;
                        NativeWIN32.GetWindowText(hNext, out sLimitedLengthWindowTitle, 256);
                        String sWindowTitle = sLimitedLengthWindowTitle.szText;
                        if (sWindowTitle.Length > 0)
                        {
                            // find this caption in the list of banned captions 

                            // if (sWindowTitle.StartsWith("要素"))    
                            //NativeWIN32.SendMessage(hNext, NativeWIN32.WM_SYSCOMMAND,  NativeWIN32.SC_CLOSE, IntPtr.Zero); // try soft kill 

                            if (sWindowTitle.StartsWith(this.textBox1.Text))
                            {
                                test.PlaySound("happy.wav", 0, 9);
                                stopTime = true;
                                 
                            }


                        }
                    }
                }
                while (!hNext.Equals(IntPtr.Zero));
          
            if(!stopTime)  m_Timer.Enabled = true;

        }

     

        class test
        {
            [DllImport("winmm.dll")]
            public static extern bool PlaySound(String Filename, int Mod, int Flags);
            
        }


        public class NativeWIN32
        {
            public const int WM_SYSCOMMAND = 0x0112;

            public const int SC_CLOSE = 0xF060;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr FindWindowEx(IntPtr parent,
            IntPtr next /*HWND*/,
            string sClassName,
            IntPtr sWindowTitle);

            public delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);
            // used for an output LPCTSTR parameter on a method call 
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct STRINGBUFFER
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
                public string szText;
            }
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int GetWindowText(IntPtr hWnd, out STRINGBUFFER ClassName, int nMaxCount);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            test.PlaySound(null,0,0x40|0x04|0x02);
 
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_Timer.Enabled = true;
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
