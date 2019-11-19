using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Client
{
    class Login
    {
        private string taikhoan, ip,password;
        public SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-RPK6PAD;Initial Catalog=NguoiDung2;Integrated Security=True");
        public Login()
        {
            taikhoan = ip = password = "";
        }
        public Login(string tk, string ipserver,string pw)
        {
            taikhoan = tk;
            ip = ipserver;
            password = pw;

        }
        public int kiemTraDangNhap(string tk, string ipserver,string pw)
        {
            try
            {
                conn.Open();
                string sql = "select *from NguoiDung1 where TaiKhoan='" + taikhoan + "' and Ipserver= '" + ip + "' and password= '"+ password + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader data = cmd.ExecuteReader();
                if (data.Read() == true)
                {
                    return 1;
                }
         
            }
            catch
            {
                return 0;
            }
            return 0;
        }
        public void themSql(string tk, string ipserver, string pass)
        {
            try
            {
                conn.Open();
                string sqlINSERT = "INSERT INTO NguoiDung1 VALUES (@TaiKhoan,@Ipserver,@password)";
                SqlCommand cmd = new SqlCommand(sqlINSERT, conn);
                cmd.Parameters.AddWithValue("TaiKhoan", tk);
                cmd.Parameters.AddWithValue("Ipserver", ipserver);
                cmd.Parameters.AddWithValue("password", pass);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Đăng ký thành công");
                
            }
            catch
            {
                MessageBox.Show("Đăng ký không thành công");
            }

            
        }
    }
}
