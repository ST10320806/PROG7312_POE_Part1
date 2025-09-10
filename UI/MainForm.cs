using System;
using System.Drawing;
using System.Windows.Forms;
using PROG7312_POE_Part1.UI;

namespace PROG7312_POE_Part1.UI
{
    public class MainForm : Form
    {
        private Button btnReportIssues;
        private Button btnViewAllIssues;
        private Button btnEvents;
        private Button btnStatus;

        public MainForm()
        {
            UiTheme.ApplyFormDefaults(this, new Size(640, 420));
            Text = "Municipal Services - Main Menu";

            Controls.Add(UiTheme.BuildHeader("Municipal Services", "Welcome"));
            Controls.Add(UiTheme.BuildFooter());

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                ColumnCount = 1,
                RowCount = 6
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 8));      
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));         
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));         
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));         
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));         
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));     

            var lblIntro = new Label
            {
                Text = "Please select a task to continue:",
                AutoSize = true,
                ForeColor = UiTheme.NeutralText
            };

            btnReportIssues = new Button { Text = "Report Issues", Height = 44, Dock = DockStyle.Top };
            UiTheme.StylePrimary(btnReportIssues);
            btnReportIssues.Click += (s, e) =>
            {
                using (var f = new ReportIssueForm()) { Hide(); f.ShowDialog(); Show(); }
            };

            btnViewAllIssues = new Button { Text = "View All Reported Issues", Height = 40, Dock = DockStyle.Top };
            UiTheme.StyleSecondary(btnViewAllIssues);
            btnViewAllIssues.Click += (s, e) =>
            {
                using (var f = new ViewIssuesForm()) { Hide(); f.ShowDialog(); Show(); }
            };

            btnEvents = new Button { Text = "Local Events & Announcements (Coming Soon)", Height = 40, Dock = DockStyle.Top };
            UiTheme.StyleDisabled(btnEvents);

            btnStatus = new Button { Text = "Service Request Status (Coming Soon)", Height = 40, Dock = DockStyle.Top };
            UiTheme.StyleDisabled(btnStatus);

            layout.Controls.Add(new Panel { Height = 1, Dock = DockStyle.Top }, 0, 0);
            layout.Controls.Add(lblIntro, 0, 1);
            layout.Controls.Add(btnReportIssues, 0, 2);
            layout.Controls.Add(btnViewAllIssues, 0, 3);
            layout.Controls.Add(btnEvents, 0, 4);
            layout.Controls.Add(btnStatus, 0, 5);

            Controls.Add(layout);
            layout.BringToFront();
        }
    }
}
//******************************************************************End Of File******************************************************************
