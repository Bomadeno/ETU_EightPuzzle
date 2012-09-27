using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ETUDriver;

namespace EightPuzzle_Mouse
{

    public partial class ETUHost : Form
    {
        private  CoETUDriverClass etuDriver;

        public ETUHost()
        {
            InitializeComponent();

            
            if(etuDriver == null)
                etuDriver = new CoETUDriverClass();

            var screenTopLeft = new SiETUDFloatPoint();
            screenTopLeft.X = 0;
            screenTopLeft.Y = 0;
            etuDriver.set_Offset(ref screenTopLeft);
            int size = 128;
            String devName = new String(' ', size);
            for(int i = 0; i< etuDriver.DeviceCount; i++)
            {
                etuDriver.getNameFromIndex(i, ref devName, ref size);
                if (devName.StartsWith("Mouse"))
                {
                    etuDriver.Index = i;
                }
            }
            etuDriver.stopTracking();
        }
    }
}
