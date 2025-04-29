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
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!");
                return;
            }

            try
            {
                bool isValid = _userRepository.ValidateUser(username, password);
                if (isValid)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    new ConformDetails().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Chức năng quên mật khẩu đang được phát triển!");
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Adminlogin adminlogin = new Adminlogin();
            adminlogin.Show();
            this.Hide();

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }

}