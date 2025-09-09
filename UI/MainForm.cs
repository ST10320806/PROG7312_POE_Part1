using System;
using System.Drawing;
using System.Windows.Forms;
using PROG7312_POE_Part1.UI;

namespace PROG7312_POE_Part1.UI
{
    public class MainForm : Form
    {
        private Label lblTitle;
        private Label lblNote;

        private Button btnReportIssues;
        private Button btnViewAllIssues;   // <- Added
        private Button btnEvents;
        private Button btnStatus;

        public MainForm()
        {
            Text = "Municipal Services - Main Menu";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(560, 360);
            Font = new Font("Segoe UI", 10);

            // Background colour
            BackColor = Color.WhiteSmoke;

            lblTitle = new Label
            {
                Text = "Welcome to Municipal Services",
                Font = new Font("Segoe UI Semibold", 18),
                AutoSize = true,
                Location = new Point(20, 20),
                ForeColor = Color.DarkGreen
            };

            btnReportIssues = new Button
            {
                Text = "Report Issues",
                Location = new Point(24, 80),
                Size = new Size(440, 44),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnReportIssues.Click += (s, e) =>
            {
                using (var f = new ReportIssueForm())
                {
                    Hide();
                    f.ShowDialog();
                    Show();
                }
            };

            // NEW: View All Reported Issues
            btnViewAllIssues = new Button
            {
                Text = "View All Reported Issues",
                Location = new Point(24, 134),
                Size = new Size(440, 38),
                BackColor = Color.SlateGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnViewAllIssues.Click += (s, e) =>
            {
                using (var f = new ViewIssuesForm())
                {
                    Hide();
                    f.ShowDialog();
                    Show();
                }
            };

            btnEvents = new Button
            {
                Text = "Local Events & Announcements (Coming Soon)",
                Location = new Point(24, 178),
                Size = new Size(440, 38),
                Enabled = false,
                BackColor = Color.LightGray
            };

            btnStatus = new Button
            {
                Text = "Service Request Status (Coming Soon)",
                Location = new Point(24, 222),
                Size = new Size(440, 38),
                Enabled = false,
                BackColor = Color.LightGray
            };

            lblNote = new Label
            {
                Text = "Only “Report Issues” and “View All Reported Issues” are available in this version.",
                AutoSize = true,
                ForeColor = Color.DimGray,
                Location = new Point(24, 274)
            };

            Controls.Add(lblTitle);
            Controls.Add(btnReportIssues);
            Controls.Add(btnViewAllIssues);   // <- Ensure it’s added to Controls
            Controls.Add(btnEvents);
            Controls.Add(btnStatus);
            Controls.Add(lblNote);
        }
    }
}
