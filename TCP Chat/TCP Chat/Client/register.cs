using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Client
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnDangky_Click(object sender, EventArgs e)
        {
            if(txttendangnhap.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập vào tên đăng nhập");
                txttendangnhap.Focus();
            }
            else if(txtipserver.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập IP Server");
            }
            else if(txtpassword.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập Password");
            }
            else if(txtreturnpass.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập xác nhận mật khẩu");
            }
            else if(txtpassword.Text != txtreturnpass.Text)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu chưa đúng");
                txtreturnpass.Focus();
                txtreturnpass.SelectAll();
            }
            Login nd1 = new Login(txttendangnhap.Text, txtipserver.Text, txtpassword.Text);
            nd1.themSql(txttendangnhap.Text, txtipserver.Text, txtpassword.Text);
            this.Close();
        }
    }
}
