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
    public partial class Admin_HomePage : Form
    {
        private readonly CompanyRepository _companyRepository;
        public Admin_HomePage()
        {
            InitializeComponent();
            _companyRepository = new CompanyRepository(new DbConnectionFactory());
        }

        // Hàm này sẽ lấy ID tiếp theo và điền vào textBox1
        private void LoadNextCompanyId()
        {
            try
            {
                // Gọi phương thức GetNextCompanyId để lấy ID tiếp theo
                string nextId = _companyRepository.GetNextCompanyId();
                textBox1.Text = nextId; // Điền vào textbox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy mã công ty: " + ex.Message);
            }
        }

        private void Admin_HomePage_Load_1(object sender, EventArgs e)
        {
            LoadNextCompanyId();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string compid = textBox1.Text;
            string cname = textBox2.Text;

            if (String.IsNullOrEmpty(compid) || String.IsNullOrEmpty(cname))
            {
                MessageBox.Show("Vui lòng nhập tên công ty");
                return;
            }
            try
            {
                bool isSuccess = _companyRepository.PostCompany(compid,cname);
                if (isSuccess) {
                    MessageBox.Show("Thành công");
                    return;
                }
                else
                {
                    MessageBox.Show("Thất bại");
                }
            }
            catch(Exception ex) 
            {
        
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
