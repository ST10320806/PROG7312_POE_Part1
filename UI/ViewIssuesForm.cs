using PROG7312_POE_Part1.Domain;
using PROG7312_POE_Part1.UI;
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
            UiTheme.ApplyFormDefaults(this, new Size(900, 560));
            Text = "All Reported Issues";

            Controls.Add(UiTheme.BuildHeader("All Reported Issues"));
            Controls.Add(UiTheme.BuildFooter());

            dgvIssues = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            dgvIssues.Columns.Add("Id", "Reference ID");
            dgvIssues.Columns.Add("Location", "Location");
            dgvIssues.Columns.Add("Category", "Category");
            dgvIssues.Columns.Add("Status", "Status");
            dgvIssues.Columns.Add("CreatedAt", "Date Reported");

            ReloadGrid();

            dgvIssues.CellDoubleClick += DgvIssues_CellDoubleClick;

            btnBack = new Button { Text = "Back", Width = 100, Height = 36 };
            UiTheme.StyleSecondary(btnBack);
            btnBack.Click += (_, __) => Close();

            var bottomBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(8),
                AutoSize = true
            };
            bottomBar.Controls.Add(btnBack);

            Controls.Add(dgvIssues);
            Controls.Add(bottomBar);
            dgvIssues.BringToFront();

            // optional auto-refresh when returning focus
            this.Activated += (_, __) => ReloadGrid();
        }

        private void ReloadGrid()
        {
            dgvIssues.Rows.Clear();
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
        }

        private void DgvIssues_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var idObj = dgvIssues.Rows[e.RowIndex].Cells["Id"].Value;
            if (idObj == null) return;

            if (Guid.TryParse(idObj.ToString(), out var issueId))
            {
                var issue = IssueRepository.FindById(issueId);
                if (issue != null)
                {
                    using (var details = new IssueDetailsForm(issue))
                    {
                        details.ShowDialog(this);
                    }
                }
                else
                {
                    MessageBox.Show("Issue could not be found.", "Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
