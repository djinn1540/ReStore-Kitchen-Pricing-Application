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

        public static Boolean isOneCheckedIn(GroupBox box)
        {
            foreach (Control control in box.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio is RadioButton) //safety if casting to Radio unexpectedly fails
                    {
                        if (radio.Checked == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false; // error - there should have been one radio button checked
        }

       public static RadioButton getCheckedRadioFrom(GroupBox box)
        {
            foreach (Control control in box.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;

                    if (radio is RadioButton) //safety if casting to Radio unexpectedly fails
                    {
                        if(radio.Checked == true)
                        {
                            return radio;
                        }
                    }
                } 

            }
            //Error -- there should have been one radioButton checked
            return null;
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            if (!verifyKitchenInput())
            {
                string messageBoxText = "Please fix the areas highlighted in red.";
                string caption = "Print Kitchen";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Warning;
                MessageBox.Show(messageBoxText, caption, button, icon);

                return;
            }


            //process kitchen info

            //put kitchen info into PDF/printable

            //move on to next kitchen?
        }

        private Boolean verifyKitchenInput()
        {
            bool ok = true;

            //TODO kitchen id regex check


            if(!isOneCheckedIn())
        }
    }
}
