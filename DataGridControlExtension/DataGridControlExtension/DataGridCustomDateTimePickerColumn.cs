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
    public class DataGridCustomDateTimePickerColumn : DataGridCustomColumnBase
    {
        // Let's add this so user can access 
        public virtual DateTimePicker DateTimePicker
        {
            get { return this.HostedControl as DateTimePicker; }
        }
        
        protected override string GetBoundPropertyName()
        {
            return "Value";                                                             // Need to bind to "Value" property on DTP.
        }

        protected override Control CreateHostedControl()                            
        {
            DateTimePicker dtp = new DateTimePicker();                                  // Our hosted control is a DTP
            
            dtp.Format = DateTimePickerFormat.Short;
            
            return dtp;
        }
    }
}


