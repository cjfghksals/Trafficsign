using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using static Trafficsign.Form1;
using System.Net.Configuration;

namespace Trafficsign
{
    

    public partial class Form1 : Form
    {
        static int num = 0;
        static int num2 = 0;
        static string time;

        Thread m_thread1 = null;
        Thread m_thread2 = null;
        Thread m_thread3 = null;

        public Form1()
        {
            InitializeComponent();
            Initialize();
            Logger.WriteLog(LogType.Info, "Application started");
        }

        public void Initialize()
        {
            m_thread1 = new Thread(() => update1());
            m_thread1.IsBackground = true;
            m_thread1.Start();
            Logger.WriteLog(LogType.Info, "Threads1 initialized");

            m_thread2 = new Thread(() => update2());
            m_thread2.IsBackground = true;
            m_thread2.Start();
            Logger.WriteLog(LogType.Info, "Threads2 initialized");
            
            m_thread2 = new Thread(() => update3());
            m_thread2.IsBackground = true;
            m_thread2.Start();
            Logger.WriteLog(LogType.Info, "Threads3 initialized");
        }

        public void update1()
        {
            while (true)
            {
                Stopwatch sw1 = new Stopwatch();

                sw1.Start();
                TracStart();
                Thread.Sleep(2000);
                sw1.Stop();
                cycle1.Text = $"Thread1 Cycle Time : {sw1.ElapsedMilliseconds}ms";
                Logger.WriteLog(LogType.Debug, $"Thread1 Cycle Time: {sw1.ElapsedMilliseconds}ms");
            }
        }

        public void update2()
        {
            while (true)
            {
                Timer_Tick();
                Thread.Sleep(1000);
            }
        }
        
        public void update3()
        {
            while (true)
            {
                ShowLog();
                Thread.Sleep(100);
            }
        }

        public void TracStart()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(TracStart));
                return;
            }
            num += 1;
            if (num > 8)
            {
                num = 1;
            }
            UpdateTrafficSignImage(num);
            Logger.WriteLog(LogType.Info, $"Traffic light state changed to: {num}");
        }

        private void UpdateTrafficSignImage(int num)
        {
            switch (num)
            {
                case 1:
                    trafficsign2.Image = Properties.Resources.신호등3_2;
                    trafficsign1.Image = Properties.Resources.신호등1_1;
                    pedestrian3_1.Image = Properties.Resources.pedestrian_traffic_lights1;
                    pedestrian3_2.Image = Properties.Resources.pedestrian_traffic_lights1;
                    pedestrian2_1.Image = Properties.Resources.pedestrian_traffic_lights2;
                    pedestrian2_2.Image = Properties.Resources.pedestrian_traffic_lights2;
                    break;
                case 2:
                    trafficsign1.Image = Properties.Resources.신호등2_1;
                    break;
                case 3:
                    trafficsign1.Image = Properties.Resources.신호등3_1;
                    trafficsign4.Image = Properties.Resources.신호등1_4;
                    pedestrian2_1.Image = Properties.Resources.pedestrian_traffic_lights1;
                    pedestrian2_2.Image = Properties.Resources.pedestrian_traffic_lights1;
                    pedestrian1_1.Image = Properties.Resources.pedestrian_traffic_lights2;
                    pedestrian1_2.Image = Properties.Resources.pedestrian_traffic_lights2;
                    break;
                case 4:
                    trafficsign4.Image = Properties.Resources.신호등2_4;
                    break;
                case 5:
                    trafficsign4.Image = Properties.Resources.신호등3_4;
                    trafficsign3.Image = Properties.Resources.신호등1_3;
                    pedestrian1_1.Image = Properties.Resources.pedestrian_traffic_lights1;
                    pedestrian1_2.Image = Properties.Resources.pedestrian_traffic_lights1;
                    pedestrian4_1.Image = Properties.Resources.pedestrian_traffic_lights2;
                    pedestrian4_2.Image = Properties.Resources.pedestrian_traffic_lights2;
                    break;
                case 6:
                    trafficsign3.Image = Properties.Resources.신호등2_3;
                    break;
                case 7:
                    trafficsign3.Image = Properties.Resources.신호등3_3;
                    trafficsign2.Image = Properties.Resources.신호등1_2;
                    pedestrian4_1.Image = Properties.Resources.pedestrian_traffic_lights1;
                    pedestrian4_2.Image = Properties.Resources.pedestrian_traffic_lights1;
                    pedestrian3_1.Image = Properties.Resources.pedestrian_traffic_lights2;
                    pedestrian3_2.Image = Properties.Resources.pedestrian_traffic_lights2;
                    break;
                case 8:
                    trafficsign2.Image = Properties.Resources.신호등2_2;
                    break;
            }
            Logger.WriteLog(LogType.Debug, $"Updated traffic signs for state {num}");
        }
        public void ShowLog()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ShowLog));
                return;
            }
            tb_ShowLog.Text = num2.ToString();
            num2++;
        }

        public enum LogType
        {
            Debug,
            Info,
            Error
        }

        public class LogMessage
        {
            public LogType logtype;
            public static string logmsg;

            public LogMessage(LogType log, string msg)
            {
                logtype = log;
                logmsg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{log}] {msg}{Environment.NewLine}";
            }


        }

        public class Logger
        {
            private static readonly object writeLock = new object();
            private const string LogFilePath = "log.txt";

            public static void WriteLog(LogType logType, string message, bool writeNow = true)
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{logType}] {message}";
                Debug.WriteLine(logMessage);

                if (writeNow)
                {
                    lock (writeLock)
                    {
                        try
                        {
                            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error writing to log file: {ex.Message}");
                        }
                    }
                }
            }
        }


        public void Timer_Tick()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Timer_Tick));
                return;
            }
            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            clock.Text = time;
            Logger.WriteLog(LogType.Debug, $"Clock updated: {time}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Logger.WriteLog(LogType.Info, "Form1 loaded");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logger.WriteLog(LogType.Info, "Button1 clicked");
            tb_ShowLog.Text = "버튼이 눌렸습니다.";
        }
    }
}