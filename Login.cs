﻿using QLTV1.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTV1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtTen.Text.Trim();
            string password = txtPassWord.Text.Trim();
            if (DBConnection.Login(username, password))
            {
                this.Hide();
                (new Form1()).Show(); 
            }
        }

        private void Login_Load_1(object sender, EventArgs e)
        {

        }
    }
}
