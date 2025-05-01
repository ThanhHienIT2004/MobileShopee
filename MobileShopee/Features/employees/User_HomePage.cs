using MobileShopee.Common;
using MobileShopee.Db;
using MobileShopee.Models;
using MobileShopee.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace MobileShopee
{
    public partial class User_HomePage : Form
    {
        private readonly CompanyRepository _companyRepository;
        private readonly ModelRepository _modelRepository;
        private readonly MobileRepository _mobileRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly SaleRepository _saleRepository;

        public User_HomePage()
        {
            InitializeComponent();
            var connectionFactory = new DbConnectionFactory();
            _companyRepository = new CompanyRepository(connectionFactory);
            _modelRepository = new ModelRepository(connectionFactory);
            _mobileRepository = new MobileRepository(connectionFactory);
            _customerRepository = new CustomerRepository(connectionFactory);
            _saleRepository = new SaleRepository(connectionFactory);
            this.StartPosition = FormStartPosition.CenterScreen; // Căn giữa Form
        }

        private void User_HomePage_Load(object sender, EventArgs e)
        {
            LoadCompanies(); // Tải danh sách công ty khi Form được tải
        }

        private void LoadCompanies()
        {
            try
            {
                List<Company> companies = _companyRepository.GetCompany();
                comboBox_company.DataSource = companies;
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
                List<Model> models = _modelRepository.GetModelsByCompany(compId);
                comboBox_model.DataSource = models;
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
                List<Mobile> mobiles = _mobileRepository.GetAvailableImeiByModel(modelId);
                comboBox_imei.DataSource = mobiles;
                comboBox_imei.DisplayMember = "IMEINO";
                comboBox_imei.ValueMember = "IMEINO";
                comboBox_imei.SelectedIndex = -1; // Không chọn mặc định
 
        }

        private void comboBox_imei_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_imei.SelectedValue != null)
            {
                try
                {
                    string selectedImei = comboBox_imei.SelectedValue.ToString();
                    decimal price = _mobileRepository.GetPriceByImei(selectedImei);
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
            if (!Common.Validator.IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng số điện thoại
            if (!Common.Validator.IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại phải có 10 chữ số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Thêm khách hàng
                string custId = "C" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                var customer = new Customer
                {
                    CustId = custId,
                    CustName = txtName.Text,
                    MobileNumber = txtPhone.Text,
                    Address = txtAddr.Text,
                    Email = txtEmail.Text
                };
                _customerRepository.AddCustomer(customer);

                // Thêm bán hàng
                string salesId = "SALE" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                string selectedImei = comboBox_imei.SelectedValue.ToString();
                decimal price = decimal.Parse(txtPrice.Text);
                var sale = new Sale
                {
                    SId = salesId,
                    IMEINO = selectedImei,
                    PurchaseDate = DateTime.Now,
                    Price = price,
                    CustId = custId
                };
                _saleRepository.AddSale(sale);

                // Cập nhật trạng thái IMEI
                _mobileRepository.UpdateMobileStatus(selectedImei, "Sold");

                // Cập nhật số lượng tồn kho
                string selectedModelId = comboBox_model.SelectedValue.ToString();
                _modelRepository.UpdateModelQuantity(selectedModelId);

                MessageBox.Show("Ghi nhận bán hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}