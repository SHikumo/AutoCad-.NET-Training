
// (C) Copyright 2002-2005 by Autodesk, Inc. 
// 
// Permission to use, copy, modify, and distribute this software in 
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and 
// restricted rights notice below appear in all supporting 
// documentation. 
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF 
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE 
// UNINTERRUPTED OR ERROR FREE. 
// 
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer 
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii) 
// (Rights in Technical Data and Computer Software), as applicable. 

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

using Autodesk.AutoCAD.Windows;

using System.Runtime.InteropServices; //(move outside of the class needed for P/Invoke) 


[assembly: ExtensionApplication(typeof(Lab8.adskClass))]

namespace Lab8
{

    // Beginning of Lab8 
    // To perform load-time initialization, an AutoCAD .NET application can 
    // implement a specific class to allow this. A class need only implement 
    // the IExtensionApplication .NET interface, and expose an assembly-level 
    // attribute which specifies this class as the ExtensionApplication. 
    // The class can then respond to one-time load and unload events. 
    // In this lab we will add a context menu and add a tab to the Options dialog 
    // We will also enable Drag & Drop to run a command when a label is dragged 
    // and dropped on the AutoCAD drawing window. 
    
    public class adskClass  : IExtensionApplication
    {


        // 1. Modify the asdkClass1 class to implement the IExtensionApplication interface. 
        // Add ": IExtensionApplication" to the class declaration.
        // Note: There are two required interfaces that need to be Implemented. 
        // These are the Initialize() and Terminate(). Create these functions using 
        // public void keywords.  
        // Copy and paste the functions so steps 23 and 37 are inside the Initialize
        // procedure and step 24 is inside the Terminate procedure. 
        // Also add this attribute by uncommented the next line of code. 
        // (needs to be oustide of the class) 
        // This line is not mandatory, but improves loading performances 
        [assembly: ExtensionApplication(typeof(Lab8.adskClass))]

        // 2. Declare a member variable as ContextMenuExtension. It will be instantiated 
        // in step 6 
        ContextMenuExtension ContextMenu;

        // 3. Create a private void procedure named AddContextMenu. 
        // Note: Put the Closing Curley brace after step 12. 
        private void AddContextMenu()
        {



            // 4. Declare an editor variable. Instantiate it by making it equal to the 
            // Editor property of the Application.DocumentManager.MdiActiveDocument.Editor 
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            // 5. Add a try catch block. 
            // Note: put the closing curley brace for the try below step 11.
            // Put the catch below this. The curley brace for the catch is 
            // after step 12.
            try
            {


                // 6. Make the ContextMenuExtension declared in step 2 equal 
                // to a new ContextMenuExtension 
                ContextMenu = new ContextMenuExtension();
                // 7. Make the Title property of the ContextMenuExtension 
                // instantiated in step 7 equal to "Circle Jig". (it is going 
                // to run the command completed in Lab7). 
                ContextMenu.Title = "Circle Jig";
                // 8. Declare a MenuItem variable named mi. Instantiate by 
                // making it eaual to a new MenuItem. For the string parameter 
                // use a string like "Run Circle Jig". 
                MenuItem mi = new MenuItem("Run Circl Jig");
                // 9. The way the Context menu works is that for each menu entry, we specify a 
                // specific member function to be called handling the menu click event. 
                // Use the MenuItem Click event (mi.Click += )to specify that we want the 
                // click event to be handled by a function named CallbackOnClick. You will 
                // create this function in step 20 - 22.
                mi.Click += CallbackOnClick;
                // 10. Use the Add method of the MenuItems collection of the 
                // ContextMenuExtension instantiated in step 6. Pass in the 
                // MenuItem created in step 8. 
                ContextMenu.MenuItems.Add(mi);
                // 11. Use the AddDefaultContextMenuExtension of the Application. 
                // Pass in the ContextMenuExtension 
                Application.AddDefaultContextMenuExtension(ContextMenu);
            }
            catch (Exception ex)
            {
                // 12. Use the editor variable created in step 4 and write a message. 
                // to the command line Something like" 
                // "Error Adding Context Menu: " + ex.Message 
                ed.WriteMessage("Error Adding Context Menu: " + ex.Message);
            }
            
        }

        // 13. Create a procedure named RemoveContextMenu. /Procedure == public void
        // Note: Put the closing curley brace after step 19. 
        public void RemoveContextMenu()
        {



            // 14. Declare an document variable. Instantiate it by making it 
            // equal the MdiActiveDocument 
            Document doc = Application.DocumentManager.MdiActiveDocument;
            // 15. Add a try catch block. 
            // Note: put the try closing curley brace below step 18.
            // put the catch after this. put the catch closing curley brace after
            // step 19.
            try
            {


                // 16. Use an "if" statement and test if the ContextMenuExtension 
                // declared in step 2 is not null (!=)
                // Note: put the closing curley brace below step 18 
                if (ContextMenu != null) 
                {
                    // 17. In the if statement, use RemoveDefaultContextMenuExtension 
                    // of the Application. Pass in the ContextMenuExtension declared 
                    // in step 2 
                    Application.RemoveDefaultContextMenuExtension(ContextMenu);
                    // 18. Make the ContextMenuExtension declared in step 2 equal to null
                    ContextMenu = null;
                }
            }
            catch (Exception ex)
            {



                // 19. Use the editor property of the active document and write a message. Something like 
                // "Error Removing Context Menu: " + ex.Message 
                // Use an "If" statement and test if the activeDoc declared in step 14 is null before using it.
                // In AutoCAD 2013, If AutoCAD is being closed, we may not have access to the active document in the Terminate method.
                if (doc != null)
                {
                    doc.Editor.WriteMessage("Error Removing Context Menu: " + ex.Message);
                }
            }
        }

