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
    public partial class initialsForm : Form
    {
        kitchenForm parent = null;
     

        public initialsForm(kitchenForm parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (validInitials())
            {
                parent.initials = initialsTextBox.Text;
                Console.Out.Write(initialsTextBox.Text);

                if (parent.initials != null)
                {
                    parent.Enabled = true;
                    parent.Visible = true;
                    this.Close();
                }
                else
                {
                    throw new Exception("setting kitchenForm.initials from initialForm did not work");
                }
            }
        }

        public bool validInitials()
        {
            return (initialsTextBox.Text.Length == 2 || initialsTextBox.Text.Length == 3);
    
        }

        public void getInitials()
        {
            this.Show();
            
            //this method is just supposed to transfer focus to the initialsForm
        }
        
    }
}
