using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReStore_Kitchen_Pricing_Application
{
    public partial class kitchenForm : Form
    {
        public kitchenForm()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void addCabinetButton_Click(object sender, EventArgs e)
        {
            
            AddCabinetForm addCabForm = new AddCabinetForm(this);
            this.Enabled = false;
            addCabForm.Show();
            
        }

       
    }
}
