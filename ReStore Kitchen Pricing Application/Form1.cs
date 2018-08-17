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
        public LinkedList<int> cabinetPrices;

        public kitchenForm()
        {
            InitializeComponent();
            cabinetPrices = new LinkedList<int>();
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


            //process kitchen info into "pastable" variables
            string cabinetsInfo = makeCabinetsInfo();

            //put kitchen info into HTML/printable

            //move on to next kitchen?
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

            if (!isOneCheckedIn(qualityRatingGroupBox))
            {
                ok = false;
                turnRed(qualityRatingGroupBox);
            }
            else
            {
                turnToColor(qualityRatingGroupBox, default(Color));
            }

            if(plusCheckBox.Checked && minusCheckBox.Checked)
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

            if(distinctiveCharTextBox.Text == null)
            {
                ok = false;
                turnRed(distinctiveCharTextBox);
            }
            else
            {
                turnToColor(distinctiveCharTextBox, default(Color));
            }

            //check if there are cabinets in the kitchen
            if(CabinetDataGrid.RowCount < 2) // the row count must be 2 because there is always an empty row
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
                if (isOneCheckedIn(endPanelGroupBox))
                {
                    if (endPanelHeightTextBox.Text == "")
                    {
                        ok = false;
                        turnRed(endPanelHeightTextBox);
                    }
                    else
                    {
                        turnToColor(endPanelHeightTextBox, default(Color));
                    }

                    if (endPanelWidthTextBox.Text == "")
                    {
                        ok = false;
                        turnRed(endPanelWidthTextBox);
                    }
                    else
                    {
                        turnToColor(endPanelWidthTextBox, default(Color));
                    }
                }

                if ((endPanelWidthTextBox.Text != "" || endPanelHeightTextBox.Text != "") && !flatEndPanelRadioButton.Checked && !pannelledEndPanelRadioButton.Checked) //if user entered some endPanel measurement info, but forgot to pick a panel type
                {
                    ok = false;
                    turnRed(endPanelGroupBox);
                }
                else
                {
                    turnToColor(endPanelGroupBox, default(Color));
                }
            }

            
            if(crownMoldingFeetNumericUpDown.Value < 0)
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
                String yearAlpha; //the last letter in a kitchen code represents the year the kitchen was evaluated: "F" = 2017
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

        private static void createPrintable(string id, string distChar, string desc, string cabinetDetails, string otherInfo)
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
            System.IO.FileStream fs = System.IO.File.OpenWrite(System.IO.Directory.GetCurrentDirectory() + "\\" + id); //the filled out template will be saved in an HTML file named the kitchen's id
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fs, Encoding.UTF8);
            writer.Write(bodyFile);
            writer.Close();
        }

        private string makeCabinetsInfo()
        {
            
            StringBuilder sb = new StringBuilder();
            
            foreach(DataGridViewRow row in CabinetDataGrid.Rows)
            {
                DataGridViewCellCollection cells = row.Cells;
                sb.Append(cells[0].Value.ToString()); //quantity
                sb.Append(" ");

                //TODO, change all literal indicies to meaningful variables

                if (!cells[5].Value.ToString().Equals("0")) // number of doors
                {
                    sb.Append(cells[5].Value.ToString());
                    sb.Append("-door (");
                    sb.Append(cells[6].Value.ToString());
                    sb.Append(" side hinges), ");
                }

                if (!cells[7].Value.ToString().Equals("0"))
                {
                    sb.Append(cells[7].Value.ToString());
                    sb.Append("-drawer, ");
                }

                sb.Append(cells[1].Value.ToString()); //type
                sb.Append(" cabinet(s) measuring ");
                sb.Append(cells[2].Value.ToString());// dimensions

                if (cells[3].Value.ToString() != "") { //features
                    sb.Append(" with ");
                    sb.Append(makeEnglishAccessoriesList(cells[3].Value.ToString()));
                }

                sb.Append(".  ");
                
                if (cells[4].Value.ToString() != "")//finished sides
                {
                    sb.Append("Finished on ");
                    sb.Append(cells[4].Value.ToString());
                    sb.Append(" side(s).");
                }

                sb.Append("\n");
                
            }

            return sb.ToString();
        }

        private string makeEnglishAccessoriesList(string list)
        {
            Regex expression = new Regex(@"^(?<beginningList>[A-Za-z, ]*), (?<lastListItem>[A-Za-z]*)$");
            Match match = expression.Match(list);

            if (match.Success)
            {
                string foreList = match.Groups["beginningList"].Value;
                string lastAccessory = match.Groups["lastListItem"].Value;

                return foreList + " and" + lastAccessory;
            }
            else
            {
                return "Error: accessory list did not match the regular expression";
            }
        }

        private string makeKitchenDescription()
        {
            
        }
       
    }
}
