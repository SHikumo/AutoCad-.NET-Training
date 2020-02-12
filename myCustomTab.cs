using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lab8
{
    public partial class myCustomTab : UserControl
    {
        public myCustomTab()
        {
            InitializeComponent();
        }

        // 35. Add a Public Sub named OnOk 
        // Note. Put the closing curley brace below step 36 
        public void OnOk()
        {
            // 36. Make the variable created in step 26 eqaul to 
            // TextBox1.Text. Need to qualify the variable with the class name. 
            // something like: adskClass.myVariable 
            adskClass.myVariable = textBox1.Text;

            // Note: There is one more step in this Custom Tab section of Lab8. 
            // Proceed to step 37 and uncomment the line that runs the 
            // AddTabDialog() procedure. (found after step 23). 

            // Before proceeding to the section where you will explore 
            // Drag & Drop from the PaletteSet (Steps Run AutoCAD and open the 
            // OPTIONS dialog. You should see the new tab. 
            // Enter in some text in the TextBox and then 
            // click ok. Then run the testTab command. It should print 
            // the value you entered on the custom tab to the command line. 

        }
    }
}

