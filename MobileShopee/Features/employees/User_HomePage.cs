using MobileShopee.Db;
using MobileShopee.Repository;
using System;
using System.Data;
using System.Windows.Forms;

namespace MobileShopee
{
    public partial class User_HomePage : Form
    {
        private readonly UserRepository _userRepository;

        public User_HomePage()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new DbConnectionFactory());
            this.StartPosition = FormStartPosition.CenterScreen; // Căn giữa Form
        }

        private void User_HomePage_Load(object sender, EventArgs e)
        {
            LoadCompanies(); // Tải danh sách công ty khi Form được tải
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void LoadCompanies()
        {
            try
            {
                DataTable companyTable = _userRepository.GetCompanies();
                comboBox_company.DataSource = companyTable;
                comboBox_company.DisplayMember = "CName";
                comboBox_company.ValueMember = "CompId";
                comboBox_company.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách công ty: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_company_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_company.SelectedValue != null)
            {
                string selectedCompId = comboBox_company.SelectedValue.ToString();
                LoadModels(selectedCompId);
                comboBox_model.SelectedIndex = -1; // Xóa lựa chọn model
                comboBox_imei.DataSource = null; // Xóa danh sách IMEI
                txtPrice.Clear(); // Xóa giá
            }
        }

        private void LoadModels(string compId)
        {
            try
            {
                DataTable modelTable = _userRepository.GetModelsByCompany(compId);
                comboBox_model.DataSource = modelTable;
                comboBox_model.DisplayMember = "ModelNum";
                comboBox_model.ValueMember = "ModelId";
                comboBox_model.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách model: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_model_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_model.SelectedValue != null)
            {
                string selectedModelId = comboBox_model.SelectedValue.ToString();
                LoadImeiNumbers(selectedModelId);
                comboBox_imei.SelectedIndex = -1; // Xóa lựa chọn IMEI
                txtPrice.Clear(); // Xóa giá
            }
        }

        private void LoadImeiNumbers(string modelId)
        {
            try
            {
                DataTable imeiTable = _userRepository.GetAvailableImeiByModel(modelId);
                comboBox_imei.DataSource = imeiTable;
                comboBox_imei.DisplayMember = "IMEINO";
                comboBox_imei.ValueMember = "IMEINO";
                comboBox_imei.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách IMEI: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_imei_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_imei.SelectedValue != null)
            {
                try
                {
                    string selectedImei = comboBox_imei.SelectedValue.ToString();
                    decimal price = _userRepository.GetPriceByImei(selectedImei);
                    txtPrice.Text = price.ToString("F2"); // Hiển thị giá với 2 chữ số thập phân
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải giá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAddr.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                comboBox_imei.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng email
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng số điện thoại (10 chữ số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại phải có 10 chữ số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Thêm khách hàng
                string custId = "C" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                _userRepository.AddCustomer(custId, txtName.Text, txtPhone.Text, txtAddr.Text, txtEmail.Text);

                // Thêm bán hàng
                string salesId = "SALE" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                string selectedImei = comboBox_imei.SelectedValue.ToString();
                decimal price = decimal.Parse(txtPrice.Text);
                _userRepository.AddSale(salesId, selectedImei, DateTime.Now, price, custId);

                // Cập nhật trạng thái IMEI
                _userRepository.UpdateMobileStatus(selectedImei, "Sold");

                // Cập nhật số lượng tồn kho
                string selectedModelId = comboBox_model.SelectedValue.ToString();
                _userRepository.UpdateModelQuantity(selectedModelId);

                MessageBox.Show("Ghi nhận bán hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}