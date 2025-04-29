using MobileShopee.Db;
using MobileShopee.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShopee
{
    public partial class Adminlogin : Form
    {
        private readonly UserRepository _userRepository;
        public Adminlogin()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new DbConnectionFactory());
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Adminlogin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string role = "admin";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!");
                return;
            }
            try
            {
                bool isValid = _userRepository.ValidateAdmin(username, password, role);
                if (isValid)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    new Admin_HomePage().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng hoặc bạn không phải admin!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
