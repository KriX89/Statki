using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MediaPlayer;









namespace statki
{
    public partial class Form7 : Form
    {
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        

        public Form7()
        {
            InitializeComponent();
                     // By the default set the volume to 0
         uint CurrVol = 0;
         // At this point, CurrVol gets assigned the volume
         waveOutGetVolume(IntPtr.Zero, out CurrVol);
         // Calculate the volume
         ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
         // Get the volume on a scale of 1 to 10 (to fit the trackbar)
            trackBar1.Value = CalcVol / (ushort.MaxValue / 10);
            label1.Text = trackBar1.Value * 10 + "%";
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int NewVolume = ((ushort.MaxValue / 10) * trackBar1.Value);
            label1.Text = trackBar1.Value*10 + "%";
            // Set the same volume for both the left and the right channels
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            // Set the volume
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
