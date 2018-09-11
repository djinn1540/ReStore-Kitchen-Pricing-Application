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

        string kitchenRating;

        const string anythingOtherThanNumbers = @"[^0-9.]";

        public AddCabinetForm(kitchenForm parentForm, string rating) //parentForm is the form that "birthed" the addCabinetForm, specifically it is the kitchenForm
        {
            InitializeComponent();
            this.parentForm = parentForm;
            kitchenRating = rating;
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

            
                //process the cabinet info into DataRow
            String dimensions = widthComboBox.SelectedItem.ToString() + "x" + heightTextBox.Text + "\"x" + depthTextBox.Text + "\"";
            String type = getCabinetType();
            String accessories = getAccessoryList();
            String finished = getFinishedSides();
            String hinges = getHingedSides();
            String corner = getCorner();

         
            //add DataRow to the DataTable in parentForm
            parentForm.CabinetDataGrid.Rows.Add(qtyNumericUpDown.Value.ToString(), type, dimensions, accessories, finished, doorsNumericUpDown.Value.ToString(), hinges, drawersNumericUpDown.Value.ToString(), corner);
            parentForm.incrementCabinetNumberLabel(Decimal.ToInt32(qtyNumericUpDown.Value));
                
            try
            {
                decimal price = computeIndividualCabinetPrice();
                parentForm.cabinetPrices.AddLast(price);
            }
            catch(PriceComputationException f)
            {
                //we want to tell the user that there is an issue with the compuation and to contact me 
                string messageBoxText = "There was an error while computing the price of the cabinet.  Please submit an issue to https://github.com/djinn1540/ReStore-Kitchen-Pricing-Application/issues";
                string caption = "Add Cabinet";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);

                return;
            }
 
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

            if (heightTextBox.Text == "" || Regex.IsMatch(heightTextBox.Text, anythingOtherThanNumbers)) 
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

            if (depthTextBox.Text == "" || Regex.IsMatch(depthTextBox.Text, anythingOtherThanNumbers)) 
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
                else
                {
                    kitchenForm.turnToColor(drawers2FloorCheckBox, default(Color));
                }
                
            }
            else
            {
                //remove any red outlines
                kitchenForm.turnToColor(drawers2FloorCheckBox, default(Color));
                kitchenForm.turnToColor(doorsNumericUpDown, default(Color));
                kitchenForm.turnToColor(glassDoorsCheckBox, default(Color));
            }

            if(doorsNumericUpDown.Value > 0  && !(hingeLeftCheckBox.Checked || hingeRightCheckBox.Checked))
            {
                ok = false;
                kitchenForm.turnRed(hingeGroupBox);
            }
            else
            {
                kitchenForm.turnToColor(hingeGroupBox, default(Color));
            }

            return ok;
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

        private String getCorner()
        {
            if (cornerCheckBox.Checked)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
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

        private decimal computeIndividualCabinetPrice()
        {
            string checkedCabinetType = getCabinetType();
            switch (checkedCabinetType)
            {
                case "Base":
                    return computeBaseCabinetPrice() * qtyNumericUpDown.Value;
                  
                case "Wall":
                    return computeWallCabinetPrice() * qtyNumericUpDown.Value;
                case "Pantry":
                    return computePantryCabinetPrice() * qtyNumericUpDown.Value;

                case "Wall Oven":
                    return computeWallOvenCabinetPrice() * qtyNumericUpDown.Value;

                case "Refrigerator Enclosure":
                    return computeFridgeEnclosureCabinetPrice() * qtyNumericUpDown.Value;

                case "Over Refrigerator":   
                case "Over Stove":
                    return computeOverFridgeStoveCabinetPrice() * qtyNumericUpDown.Value;

                case "Desk":
                    return computeDeskCabinetPrice() * qtyNumericUpDown.Value;
                default:
                    throw new PriceComputationException("No case triggered in Individual Cabinet Price");
                    
                   
            }
        }

        private decimal computeBaseCabinetPrice()
        {
            decimal price = 0M;


            if (widthComboBox.SelectedItem.ToString().Equals("12-15\""))
            {
                Console.WriteLine("no we didnt skip it");
                switch (kitchenRating)
                {
                    case "A":
                        Console.WriteLine("yes, should be 95");
                        price = 95M;
                        break;
                    case "B":
                        price = 80M;
                        break;
                    case "C":
                        price = 65M;
                        break;
                    default:
                        Console.WriteLine("did not match case?");
                        throw new PriceComputationException("No rating case triggered in Base Cabinet Price 1");
                }
            }

            Console.WriteLine("did we skip it?");

            if (widthComboBox.SelectedItem.ToString().Equals("18-21\""))
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 130M;
                        break;
                    case "B":
                        price = 110M;
                        break;
                    case "C":
                        price = 90M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Base Cabinet Price 2");
                }
            }

            if (widthComboBox.SelectedItem.ToString().Equals("23-30\""))
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 145M;
                        break;
                    case "B":
                        price = 120M;
                        break;
                    case "C":
                        price = 95M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Base Cabinet Price 3");
                }
            }

            if (widthComboBox.SelectedItem.ToString().Equals("33\"+"))
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 160M;
                        break;
                    case "B":
                        price = 135M;
                        break;
                    case "C":
                        price = 110M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Base Cabinet Price 4");
                }
            }

            if (cornerCheckBox.Checked && lazySusanCheckBox.Checked)
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 175M;
                        break;
                    case "B":
                        price = 145M;
                        break;
                    case "C":
                        price = 105M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Base Cabinet Price 5");
                }
            }

            if (drawers2FloorCheckBox.Checked)
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 175M;
                        break;
                    case "B":
                        price = 150M;
                        break;
                    case "C":
                        price = 125M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Base Cabinet Price 6");
                }
            }

            if (selfExtendingCheckBox.Checked)
            {
                switch (kitchenRating)
                {
                    case "A":
                        price += 10;
                        break;
                    case "B":
                        price += 8;
                        break;
                    case "C":
                        price += 5;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Base Cabinet Price 7");
                }
            }

            if (rollOutShelvesCheckBox.Checked)
            {
                switch (kitchenRating)
                {
                    case "A":
                        price += 15;
                        break;
                    case "B":
                        price += 12;
                        break;
                    case "C":
                        price += 10;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Base Cabinet Price 8");
                }
            }



            return price;

        }

        private decimal computeWallCabinetPrice()
        {
            decimal price = 0.0M;

            if (widthComboBox.SelectedItem.ToString().Equals("12-15\""))
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 70M;
                        break;
                    case "B":
                        price = 60M;
                        break;
                    case "C":
                        price = 45M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Wall Cabinet Price 1");
                }
            }

            if (widthComboBox.SelectedItem.ToString().Equals("18-21\""))
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 110M;
                        break;
                    case "B":
                        price = 90M;
                        break;
                    case "C":
                        price = 70M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Wall Cabinet Price 2");
                }
            }

            if (widthComboBox.SelectedItem.ToString().Equals("23-30\""))
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 135M;
                        break;
                    case "B":
                        price = 110M;
                        break;
                    case "C":
                        price = 85M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Wall Cabinet Price 3");
                }
            }

            if (widthComboBox.SelectedItem.ToString().Equals("33\"+"))
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 150;
                        break;
                    case "B":
                        price = 125;
                        break;
                    case "C":
                        price = 100;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Wall Cabinet Price 4");
                }
            }

            if (cornerCheckBox.Checked && lazySusanCheckBox.Checked)
            {
                switch (kitchenRating)
                {
                    case "A":
                        price = 175M;
                        break;
                    case "B":
                        price = 145M;
                        break;
                    case "C":
                        price = 105M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Wall Cabinet Price 5");
                }
            }

            if (glassDoorsCheckBox.Checked)
            {
                switch (kitchenRating)
                {
                    case "A":
                        price += 20M;
                        break;
                    case "B":
                        price += 15M;
                        break;
                    case "C":
                        price += 10M;
                        break;
                    default:
                        throw new PriceComputationException("No rating case triggered in Wall Cabinet Price 6");
                }
            }

            if (Decimal.Parse(heightTextBox.Text) >= 42M)
            {
                price += 20M;
            }
            else if (Decimal.Parse(heightTextBox.Text) >= 36M)
            {
                price += 10M;
            }


            return price;
        }

        private decimal computePantryCabinetPrice()
        {
            if (rollOutShelvesCheckBox.Checked)
            {
                switch (kitchenRating)
                {
                    case "A":
                        return 175M;
                    case "B":
                        return 160M;
                    case "C":
                        return 145M;
                    default:
                        throw new PriceComputationException("No rating case triggered in Pantry Cabinet Price 1");
                }
            }
            else
            {
                switch (kitchenRating)
                {
                    case "A":
                        return 150M;
                    case "B":
                        return 135M;
                    case "C":
                        return 120M;
                    default:
                        throw new PriceComputationException("No rating case triggered in Pantry Cabinet Price 2");
                }
            }
          
        }

        private decimal computeWallOvenCabinetPrice()
        {
            switch (kitchenRating)
            {
                case "A":
                    return 125M;
                case "B":
                    return 110M;
                case "C":
                    return 95M;
                default:
                    throw new PriceComputationException("No rating case triggered in Wall Oven Cabinet Price");
            }
        }

        private decimal computeFridgeEnclosureCabinetPrice()  //the method is here in case the pricing changes for it specifically
        {
            return computeWallCabinetPrice();
        }

        private decimal computeOverFridgeStoveCabinetPrice()
        {
            switch (kitchenRating)
            {
                case "A":
                    return 100M;
                case "B":
                    return 75M;
                case "C":
                    return 50M;
                default:
                    throw new PriceComputationException("No rating case triggered in Over Fridge/Stove Cabinet Price");
            }
        }

        
        private decimal computeDeskCabinetPrice()
        {
            switch (kitchenRating)
            {
                case "A":
                    return 125M;
                case "B":
                    return 110M;
                case "C":
                    return 95M;
                default:
                    throw new PriceComputationException("No rating case triggered in Desk Cabinet Price");
            }
        }


    }

    public class PriceComputationException : Exception
    {

        public PriceComputationException()
        {

        }
        public PriceComputationException(string message)
            : base(message)
        {
        }
    }
}
