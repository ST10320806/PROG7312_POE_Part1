using PROG7312_POE_Part1.Domain;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PROG7312_POE_Part1.UI
{
    public class ReportIssueForm : Form
    {
        private Label lblHeader;

        private Label lblLocation;
        private TextBox txtLocation;

        private Label lblCategory;
        private ComboBox cmbCategory;

        private Label lblDescription;
        private RichTextBox rtbDescription;

        private Label lblAttachments;
        private ListBox lstAttachments;
        private Button btnAttach;

        private ProgressBar pbEngagement;
        private Label lblEncouragement;

        private Button btnSubmit;
        private Button btnBack;

        private OpenFileDialog openFileDialog;

        public ReportIssueForm()
        {
            Text = "Report an Issue";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(720, 560);
            Font = new Font("Segoe UI", 10);

            // Background colour
            BackColor = Color.WhiteSmoke;

            // Header
            lblHeader = new Label
            {
                Text = "Report an Issue",
                Font = new Font("Segoe UI Semibold", 18),
                AutoSize = true,
                Location = new Point(20, 18),
                ForeColor = Color.DarkGreen
            };

            // Location
            lblLocation = new Label
            {
                Text = "Location:",
                AutoSize = true,
                Location = new Point(24, 70)
            };
            txtLocation = new TextBox
            {
                Location = new Point(120, 66),
                Width = 540,
                
            };
            txtLocation.TextChanged += (_, __) => UpdateEngagement();

            // Category
            lblCategory = new Label
            {
                Text = "Category:",
                AutoSize = true,
                Location = new Point(24, 110)
            };
            cmbCategory = new ComboBox
            {
                Location = new Point(120, 106),
                Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.Items.AddRange(new object[]
            {
                "Sanitation",
                "Roads",
                "Water",
                "Electricity",
                "Stormwater",
                "Parks & Recreation",
                "Other"
            });
            cmbCategory.SelectedIndexChanged += (_, __) => UpdateEngagement();

            // Description
            lblDescription = new Label
            {
                Text = "Description:",
                AutoSize = true,
                Location = new Point(24, 152)
            };
            rtbDescription = new RichTextBox
            {
                Location = new Point(120, 148),
                Size = new Size(540, 140)
            };
            rtbDescription.TextChanged += (_, __) => UpdateEngagement();

            // Attachments
            lblAttachments = new Label
            {
                Text = "Attachments:",
                AutoSize = true,
                Location = new Point(24, 300)
            };
            lstAttachments = new ListBox
            {
                Location = new Point(120, 300),
                Size = new Size(420, 90)
            };
            btnAttach = new Button
            {
                Text = "Attach Files…",
                Location = new Point(550, 300),
                Size = new Size(110, 32),
                BackColor = Color.SlateGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAttach.Click += BtnAttach_Click;

            openFileDialog = new OpenFileDialog
            {
                Title = "Select images or documents",
                Filter = "Images and Documents|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pdf;*.docx;*.xlsx;*.pptx;*.txt|All files|*.*",
                Multiselect = true
            };

            // Engagement feature
            pbEngagement = new ProgressBar
            {
                Location = new Point(24, 410),
                Size = new Size(636, 18),
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };
            lblEncouragement = new Label
            {
                Text = "Let’s make your report as clear as possible.",
                AutoSize = true,
                ForeColor = Color.SteelBlue,
                Location = new Point(24, 432)
            };

            // Buttons
            btnSubmit = new Button
            {
                Text = "Submit",
                Location = new Point(480, 460),
                Size = new Size(90, 36),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSubmit.Click += BtnSubmit_Click;

            btnBack = new Button
            {
                Text = "Back to Main Menu",
                Location = new Point(580, 460),
                Size = new Size(140, 36),
                BackColor = Color.SlateGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += (_, __) => Close();

            // Add controls
            Controls.AddRange(new Control[]
            {
                lblHeader,
                lblLocation, txtLocation,
                lblCategory, cmbCategory,
                lblDescription, rtbDescription,
                lblAttachments, lstAttachments, btnAttach,
                pbEngagement, lblEncouragement,
                btnSubmit, btnBack
            });
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
            // Basic validation
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
                $"Thank you! Your issue has been submitted.\n\nReference: {issue.Id}\nStatus: {issue.Status}\nSubmitted: {issue.CreatedAt}",
                "Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // Reset form
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