        // 20. Create a private void function named CallbackOnClick. This is the
        // function that will be called when the Menu Item is clicked. Set in step 9. 
        // Note: Put the closing curley brace after step 22. 
        private void CallbackOnClick(object Sender, System.EventArgs e)
        {


            // 21. Use the using statement and create a variable as a DcoumentLock. 
            // Instantiate it by making it equal to the .MdiActiveDocument.LockDocument 
            // method. (Because this event is running outside of the Document context we 
            // need to lock the document). By design, AutoCAD’s data is stored in documents, 
            // where commands that access entities within them have rights to make 
            // modifications. When we run our code in response to a context-menu click, 
            // we are accessing the document from outside the command structure. 
            // In order to unlock the document we simply dispose DocumentLock object 
            // returned on the original lock request. 
            // Note: put the using closing curley brace after step 22. 
            using (DocumentLock docLock = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                // 22. Call the CircleJig() function. 
                CircleJig();
                // Added from step 25 
                //acedPostCommand("CANCELCMD");
                if (is64bits)
                {
                    acedPostCommand64("CANCELCMD");
                }
                
            }


        }
            

            public void Initialize()
        {
            // 23. Inside the Initialize procedure. (created in step 1) add 
            // a call to AddContextMenu(). This will add the Context menu when this .NET dll 
            // is loaded. 
            // Note: Proceed to step 24 below 
            AddContextMenu();

            // 37. Inside the Initialize procedure. Uncomment this line after doing 
            // steps through 24 - 27 
            // This will run this function when the dll is loaded in AutoCAD 
            AddTabDialog(); 
        }

        public void Terminate()
        {
            // 24. Inside the Terminate procedure. (created in step 1) add 
            // a call to RemoveContextMenu(). 
            // Before proceding to step 25 build and netload the dll. Right click 
            // on the ModelSpace background. You should see the new MenuItem. "Circle Jig" 
            RemoveContextMenu();

            Application.DisplayingOptionDialog -= TabHandler;
        }



        // 25. Uncomment this to fix behavior where command line 
        // is not automatically set back to normal command after running 
        // the MenuItem. 
        // using System.Runtime.InteropServices; //(move outside of the class needed for P/Invoke) 
        // acedPostCommand("CANCELCMD") ' Move this after CircleJig() in step 22 
#if ACAD2011 || ACAD2012
                            [DllImport("acad.exe", CharSet = CharSet.Unicode, EntryPoint = "?acedPostCommand@@YAHPB_W@Z")]
                            public static extern bool acedPostCommand32(string cmd);

                            [DllImport("acad.exe", CharSet = CharSet.Unicode, EntryPoint = "?acedPostCommand@@YAHPEB_W@Z")]
                            public static extern bool acedPostCommand64(string cmd);
#endif

#if ACAD2013 || ACAD2014
                            [DllImport("accore.dll", CharSet = CharSet.Unicode, EntryPoint = "?acedPostCommand@@YAHPB_W@Z")]
                            public static extern bool acedPostCommand32(string cmd);

                            [DllImport("accore.dll", CharSet = CharSet.Unicode, EntryPoint = "?acedPostCommand@@YAHPEB_W@Z")]
                            public static extern bool acedPostCommand64(string cmd);
#endif

#if ACAD2015
        [DllImport("acad.exe", CharSet = CharSet.Auto,CallingConvention = CallingConvention.Cdecl)]

        extern static private int ads_queueexpr(string strExpr);

        [DllImport("acad.exe", CharSet = CharSet.Auto,CallingConvention = CallingConvention.Cdecl,EntryPoint = "?acedPostCommand@@YAHPB_W@Z")]

        extern static private int acedPostCommand64(string strExpr);
#endif
        static public bool is64bits
        {
            get
            {
                return (Application.GetSystemVariable("PLATFORM").ToString().IndexOf("64") > 0);
            }
        }
        // 25. Uncomment this to fix behavior where command line 
        // is not automatically set back to normal command after running 
        // the MenuItem. 
        // using System.Runtime.InteropServices; //(move outside of the class needed for P/Invoke) 
        // acedPostCommand("CANCELCMD") ' Move this after CircleJig() in step 22 


        // Lab8 Steps 26 - 37. Add a tab to the Options dialog. 
        // To add a tab you need to subscribe to notifications when the options dialog 
        // is launched. This is done by passing the address of a member function to be called. 
        // You will also need to implement the callback function; the second argument 
        // passed into the callback is a "TabbedDialogEventArgs" object which we must 
        // use to call its AddTab member. AddTab takes a title string, and an instance 
        // of a TabbedDialogExtension object, which wraps our form. Within the constructor 
        // of TabbedDialogExtension, we pass a new instance of our usercontrol, and callback 
        // addresses. We can handle either OnOK, OnCancel or OnHelp. 

