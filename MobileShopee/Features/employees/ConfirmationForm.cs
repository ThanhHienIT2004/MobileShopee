using MobileShopee.Models;
using System;
using System.Windows.Forms;

namespace MobileShopee
{
    public partial class ConfirmationForm : Form
    {
        private readonly Sale _sale;
        private readonly Customer _customer;
        private readonly User_HomePage _parentForm;
        private readonly string _companyName;
        private readonly string _modelNumber;

        public ConfirmationForm(Sale sale, Customer customer, User_HomePage parentForm, string companyName, string modelNumber)
        {
            InitializeComponent();
            _sale = sale;
            _customer = customer;
            _parentForm = parentForm;
            _companyName = companyName;
            _modelNumber = modelNumber;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Confirm Details";
            DisplayDetails();
        }

        private void DisplayDetails()
        {
            lblCustomerNameValue.Text = _customer.CustName;
            lblCompanyNameValue.Text = _companyName;
            lblMobileNumberValue.Text = _customer.MobileNumber;
            lblAddressValue.Text = _customer.Address;
            lblEmailValue.Text = _customer.Email;
            lblModelNumberValue.Text = _modelNumber;
            lblIMEIValue.Text = _sale.IMEINO;
            lblPriceValue.Text = _sale.Price.ToString("C2");
            lblWarrantyValue.Text = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // Insert Customer and Sale into the database via the parent form
                _parentForm.ConfirmAndSave(_sale, _customer);
                MessageBox.Show("Giao dịch đã được xác nhận thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear all fields in User_HomePage
                _parentForm.ClearFormFields();

                // Close the ConfirmationForm
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xác nhận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfirmationForm_Load(object sender, EventArgs e)
        {

        }
    }
}