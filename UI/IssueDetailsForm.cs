using PROG7312_POE_Part1.Domain;
using PROG7312_POE_Part1.UI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PROG7312_POE_Part1.UI
{
    public class IssueDetailsForm : Form
    {
        private readonly Issue _issue;

        private TextBox txtId, txtLocation, txtCategory, txtStatus, txtCreated;
        private RichTextBox rtbDescription;
        private ListBox lstAttachments;
        private Button btnOpenAttachment, btnClose;

        //*********************************************************************************************************************
        public IssueDetailsForm(Issue issue)
        {
            _issue = issue ?? throw new ArgumentNullException(nameof(issue));

            UiTheme.ApplyFormDefaults(this, new Size(900, 640));
            Text = "Issue Details";

            Controls.Add(UiTheme.BuildHeader("Issue Details"));
            Controls.Add(UiTheme.BuildFooter());

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                ColumnCount = 2,
                RowCount = 8
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            //Adding rows to show the details of the report
            AddReadOnlyRow(grid, "Reference ID:", out txtId, 0);
            AddReadOnlyRow(grid, "Location:", out txtLocation, 1);
            AddReadOnlyRow(grid, "Category:", out txtCategory, 2);
            AddReadOnlyRow(grid, "Status:", out txtStatus, 3);
            AddReadOnlyRow(grid, "Date Reported:", out txtCreated, 4);

            var lblDesc = new Label { Text = "Description:", AutoSize = true };
            UiTheme.StyleSectionHeader(lblDesc);
            rtbDescription = new RichTextBox { Dock = DockStyle.Fill, Height = 160, ReadOnly = true };
            grid.Controls.Add(lblDesc, 0, 5);
            grid.Controls.Add(rtbDescription, 1, 5);

            var lblAtt = new Label { Text = "Attachments:", AutoSize = true };
            UiTheme.StyleSectionHeader(lblAtt);
            lstAttachments = new ListBox { Dock = DockStyle.Fill, Height = 140 };
            btnOpenAttachment = new Button { Text = "Open Selected", Width = 130, Height = 32 };
            UiTheme.StyleSecondary(btnOpenAttachment);
            btnOpenAttachment.Click += (_, __) => OpenAttachment();

            var attachRow = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill };
            attachRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            attachRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            attachRow.Controls.Add(lstAttachments, 0, 0);
            attachRow.Controls.Add(btnOpenAttachment, 1, 0);

            grid.Controls.Add(lblAtt, 0, 6);
            grid.Controls.Add(attachRow, 1, 6);

            btnClose = new Button { Text = "Close", Width = 110, Height = 36 };
            UiTheme.StyleSecondary(btnClose);
            btnClose.Click += (_, __) => Close();

            var bottomBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(8),
                AutoSize = true
            };
            bottomBar.Controls.Add(btnClose);

            Controls.Add(bottomBar);
            Controls.Add(grid);
            grid.BringToFront();

            GetIssue();
        }

        private void AddReadOnlyRow(TableLayoutPanel grid, string label, out TextBox textBox, int rowIndex)
        {
            var lbl = new Label { Text = label, AutoSize = true };
            UiTheme.StyleSectionHeader(lbl);
            textBox = new TextBox { Dock = DockStyle.Fill, ReadOnly = true };
            grid.Controls.Add(lbl, 0, rowIndex);
            grid.Controls.Add(textBox, 1, rowIndex);
        }

        private void GetIssue()
        {
            txtId.Text = _issue.Id.ToString();
            txtLocation.Text = _issue.Location;
            txtCategory.Text = _issue.Category;
            txtStatus.Text = _issue.Status.ToString();
            txtCreated.Text = _issue.CreatedAt.ToString("g");
            rtbDescription.Text = _issue.Description;

            lstAttachments.Items.Clear();
            foreach (var path in _issue.Attachments ?? Enumerable.Empty<string>())
                lstAttachments.Items.Add(path);
        }

        //Code for viewing an attachment
        private void OpenAttachment()
        {
            if (lstAttachments.SelectedItem == null)
            {
                MessageBox.Show("Please select an attachment to open.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var path = lstAttachments.SelectedItem.ToString();
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("The file no longer exists at this path:\n" + path,
                        "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the file.\n\n" + ex.Message,
                    "Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
//******************************************************************End Of File******************************************************************
