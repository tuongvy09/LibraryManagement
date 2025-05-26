using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using LibraryManagement.UserControls;
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
            // contentPanel (main content)
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
            this.Text = "Hệ thống Quản lý Thư viện";
            this.WindowState = FormWindowState.Maximized;

            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
        }

        private void InitializeMenuButtons()
        {
            // Cập nhật menu items để bao gồm chức năng của AT
            string[] menuItems = {
                //"Trang chủ",
                //"--- QUẢN LÝ NHÂN SỰ ---",
                "Quản lý Thủ thư",
                //"--- QUẢN LÝ ĐỘC GIẢ ---",
                "Quản lý Độc giả",
                "Quản lý Thẻ thư viện",
                //"--- QUẢN LÝ SÁCH ---",
                "Sách",
                //"--- QUẢN LỚ MƯỢN TRẢ ---",
                "Mượn sách",
                "Phiếu phạt",
                "Biên lai",
                //"--- THỐNG KÊ - BÁO CÁO ---",
                "Thống kê Độc giả",
                //"--- QUẢN TRỊ HỆ THỐNG ---",
                "Người dùng"
            };

            Dictionary<string, Image> menuIcons = new Dictionary<string, Image>()
            {
                //{ "Trang chủ", Properties.Resources.home ?? CreateDefaultIcon() },
                { "Quản lý Thủ thư", Properties.Resources.librarian ?? CreateDefaultIcon() },
                { "Quản lý Độc giả", Properties.Resources.readers ?? CreateDefaultIcon() },
                { "Quản lý Thẻ thư viện", Properties.Resources.library_card ?? CreateDefaultIcon() },
                { "Sách", Properties.Resources.books ?? CreateDefaultIcon() },
                { "Mượn sách", Properties.Resources.book__1_ ?? CreateDefaultIcon() },
                { "Phiếu phạt", Properties.Resources.voucher ?? CreateDefaultIcon() },
                { "Biên lai", Properties.Resources.reciept ?? CreateDefaultIcon() },
                { "Thống kê Độc giả", Properties.Resources.statistics ?? CreateDefaultIcon() },
                { "Người dùng", Properties.Resources.teamwork ?? CreateDefaultIcon() }
            };

            foreach (var item in menuItems)
            {
                // Tạo separator cho các mục bắt đầu bằng "---"
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
                //case "Trang chủ":
                //    newContent = new DashboardControl();
                //    break;

                case "Quản lý Thủ thư":
                    newForm = new FormThuThuManagement();
                    break;

                case "Quản lý Độc giả":
                    newForm = new FormDocGiaManagement();
                    break;

                //case "Quản lý Thẻ thư viện":
                //    newForm = new FormTheThuVienManagement();
                //    break;

                case "Sách":
                    newContent = new BookControl();
                    break;

                case "Thống kê Độc giả":
                    newForm = new FormThongKe();
                    break;

                //case "Người dùng":
                //    newForm = new FormUserManagement();
                //    break;

                    // Thêm các case khác cho phần của Ngân và TVy
            }

            if (newContent != null)
            {
                newContent.Dock = DockStyle.Fill;
                contentPanel.Controls.Add(newContent);
            }
            else if (newForm != null)
            {
                // Hiển thị form trong panel hoặc mở form mới
                ShowFormInPanel(newForm);
            }
        }

        private void ShowFormInPanel(Form form)
        {
            // Option 1: Embed form trong panel
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(form);
            form.Show();

            // Option 2: Mở form riêng biệt (comment dòng trên và uncomment dòng dưới)
            // form.ShowDialog();
        }

        // Tạo icon mặc định nếu không có icon
        private Image CreateDefaultIcon()
        {
            Bitmap bmp = new Bitmap(24, 24);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Gray, 0, 0, 24, 24);
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