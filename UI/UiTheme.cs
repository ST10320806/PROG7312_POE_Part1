using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROG7312_POE_Part1.UI
{
    
    public static class UiTheme
    {
        public static readonly Color BrandDark    = Color.FromArgb(0x00, 0x5A, 0x30); 
        public static readonly Color BrandMid     = Color.FromArgb(0x00, 0x7E, 0x3A); 
        public static readonly Color BrandLight   = Color.FromArgb(0xE9, 0xF3, 0xEC); 
        public static readonly Color NeutralBG    = Color.WhiteSmoke;
        public static readonly Color NeutralText  = Color.FromArgb(33, 37, 41);

        public static void ApplyFormDefaults(Form f, Size? minSize = null)
        {
            f.AutoScaleMode = AutoScaleMode.Dpi;        
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Font = new Font("Segoe UI", 10f);
            f.BackColor = NeutralBG;
            if (minSize.HasValue) f.MinimumSize = minSize.Value;
        }

        public static Panel BuildHeader(string title, string subtitle = null)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = BrandDark
            };

            var lblTitle = new Label
            {
                Text = title,
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 18f, FontStyle.Bold),
                Location = new Point(20, 16)
            };

            var lblSubtitle = new Label
            {
                Text = subtitle ?? "Municipal Services • Republic of South Africa",
                AutoSize = true,
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                Location = new Point(22, 46)
            };

       
            var crest = new Label
            {
                Text = "🛡️", 
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20f),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panel.Width - 48, 16)
            };
            crest.Resize += (_, __) =>
            {
                crest.Left = panel.Width - 48;
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblSubtitle);
            panel.Controls.Add(crest);
            panel.Resize += (_, __) =>
            {
                crest.Left = panel.Width - 48;
            };

            return panel;
        }

        public static Panel BuildFooter(string text = "© Municipal Services • IIE PoE")
        {
            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 36,
                BackColor = BrandLight
            };
            var lbl = new Label
            {
                Text = text,
                AutoSize = true,
                ForeColor = NeutralText,
                Location = new Point(12, 9)
            };
            footer.Controls.Add(lbl);
            return footer;
        }

        public static void StylePrimary(Button b)
        {
            b.BackColor = BrandMid;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
        }

        public static void StyleSecondary(Button b)
        {
            b.BackColor = Color.FromArgb(90, 98, 104); // slate-ish
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
        }

        public static void StyleDisabled(Button b)
        {
            b.BackColor = Color.LightGray;
            b.ForeColor = Color.FromArgb(80, 80, 80);
            b.Enabled = false;
            b.FlatStyle = FlatStyle.Flat;
        }

        public static void StyleSectionHeader(Label l)
        {
            l.AutoSize = true;
            l.ForeColor = BrandDark;
            l.Font = new Font("Segoe UI Semibold", 12f);
        }
    }
}
