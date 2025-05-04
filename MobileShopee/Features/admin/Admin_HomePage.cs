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
        private readonly TransactionRepository _transactionRepository;
        private readonly SaleRepository _saleRepository;
        public Admin_HomePage()
        {
            InitializeComponent();
            _companyRepository = new CompanyRepository(new DbConnectionFactory());
            _modelRepository = new ModelRepository(new DbConnectionFactory());
            _mobileRepository = new MobileRepository(new DbConnectionFactory());
            _transactionRepository = new TransactionRepository(new DbConnectionFactory());
            _saleRepository = new SaleRepository(new DbConnectionFactory());
        }

        private void Admin_HomePage_Load_1(object sender, EventArgs e)
        {
            LoadNextCompanyId();
            LoadNextModelId();
            LoadNextTransId();
            LoadCompaniesIntoComboBox();
            LoadSaleReportsByDate();
            LoadSaleReportsByDtD();

            comboBox4.SelectedIndexChanged += (s, ev) =>
            {
                string selectedCompId = comboBox4.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(selectedCompId))
                {
                    LoadModelsByCompany(selectedCompId);
                }
                else
                {
                    comboBox5.DataSource = null;
                    comboBox5.Items.Clear();
                }
            };

            comboBox2.SelectedIndexChanged += (s, ev) =>
            {
                string selectedCompId = comboBox2.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(selectedCompId))
                {
                    LoadModelsByCompanyForTrans(selectedCompId);
                }
                else
                {
                    comboBox3.DataSource = null;
                    comboBox3.Items.Clear();
                }
            };

            if (comboBox4.SelectedValue != null)
            {
                LoadModelsByCompany(comboBox4.SelectedValue.ToString());
            }
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

        private void LoadNextTransId()
        {
            try
            {
                string nextId = _transactionRepository.GetNextTransId();
                textBox5.Text = nextId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy mã giao dịch: " + ex.Message);
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

                comboBox2.DataSource = companies.ToList();
                comboBox2.DisplayMember = "CName";
                comboBox2.ValueMember = "CompId";
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

        private void LoadModelsByCompanyForTrans(string compId)
        {
            try
            {
                comboBox3.DataSource = null;
                comboBox3.Items.Clear();

                if (string.IsNullOrEmpty(compId))
                {
                    return;
                }

                var models = _modelRepository.GetModelsByCompany(compId);
                if (models.Count == 0)
                {
                    MessageBox.Show("Không có model nào cho công ty này.");
                }

                comboBox3.DataSource = models;
                comboBox3.DisplayMember = "ModelNum";
                comboBox3.ValueMember = "ModelId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách model: " + ex.Message);
            }
        }

        private void LoadSaleReportsByDate()
        {
            try
            {
                DateTime selectedDate = dateSalesDay.Value.Date;
                var (success, reports, message) = _saleRepository.GetSaleReportsbyDate(selectedDate);

                if (success)
                {
                    dataGridViewSales.DataSource = reports;
                    dataGridViewSales.AutoGenerateColumns = false;
                    dataGridViewSales.Columns.Clear();
                    dataGridViewSales.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colSaleId",
                        DataPropertyName = "SaleId",
                        HeaderText = "Mã bán hàng"
                    });
                    dataGridViewSales.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colCompanyName",
                        DataPropertyName = "CompanyName",
                        HeaderText = "Hãng"
                    });
                    dataGridViewSales.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colModelNumber",
                        DataPropertyName = "ModelNumber",
                        HeaderText = "Mẫu điện thoại"
                    });
                    dataGridViewSales.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colIMEI",
                        DataPropertyName = "IMEINO",
                        HeaderText = "IMEI"
                    });
                    dataGridViewSales.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colPrice",
                        DataPropertyName = "Price",
                        HeaderText = "Giá bán"
                    });

                    txtTotalPriceSalesDay.Text = reports
                        .Where(r => !string.IsNullOrEmpty(r.Price))
                        .Sum(r => decimal.Parse(r.Price)).ToString();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSaleReportsByDtD()
        {
            try
            {
                DateTime startDate = dateStartDtD.Value.Date;
                DateTime endDate = dateEndDtD.Value.Date;
                var (success, reports, message) = _saleRepository.GetSaleReportsbyDtD(startDate, endDate);

                if (success)
                {
                    dataGridViewSalesDtD.DataSource = reports;
                    dataGridViewSalesDtD.AutoGenerateColumns = false;
                    dataGridViewSalesDtD.Columns.Clear();
                    dataGridViewSalesDtD.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colSaleId",
                        DataPropertyName = "SaleId",
                        HeaderText = "Mã bán hàng"
                    });
                    dataGridViewSalesDtD.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colCompanyName",
                        DataPropertyName = "CompanyName",
                        HeaderText = "Hãng"
                    });
                    dataGridViewSalesDtD.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colModelNumber",
                        DataPropertyName = "ModelNumber",
                        HeaderText = "Mẫu điện thoại"
                    });
                    dataGridViewSalesDtD.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colIMEI",
                        DataPropertyName = "IMEINO",
                        HeaderText = "IMEI"
                    });
                    dataGridViewSalesDtD.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "colPrice",
                        DataPropertyName = "Price",
                        HeaderText = "Giá bán"
                    });

                    txtTotalPriceSalesDtD.Text = reports
                        .Where(r => !string.IsNullOrEmpty(r.Price))
                        .Sum(r => decimal.Parse(r.Price)).ToString();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void btnAdd_Click(object sender, EventArgs e)
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
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string CompanyName = comboBox4.SelectedValue?.ToString();
            string ModelNum = textBox4.Text;
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
                if (isSuccess)
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
        private void btnSearchSalesDay_Click(object sender, EventArgs e)
        {
            LoadSaleReportsByDate();
        }

        private void btnSearchDtD_Click(object sender, EventArgs e)
        {
            LoadSaleReportsByDtD();
        }
    }
}
