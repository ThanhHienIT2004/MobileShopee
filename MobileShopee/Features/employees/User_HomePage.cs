using MobileShopee.Common;
using MobileShopee.Db;
using MobileShopee.Models;
using MobileShopee.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
            this.StartPosition = FormStartPosition.CenterScreen;
            SetupCustomerDataGridView();
        }

        private void User_HomePage_Load(object sender, EventArgs e)
        {
            LoadCompanies();
        }

        private void LoadCompanies()
        {
            try
            {
                List<Company> companies = _companyRepository.GetCompany();

                comboBox_company.DataSource = new List<Company>(companies);
                comboBox_company.DisplayMember = "CName";
                comboBox_company.ValueMember = "CompId";
                comboBox_company.SelectedIndex = -1;

                comboBox_company_2.DataSource = new List<Company>(companies);
                comboBox_company_2.DisplayMember = "CName";
                comboBox_company_2.ValueMember = "CompId";
                comboBox_company_2.SelectedIndex = -1;
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
                comboBox_model.SelectedIndex = -1;
                comboBox_imei.DataSource = null;
                txtPrice.Clear();
            }
        }

        private void comboBox_company_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_company_2.SelectedValue != null)
            {
                string selectedCompId = comboBox_company_2.SelectedValue.ToString();
                LoadModels_2(selectedCompId);
                comboBox_model_2.SelectedIndex = -1;
                comboBox_imei.DataSource = null;
                txt_quantity.Clear();
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
                comboBox_model.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách model: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadModels_2(string compId)
        {
            try
            {
                List<Model> models = _modelRepository.GetModelsByCompany(compId);
                comboBox_model_2.DataSource = models;
                comboBox_model_2.DisplayMember = "ModelNum";
                comboBox_model_2.ValueMember = "ModelId";
                comboBox_model_2.SelectedIndex = -1;
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
                comboBox_imei.SelectedIndex = -1;
                txtPrice.Clear();
            }
        }

        private void comboBox_model_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_model_2.SelectedValue != null)
            {
                string selectedModelId = comboBox_model_2.SelectedValue.ToString();
                string compId = comboBox_company_2.SelectedValue.ToString();
                decimal quantity = _modelRepository.GetModelQuantity(compId, selectedModelId);
                txt_quantity.Text = quantity.ToString();
            }
        }

        private void LoadImeiNumbers(string modelId)
        {
            List<Mobile> mobiles = _mobileRepository.GetAvailableImeiByModel(modelId);
            comboBox_imei.DataSource = mobiles;
            comboBox_imei.DisplayMember = "IMEINO";
            comboBox_imei.ValueMember = "IMEINO";
            comboBox_imei.SelectedIndex = -1;
        }

        private void comboBox_imei_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_imei.SelectedValue != null)
            {
                try
                {
                    string selectedImei = comboBox_imei.SelectedValue.ToString();
                    decimal price = _mobileRepository.GetPriceByImei(selectedImei);
                    txtPrice.Text = price.ToString("F2");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải giá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
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

            if (!Common.Validator.IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Common.Validator.IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại phải có 10 chữ số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string custId = "C" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                var customer = new Customer
                {
                    CustId = custId,
                    CustName = txtName.Text,
                    MobileNumber = txtPhone.Text,
                    Address = txtAddr.Text,
                    Email = txtEmail.Text
                };

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

                // Get company name from comboBox_company
                string companyName = comboBox_company.Text;
                // Get model number from comboBox_model
                string modelNumber = comboBox_model.Text;

                // Show confirmation form
                using (var confirmationForm = new ConfirmationForm(sale, customer, this, companyName, modelNumber))
                {
                    confirmationForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupCustomerDataGridView()
        {
            dataGridViewCustomer.Columns.Clear();
            dataGridViewCustomer.Columns.Add("Cust_Name", "Customer Name");
            dataGridViewCustomer.Columns.Add("MobileNumber", "Mobile Number");
            dataGridViewCustomer.Columns.Add("Email", "Email");
            dataGridViewCustomer.Columns.Add("Address", "Address");
            dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCustomer.BorderStyle = BorderStyle.FixedSingle;
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            string imei = txtSearchIMEI.Text.Trim();
            if (!string.IsNullOrEmpty(imei))
            {
                try
                {
                    DataTable dt = _saleRepository.GetCustomerByIMEI(imei);
                    dataGridViewCustomer.Rows.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            dataGridViewCustomer.Rows.Add(
                                row["Cust_Name"],
                                row["MobileNumber"],
                                row["Email"],
                                row["Address"]
                            );
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập IMEI để tìm kiếm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void FinalizeSale(string imei, string custId)
        {
            try
            {
                _mobileRepository.UpdateMobileStatus(imei, "Sold");
                string selectedModelId = comboBox_model.SelectedValue.ToString();
                _modelRepository.UpdateModelQuantity(selectedModelId);
                MessageBox.Show("Ghi nhận bán hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hoàn tất giao dịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}