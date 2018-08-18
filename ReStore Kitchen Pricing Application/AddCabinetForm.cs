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

        const string baseStandardHeight = "34.5"; // \
        const string baseStandardDepth = "24"; //     \if you add more default values for types of cabinets, you will have to add to the conditions
        const string wallStandardHeight = "30"; //    /   within the "...RadioButton_CheckedChanged" methods to maintain autofill capabilities when going between cabinet types
        const string wallStandardDepth = "12"; //    /

        public AddCabinetForm(kitchenForm parentForm) //parentForm is the form that "birthed" the addCabinetForm, specifically it is the kitchenForm
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void AddCabinetForm_Load(object sender, EventArgs e)
        {

        }

        private void finishAddingCabinetButton_Click(object sender, EventArgs e)
        {
            //verify proper input
            if (!verifyCabinetInput())
            {
                string messageBoxText = "Please fix the areas highlighted in red.";
                string caption = "Add Cabinet";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Warning;
                MessageBox.Show(messageBoxText, caption, button, icon);

                return;
            }

            //TODO ask user if the cabinet sentence is good - use compileCabinetSentence

            {
                //process the cabinet info into DataRow
                String dimensions = widthComboBox.SelectedItem.ToString() + "x" + heightTextBox.Text + "\"x" + depthTextBox.Text + "\"";
                String type = getCabinetType();
                String accessories = getAccessoryList();
                String finished = getFinishedSides();
                String hinges = getHingedSides();


                //add DataRow to the DataTable in parentForm
                parentForm.CabinetDataGrid.Rows.Add(qtyNumericUpDown.Value.ToString(), type, dimensions, accessories, finished, doorsNumericUpDown.Value.ToString(), hinges, drawersNumericUpDown.Value.ToString());

                //TODO add the cabinet price to the price array in the parent form

                //change back to the KitchenForm
                this.Close();
            }
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
                //outline qtyBox in red 
                kitchenForm.turnRed(qtyNumericUpDown);
            }
            else
            {
                //remove any red outlines
                kitchenForm.turnToColor(qtyNumericUpDown, default(Color));
            }

            if (!kitchenForm.isOneCheckedIn(typeGroupBox))
            {
                ok = false;
                //outline typeBox in red 
                kitchenForm.turnRed(typeGroupBox);
            }
            else
            {
                //remove any red outlines
                kitchenForm.turnToColor(typeGroupBox, default(Color));
            }

            if(widthComboBox.SelectedItem == null)
            {
                ok = false;
                //paint in red
                kitchenForm.turnRed(widthComboBox);
            }
            else
            {
                //remove red outline
                kitchenForm.turnToColor(widthComboBox, default(Color));
            }

            if (heightTextBox.Text == "" || Regex.IsMatch(heightTextBox.Text, @"[^0-9]"))
            {
                ok = false;
                //outline heightTxtBox in red
                kitchenForm.turnRed(heightTextBox);
            }
            else
            {
                //remove any red outlines
                kitchenForm.turnToColor(heightTextBox, default(Color));
            }

            if (depthTextBox.Text == "" || Regex.IsMatch(depthTextBox.Text, @"[^0-9]"))
            {
                ok = false;
                //outline depthTxtBox in red
                kitchenForm.turnRed(depthTextBox);
            }
            else
            {
                //remove any red outlines
                kitchenForm.turnToColor(depthTextBox, default(Color));
            }

            //should not be "drawers to floor" if there is a door in the cabinet
            if (drawers2FloorCheckBox.Checked)
            {
                if(doorsNumericUpDown.Value > 0)
                {
                    ok = false;
                    //outline both in red 
                    kitchenForm.turnRed(drawers2FloorCheckBox);
                    kitchenForm.turnRed(doorsNumericUpDown);
                }
                else {
                    kitchenForm.turnToColor(doorsNumericUpDown, default(Color));
                }

                if (glassDoorsCheckBox.Checked)
                {
                    ok = false;
                    //outline in red
                    kitchenForm.turnRed(drawers2FloorCheckBox);
                    kitchenForm.turnRed(glassDoorsCheckBox);
                }
                else
                {
                    kitchenForm.turnToColor(glassDoorsCheckBox, default(Color));
                }

                if(glassDoorsCheckBox.Checked || doorsNumericUpDown.Value > 0)
                {
                    //Dialogbox to let user no there cant be any doors with drawer2floor
                    string messageBoxText = "A cabinet with 'drawers to floor' cannot have any doors.";
                    string caption = "Add Cabinet";
                    MessageBoxButtons button = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Warning;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                }
                
            }
            else
            {
                //remove any red outlines
                kitchenForm.turnToColor(drawers2FloorCheckBox, default(Color));
                kitchenForm.turnToColor(doorsNumericUpDown, default(Color));
                kitchenForm.turnToColor(glassDoorsCheckBox, default(Color));
            }

            return ok;
        }

        private void compileDescriptionSentence() {
            //TODO
        }

        private String getAccessoryList()
        {
            int numOfAcc = 0;
            StringBuilder sentence = new StringBuilder();
            foreach (CheckBox check in accessoryGroupBox.Controls)
            {
                if(check.Checked == true)
                {
                    if(numOfAcc == 0)
                    {
                       //do nothing
                        
                    }
                    else
                    {
                        sentence.Append(", ");
                    }
                    sentence.Append(check.Text);
                    numOfAcc++;
                }
            }
            return sentence.ToString();
        }
        

        private String getCabinetType()
        {
            if (kitchenForm.isOneCheckedIn(typeGroupBox))
            {
                RadioButton selected = kitchenForm.getCheckedRadioFrom(typeGroupBox);
                return selected.Text;
            }
            else
            {
                return null;
            }
        }

        private String getFinishedSides()
        {
            if (finishedLeftCheckBox.Checked && finishedRightCheckBox.Checked)
            {
                return "Left/Right";
            }
            else if (finishedLeftCheckBox.Checked)
            {
                return "Left";
            }
            else if (finishedRightCheckBox.Checked)
            {
                return "Right";
            }
            else
                return "";
        }
        
        private String getHingedSides()
        {
            if (hingeLeftCheckBox.Checked && hingeRightCheckBox.Checked)
            {
                return "Left/Right";
            }
            else if (hingeLeftCheckBox.Checked)
            {
                return "Left";
            }
            else if (hingeRightCheckBox.Checked)
            {
                return "Right";
            }
            else
                return "";
        }

        private void baseRadioButton_CheckedChanged(object sender, EventArgs e) //fills the industry standard measurement into the width and height textboxes
        {
            if (heightTextBox.Text == "" || heightTextBox.Text.Equals(wallStandardHeight))//do not change the text if the user has already entered a custom number (ie not the standard height of a wall unit --> enables autofill if the user switches the cabinet type)
            {
                heightTextBox.Text = baseStandardHeight;
            }

            if (depthTextBox.Text == "" || depthTextBox.Text.Equals(wallStandardDepth))
            {
                depthTextBox.Text = baseStandardDepth;
            }
        }

        private void wallRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(heightTextBox.Text == "" || heightTextBox.Text.Equals(baseStandardHeight))
            {
                heightTextBox.Text = wallStandardHeight;
            }

            if(depthTextBox.Text == "" || depthTextBox.Text.Equals(baseStandardDepth))
            {
                depthTextBox.Text = wallStandardDepth;
            }
        }

    }
}
