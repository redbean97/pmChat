using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class PublicChatForm : Form
    {
        private readonly ListenerUser listenerUser;
        public List<Socket> clients = new List<Socket>();

        public PublicChatForm()
        {
            pChat = new PrivateChatForm(this);
            InitializeComponent();
            listenerUser = new ListenerUser(2014);
            listenerUser.SocketAccepted += listenerUser_SocketAccepted;
        }
        private void listenerUser_SocketAccepted(Socket e)
        {
            var client = new ClientUser(e);
            client.Received += client_Received;
 
            this.Invoke(() =>
            {
                string ip = client.Ip.ToString().Split(':')[0];
                var item = new ListViewItem(ip); // ip
                item.SubItems.Add(" "); // nickname
                item.SubItems.Add(" "); // status
                item.Tag = client;
                userList.Items.Add(item);
                clients.Add(e);
            });
        }
        public void BroadcastData(string data) // send to all clients
        {
            foreach (var socket in clients)
            {
                try { socket.Send(Encoding.ASCII.GetBytes(data)); }
                catch (Exception) { }
            }
        }
        private void client_Received(ClientUser sender, byte[] data)
        {
            this.Invoke(() =>
            {
                for (int i = 0; i < userList.Items.Count; i++)
                {
                    var client =userList.Items[i].Tag as ClientUser;
                    if (client == null || client.Ip != sender.Ip) continue;
                    var command = Encoding.ASCII.GetString(data).Split('|');
                    switch (command[0])
                    {
                        case "Connect":
                            txtReceive.Text += "<< " + command[1] + " joined the room >>\r\n";
                            userList.Items[i].SubItems[1].Text = command[1]; // nickname
                            userList.Items[i].SubItems[2].Text = command[2]; // status
                            string users = string.Empty;
                            for (int j = 0; j < userList.Items.Count; j++)
                            {
                                users += userList.Items[j].SubItems[1].Text + "|";
                            }
                            BroadcastData("Users|" + users.TrimEnd('|'));
                            BroadcastData("RefreshChat|" + txtReceive.Text);
                            break;
                        case "Message":
                            txtReceive.Text += command[1] + " says: " + command[2] + "\r\n";
                            BroadcastData("RefreshChat|" + txtReceive.Text);
                            break;
                        case "pMessage":
                            this.Invoke(() =>
                            {
                                pChat.txtReceive.Text += command[1] + " says: " + command[2] + "\r\n";
                            });
                            break;
                        case "pChat":

                            break;
                    }
                }
            });
        }
        public readonly LoginForm formLogin = new LoginForm();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            formLogin.Client.Received += _client_Received;
            formLogin.Client.Disconnected += Client_Disconnected;
            formLogin.ShowDialog();
            Text = "TCP Chat - " + formLogin.txtIP.Text + " - (Connected as: " + formLogin.txtNickname.Text + ")";
            
        }

        private static void Client_Disconnected(ClientSettings cs)
        {
            
        }

        private readonly PrivateChatForm pChat;

        public void _client_Received(ClientSettings cs, string received)
        {
            var cmd = received.Split('|');
            switch (cmd[0])
            {
                case "Users":
                    this.Invoke(() =>
                    {
                        userList.Items.Clear();
                        for (int i = 1; i < cmd.Length; i++)
                        {
                            if (cmd[i] != "Connected" | cmd[i] != "RefreshChat")
                            {
                                userList.Items.Add(cmd[i]);
                            }
                        }
                    });
                    break;
                case "Message":
                    this.Invoke(() =>
                    {
                        txtReceive.Text += cmd[1] + "\r\n";
                    });
                    break;
                case "RefreshChat":
                    this.Invoke(() =>
                    {
                        txtReceive.Text = cmd[1];
                    });
                    break;
                case "Chat":
                    this.Invoke(() =>
                    {
                        pChat.Text = pChat.Text.Replace("user", formLogin.txtNickname.Text);
                        pChat.Show();
                    });
                    break;
                case "pMessage":
                    this.Invoke(() =>
                    {
                        pChat.txtReceive.Text += "Server says: " + cmd[1] + "\r\n";
                    });
                    break;
                case "Disconnect":
                    Application.Exit();
                    break;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != string.Empty)
            {
                formLogin.Client.Send("Message|" + formLogin.txtNickname.Text + "|" + txtInput.Text);
                txtReceive.Text += formLogin.txtNickname.Text + " says: " + txtInput.Text + "\r\n";
                txtInput.Text = string.Empty;
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.PerformClick();
            }
        }

        private void txtReceive_TextChanged(object sender, EventArgs e)
        {
            txtReceive.SelectionStart = txtReceive.TextLength;
        }

        private void privateChat_Click(object sender, EventArgs e)
        {
            formLogin.Client.Send("pChat|" + formLogin.txtNickname.Text);
        }
    }
}