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

namespace MobileShopee.Features.auth
{
    public partial class Forget : Form
    {
        private readonly UserRepository _userRepository;
        public Forget()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new DbConnectionFactory());
        }

        private void Forget_Load(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text.Trim();
            string hint = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(hint))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin để lấy lại mật khẩu");
                return;
            }

            string result = _userRepository.CheckUser(userName, hint);

            label3.Visible = true;

            if (result.StartsWith("Lỗi") || result.StartsWith("Không") || result == "Gợi ý không đúng")
            {
                label3.Text = result; // hiển thị lỗi trực tiếp lên label3
            }
            else
            {
                label3.Text = "Your Password is: " + result;
            }
        }

    }
}
