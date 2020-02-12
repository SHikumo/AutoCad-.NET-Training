using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.DatabaseServices;

//using Autodesk.AutoCAD.Windows;
//using Autodesk.AutoCAD.Runtime;

namespace Lab8
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            // 38. To detect when a drag event is taking place register the MouseMove event
            // of the DragLabel. (DragLabel.MouseMove) Use += and the new keyword
            // to create a new System.Windows.Forms.MouseEventHandler for the 
            // object parameter use the function we will create in steps 39-41.
            // we will name it DragLabel_MouseMove
            DragLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(DragLabel_MouseMove);
        }

        // 39. create a private void function named DragLabel_MouseMove. Have it take
        // two parameters and object and System.Windows.Forms.MouseEventArgs. Name
        // the object sender and the MouseEventArgs e
        // Note: Put the closing curley brace below step 41
        private void DragLabel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // 40. It’s enough to see that we know when a mouse-move operation takes place. 
            // We can even go further to tell that the "left" mouse button is currently pressed 
            // Use an "if" statement. For the test see if the Left mouse button is being 
            // used: System.Windows.Forms.Control.MouseButtons == System.Windows.Forms.MouseButtons.Left 
            // Put the closing curley brace beow step 31. 
            if (System.Windows.Forms.Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)
            {

                // 41. (Ensure a reference to Windowsbase has been added). 
                // This is needed for System.Windows.DependencyObject() 
                // Call the DoDragDrop method of the Application. For the drag source 
                // paramter use the "this" keyword. For the data paramter use "this" as well. 
                // For Allowed effects use System.Windows.Forms.DragDropEffects.All 
                // For the Target paramter use the new statement and a class named "MyDropTarget" 
                // This class is created in steps 42-48. The DropTarget is how our MyDropTarget 
                // override is hooked up to the mechanism. 

                Autodesk.AutoCAD.ApplicationServices.Application.DoDragDrop(this, this, System.Windows.Forms.DragDropEffects.All, new MyDropTarget());
            }
        }

        private void DragLabel_Click(object sender, EventArgs e)
        {

        }

    }
    // 42. Here we create a class that will detect when the object is "dropped" 
    // in the AutoCAD editor. Add a public class to the project called MyDropTarget which 
    // inherits from Autodesk.AutoCAD.Windows.DropTarget. (the name needs to be the 
    // same as used in step 41). 
    // Note: Put the closing curley brace below in step 48 
    public class MyDropTarget : Autodesk.AutoCAD.Windows.DropTarget
    {

        // 43. Override the OnDrop procedure. (Use public override void) For the parameter 
        // use "System.Windows.Forms.DragEventArgs e" 
        public override void OnDrop(System.Windows.Forms.DragEventArgs e)
        {
            // 44. Declare a variable as an Editor. Instantiate it by making it equal 
            // to Application.DocumentManager.MdiActiveDocument.Editor 
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;

            // 45. Add a try catch block. 
            // Note: Put the try closing curley brace after step 47.
            // put the catch after this and the closing curley brace after step 48 
            try
            {
                // 46. Use the using statement and declare a DocumentLock variable named 
                // docLock. Instantitate it using: 
                // Application.DocumentManager.MdiActiveDocument.LockDocument() 
                // Note: Put the using closing curley brace after step 47. 
                // Creating the variable will lock the document and unlock it
                // when the vairable is disposed
                using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                {
                    // 47. Run the AddAnEnt procedure from lab 4. Need to 
                    // qualify the name with the namespace and Class. (Lab8_Complete.adskClass.) 
                    Lab8.adskClass.AddAnEnt();
                }
            }
            catch (System.Exception ex)
            {
                // 48. Add System.Exception ex to the Catch statement 
                // Use the Editor created in step 42 to display message to the 
                // user. If we get here something went wrong. Use something like 
                // this for the message parameter: 
                // "Error Handling OnDrop: " + ex.Message 
                ed.WriteMessage("Error Handling OnDrop: " + ex.Message);
            }

            // Note this is the End of Lab8. Run AutoCAD load the dll and run the 
            // Palette command. Drag the label to the drawing area. 
            // The AddAnEnd command should start when you drop it. 
        }
    }
}
