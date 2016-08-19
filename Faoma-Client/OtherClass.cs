using System.Collections.Generic;
using System.Windows.Controls;
using Faoma_Client;
using System;

namespace Faoma_Client
{
    internal class OtherClass
    {
        private ListBox lbLog;
        //private MainWindow mMainForm;

       // public OtherClass(MainWindow mainForm)
        //{
       //     mMainForm = mainForm;
       // }

        public OtherClass()
        {
        }

        public OtherClass(ListBox lbLog, List<string> logging)
        {
            this.lbLog = lbLog;

            foreach (var item in logging)
            {
                lbLog.Items.Add(item);
            }

            //lbLog.Items.Add
        }

        public void addLog(List<string> logging)
        {
            // MainWindow.Dispatcher.Invoke((System.Action)(() =>
            // System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
             //{
                 //OtherClass o = new OtherClass( lbLog, logging);
                foreach (var item in logging)
                {
                  lbLog.Items.Add(item);
                }
            //}));
            
        }
    }
}