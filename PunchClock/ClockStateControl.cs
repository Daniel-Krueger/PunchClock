using System;
using System.Drawing;
using System.Windows.Forms;

namespace PunchClock
{
    public class ClockStateControl : IDisposable
    {
        public Color BackgroundColor { get; set; }
        public Brush ForegroundColor { get; set; }

        public string IconText { get; set; }
        public string NotifyLabel { get; set; }

        public NotifyIcon Icon { get; set; }

        public ClockState ClockState { get; set; }
        public void Dispose()
        {
            Icon.Dispose();
        }
    }

    public enum ClockState
    {
        ClockedIn=0,
        ClockedOut=1
    }    
}
