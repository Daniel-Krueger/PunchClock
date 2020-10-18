# Punch Clock
Yet another windows application for time tracking.

Features:
- You clock in/out by clicking on an icon in the windows 10 notifications area
- The icon is green if you clocked in and red if you have clocked out
- The current time and state 'clocked in/out' is written to a csv file
- Before a new 'clocked in' time is written a line break will be added. This ensures, that each clock in is on a separte line, even if you forgott to clock out.
- Format of the csv entries, time format and labels are configurable in the config file.
- A new csv file will be created per month
- While the program runs, the csv file is locked.

If the notification icon is not displayed, after you started the program, you have to click on the error to show all hidden icons. You can drag and drop the icon from there to the visible icon area.
