using MobileShopee.Db;
using MobileShopee.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
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
        private readonly MobileRepository _mobileRepository;
        public Admin_HomePage()
        {
            InitializeComponent();
            _companyRepository = new CompanyRepository(new DbConnectionFactory());
            _modelRepository = new ModelRepository(new DbConnectionFactory());
            _mobileRepository = new MobileRepository(new DbConnectionFactory());
        }

        private void LoadNextCompanyId()
        {
            try
            {
                string nextId = _companyRepository.GetNextCompanyId();
                textBox1.Text = nextId;
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

                comboBox4.DataSource = companies.ToList(); // Sao chép để tránh ảnh hưởng comboBox1
                comboBox4.DisplayMember = "CName";
                comboBox4.ValueMember = "CompId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách công ty: " + ex.Message);
            }
        }

        private void LoadModelsByCompany(string compId)
        {
            try
            {
                // Xóa DataSource cũ để đảm bảo làm mới
                comboBox5.DataSource = null;
                comboBox5.Items.Clear();

                if (string.IsNullOrEmpty(compId))
                {
                    return; // Không load model nếu compId rỗng
                }

                var models = _modelRepository.GetModelsByCompany(compId);
                if (models.Count == 0)
                {
                    MessageBox.Show("Không có model nào cho công ty này.");
                }

                comboBox5.DataSource = models;
                comboBox5.DisplayMember = "ModelNum";
                comboBox5.ValueMember = "ModelId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách model: " + ex.Message);
            }
        }

        private void Admin_HomePage_Load_1(object sender, EventArgs e)
        {
            LoadNextCompanyId();
            LoadNextModelId();
            LoadCompaniesIntoComboBox();

            // Gán sự kiện SelectedIndexChanged cho comboBox4
            comboBox4.SelectedIndexChanged += (s, ev) =>
            {
                string selectedCompId = comboBox4.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(selectedCompId))
                {
                    LoadModelsByCompany(selectedCompId);
                }
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string compid = textBox1.Text;
            string cname = textBox2.Text;

            if (string.IsNullOrEmpty(compid) || string.IsNullOrEmpty(cname))
            {
                MessageBox.Show("Vui lòng nhập tên công ty");
                return;
            }
            try
            {
                bool isSuccess = _companyRepository.PostCompany(compid, cname);
                if (isSuccess)
                {
                    MessageBox.Show("Thành công");
                    LoadNextCompanyId();
                    textBox2.Clear();
                    LoadCompaniesIntoComboBox(); // Cập nhật lại danh sách công ty
                }
                else
                {
                    MessageBox.Show("Thất bại");
                }
            }
            catch (Exception ex)
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

            if (string.IsNullOrEmpty(modelNum) || string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(modelId))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin trước khi thêm model");
                return;
            }

            try
            {
                bool isSuccess = _modelRepository.PostModel(modelId, companyId, modelNum, availableQty);
                if (isSuccess)
                {
                    MessageBox.Show("Thêm model thành công");
                    LoadNextModelId();
                    textBox4.Clear();
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

        private void btnInsert_Click(object sender, EventArgs e)
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
    }

}

