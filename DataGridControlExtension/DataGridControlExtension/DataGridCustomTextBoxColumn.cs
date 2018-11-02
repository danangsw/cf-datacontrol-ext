//--------------------------------------------------------------------- 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY 
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
//PARTICULAR PURPOSE. 
//---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace DataGridColumnExtensions
{
    // This is our editable TextBox column.
    public class DataGridCustomTextBoxColumn : DataGridCustomColumnBase
    {
        // Let's add this so user can access 
        public virtual TextBox TextBox
        {
            get { return this.HostedControl as TextBox; }
        }
        
        protected override string GetBoundPropertyName()
        {
            return "Text";                                                          // Need to bount to "Text" property on TextBox
        }

        protected override Control CreateHostedControl()                            
        {
            TextBox box = new TextBox();                                            // Our hosted control is a TextBox
           
            box.BorderStyle = BorderStyle.None;                                     // It has no border
            box.Multiline = true;                                                   // And it's multiline
            box.TextAlign = this.Alignment;                                         // Set up aligment.

            return box;
        }
    }
}
