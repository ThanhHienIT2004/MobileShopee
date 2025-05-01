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
        private readonly ModelRepository _modelRepository;
        public Admin_HomePage()
        {
            InitializeComponent();
            _companyRepository = new CompanyRepository(new DbConnectionFactory());
            _modelRepository = new ModelRepository(new DbConnectionFactory());
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

        private void LoadNextModelId()
        {
            try
            {
                string nextId = _modelRepository.GetNextModelId();
                textBox3.Text = nextId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy mã công ty: " + ex.Message);
            }
        }

        private void LoadCompaniesIntoComboBox()
        {
            try
            {
                var companies = _companyRepository.GetCompany();
                if (companies.Count == 0)
                {
                    MessageBox.Show("Không có công ty nào trong cơ sở dữ liệu.");
                }
                comboBox1.DataSource = companies;
                comboBox1.DisplayMember = "CName"; 
                comboBox1.ValueMember = "CompId";

                comboBox4.DataSource = companies;
                comboBox4.DisplayMember = "CName";
                comboBox4.ValueMember = "CompId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách công ty: " + ex.Message);
            }
        }

        private void Admin_HomePage_Load_1(object sender, EventArgs e)
        {
            LoadNextCompanyId();
            LoadNextModelId();
            LoadCompaniesIntoComboBox();
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
                    LoadNextCompanyId();
                    textBox2.Clear();
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

        private void button2_Click(object sender, EventArgs e)
        {
            string modelId = textBox3.Text;
            string companyId = comboBox1.SelectedValue?.ToString();
            string modelNum = textBox4.Text;
            int availableQty = 0;

            if (string.IsNullOrEmpty(modelNum) || string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(modelId)) {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin trước khi thêm model");
                return;
            }

            try
            {
                bool isSuccess = _modelRepository.PostModel(modelId, companyId, modelNum, availableQty);
                if (isSuccess)
                {
                    MessageBox.Show("Thêm model thành công");
                    LoadNextModelId(); // Cập nhật ID mới
                    textBox4.Clear();  // Xóa textbox
                }
                else
                {
                    MessageBox.Show("Thêm model thất bại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            string modelId = comboBox5.SelectedValue?.ToString();
            string imeiNo = textBox8.Text;
            string status = "Không bán";
            DateTime warranty = dateTimePicker4.Value;
            if (!decimal.TryParse(textBox9.Text, out decimal priceValue))
            {
                MessageBox.Show("Giá tiền phải là số hợp lệ.");
                return;
            }
            SqlMoney price = new SqlMoney(priceValue);

            if (string.IsNullOrEmpty(modelId) || string.IsNullOrEmpty(imeiNo))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin trước khi thêm.");
                return;
            }

            try
            {
                bool isSuccess = _mobileRepository.PostMobile(modelId, imeiNo, status, warranty, price);
                if (isSuccess)
                {
                    MessageBox.Show("Thêm mobile thành công.");
                    textBox8.Clear();
                    textBox9.Clear();
                    comboBox4.SelectedIndex = -1;
                    comboBox5.DataSource = null; // Xóa danh sách model
                    dateTimePicker4.Value = DateTime.Now;
                }
                else
                {
                    MessageBox.Show("Thêm mobile thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string transId = textBox5.Text; // Lấy TransId từ textBox5
            string modelId = comboBox3.SelectedValue?.ToString();
            if (!int.TryParse(textBox6.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương hợp lệ.");
                return;
            }
            DateTime date = DateTime.Now;
            if (!decimal.TryParse(textBox7.Text, out decimal amountValue))
            {
                MessageBox.Show("Số tiền phải là số hợp lệ.");
                return;
            }
            SqlMoney amount = new SqlMoney(amountValue);

            if (string.IsNullOrEmpty(transId) || string.IsNullOrEmpty(modelId))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin trước khi thêm giao dịch.");
                return;
            }

            try
            {
                bool isSuccess = _transactionRepository.PostTrans(transId, modelId, quantity, date, amount);
                bool isUpdateModel = _modelRepository.UpdateModelQuantity(modelId, quantity);
                if (isSuccess && isUpdateModel)
                {
                    MessageBox.Show("Thêm giao dịch thành công.");
                    LoadNextTransId(); // Cập nhật TransId mới
                    textBox6.Clear();
                    textBox7.Clear();
                    comboBox2.SelectedIndex = -1;
                    comboBox3.DataSource = null;
                    comboBox3.Items.Clear();
                }
                else
                {
                    MessageBox.Show("Thêm giao dịch thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
