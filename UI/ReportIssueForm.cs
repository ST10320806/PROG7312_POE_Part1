using PROG7312_POE_Part1.Domain;
using PROG7312_POE_Part1.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PROG7312_POE_Part1.UI
{
    public class ReportIssueForm : Form
    {
        private TextBox txtLocation;
        private ComboBox cmbCategory;
        private RichTextBox rtbDescription;
        private ListBox lstAttachments;
        private Button btnAttach;
        private ProgressBar pbEngagement;
        private Label lblEncouragement;
        private Button btnSubmit;
        private Button btnBack;
        private OpenFileDialog openFileDialog;
        private ToolTip tips;

        public ReportIssueForm()
        {
            UiTheme.ApplyFormDefaults(this, new Size(800, 600));
            Text = "Report an Issue";

            Controls.Add(UiTheme.BuildHeader("Report an Issue"));
            Controls.Add(UiTheme.BuildFooter());

            tips = new ToolTip();

            // 2-column grid for labels/inputs
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                ColumnCount = 2,
                RowCount = 8
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140)); // labels
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));  // inputs

            var lblLocation = new Label { Text = "Location:", AutoSize = true };
            UiTheme.StyleSectionHeader(lblLocation);
            txtLocation = new TextBox { Dock = DockStyle.Fill };
            tips.SetToolTip(txtLocation, "Example: 123 Main Rd, Ward 4, Cape Town");

            var lblCategory = new Label { Text = "Category:", AutoSize = true };
            UiTheme.StyleSectionHeader(lblCategory);
            cmbCategory = new ComboBox { Dock = DockStyle.Left, Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategory.Items.AddRange(new object[]
            {
                "Sanitation", "Roads", "Water", "Electricity", "Stormwater", "Parks & Recreation", "Other"
            });

            var lblDescription = new Label { Text = "Description:", AutoSize = true };
            UiTheme.StyleSectionHeader(lblDescription);
            rtbDescription = new RichTextBox { Dock = DockStyle.Fill, Height = 140 };

            var lblAttachments = new Label { Text = "Attachments:", AutoSize = true };
            UiTheme.StyleSectionHeader(lblAttachments);

            // attachments area with a right-aligned button
            lstAttachments = new ListBox { Dock = DockStyle.Fill, Height = 90 };
            btnAttach = new Button { Text = "Attach Files…", Width = 130, Height = 32 };
            UiTheme.StyleSecondary(btnAttach);
            btnAttach.Click += BtnAttach_Click;

            var attachRow = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill };
            attachRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            attachRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            attachRow.Controls.Add(lstAttachments, 0, 0);
            attachRow.Controls.Add(btnAttach, 1, 0);

            // engagement
            pbEngagement = new ProgressBar { Dock = DockStyle.Fill, Minimum = 0, Maximum = 100, Value = 0 };
            lblEncouragement = new Label { Text = "Let’s make your report as clear as possible.", AutoSize = true, ForeColor = Color.SteelBlue };

            // buttons bar
            btnSubmit = new Button { Text = "Submit", Width = 100, Height = 36 };
            UiTheme.StylePrimary(btnSubmit);
            btnSubmit.Click += BtnSubmit_Click;

            btnBack = new Button { Text = "Back to Main Menu", Width = 150, Height = 36 };
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
            bottomBar.Controls.Add(btnSubmit);

            // add rows to grid
            grid.Controls.Add(lblLocation, 0, 0);
            grid.Controls.Add(txtLocation, 1, 0);

            grid.Controls.Add(lblCategory, 0, 1);
            grid.Controls.Add(cmbCategory, 1, 1);

            grid.Controls.Add(lblDescription, 0, 2);
            grid.Controls.Add(rtbDescription, 1, 2);

            grid.Controls.Add(lblAttachments, 0, 3);
            grid.Controls.Add(attachRow, 1, 3);

            grid.Controls.Add(new Label { Text = "Completion:", AutoSize = true }, 0, 4);
            grid.Controls.Add(pbEngagement, 1, 4);

            grid.Controls.Add(new Label { Text = "", AutoSize = true }, 0, 5);
            grid.Controls.Add(lblEncouragement, 1, 5);

            // Dock order: header (Top), footer (Bottom), bottomBar (Bottom), grid (Fill)
            Controls.Add(bottomBar);
            Controls.Add(grid);
            grid.BringToFront();

            openFileDialog = new OpenFileDialog
            {
                Title = "Select images or documents",
                Filter = "Images and Documents|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pdf;*.docx;*.xlsx;*.pptx;*.txt|All files|*.*",
                Multiselect = true
            };

            // engagement updates
            txtLocation.TextChanged += (_, __) => UpdateEngagement();
            cmbCategory.SelectedIndexChanged += (_, __) => UpdateEngagement();
            rtbDescription.TextChanged += (_, __) => UpdateEngagement();
        }

        private void BtnAttach_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var f in openFileDialog.FileNames)
                {
                    if (!lstAttachments.Items.Contains(f))
                        lstAttachments.Items.Add(f);
                }
                UpdateEngagement();
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter the location.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocation.Focus();
                return;
            }
            if (cmbCategory.SelectedItem is null)
            {
                MessageBox.Show("Please select a category.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.DroppedDown = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(rtbDescription.Text) || rtbDescription.Text.Trim().Length < 10)
            {
                MessageBox.Show("Please provide a brief description (at least 10 characters).", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescription.Focus();
                return;
            }

            var issue = new Issue
            {
                Location = txtLocation.Text.Trim(),
                Category = cmbCategory.SelectedItem.ToString(),
                Description = rtbDescription.Text.Trim(),
            };
            issue.Attachments.AddRange(lstAttachments.Items.Cast<string>());

            IssueRepository.Add(issue);

            MessageBox.Show(
                $"Thank you! Your issue has been submitted.\n\nReference: {issue.Id}\nStatus: {issue.Status}\nSubmitted: {issue.CreatedAt:g}",
                "Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // reset
            txtLocation.Clear();
            cmbCategory.SelectedIndex = -1;
            rtbDescription.Clear();
            lstAttachments.Items.Clear();
            UpdateEngagement();
        }

        private void UpdateEngagement()
        {
            int score = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text)) score += 30;
            if (cmbCategory.SelectedItem != null) score += 30;

            var descLen = rtbDescription.Text.Trim().Length;
            if (descLen >= 10 && descLen < 50) score += 20;
            else if (descLen >= 50) score += 30;

            if (lstAttachments.Items.Count > 0) score += 10;

            if (score > 100) score = 100;
            pbEngagement.Value = score;

            if (score == 0)
                lblEncouragement.Text = "Let’s make your report as clear as possible.";
            else if (score < 40)
                lblEncouragement.Text = "Great start! Add a category to help route it.";
            else if (score < 70)
                lblEncouragement.Text = "Looking good — a few more details will help.";
            else if (score < 100)
                lblEncouragement.Text = "Almost there! Attach a photo if you have one.";
            else
                lblEncouragement.Text = "Perfect — ready to submit!";
        }
    }
}
