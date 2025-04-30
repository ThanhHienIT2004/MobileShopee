using System;
using System.Windows.Forms;
using MobileShopee.Db;
using MobileShopee.Repository;

namespace MobileShopee
{
    public partial class Form1 : Form
    {
        private readonly UserRepository _userRepository;

        public Form1()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new DbConnectionFactory());
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            new Adminlogin().Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = txt_uname.Text;
            string password = txt_upass.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!");
                return;
            }

            var (success, role, message) = _userRepository.Login(username, password);
            if (success)
            {
                switch (role?.Trim())
                {
                    case "User":
                        new User_HomePage().Show();
                        break;
                    case "Admin":
                        new Admin_HomePage().Show();
                        break;
                    default:
                        MessageBox.Show($"Vai trò không được hỗ trợ: {role}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Chức năng quên mật khẩu đang được phát triển!");
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }

}