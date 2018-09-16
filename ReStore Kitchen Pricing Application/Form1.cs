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
    public partial class kitchenForm : Form
    {
        public LinkedList<decimal> cabinetPrices;
        public string initials = null;
        public String yearAlpha; //the last letter in a kitchen code represents the year the kitchen was evaluated: "F" = 2017

        private int qtyIndex = 0;
        private int typeIndex = 1;
        private int dimensionsIndex = 2;
        private int featuresIndex = 3;
        private int finishedSidesIndex = 4;
        private int numDoorsIndex = 5;
        private int hingesIndex = 6;
        private int numDrawersIndex = 7;
        private int cornerIndex = 8;

        public kitchenForm()
        {
            InitializeComponent();
            cabinetPrices = new LinkedList<decimal>();
        }

        private void addCabinetButton_Click(object sender, EventArgs e)
        {

            if (!verifyQualityRating())
            {
                string messageBoxText = "Please give the kitchen a quality rating.";
                string caption = "Add Cabinet";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Warning;
                MessageBox.Show(messageBoxText, caption, button, icon);

                return;
            }

            string rating = getCheckedRadioFrom(qualityRatingGroupBox).Text;


            AddCabinetForm addCabForm = new AddCabinetForm(this, rating);
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
                        if (radio.Checked == true)
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


            //process kitchen info into "pastable" variables

            string kitchDesc = makeKitchenDescription();
            string otherInfo;
            if(otherInfoTextBox.Text == null)
            {
                otherInfo = " ";
            }
            else
            {
                otherInfo = otherInfoTextBox.Text;
            }

            //put kitchen info into HTML/printable
            createPrintable(kitchenIdentifierTextBox.Text, distinctiveCharTextBox.Text, kitchDesc, otherInfo);

            resetKitchen();
        }

        private Boolean verifyKitchenInput()
        {
            bool ok = true;

            if (!validKitchenIdentifier())
            {
                ok = false;
                turnRed(kitchenIdentifierTextBox);
            }
            else
            {
                turnToColor(kitchenIdentifierTextBox, default(Color));
            }

            if (!isOneCheckedIn(constructionGroupBox))
            {
                ok = false;
                turnRed(constructionGroupBox);
            }
            else
            {
                turnToColor(constructionGroupBox, default(Color));
            }

            if (!isOneCheckedIn(cabStyleGroupBox))
            {
                ok = false;
                turnRed(cabStyleGroupBox);
            }
            else
            {
                turnToColor(cabStyleGroupBox, default(Color));
            }


            if (!isOneCheckedIn(doorStyleGroupBox))
            {
                ok = false;
                turnRed(doorStyleGroupBox);
            }
            else
            {
                turnToColor(doorStyleGroupBox, default(Color));
            }

            if (!isOneCheckedIn(panelStyleGroupBox))
            {
                ok = false;
                turnRed(panelStyleGroupBox);
            }
            else
            {
                turnToColor(panelStyleGroupBox, default(Color));
            }

            if (!verifyQualityRating()) //the boolean function will turn the group box red on failure
            {
                ok = false;
            }

            if (plusCheckBox.Checked && minusCheckBox.Checked)
            {
                ok = false;
                turnRed(plusCheckBox);
                turnRed(minusCheckBox);
            }
            else
            {
                turnToColor(plusCheckBox, default(Color));
                turnToColor(minusCheckBox, default(Color));
            }

            if (distinctiveCharTextBox.Text == null)
            {
                ok = false;
                turnRed(distinctiveCharTextBox);
            }
            else
            {
                turnToColor(distinctiveCharTextBox, default(Color));
            }

            //check if there are cabinets in the kitchen
            if (CabinetDataGrid.RowCount < 1)
            {
                ok = false;
                turnRed(gridGroupBox);
            }
            else
            {
                turnToColor(gridGroupBox, default(Color));
            }

            if (materialsComboBox.SelectedItem == null)
            {
                ok = false;
                turnRed(materialsComboBox);
            }
            else
            {
                turnToColor(materialsComboBox, default(Color));
            }

            {//checks for if the user has partly entered info for the End Panel
                //sets "attempt" flag if the user has entered any information for the end panel section
                bool attempt = false;
                if (isOneCheckedIn(endPanelGroupBox) || endPanelNumericUpDown.Value > 0 || endPanelWidthTextBox.Text != "" || endPanelHeightTextBox.Text != "")
                {
                    attempt = true;
                }

                if (attempt && endPanelHeightTextBox.Text == "")
                {
                    ok = false;
                    turnRed(endPanelHeightTextBox);
                }
                else
                {
                    turnToColor(endPanelHeightTextBox, default(Color));
                }

                if (attempt && endPanelWidthTextBox.Text == "")
                {
                    ok = false;
                    turnRed(endPanelWidthTextBox);
                }
                else
                {
                    turnToColor(endPanelWidthTextBox, default(Color));
                }


                if (attempt && !isOneCheckedIn(endPanelGroupBox))
                {
                    ok = false;
                    turnRed(endPanelGroupBox);
                }
                else
                {
                    turnToColor(endPanelGroupBox, default(Color));
                }

                if (attempt && endPanelNumericUpDown.Value == 0)
                {
                    turnRed(endPanelNumericUpDown);
                }
                else
                {
                    turnToColor(endPanelNumericUpDown, default(Color));
                }


            }


            if (crownMoldingFeetNumericUpDown.Value < 0)
            {
                ok = false;
                turnRed(crownMoldingFeetNumericUpDown);
            }
            else
            {
                turnToColor(crownMoldingFeetNumericUpDown, default(Color));
            }

            return ok;
        }

        private Boolean validKitchenIdentifier()
        {


            if (!kitchenIdentifierTextBox.Text.Equals(""))
            {

                String kitchenPattern = "K-[0-9]+(?<yearCode>[F-Z]$)"; //by choosing "F-Z" we are limiting the date range to 2017 to 2037
                bool success = Regex.IsMatch(kitchenIdentifierTextBox.Text, kitchenPattern);
                if (success)
                {
                    Match match = Regex.Match(kitchenIdentifierTextBox.Text, kitchenPattern);
                    yearAlpha = match.Groups[1].Value;

                    int year = DateTime.Today.Year;
                    //map an int to a char s.t. 2017 == F
                    int differenceFromBaseYear = year - 2017;
                    //iterate from F until difference is decremented to 0
                    char letterCode = 'F';
                    while (differenceFromBaseYear > 0)
                    {
                        differenceFromBaseYear--;
                        letterCode++;
                    }

                    //Compare the letter code with yearAlpha
                    if (letterCode.ToString().Equals(yearAlpha))
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        private Boolean verifyQualityRating()
        {
            if (!isOneCheckedIn(qualityRatingGroupBox))
            {
                turnRed(qualityRatingGroupBox);
                return false;
            }
            else
            {
                turnToColor(qualityRatingGroupBox, default(Color));
                return true;
            }
        }

        public static void turnRed(Control c)
        {
            if (c != null)
                c.BackColor = Color.Red;
        }

        public static void turnToColor(Control c, Color color)
        {
            if (c != null && color != null)
                c.BackColor = color;
        }

        private void createPrintable(string id, string distChar, string desc, string otherInfo)
        {

            string cabinetsInfo = makeCabinetsInfo(false);
            string cabinetsInfoWithPricing = makeCabinetsInfo(true);

            string price = "$" + Decimal.Round(calculateKitchenPrice()).ToString();

            try
            {
                createCabinetListing(id, distChar, desc, cabinetsInfo, otherInfo);
                createCabinetListing(id + " with Pricing Info", distChar, desc, cabinetsInfoWithPricing, otherInfo);
                createPriceListing(id, distChar, price, initials, getMonthCode());
            }
            catch (System.IO.FileNotFoundException e)
            {
                string messageBoxText = "There was an error in operation, an expected file in the local directory of the application was not found.  Please make sure you did not rename any template files and refer to the documentation for the original names if you did.";
                string caption = "Print Kitchen";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
                Application.Exit();
            }

        }

        private static void createCabinetListing(string id, string distChar, string desc, string cabinetDetails, string otherInfo)
        {
            string bodyFile;
            string template = System.IO.Directory.GetCurrentDirectory() + "\\template.html";

            using (System.IO.StreamReader reader = new System.IO.StreamReader(template))
            {
                bodyFile = reader.ReadToEnd();
                bodyFile = bodyFile.Replace("<%Identifier%>", id);
                bodyFile = bodyFile.Replace("<%DistinctChars%>", distChar);
                bodyFile = bodyFile.Replace("<%Description%>", desc);
                bodyFile = bodyFile.Replace("<%CabinetsParagraph%>", cabinetDetails);
                bodyFile = bodyFile.Replace("<%OtherInfo%>", otherInfo);
            }
            System.IO.FileStream fs = System.IO.File.OpenWrite(System.IO.Directory.GetCurrentDirectory() + "\\" + id + " Cabinet Spec Sheet.html"); //the filled out template will be saved in an HTML file named the kitchen's id
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fs, Encoding.UTF8);
            writer.Write(bodyFile);
            writer.Close();
            fs.Close();
        }

        private static void createPriceListing(string id, string distChar, string price, string initials, string monthCode)
        {
            string bodyFile;
            string template = System.IO.Directory.GetCurrentDirectory() + "\\priceTemplate.html";

            using (System.IO.StreamReader reader = new System.IO.StreamReader(template))
            {
                bodyFile = reader.ReadToEnd();
                bodyFile = bodyFile.Replace("<%KitchenID%>", id);
                bodyFile = bodyFile.Replace("<%DistinctiveCharacteristics%>", distChar);
                bodyFile = bodyFile.Replace("<%KitchenPrice%>", price);
                bodyFile = bodyFile.Replace("<%VolunteerInitials%>", initials);
                bodyFile = bodyFile.Replace("<%MonthCode%>", monthCode);
            }
            System.IO.FileStream fs = System.IO.File.OpenWrite(System.IO.Directory.GetCurrentDirectory() + "\\" + id + " price listing.html"); //the filled out template will be saved in an HTML file named the kitchen's id
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fs, Encoding.UTF8);
            writer.Write(bodyFile);
            writer.Close();
            fs.Close();
        }

        private string makeCabinetsInfo(bool pricingFlag) //relies on verifyKitchen to have returned true
        {

            StringBuilder sb = new StringBuilder();

            foreach (DataGridViewRow row in CabinetDataGrid.Rows)
            {
                DataGridViewCellCollection cells = row.Cells;


                sb.Append(cells[qtyIndex].Value.ToString()); //quantity
                sb.Append(" ");


                if (!cells[numDoorsIndex].Value.ToString().Equals("0")) // number of doors
                {
                    sb.Append(cells[numDoorsIndex].Value.ToString());
                    sb.Append("-door (");
                    sb.Append(cells[hingesIndex].Value.ToString());
                    sb.Append(" side hinges), ");
                }

                if (!cells[numDrawersIndex].Value.ToString().Equals("0"))
                {
                    sb.Append(cells[numDrawersIndex].Value.ToString());
                    sb.Append("-drawer, ");
                }

                if (cells[cornerIndex].Value.ToString().Equals("Yes"))
                {
                    sb.Append("corner ");
                }

                sb.Append(cells[typeIndex].Value.ToString()); //type
                sb.Append(" cabinet(s) measuring ");
                sb.Append(cells[dimensionsIndex].Value.ToString());// dimensions

                if (cells[featuresIndex].Value.ToString() != "")
                { //features
                    sb.Append(" with ");
                    sb.Append(makeEnglishAccessoriesList(cells[featuresIndex].Value.ToString()));
                }

                sb.Append(".  ");

                if (cells[finishedSidesIndex].Value.ToString() != "")//finished sides
                {
                    sb.Append("Finished on ");
                    sb.Append(cells[finishedSidesIndex].Value.ToString());
                    sb.Append(" side(s).");
                }

                if (pricingFlag)
                {
                    sb.Append("<span style=\"color: Tomato;\">  $");
                    sb.Append(Decimal.Round(cabinetPrices.ElementAt(row.Index), 2).ToString());
                    sb.Append("</span>");
                }

                sb.Append("<br>");

            }

            return sb.ToString();
        }

        private string makeEnglishAccessoriesList(string list)
        {
            Regex expression = new Regex(@"^(?<beginningList>[A-Za-z, /\-]+), (?<lastListItem>[A-Za-z /\-]+)$");
            Match match = expression.Match(list);

            if (match.Success)
            {
                string foreList = match.Groups["beginningList"].Value;
                string lastAccessory = match.Groups["lastListItem"].Value;

                return foreList + ", and " + lastAccessory;
            }
            else
            {
                return list;//this is the case where there is only one accessory
            }
        }

        private string makeKitchenDescription()  //relies on verifyKitchen to have returned true
        {
            //This (material) kitchen has (total cabinets #) (cabinet style) cabinets with (?fulloverlay) 
            //(doorstyle) doors with (panel style) panels.  
            //The interior of the cabinets is constructed from (construction).  
            //(?The kitchen also comes with (? feet of crown molding) (??and) (? # end panels) end panels )

            StringBuilder sb = new StringBuilder();
            sb.Append("This ");
            sb.Append(materialsComboBox.SelectedItem.ToString()); //kitchen material
            sb.Append(" kitchen has ");
            sb.Append(cabinetCountLabel.Text); //# of cabinets
            sb.Append(" ");
            sb.Append(getCheckedRadioFrom(cabStyleGroupBox).Text);//cabinet style
            sb.Append(" cabinets with ");

            if (fullOverlayCheckBox.Checked)//? full overlay
            {
                sb.Append("full-overlay, ");
            }

            sb.Append(getCheckedRadioFrom(doorStyleGroupBox).Text);//doorstyle
            sb.Append(" doors with ");
            sb.Append(getCheckedRadioFrom(panelStyleGroupBox).Text);//panelstyle
            sb.Append(" panels.  The interior of the cabinets is constructed from ");
            sb.Append(getCheckedRadioFrom(constructionGroupBox).Text);//construction
            sb.Append(".  ");

            if (crownMoldingFeetNumericUpDown.Value > 0 || endPanelNumericUpDown.Value > 0)
            {
                bool needAnd = false;

                sb.Append("The kitchen also comes with ");

                if (crownMoldingFeetNumericUpDown.Value > 0)
                {
                    sb.Append(crownMoldingFeetNumericUpDown.Value.ToString());
                    sb.Append(" feet of crown molding");
                    needAnd = true;
                }

                if (endPanelNumericUpDown.Value > 0)
                {
                    if (needAnd)
                    {
                        sb.Append(" and ");
                    }

                    sb.Append(endPanelNumericUpDown.Value.ToString());
                    sb.Append(" end panels");
                }

                sb.Append(".");
            }

            sb.Append("\n");

            return sb.ToString();
        }


        private decimal calculateKitchenPrice()
        {
            decimal cabinetsTotal = getCabinetsTotal();

            cabinetsTotal += getEndPanelPrice();
            cabinetsTotal += computeCrownMoldingPrice();

            decimal adjustedPrice = adjustedPriceForPlusMinus(cabinetsTotal);

            adjustedPrice = adjustedPriceForMaterial(cabinetsTotal, adjustedPrice);
            adjustedPrice = adjustedPriceForParticleBoard(cabinetsTotal, adjustedPrice);

            return adjustedPrice;
        }

        private decimal getCabinetsTotal()
        {
            decimal total = 0.0M;
            foreach (decimal cabinetPrice in cabinetPrices)
            {
                total += cabinetPrice;
            }

            return total;
        }

        private decimal getEndPanelPrice() //depends on verify kitchen returning true bc of computeSQFootage and quality rating
        {
            if (endPanelNumericUpDown.Value > 0)
            {
                decimal footage = computeSquareFootage();

                if (flatEndPanelRadioButton.Checked)
                {
                    if (aRatingRadioButton.Checked)
                    {
                        return 8M * footage;
                    }
                    else if (bRatingRadioButton.Checked)
                    {
                        return 7M * footage;
                    }
                    else if (cRatingRadioButton.Checked)
                    {
                        return 6M * footage;
                    }
                    else
                    {
                        throw new PriceComputationException("No rating button checked in EndPanelPrice");
                    }
                }
                else if (pannelledEndPanelRadioButton.Checked)
                {
                    if (aRatingRadioButton.Checked)
                    {
                        return 10M * footage;
                    }
                    else if (bRatingRadioButton.Checked)
                    {
                        return 9M * footage;
                    }
                    else if (cRatingRadioButton.Checked)
                    {
                        return 8M * footage;
                    }
                    else
                    {
                        throw new PriceComputationException("No rating button checked in EndPanelPrice");
                    }
                }

                else
                {
                    throw new PriceComputationException("No end panel type button checked in EndPanelPrice");
                }

            }
            else
            {
                return 0.0M;
            }
        }

        private decimal computeSquareFootage() //depends on verifyKitchen returning true
        {

            decimal height = Decimal.Parse(endPanelHeightTextBox.Text);
            decimal width = Decimal.Parse(endPanelWidthTextBox.Text);

            return height * width * endPanelNumericUpDown.Value;
        }

        private decimal computeCrownMoldingPrice()
        {
            return crownMoldingFeetNumericUpDown.Value;
        }

        private decimal adjustedPriceForPlusMinus(decimal total)
        {
            if (minusCheckBox.Checked)
            {
                return total * 0.95M;
            }
            else if (plusCheckBox.Checked)
            {
                return total * 1.05M;
            }
            else
            {
                return total;
            }
        }//depends on verify kitchen returning true

        private decimal adjustedPriceForMaterial(decimal total, decimal currentPrice)
        {
            switch (materialsComboBox.SelectedItem.ToString())
            {
                case "Birch":
                    return (total * .1M) + currentPrice;
                case "Cherry":
                    return (total * .1M) + currentPrice;
                case "Dark Oak":
                    return currentPrice;
                case "Formica":
                    return (total * -.1M) + currentPrice;
                case "Light Oak":
                    return (total * .05M) + currentPrice;
                case "Maple":
                    return (total * .1M) + currentPrice;
                case "Metal":
                    return (total * -.1M) + currentPrice;
                case "Pine":
                    return currentPrice;
                case "Thermofoil (PVC)":
                    return (total * -.05M) + currentPrice;
                default:
                    throw new PriceComputationException("No material-selected matched in adjusting price for material");
            }
        } //depends on verifyKitchen returning true

        private decimal adjustedPriceForParticleBoard(decimal total, decimal currentPrice)// depends on verifyKitchen returning true
        {
            if (particleBoardRadioButton.Checked)
            {
                return currentPrice - (total * .05M);
            }
            else
            {
                return currentPrice;
            }
        }

        public void incrementCabinetNumberLabel(int newCabinets)
        {
            int sum = int.Parse(cabinetCountLabel.Text);
            cabinetCountLabel.Text = (sum + newCabinets).ToString();
        }

        private void resetKitchen()
        {

            cabinetPrices = new LinkedList<decimal>();
            kitchenIdentifierTextBox.Text = "";
            distinctiveCharTextBox.Text = "";
            otherInfoTextBox.Text = "";
            endPanelHeightTextBox.Text = "";
            endPanelWidthTextBox.Text = "";

            fullOverlayCheckBox.Checked = false;
            minusCheckBox.Checked = false;
            plusCheckBox.Checked = false;

            getCheckedRadioFrom(constructionGroupBox).Checked = false;
            getCheckedRadioFrom(cabStyleGroupBox).Checked = false;
            getCheckedRadioFrom(doorStyleGroupBox).Checked = false;
            getCheckedRadioFrom(panelStyleGroupBox).Checked = false;
            getCheckedRadioFrom(qualityRatingGroupBox).Checked = false;
            setEnableRatings(true);//must come after setting checked in quality rating to false

            if (isOneCheckedIn(endPanelGroupBox))
            {
                getCheckedRadioFrom(endPanelGroupBox).Checked = false;
            }

            cabinetCountLabel.Text = "0";
            endPanelNumericUpDown.Value = 0M;
            crownMoldingFeetNumericUpDown.Value = 0M;

            removeAllRowsFromDataGrid();

            materialsComboBox.SelectedItem = null;
            
        }

        private void removeAllRowsFromDataGrid()
        {
            while (CabinetDataGrid.RowCount > 0)
            {
                CabinetDataGrid.Rows.RemoveAt(0);//remove the first row until there are no more
            }
        }

        private string getMonthCode()
        {
            return DateTime.Now.Month.ToString() + yearAlpha;
        }

        private void kitchenForm_Shown(object sender, EventArgs e)
        {
            initialsForm initForm = new initialsForm(this);


            this.Hide();
            initForm.getInitials(); //kitchenForm will be deactivated until it is reactivated from the initialsForm
        }

        private void setEnableRatings(bool val)
        {
            foreach (Control c in qualityRatingGroupBox.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton radio = c as RadioButton;
                    radio.Enabled = val;
                }
            }
        }

        private void aRatingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            setEnableRatings(false);
        }

        private void bRatingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            setEnableRatings(false);
        }

        private void cRatingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            setEnableRatings(false);
        }
    }
}
    
