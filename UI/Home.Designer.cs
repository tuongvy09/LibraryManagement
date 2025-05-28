using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using LibraryManagement.UserControls;
using System.Linq;
using LibraryManagement.UI;

namespace LibraryManagement
{
    partial class Home
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.FlowLayoutPanel menuPanel;
        private System.Windows.Forms.Panel contentPanel;
        private string currentUserRole;

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
            this.menuPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.sidebarPanel.SuspendLayout();
            this.menuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // sidebarPanel
            // 
            this.sidebarPanel.Controls.Add(this.menuPanel);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Location = new System.Drawing.Point(0, 0);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new System.Drawing.Size(330, 800);
            this.sidebarPanel.TabIndex = 1;
            this.sidebarPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.sidebarPanel_Paint);
            // 
            // menuPanel
            // 
            this.menuPanel.AutoScroll = true;
            this.menuPanel.BackColor = System.Drawing.Color.Transparent;
            this.menuPanel.Controls.Add(this.logoPictureBox);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.menuPanel.Location = new System.Drawing.Point(0, 0);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Padding = new System.Windows.Forms.Padding(10);
            this.menuPanel.Size = new System.Drawing.Size(330, 800);
            this.menuPanel.TabIndex = 0;
            this.menuPanel.WrapContents = false;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = global::LibraryManagement.Properties.Resources.home_logo__2_;
            this.logoPictureBox.Location = new System.Drawing.Point(35, 30);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(25, 20, 25, 10);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(220, 120);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(330, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(1551, 800);
            this.contentPanel.TabIndex = 0;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1881, 800);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Name = "Home";
            this.Text = "Hệ thống Quản lý Thư viện";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.sidebarPanel.ResumeLayout(false);
            this.menuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        // Method overload - có thể gọi với hoặc không có role
        private void InitializeMenuButtons()
        {
            InitializeMenuButtons("user"); // Default role
        }

        private void InitializeMenuButtons(string role)
        {
            currentUserRole = role;

            List<string> menuItems = new List<string>();

            menuItems.Add("Quản lý Thủ thư");

            menuItems.Add("Quản lý Độc giả");
            menuItems.Add("Quản lý Thẻ thư viện");

            // Các mục menu chính
            menuItems.AddRange(new[] { "Sách", "Độc giả", "Mượn sách", "Phiếu Mượn", "Phiếu phạt", "Biên lai" });

            menuItems.Add("Thống kê Độc giả");

            if (role == "admin")
            {
                menuItems = menuItems.Concat(new[] { "Người dùng", "Lịch sử thao tác" }).ToArray().ToList();
            }

            Dictionary<string, Image> menuIcons = new Dictionary<string, Image>()
    {
        { "Quản lý Thủ thư", Properties.Resources.librarian ?? CreateDefaultIcon() },
        { "Quản lý Độc giả", Properties.Resources.readers ?? CreateDefaultIcon() },
        { "Quản lý Thẻ thư viện", Properties.Resources.library_card ?? CreateDefaultIcon() },
        { "Thống kê Độc giả", Properties.Resources.statistics ?? CreateDefaultIcon() },

        { "Sách", Properties.Resources.books ?? CreateDefaultIcon() },
        { "Độc giả", Properties.Resources.readers ?? CreateDefaultIcon() },
        { "Mượn sách", Properties.Resources.book__1_ ?? CreateDefaultIcon() },

        { "Phiếu Mượn", Properties.Resources.reciept ?? CreateDefaultIcon() },

        { "Phiếu phạt", Properties.Resources.voucher ?? CreateDefaultIcon() },
        { "Biên lai", Properties.Resources.reciept ?? CreateDefaultIcon() },
        { "Người dùng", Properties.Resources.teamwork ?? CreateDefaultIcon() },
        { "Lịch sử thao tác", Properties.Resources.history_book ?? CreateDefaultIcon() }
    };

            foreach (var item in menuItems)
            {
                if (item.StartsWith("---"))
                {
                    Label separator = new Label()
                    {
                        Text = item.Replace("---", "").Trim(),
                        Width = 300,
                        Height = 25,
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = Color.Transparent,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(10, 10, 10, 2)
                    };
                    this.menuPanel.Controls.Add(separator);
                    continue;
                }

                RoundedButton btn = new RoundedButton()
                {
                    Text = item,
                    Width = 280,
                    Height = 45,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = ColorTranslator.FromHtml("#5c7d3d"),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(10, 3, 10, 3),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(50, 0, 10, 0),
                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextImageRelation = TextImageRelation.ImageBeforeText
                };

                if (menuIcons.ContainsKey(item))
                {
                    btn.Image = new Bitmap(menuIcons[item], new Size(24, 24));
                    btn.Padding = new Padding(10, 0, 10, 0);
                }

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
            Form newForm = null;

            switch (item)
            {
                case "Quản lý Thủ thư":
                    newForm = new FormThuThuManagement();
                    break;

                case "Quản lý Độc giả":
                    newForm = new FormDocGiaManagement();
                    break;

                case "Quản lý Thẻ thư viện":
                    newContent = new TheThuVienManagement();
                    newContent.Dock = DockStyle.Fill;
                    break;

                case "Thống kê Độc giả":
                    newForm = new FormThongKe();
                    break;

                case "Sách":
                    newContent = new SachControl();
                    newContent.Dock = DockStyle.Fill;
                    break;

                case "Độc giả":
                    newContent = new NguoiDungControl();
                    newContent.Dock = DockStyle.Fill;
                    break;

                case "Mượn sách":
                    break;

                case "Phiếu phạt":
                    break;

                case "Biên lai":
                    newContent = new BienLaiManagement();
                    newContent.Dock = DockStyle.Fill;
                    break;

                case "Người dùng":
                    newContent = new NguoiDungControl();
                    newContent.Dock = DockStyle.Fill;
                    break;

                case "Phiếu Mượn":
                    newContent = new PhieuMuonManagement(); 
                    break;

                case "Lịch sử thao tác":
                    break;

                default:
                    Label lblNotImplemented = new Label()
                    {
                        Text = $"Chức năng '{item}' đang được phát triển...",
                        Font = new Font("Segoe UI", 14F),
                        ForeColor = Color.Gray,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };
                    contentPanel.Controls.Add(lblNotImplemented);
                    return;
            }

            if (newContent != null)
            {
                newContent.Dock = DockStyle.Fill;
                contentPanel.Controls.Add(newContent);
            }
            else if (newForm != null)
            {
                // Hiển thị form trong panel
                ShowFormInPanel(newForm);
            }
        }

        private void ShowFormInPanel(Form form)
        {
            try
            {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.WindowState = FormWindowState.Normal;
                contentPanel.Controls.Add(form);
                form.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form: {ex.Message}", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image CreateDefaultIcon()
        {
            Bitmap bmp = new Bitmap(24, 24);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Gray, 0, 0, 24, 24);
                g.DrawRectangle(Pens.White, 1, 1, 22, 22);
            }
            return bmp;
        }

        // Vẽ nền bên trái
        private void sidebarPanel_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Resources.bg_home_left != null)
            {
                e.Graphics.DrawImage(Properties.Resources.bg_home_left,
                    new Rectangle(0, 0, sidebarPanel.Width, sidebarPanel.Height));
            }
            else
            {
                // Vẽ gradient nếu không có background image
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    sidebarPanel.ClientRectangle,
                    ColorTranslator.FromHtml("#3a5a2a"),
                    ColorTranslator.FromHtml("#5c7d3d"),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, sidebarPanel.ClientRectangle);
                }
            }
        }

        // Phương thức public để set role từ bên ngoài
        public void SetUserRole(string role)
        {
            currentUserRole = role;
            menuPanel.Controls.Clear();
            menuPanel.Controls.Add(logoPictureBox);
            InitializeMenuButtons(role);
        }

        // Property để lấy current role
        public string CurrentUserRole
        {
            get { return currentUserRole; }
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
