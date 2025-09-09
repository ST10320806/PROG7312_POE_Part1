using PROG7312_POE_Part1.Domain;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROG7312_POE_Part1.UI
{
    public class ViewIssuesForm : Form
    {
        private DataGridView dgvIssues;
        private Button btnBack;

        public ViewIssuesForm()
        {
            Text = "All Reported Issues";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(820, 500);
            Font = new Font("Segoe UI", 10);

            dgvIssues = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(760, 360),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Define columns
            dgvIssues.Columns.Add("Id", "Reference ID");
            dgvIssues.Columns.Add("Location", "Location");
            dgvIssues.Columns.Add("Category", "Category");
            dgvIssues.Columns.Add("Status", "Status");
            dgvIssues.Columns.Add("CreatedAt", "Date Reported");

            // Fill data from IssueRepository (LinkedList<Issue>)
            foreach (var issue in IssueRepository.GetAll())
            {
                dgvIssues.Rows.Add(
                    issue.Id,
                    issue.Location,
                    issue.Category,
                    issue.Status,
                    issue.CreatedAt.ToString("g")
                );
            }

            btnBack = new Button
            {
                Text = "Back to Main Menu",
                Location = new Point(640, 400),
                Size = new Size(140, 36)
            };
            btnBack.Click += (_, __) => Close();

            Controls.Add(dgvIssues);
            Controls.Add(btnBack);
        }
    }
}
