using System;
using System.Windows.Forms;
using MobileShopee.Db;
using MobileShopee.Repository;

namespace MobileShopee
{
    public partial class AuthScreen : Form
    {
        private readonly UserRepository _userRepository;

        public AuthScreen()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new DbConnectionFactory());
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Chức năng quên mật khẩu đang được phát triển!");
        }

        private void btn_login_Click(object sender, EventArgs e)
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
                var form = new Form();
                switch (role?.Trim())
                {
                    case "User":
                        form = new User_HomePage();
                        break;
                    case "Admin":
                        form = new Admin_HomePage();
                        break;
                    default:
                        MessageBox.Show($"Vai trò không được hỗ trợ: {role}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
                form.FormClosed += (s, args) => this.Close();
                form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(message);
            }
        }

    }

}