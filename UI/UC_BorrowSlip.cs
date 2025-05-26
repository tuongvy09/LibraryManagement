using LibraryManagement.Repositories;
using LibraryManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagement.UI
{
    public partial class UC_BorrowSlip : UserControl
    {
        BorrowService borrowService;

        public UC_BorrowSlip()
        {
            InitializeComponent();
            var db = new DBConnection();
            db.conn.Open();
            borrowService = new BorrowService(new BorrowSlipRepository(db), new BorrowSlipDetailRepository(db)
            );

            LoadBorrowSlips();
        }

        private void LoadBorrowSlips()
        {
            dgvBorrowSlips.DataSource = borrowService.GetAllSlips();
        }

        private void dgvBorrowSlips_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int slipId = Convert.ToInt32(dgvBorrowSlips.Rows[e.RowIndex].Cells["SlipID"].Value);
                dgvBorrowDetails.DataSource = borrowService.GetDetailsBySlipId(slipId);
            }
        }
    }
}
