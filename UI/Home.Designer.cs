using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using LibraryManagement.UserControls;

namespace LibraryManagement
{
    partial class Home
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.FlowLayoutPanel menuPanel;
        private System.Windows.Forms.Panel contentPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.menuPanel = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();

            // 
            // pictureBox1 (main content)
            // 
            this.contentPanel = new System.Windows.Forms.Panel();
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.BackColor = Color.White;

            // 
            // sidebarPanel
            // 
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Width = 330;
            this.sidebarPanel.Paint += new PaintEventHandler(this.sidebarPanel_Paint);
            this.sidebarPanel.Controls.Add(this.menuPanel);

            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Size = new System.Drawing.Size(220, 120);
            this.logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.logoPictureBox.Image = Properties.Resources.home_logo__2_;
            this.logoPictureBox.Margin = new Padding(25, 20, 25, 10);

            // 
            // menuPanel
            // 
            this.menuPanel.Dock = DockStyle.Fill;
            this.menuPanel.FlowDirection = FlowDirection.TopDown;
            this.menuPanel.WrapContents = false;
            this.menuPanel.Padding = new Padding(10, 10, 10, 10);
            this.menuPanel.AutoScroll = true;
            this.menuPanel.BackColor = Color.Transparent;
            this.menuPanel.Controls.Add(this.logoPictureBox);

            // 
            // Home Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 800);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Name = "Home";
            this.Text = "Home";

            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
        }
        private void InitializeMenuButtons()
        {
            string[] menuItems = { "Sách", "Độc giả", "Mượn sách", "Phiếu phạt", "Biên lai", "Người dùng" };
            Dictionary<string, Image> menuIcons = new Dictionary<string, Image>()
    {
        { "Sách", Properties.Resources.books },
        { "Độc giả", Properties.Resources.readers },
        { "Mượn sách", Properties.Resources.book__1_ },
        { "Phiếu phạt", Properties.Resources.voucher },
        { "Biên lai", Properties.Resources.reciept },
        { "Người dùng", Properties.Resources.teamwork }
    };

            foreach (var item in menuItems)
            {
                RoundedButton btn = new RoundedButton()
                {
                    Text = item,
                    Width = 210,
                    Height = 45,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = ColorTranslator.FromHtml("#5c7d3d"),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(10, 5, 10, 5),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 0, 0),
                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextImageRelation = TextImageRelation.ImageBeforeText
                };

                if (menuIcons.ContainsKey(item))
                    btn.Image = new Bitmap(menuIcons[item], new Size(24, 24));

                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#8cbf5f");

                btn.MouseEnter += (s, e) => { btn.BackColor = ColorTranslator.FromHtml("#7eb957"); };
                btn.MouseLeave += (s, e) => { btn.BackColor = ColorTranslator.FromHtml("#5c7d3d"); };

                btn.Click += (s, e) => { ShowContent(item); };

                this.menuPanel.Controls.Add(btn);
            }
        }
        private void ShowContent(string item)
        {
            contentPanel.Controls.Clear(); // Xóa nội dung cũ

            UserControl newContent = null;

            switch (item)
            {
                case "Sách":
                    newContent = new BookControl();
                    newContent.Dock = DockStyle.Fill;
                    break;

                    // Thêm các mục khác tương ứng
            }

            if (newContent != null)
            {
                newContent.Dock = DockStyle.Fill;
                contentPanel.Controls.Add(newContent);
            }
        }

        // Vẽ nền bên trái
        private void sidebarPanel_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Resources.bg_home_left != null)
            {
                e.Graphics.DrawImage(Properties.Resources.bg_home_left,
                    new Rectangle(0, 0, sidebarPanel.Width, sidebarPanel.Height));
            }
        }

    }
    public class RoundedButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, 20, 20, 180, 90);
            path.AddArc(bounds.Right - 20, bounds.Y, 20, 20, 270, 90);
            path.AddArc(bounds.Right - 20, bounds.Bottom - 20, 20, 20, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - 20, 20, 20, 90, 90);
            path.CloseAllFigures();

            this.Region = new Region(path);

            base.OnPaint(pevent);
        }
    }

}
