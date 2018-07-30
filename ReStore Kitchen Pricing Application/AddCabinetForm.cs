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

            if(qtyNumericUpDown.Value < 1)
            {
                ok = false;
                //outline qtyBox in red TODO
            }

            //check if no type is chosen

            if (Regex.IsMatch(heightTextBox.Text, @"[^0-9]"))
            {
                ok = false;
                //outline heightTxtBox in red TODO
            }

            if (Regex.IsMatch(depthTextBox.Text, @"[^0-9]"))
            {
                ok = false;
                //outline depthTxtBox in red TODO
            }

            //should not be "drawers to floor" if there is a door in the cabinet
            if (drawers2FloorCheckBox.Checked && doorsNumericUpDown.Value > 1)
            {
                ok = false;
                //outline both  in red TODO

                //Dialogbox to let user no there cant be any doors with drawer2floor
            }

            return ok;
        }

        

        private String getCabinetType()
        {
            //make a generic get radio in groupbox?
        }
    }
}