        // 26. First Declare a public static shared variable as a String. This variable will be set 
        // from our custom tab in the Options dialog. Name it something like "myVariable" 
        public static string myVariable = string.Empty;

        // 27. Add a publc static void procedure called AddTabDialog. We will call this procedure 
        // from the Initialize() function that was created in step 1 
        // Note: Put the closing curley brace after step 28. 
        public static void AddTabDialog()
        {

            // 28. Use the Application DisplayingOptionDialog event. Use += to specify
            // the function that will be called when the Options command is run.
            // you will create this function in step 29. (name it TabHandler) 
            Application.DisplayingOptionDialog += TabHandler;
        }

        // 29. Create a private static void function named TabHandler. This is the Sub that 
        // will be called when the Options dialog is displayed. (The name needs to be 
        // the name used in the Delegate parameter of step 28). The first parameter is an 
        // object name it something like sender. 
        // The second parameter is a TabbedDialogEventArgs.
        // Use Autodesk.AutoCAD.ApplicationServices.TabbedDialogEventArgs e
        // Note: Put the closing curley brace after step 33 
        private static void TabHandler(object sender, Autodesk.AutoCAD.ApplicationServices.TabbedDialogEventArgs e)
        {

            // 30. Declare a variable as the user control that we are going to add to 
            // the Options dialog. (myCustomTab) Use the new keyword to instantiate it. 
            myCustomTab myCustomTab = new myCustomTab();

            // 31. Delcare a variable as a TabbedDialogAction. Instantiate it using the new statement 
            // to create a new TabbedDialogAction for the parameter use the OnOk method of the
            // User control variable from step 29. We can subscribe to, Ok, 
            // Cancel and Help. In this example, we chose to handle only OK. 
            // Note: The OnOk method will be added in Steps 35 - 36. (You can skip forward to these steps 
            // in the code window for the user form - myCustomTab - come back here of course) 
            TabbedDialogAction tabbedDialogAct = new TabbedDialogAction(myCustomTab.OnOk);

            // 32. Declare a variable as a TabbedDialogExtension. Instantiate it using the New statement 
            // to create a new TabbedDialogExtension for the control parameter pass in the User control variable 
            // from step 30. and the TabbedDialogAction pass in the TabbedDialogAction variable from step 31. 
            TabbedDialogExtension tabbedDialogExt = new TabbedDialogExtension(myCustomTab, tabbedDialogAct);

            // 33. Use the AddTab method of the TabbedDialogEventArgs passed into the event. 
            // (named e). For the Name parameter use something like "Value for custom variable". For the 
            // TabbedDialogExtension parameter use the TabbedDialogExtension from step 32. 
            // Note: Proceed to step 34 in the TestTab function below. 
            e.AddTab("Value for custom variable", tabbedDialogExt);
        }

        // Run this command to get the value set from 
        [CommandMethod("testTab")]
        public void TestTab()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            // 34. Uncomment this line to print the value set from the Tab added in the previous 
            // steps to the command line. 
            // Note: You may need to change myVariable to the name you used in step 26 
            // Proceed to steps 35 - 36 in the Code window for the UserControl. (myCustomTab) 
            // if you have not already done so. Also do step 37, close to step 23 

            // After steps 35-36 are complete Build, Load and run the AutoCAD OPTIONS to see 
            // the custom dialog. Set the value in the text box click Ok and then run 
            // this command you should see the value set in the Options dialog printed 
            // on the command line. 
            // After this proceed to steps 38-48 in the code 
            // window for UserControl1. (Adds functionality for Drag & Drop)
            ed.WriteMessage(myVariable.ToString());
        }

