using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Server
{
    public partial class quanlytk : Form
    {
        SqlConnection connection1;
        SqlCommand cmd1;
        string str = @"Data Source=DESKTOP-RPK6PAD;Initial Catalog=NguoiDung2;Integrated Security=True";

        SqlDataAdapter adapter1 = new SqlDataAdapter();
        DataTable table1 = new DataTable();
        void loaddata()
        {
            cmd1 = connection1.CreateCommand();
            cmd1.CommandText = "select *from NguoiDung1 ";
            adapter1.SelectCommand = cmd1;
            table1.Clear();
            adapter1.Fill(table1);
            dgv.DataSource = table1;
        }

        public quanlytk()
        {
            InitializeComponent();
        }

        

        private void quanlytk_Load(object sender, EventArgs e)
        {
            connection1 = new SqlConnection(str);
            connection1.Open();
            loaddata();

        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            tbnametk.ReadOnly = true;
            int i;
            i = dgv.CurrentRow.Index;
            tbnametk.Text = dgv.Rows[i].Cells[0].Value.ToString();
            tbIP.Text = dgv.Rows[i].Cells[1].Value.ToString();
            tbPass.Text = dgv.Rows[i].Cells[2].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmd1 = connection1.CreateCommand();
            cmd1.CommandText = "insert into NguoiDung1 values ('"+tbnametk.Text+"','"+tbIP.Text+"','"+tbPass.Text+ "')";
            cmd1.ExecuteNonQuery();
            loaddata();
        }

        private void btndlt_Click(object sender, EventArgs e)
        {
            cmd1 = connection1.CreateCommand();
            cmd1.CommandText = "delete from NguoiDung1 where TaiKhoan= '" + tbnametk.Text + "'";
            cmd1.ExecuteNonQuery();
            loaddata();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            cmd1 = connection1.CreateCommand();
            cmd1.CommandText = "update NguoiDung1 set Ipserver= '" + tbIP.Text + "',password= '" + tbPass.Text + "' where TaiKhoan= '" + tbnametk.Text+"'";
            cmd1.ExecuteNonQuery();
            loaddata();

        }

        private void btnKhoitao_Click(object sender, EventArgs e)
        {
            tbnametk.ReadOnly = false;
            tbnametk.Text = "";
            tbIP.Text = "";
            tbPass.Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            cmd1 = connection1.CreateCommand();
            cmd1.CommandText = "select *from NguoiDung1 where TaiKhoan= @TaiKhoan ";
            cmd1.Parameters.AddWithValue("TaiKhoan", tbtkcantim.Text);
            cmd1.Parameters.AddWithValue("Ipserver", tbIP.Text);
            cmd1.Parameters.AddWithValue("password", tbPass.Text);
            loaddata();
        }
    }
}
