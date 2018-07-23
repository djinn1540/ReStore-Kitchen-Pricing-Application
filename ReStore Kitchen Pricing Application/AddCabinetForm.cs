using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReStore_Kitchen_Pricing_Application
{
    public partial class AddCabinetForm : Form
    {
        kitchenForm parentForm;

        public AddCabinetForm(kitchenForm parentForm) //parentForm is the form that "birthed" the addCabinetForm, specifically it is the kitchenForm
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddCabinetForm_Load(object sender, EventArgs e)
        {

        }

        private void finishAddingCabinetButton_Click(object sender, EventArgs e)
        {
            //verify proper input
            if (!verifyCabinetInput())
            {
                //TODO add popup box 
                return;
            }

            //process the cabinet info into DataRow
            String dimensions = widthListBox.Text + heightTextBox.Text + depthTextBox.Text;
            String type = getCabinetType();


            //add DataRow to the DataTable in parentForm
            DataRow cabinet = parentForm.CabinetDataGrid.Rows.Add(qtyTextBox.Text, dimensions, ,,);

            //change back to the KitchenForm
            this.Close();
        }

        private void AddCabinetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Enabled = true;
            parentForm.BringToFront();
        }

        private Boolean verifyCabinetInput() // checks input, outlines in red the violating input, returns a true if all input can be processed
        {
            bool ok = true;

            if(Regex.IsMatch(qtyTextBox.Text , @"[^0-9]"))
            {
                ok = false;
                //outline qtyBox in red
            }



            return ok;
        }

        private String getCabinetType()
        {

        }
    }
}