        [CommandMethod("addAnEntity")]
        public static void AddAnEnt()
        {

            // get the editor object so we can carry out some input 
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // first decide what type of entity we want to create 
            PromptKeywordOptions getWhichEntityOptions = new PromptKeywordOptions("Which entity do you want to create? [Circle/Block] : ", "Circle Block");
            // now input it 
            PromptResult getWhichEntityResult = ed.GetKeywords(getWhichEntityOptions);
            // if ok 
            if ((getWhichEntityResult.Status == PromptStatus.OK))
            {

                // test which one is required 
                switch (getWhichEntityResult.StringResult)
                {

                    case "Circle":

                        // pick the center point of the circle 
                        PromptPointOptions getPointOptions = new PromptPointOptions("Pick Center Point : ");
                        PromptPointResult getPointResult = ed.GetPoint(getPointOptions);
                        // if ok 
                        if ((getPointResult.Status == PromptStatus.OK))
                        {

                            // the get the radius 
                            PromptDistanceOptions getRadiusOptions = new PromptDistanceOptions("Pick Radius : ");
                            // set the start point to the center (the point we just picked) 
                            getRadiusOptions.BasePoint = getPointResult.Value;
                            // now tell the input mechanism to actually use the basepoint! 
                            getRadiusOptions.UseBasePoint = true;
                            // now get the radius 
                            PromptDoubleResult getRadiusResult = ed.GetDistance(getRadiusOptions);
                            // if all is ok 
                            if ((getRadiusResult.Status == PromptStatus.OK))
                            {

                                // need to add the circle to the current space
                                //get the current working database
                                Database dwg = ed.Document.Database;

                                // now start a transaction
                                Transaction trans = dwg.TransactionManager.StartTransaction();
                                try
                                {
                                    //create a new circle
                                    Circle circle = new Circle(getPointResult.Value, Vector3d.ZAxis, getRadiusResult.Value);

                                    // open the current space (block table record) for write
                                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite);

                                    // now the circle to the current space, model space more than likely
                                    btr.AppendEntity(circle);

                                    // tell the transaction about the new circle so that it can autoclose it
                                    trans.AddNewlyCreatedDBObject(circle, true);

                                    // now commit the transaction
                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    // ok so we have an exception
                                    ed.WriteMessage("problem due to " + ex.Message);
                                }
                                finally
                                {
                                    // all done, whether an error on not - dispose the transaction.
                                    trans.Dispose();

                                }
                            }
                        }
                        break;
                    case "Block":

                        // enter the name of the block 
                        PromptStringOptions blockNameOptions = new PromptStringOptions("Enter name of the Block to create : ");
                        // no spaces are allowed in a blockname so disable it 
                        blockNameOptions.AllowSpaces = false;
                        // get the name 
                        PromptResult blockNameResult = ed.GetString(blockNameOptions);
                        // if ok 
                        if ((blockNameResult.Status == PromptStatus.OK))
                        {

                            // lets create the block definition
                            // get the current drawing
                            Database dwg = ed.Document.Database;

                            // now start a transaction
                            Transaction trans = (Transaction)dwg.TransactionManager.StartTransaction();
                            try
                            {

                                // create the new block definition
                                BlockTableRecord newBlockDef = new BlockTableRecord();

                                // name the block definition
                                newBlockDef.Name = blockNameResult.StringResult;

                                // now add the new block defintion to the block table
                                // open the blok table for read so we can check to see if the name already exists
                                BlockTable blockTable = (BlockTable)trans.GetObject(dwg.BlockTableId, OpenMode.ForRead);

                                // check to see if the block already exists
                                if ((blockTable.Has(blockNameResult.StringResult) == false))
                                {

                                    // if it's not there, then we are ok to add it
                                    // but first we need to upgrade the open to write 
                                    blockTable.UpgradeOpen();

                                    // Add the BlockTableRecord to the blockTable
                                    blockTable.Add(newBlockDef);

                                    // tell the transaction manager about the new object so that the transaction will autoclose it
                                    trans.AddNewlyCreatedDBObject(newBlockDef, true);

                                    // now add some objects to the block definition
                                    Circle circle1 = new Circle(new Point3d(0, 0, 0), Vector3d.ZAxis, 10);
                                    newBlockDef.AppendEntity(circle1);

                                    Circle circle2 = new Circle(new Point3d(20, 10, 0), Vector3d.ZAxis, 10);
                                    newBlockDef.AppendEntity(circle2);

                                    // tell the transaction manager about the new objects 
                                    //so that the transaction will autoclose it
                                    trans.AddNewlyCreatedDBObject(circle1, true);
                                    trans.AddNewlyCreatedDBObject(circle2, true);


                                    // now set where it should appear in the current space
                                    PromptPointOptions blockRefPointOptions = new PromptPointOptions("Pick insertion point of BlockRef : ");
                                    PromptPointResult blockRefPointResult = ed.GetPoint(blockRefPointOptions);

                                    // check to see if everything was ok - if not
                                    if ((blockRefPointResult.Status != PromptStatus.OK))
                                    {
                                        //dispose of everything that we have done so far and return
                                        trans.Dispose();
                                        return;
                                    }

                                    // now we have the block defintion in place and the position we need to create the reference to it
                                    BlockReference blockRef = new BlockReference(blockRefPointResult.Value, newBlockDef.ObjectId);

                                    // otherwise add it to the current space, first open the current space for write
                                    BlockTableRecord curSpace = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite);

                                    // now add the block reference to it
                                    curSpace.AppendEntity(blockRef);

                                    // remember to tell the transaction about the new block reference so that the transaction can autoclose it
                                    trans.AddNewlyCreatedDBObject(blockRef, true);

                                    // all ok, commit it
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                // a problem occured, lets print it
                                ed.WriteMessage("a problem occured because " + ex.Message);
                            }
                            finally
                            {
                                // whatever happens we must dispose the transaction
                                trans.Dispose();

                            }
                        }
                        break;
                }

            }
        }

        // declare a paletteset object, this will only be created once
        public PaletteSet myPaletteSet;

        // we need a palette which will be housed by the paletteSet
        public Lab8.UserControl1 myPalette;

        // palette command
        [CommandMethod("palette")]
        public void palette()
        {

            // check to see if it is valid
            if (myPaletteSet == null)
            {
                // create a new palette set, with a unique guid
                myPaletteSet = new PaletteSet("My Palette", new System.Guid("D61D0875-A507-4b73-8B5F-9266BEACD596"));

                // now create a palette inside, this has our tree control
                myPalette = new Lab8.UserControl1();

                // now add the palette to the paletteset
                myPaletteSet.Add("Palette1", myPalette);
            }

            // now display the paletteset
            myPaletteSet.Visible = true;
        }

        [CommandMethod("addDBEvents")]
        public void addDBEvents()
        {

            // the palette needs to be created for this
            if (myPalette == null)
            {
                //  get the editor object
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                // now write to the command line
                ed.WriteMessage("\n" + "Please call the 'palette' command first");
                return;
            }

            // get the current working database
            Database curDwg = Application.DocumentManager.MdiActiveDocument.Database;

            // add a handlers for what we need
            curDwg.ObjectAppended += new ObjectEventHandler(callback_ObjectAppended);
            curDwg.ObjectErased += new ObjectErasedEventHandler(callback_ObjectErased);
            curDwg.ObjectReappended += new ObjectEventHandler(callback_ObjectReappended);
            curDwg.ObjectUnappended += new ObjectEventHandler(callback_ObjectUnappended);
        }


        private void callback_ObjectAppended(object sender, ObjectEventArgs e)
        {

            // add the class name of the object to the tree view
            System.Windows.Forms.TreeNode newNode = myPalette.treeView1.Nodes.Add(e.DBObject.GetType().ToString());

            // we need to record its id for recognition later
            newNode.Tag = e.DBObject.ObjectId.ToString();
        }

        private void callback_ObjectErased(object sender, ObjectErasedEventArgs e)
        {

            // if the object was erased
            if (e.Erased)
            {

                // find the object in the treeview control so we can remove it
                foreach (System.Windows.Forms.TreeNode node in myPalette.treeView1.Nodes)
                {
                    // is this the one we want
                    if (node.Tag.ToString() == e.DBObject.ObjectId.ToString())
                    {
                        node.Remove();
                        break;
                    }

                }
            }
            else
            {
                // if the object was unerased
                // add the class name of the object to the tree view
                System.Windows.Forms.TreeNode newNode = myPalette.treeView1.Nodes.Add(e.DBObject.GetType().ToString());

                // we need to record its id for recognition later
                newNode.Tag = e.DBObject.ObjectId.ToString();
            }
        }

        private void callback_ObjectReappended(object sender, ObjectEventArgs e)
        {

            // add the class name of the object to the tree view
            System.Windows.Forms.TreeNode newNode = myPalette.treeView1.Nodes.Add(e.DBObject.GetType().ToString());

            // we need to record its id for recognition later
            newNode.Tag = e.DBObject.ObjectId.ToString();
        }

        private void callback_ObjectUnappended(object sender, ObjectEventArgs e)
        {

            // find the object in the treeview control so we can remove it 
            foreach (System.Windows.Forms.TreeNode node in myPalette.treeView1.Nodes)
            {
                // is this the one we want
                if (node.Tag.ToString() == e.DBObject.ObjectId.ToString())
                {
                    node.Remove();
                    break;
                }

            }
        }

        [CommandMethod("addData")]
        public void addData()
        {

            // get the editor object 
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            // pick entity to add data to! 
            PromptEntityResult getEntityResult = ed.GetEntity("Pick an entity to add an Extension Dictionary to : ");
            // if all was ok 
            if ((getEntityResult.Status == PromptStatus.OK))
            {
                // now start a transaction 
                Transaction trans = ed.Document.Database.TransactionManager.StartTransaction();
                try
                {

                    // Declare an Entity variable named ent.  
                    Entity ent = (Entity)trans.GetObject(getEntityResult.ObjectId, OpenMode.ForRead);

                    // test the IsNull property of the ExtensionDictionary of the ent. 
                    if (ent.ExtensionDictionary.IsNull)
                    {
                        // Upgrade the open of the entity. 
                        ent.UpgradeOpen();

                        // Create the ExtensionDictionary
                        ent.CreateExtensionDictionary();
                    }

                    // variable as DBDictionary. 
                    DBDictionary extensionDict = (DBDictionary)trans.GetObject(ent.ExtensionDictionary, OpenMode.ForRead);

                    if (extensionDict.Contains("MyData"))
                    {
                        // Check to see if the entry we are going to add is already there. 
                        ObjectId entryId = extensionDict.GetAt("MyData");

                        // If this line gets hit then data is already added 
                        ed.WriteMessage("\nThis entity already has data...");

                        // Extract the Xrecord. Declare an Xrecord variable. 
                        Xrecord myXrecord = default(Xrecord);

                        // Instantiate the Xrecord variable 
                        myXrecord = (Xrecord)trans.GetObject(entryId, OpenMode.ForRead);

                        // Here print out the values in the Xrecord to the command line. 
                        foreach (TypedValue value in myXrecord.Data)
                        {

                            ed.WriteMessage("\n" + value.TypeCode.ToString() + " . " + value.Value.ToString());

                        }
                    }
                    else
                    {
                        // If the code gets to here then the data entry does not exist 
                        // upgrade the ExtensionDictionary created in step 5 to write
                        extensionDict.UpgradeOpen();


                        //  Create a new XRecord. 
                        Xrecord myXrecord = new Xrecord();

                        // Create the resbuf list. 
                        ResultBuffer data = new ResultBuffer(new TypedValue((int)DxfCode.Int16, 1),
                            new TypedValue((int)DxfCode.Text, "MyStockData"),
                            new TypedValue((int)DxfCode.Real, 51.9),
                            new TypedValue((int)DxfCode.Real, 100.0),
                            new TypedValue((int)DxfCode.Real, 320.6));

                        // Add the ResultBuffer to the Xrecord 
                        myXrecord.Data = data;

                        // Create the entry in the ExtensionDictionary. 
                        extensionDict.SetAt("MyData", myXrecord);

                        // Tell the transaction about the newly created Xrecord 
                        trans.AddNewlyCreatedDBObject(myXrecord, true);

                        // Here we will populate the treeview control with the new data 
                        if (myPalette != null)
                        {

                            foreach (System.Windows.Forms.TreeNode node in myPalette.treeView1.Nodes)
                            {

                                // Test to see if the node Tag is the ObjectId 
                                // of the ent 
                                if (node.Tag.ToString() == ent.ObjectId.ToString())
                                {

                                    // Now add the new data to the treenode. 
                                    System.Windows.Forms.TreeNode childNode = node.Nodes.Add("Extension Dictionary");

                                    // Add the data. 
                                    foreach (TypedValue value in myXrecord.Data)
                                    {
                                        // Add the value from the TypedValue 
                                        childNode.Nodes.Add(value.ToString());
                                    }

                                    // Exit the for loop (all done - break out of the loop) 
                                    break;
                                }
                            }
                        }
                    }

                    // all ok, commit it 

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // a problem occured, lets print it 
                    ed.WriteMessage("a problem occured because " + ex.Message);
                }
                finally
                {
                    // whatever happens we must dispose the transaction 

                    trans.Dispose();

                }

            }
        }
        [CommandMethod("addDataToNOD")]
        public void addDataToNOD()
        {

            // get the editor object 
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            // pick entity to add data to! 
            Transaction trans = ed.Document.Database.TransactionManager.StartTransaction();
            try
            {

                //  Here we will add our data to the Named Objects Dictionary.(NOD) 
                DBDictionary nod = (DBDictionary)trans.GetObject(ed.Document.Database.NamedObjectsDictionaryId, OpenMode.ForRead);

               if (nod.Contains("MyData"))
               {

                    // Check to see if our entry is in Named Objects Dictionary. 
                    ObjectId entryId = nod.GetAt("MyData");

                    // If we are here, then the Name Object Dictionary already has our data 
                    ed.WriteMessage("\n" + "This entity already has data...");

                    // Get the the Xrecord from the NOD. 
                    Xrecord myXrecord = null;

                    // Open the Xrecord for read 
                    myXrecord = (Xrecord)trans.GetObject(entryId, OpenMode.ForRead);

                    // Print out the values of the Xrecord to the command line. 
                    foreach (TypedValue value in myXrecord.Data)
                    {
                        // Use the WriteMessage method of the editor. 
                        ed.WriteMessage("\n" + value.TypeCode.ToString() + " . " + value.Value.ToString());

                    }
                }
               else
                {
                    // Our data is not in the Named Objects Dictionary so need to add it 
                    nod.UpgradeOpen();

                    // Declare a varable as a new Xrecord. 
                    Xrecord myXrecord = new Xrecord();

                    // Create the resbuf list. 
                    ResultBuffer data = new ResultBuffer(new TypedValue((int)DxfCode.Int16, 1),
                        new TypedValue((int)DxfCode.Text, "MyCompanyDefaultSettings"),
                        new TypedValue((int)DxfCode.Real, 51.9),
                        new TypedValue((int)DxfCode.Real, 100.0),
                        new TypedValue((int)DxfCode.Real, 320.6));

                    //  Add the ResultBuffer to the Xrecord 
                    myXrecord.Data = data;

                    // Create the entry in the ExtensionDictionary. 
                    nod.SetAt("MyData", myXrecord);


                    // Tell the transaction about the newly created Xrecord 
                    trans.AddNewlyCreatedDBObject(myXrecord, true);
                }

                // all ok, commit it 
                trans.Commit();
            }
            catch (Exception ex)
            {
                // a problem occurred, lets print it 
                ed.WriteMessage("a problem occurred because " + ex.Message);
            }
            finally
            {
                // whatever happens we must dispose the transaction 
                trans.Dispose();

            }
        }



        [CommandMethod("addPointmonitor")]
        public void startMonitor()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // Connect to the PointMonitor event. 
            ed.PointMonitor += new PointMonitorEventHandler(MyPointMonitor);
        }


        // Create a Public function named MyPointMonitor. 
        public void MyPointMonitor(object sender, PointMonitorEventArgs e)
        {
            if (e.Context == null)
            {
                return;
            }

            // array of the Type FullSubentityPath type. 
            FullSubentityPath[] fullEntPath = e.Context.GetPickedEntities();


            if (fullEntPath.Length > 0)
            {
                Transaction trans = Application.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction();

                try
                {

                    Entity ent = (Entity)trans.GetObject((ObjectId)fullEntPath[0].GetObjectIds()[0], OpenMode.ForRead);

                    // Add a tooltip by using the AppendToolTipText method 
                    e.AppendToolTipText("The Entity is a " + ent.GetType().ToString());

                    // Make sure the palette has been created     
                    if (myPalette == null)
                    {
                        return;
                    }

                    // The following steps will make the text of the entry for a DBEntity
                    // in the palette created in Lab4 Bold. 
                    System.Drawing.Font fontRegular = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Regular);

                    System.Drawing.Font fontBold = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Bold);

                    // Use the SuspendLayout() method of the treeView1 created in Lab4 to 
                    // wait until after the steps below are processed. 
                    this.myPalette.treeView1.SuspendLayout();

                    // Here we will search for an object in the treeview control so the font 
                    // can be chaged to bold.
                    foreach (System.Windows.Forms.TreeNode node in myPalette.treeView1.Nodes)
                    {
                        if (((string)node.Tag == ent.ObjectId.ToString()))
                        {
                            //  If we get here then the node is the one we want.
                            if (!fontBold.Equals(node.NodeFont))
                            {
                                //  Make the NodeFont property of the node equal to the
                                //  System.Drawing.Font variable created above
                                node.NodeFont = fontBold;

                                node.Text = node.Text;
                            }
                        }
                        else
                        {
                            //  If we get here then the node is not the node we want.
                            if (!fontRegular.Equals(node.NodeFont))
                            {
                                node.NodeFont = fontRegular;
                            }
                        }
                    }

                    //  Now it's time to recalc the layout of the treeview. 
                    this.myPalette.treeView1.ResumeLayout();

                    this.myPalette.treeView1.Refresh();

                    this.myPalette.treeView1.Update();

                    // All is ok if we get here so Commit the transaction created in 
                    trans.Commit();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    //  Whatever happens we must dispose the transaction. 
                    trans.Dispose();
                }
            }
        }


        [CommandMethod("newInput")]
        public void NewInput()
        {
            // start our input point Monitor    
            // get the editor object
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // now add the delegate to the events list
            ed.PointMonitor += new PointMonitorEventHandler(MyInputMonitor);

            // Need to enable the AutoCAD input event mechanism to do a pick under the prevailing
            // pick aperture on all digitizer events, regardless of whether a point is being acquired 
            // or whether any OSNAP modes are currently active.
            ed.TurnForcedPickOn();

            // Here we are going to ask the user to pick a point. 
            PromptPointOptions getPointOptions = new PromptPointOptions("Pick A Point : ");

            PromptPointResult getPointResult = ed.GetPoint(getPointOptions);

            //  if ok
            //if (getPointResult.Status == PromptStatus.OK)
            //{
            //    //     ' do something...
            //}


            // Now remove our point monitor as we are finished With it.
            ed.PointMonitor -= new PointMonitorEventHandler(MyInputMonitor);

        }

        public void MyInputMonitor(object sender, PointMonitorEventArgs e)
        {
            if (e.Context == null)
            {
                return;
            }

            //  first lets check what is under the Cursor
            FullSubentityPath[] fullEntPath = e.Context.GetPickedEntities();
            if (fullEntPath.Length > 0)
            {
                //  start a transaction
                Transaction trans = Application.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction();
                try
                {
                    //  open the Entity for read, it must be derived from Curve
                    Curve ent = (Curve)trans.GetObject(fullEntPath[0].GetObjectIds()[0], OpenMode.ForRead);

                    //  ok, so if we are over something - then check to see if it has an extension dictionary
                    if (ent.ExtensionDictionary.IsValid)
                    {
                        // open it for read
                        DBDictionary extensionDict = (DBDictionary)trans.GetObject(ent.ExtensionDictionary, OpenMode.ForRead);

                        // find the entry
                        ObjectId entryId = extensionDict.GetAt("MyData");

                        // if we are here, then all is ok
                        // extract the xrecord
                        Xrecord myXrecord;

                        //  read it from the extension dictionary
                        myXrecord = (Xrecord)trans.GetObject(entryId, OpenMode.ForRead);

                        // We will draw temporary graphics at certain positions along the entity
                        foreach (TypedValue myTypedVal in myXrecord.Data)
                        {
                            if (myTypedVal.TypeCode == (short)DxfCode.Real)
                            {
                                //  To locate the temporary graphics along the Curve 
                                // to show the distances we need to get the point along the curve.
                                Point3d pnt = ent.GetPointAtDist((double)myTypedVal.Value);

                                //  We need to work out how many pixels are in a unit square
                                // so we can keep the temporary graphics a set size regardless of
                                // the zoom scale. 
                                Point2d pixels = e.Context.DrawContext.Viewport.GetNumPixelsInUnitSquare(pnt);

                                //  We need some constant distances. 
                                double xDist = (10 / pixels.X);

                                double yDist = (10 / pixels.Y);

                                // Draw the temporary Graphics. 
                                Circle circle = new Circle(pnt, Vector3d.ZAxis, xDist);

                                e.Context.DrawContext.Geometry.Draw(circle);

                                DBText text = new DBText();

                                // Always a good idea to set the Database defaults With things like 
                                // text, dimensions etc. 
                                text.SetDatabaseDefaults();

                                // Set the position of the text to the same point as the circle, 
                                // but offset by the radius. 
                                text.Position = (pnt + new Vector3d(xDist, yDist, 0));

                                // Use the data from the Xrecord for the text. 
                                text.TextString = myTypedVal.Value.ToString();

                                text.Height = yDist;

                                //  Use the Draw method to display the text. 
                                e.Context.DrawContext.Geometry.Draw(text);


                            }
                        }
                    }
                    //  all ok, commit it
                    trans.Commit();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    //  whatever happens we must dispose the transaction
                    trans.Dispose();
                }
            }
        }

        // create a command to invoke the EntityJig 
        [CommandMethod("circleJig")]
        public void CircleJig()
        {

            // Create a new instance of a circle we want to form with the jig 
            Circle circle = new Circle(Point3d.Origin, Vector3d.ZAxis, 10);

            // Create a new jig. 
            MyCircleJig jig = new MyCircleJig(circle);

            // Now loop for the inputs. 
            for (int i = 0; i <= 1; i++)
            {

                // Set the current input to the loop counter. ) 
                jig.CurrentInput = i;

                // Get the editor object. 
                Autodesk.AutoCAD.EditorInput.Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                // Invoke the jig. 
                PromptResult promptResult = ed.Drag(jig);

                // Make sure the Status property of the PromptResult variable is ok.
                if (promptResult.Status == PromptStatus.Cancel | promptResult.Status == PromptStatus.Error)
                {
                    // some problem occured. Return 
                    return;

                }
            }

            // once we are finished with the jig, time to add the newly formed circle to the database 
            // get the working database 
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            // now start a transaction 
            Transaction trans = dwg.TransactionManager.StartTransaction();
            try
            {

                // open the current space for write 
                BlockTableRecord currentSpace = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite);
                // add it to the current space 
                currentSpace.AppendEntity(circle);
                // tell the transaction manager about it 
                trans.AddNewlyCreatedDBObject(circle, true);

                // all ok, commit it 

                trans.Commit();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // whatever happens we must dispose the transaction 

                trans.Dispose();

            }
        }

        
    }


    // Create a Class named MyCircleJig that Inherits from EntityJig. 
    // The EntityJig class allows you to "jig" one entity at a time. 
    class MyCircleJig : EntityJig
    {

        // We need two inputs for a circle, the center and the radius. 
        private Point3d centerPoint;
        private double radius;

        // Because we are going to have 2 inputs, a center point and a radius we need 
        // to keep track of the input number. 
        private int currentInputValue;

        // We will use a Property to get and set the variable created in step 3. 
        public int CurrentInput
        {
            get { return currentInputValue; }
            set { currentInputValue = value; }
        }


        // Create the default constructor. Pass in an Entity variable named ent. 
        // Derive from the base class and also pass in the ent passed into the constructor. 
        public MyCircleJig(Entity ent)
            : base(ent)
        {

        }

        // Override the Sampler function.
        protected override Autodesk.AutoCAD.EditorInput.SamplerStatus Sampler(Autodesk.AutoCAD.EditorInput.JigPrompts prompts)
        {

            // Create a switch statement. 
            switch (currentInputValue)
            {

                // se 0 (zero) for the case. (getting center for the circle) 
                case 0:

                    Point3d oldPnt = centerPoint;

                    PromptPointResult jigPromptResult = prompts.AcquirePoint("Pick center point : ");

                    // Check the status of the PromptPointResult 
                    if (jigPromptResult.Status == PromptStatus.OK)
                    {

                        // Make the centerPoint member variable equal to the Value 
                        // property of the PromptPointResult 
                        centerPoint = jigPromptResult.Value;

                        // Check to see if the cursor has moved. 
                        if ((oldPnt.DistanceTo(centerPoint) < 0.001))
                        {
                            // If we get here then there has not been any change to the location 
                            // return SamplerStatus.NoChange 
                            return SamplerStatus.NoChange;
                        }
                    }


                    // If the code gets here than there has been a change in the location so 
                    // return SamplerStatus.OK 
                    return SamplerStatus.OK;

                // Use 1 for the case. (getting radius for the circle) 
                case 1:

                    double oldRadius = radius;
                    JigPromptDistanceOptions jigPromptDistanceOpts = new JigPromptDistanceOptions("Pick radius : ");

                    jigPromptDistanceOpts.UseBasePoint = true;

                    jigPromptDistanceOpts.BasePoint = centerPoint;

                    // Now we ready to get input. 
                    PromptDoubleResult jigPromptDblResult = prompts.AcquireDistance(jigPromptDistanceOpts);


                    //  Check the status of the PromptDoubleResult 
                    if ((jigPromptDblResult.Status == PromptStatus.OK))
                    {
                        radius = jigPromptDblResult.Value;

                        // Check to see if the radius is too small  
                        if (System.Math.Abs(radius) < 0.1)
                        {
                            // Make the Member variable radius = to 1. This is 
                            // just an arbitrary value to keep the circle from being too small 
                            radius = 1;
                        }

                        // Check to see if the cursor has moved. 
                        if ((System.Math.Abs(oldRadius - radius) < 0.001))
                        {
                            // If we get here then there has not been any change to the location 
                            // Return SamplerStatus.NoChange 
                            return SamplerStatus.NoChange;
                        }
                    }

                    // If we get here the cursor has moved. return SamplerStatus.OK 
                    return SamplerStatus.OK;
            }
            // Return SamplerSataus.NoChange. This will not ever be hit as we are returning
            // in the switch statement. (just avoiding the compile error)
            return SamplerStatus.NoChange;
        }

        // Override the Update function. 
        protected override bool Update()
        {

            // In this function (Update) for every input, we need to update the entity 
            switch (currentInputValue)
            {
                // Use 0 (zero) for the case. (Updating center for the circle) 
                case 0:

                    // The jig stores the circle as an Entity type. 
                    ((Circle)this.Entity).Center = centerPoint;

                    // break out of the switch statement
                    break;

                // Use 1 for the case. (Updating radius for the circle) 
                case 1:

                    // The jig stores the circle as an Entity type. 
                    ((Circle)this.Entity).Radius = radius;

                    // break out of the switch statement
                    break;

            }
            // Return true. 
            return true;
        }
    }

}