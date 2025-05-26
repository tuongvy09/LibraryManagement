using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.UI;

namespace LibraryManagement.UserControls
{
    public partial class ThuThuUC : UserControl
    {
        private ThuThuManagement thuThuManagement;

        public ThuThuUC()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 
            // ThuThuUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ThuThuUC";
            this.Size = new System.Drawing.Size(1000, 600);
            this.Load += new System.EventHandler(this.ThuThuUC_Load);
            this.ResumeLayout(false);
        }

        private void ThuThuUC_Load(object sender, EventArgs e)
        {
            // Tạo và nhúng ThuThuManagement UserControl
            thuThuManagement = new ThuThuManagement();
            thuThuManagement.Dock = DockStyle.Fill;

            this.Controls.Add(thuThuManagement);
        }

        // Public methods để tương tác với ThuThuManagement từ bên ngoài
        public void RefreshData()
        {
            thuThuManagement?.RefreshData();
        }

        public void SearchData(string searchText)
        {
            thuThuManagement?.SearchData(searchText);
        }
    }
}
