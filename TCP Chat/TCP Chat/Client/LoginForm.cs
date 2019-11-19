using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Client
{
    public partial class LoginForm : Form
    {
        public ClientSettings Client { get; set; }
        
        public LoginForm()
        {
            Client = new ClientSettings();
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Login nd = new Login(txtNickname.Text, txtIP.Text,txtpass.Text);
            //SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-RPK6PAD;Initial Catalog=NguoiDung;Integrated Security=True");
            try
            {
                //conn.Open();
                string tk = txtNickname.Text.Trim();
                string ip = txtIP.Text.Trim();
                string pw = txtpass.Text.Trim();
                //string sql = "select *from NguoiDung where TaiKhoan='"+tk+"' and Ip= '"+ip+"'" ;
                //SqlCommand cmd = new SqlCommand(sql, conn);
                //SqlDataReader dta = cmd.ExecuteReader();
                //if(dta.Read() == true)
                if(nd.kiemTraDangNhap(tk,ip,pw) == 1)
                {
                    MessageBox.Show("Login sucessed", "Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    Client.Connected += Client_Connected;
                    Client.Connect(txtIP.Text, 2014);
                    Client.Send("Connect|" + txtNickname.Text + "|connected");
                }
                else
                {
                    MessageBox.Show("Error","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Client_Connected(object sender, EventArgs e)
        {
            this.Invoke(Close);
        }

        private void LoginForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
          // Application.Exit();
        }

        private void btnexit_Click_1(object sender, EventArgs e)
        {
            DialogResult tb = MessageBox.Show("Bạn có thoát hay không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (tb == DialogResult.OK)
                Application.Exit();
        }
        public  register formregister = new register();
        private void btnregister_Click(object sender, EventArgs e)
        {
            formregister.ShowDialog();
        }
    }
}