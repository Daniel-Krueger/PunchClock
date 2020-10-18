using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace PunchClock
{
    public class PunchClockApplicationContext : System.Windows.Forms.ApplicationContext
    {
        private System.Windows.Forms.NotifyIcon ClockStateIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        private ClockStateControl clockStateControl = new ClockStateControl()
        {
            BackgroundColor = Color.Red,
            ForegroundColor = Brushes.White,
            ClockState = ClockState.ClockedOut,
        };

        private readonly string csvFolderPath;
        private readonly string clockInCSVLabel;
        private readonly string clockOutCSVLabel;
        private readonly string clockInIconLabel;
        private readonly string clockOutIconLabel;
        private readonly string csvHeader ;
        private readonly string csvDelimiter;
        private readonly string loggedDateTimeFormat;
        private StreamWriter file;

        public PunchClockApplicationContext()
        {
            InitializeComponent();

            csvFolderPath = ConfigurationManager.AppSettings["folder"];
            clockInCSVLabel = ConfigurationManager.AppSettings["clockInCSVLabel"];
            clockOutCSVLabel = ConfigurationManager.AppSettings["clockOutCSVLabel"];
            clockInIconLabel = ConfigurationManager.AppSettings["clockInIconLabel"];
            clockOutIconLabel = ConfigurationManager.AppSettings["clockOutIconLabel"];
            csvHeader = ConfigurationManager.AppSettings["csvHeader"];
            csvDelimiter = ConfigurationManager.AppSettings["csvDelimiter"];
            loggedDateTimeFormat = ConfigurationManager.AppSettings["loggedDateTimeFormat"];
            EnsureFiles();

            clockStateControl.Icon = ClockStateIcon;
            SetClockedOutState(clockStateControl);
            UpdateIcon(clockStateControl);

        }
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClockStateIcon = new System.Windows.Forms.NotifyIcon(this.components);

            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(121, 26);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(120, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // AdultNotifyIcon
            // 
            this.ClockStateIcon.ContextMenuStrip = this.contextMenu;
            this.ClockStateIcon.Visible = true;
            this.ClockStateIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.changeClockStateClick);
            this.ClockStateIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.changeClockStateClick);

        }

        private void WriteClockInState(ClockStateControl type)
        {
            DateTime date = DateTime.Now;
            switch (type.ClockState)
            {
                case ClockState.ClockedOut:
                    SetClockedInState(type);
                    break;
                case ClockState.ClockedIn:
                    SetClockedOutState(type);
                    break;
                default:
                    break;
            }
            string textToWrite = string.Concat((type.ClockState == ClockState.ClockedIn ? clockInCSVLabel : clockOutCSVLabel), csvDelimiter, date.ToString(loggedDateTimeFormat),csvDelimiter);
            WriteToFile(file, textToWrite, ClockState.ClockedIn == type.ClockState);
            UpdateIcon(type);
        }

        private void SetClockedInState(ClockStateControl type)
        {
            type.BackgroundColor = Color.Green;
            type.IconText = $"{clockInIconLabel} {clockInCSVLabel}"; ;
            type.ClockState = ClockState.ClockedIn;
        }

        private void SetClockedOutState(ClockStateControl type)
        {
            type.BackgroundColor = Color.Red;
            type.IconText = $"{clockOutIconLabel} {clockInCSVLabel}"; ;
            type.ClockState = ClockState.ClockedOut;
        }

        private void changeClockStateClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                WriteClockInState(clockStateControl);
            }
        }

        private void UpdateIcon(ClockStateControl type)
        {
            // Load the original image
            Bitmap bmp = new Bitmap(32, 32);

            RectangleF rectf = new RectangleF(0, 0, bmp.Width, bmp.Height);
            // Create graphic object that will draw onto the bitmap
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(type.BackgroundColor);
            // ------------------------------------------
            // Ensure the best possible quality rendering
            // ------------------------------------------
            // The smoothing mode specifies whether lines, curves, and the edges of filled areas use smoothing (also called antialiasing). 
            // One exception is that path gradient brushes do not obey the smoothing mode. 
            // Areas filled using a PathGradientBrush are rendered the same way (aliased) regardless of the SmoothingMode property.
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // The interpolation mode determines how intermediate values between two endpoints are calculated.
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Use this property to specify either higher quality, slower rendering, or lower quality, faster rendering of the contents of this Graphics object.
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // This one is important
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;


            // Create string formatting options (used for alignment)
            StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Draw the text onto the image
            g.DrawString(type.IconText, new Font("Tahoma", 16), type.ForegroundColor, rectf, format);


            // Flush all graphics changes to the bitmap
            g.Flush();

            // Now save or use the bitmap
            type.Icon.Icon = Icon.FromHandle(bmp.GetHicon());
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
            Program.Quit();
        }

        private void EnsureFiles()
        {
            DirectoryInfo csvFolder;
            if (!System.IO.Directory.Exists(csvFolderPath))
            {
                csvFolder = System.IO.Directory.CreateDirectory(csvFolderPath);
            }
            else
            {
                csvFolder = new DirectoryInfo(csvFolderPath);
            }
            string fileName = System.IO.Path.Combine(csvFolder.FullName, $"{DateTime.Now.Year}-{DateTime.Now.Month}.csv");
            bool fileExists = File.Exists(fileName);
            file = File.AppendText(fileName);
            if (!fileExists)
            {
                WriteToFile(file, csvHeader, false);
            }
        }

        private void WriteToFile(StreamWriter file, string text, bool addLinebreak)
        {
            if (addLinebreak)
            {
                file.Write("\r\n");
            }
            file.Write(text);

            file.Flush();
        }

        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (file != null)
            {
                file.Dispose();
            }
            base.Dispose(disposing);

        }

    }
}
