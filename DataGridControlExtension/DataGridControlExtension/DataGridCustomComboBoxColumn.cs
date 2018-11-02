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

namespace DataGridColumnExtensions
{
    public class DataGridCustomComboBoxColumn : DataGridCustomTextBoxColumn
    {
        protected override Control CreateHostedControl()
        {
            ComboBox box = new ComboBox();

            box.DropDownStyle = ComboBoxStyle.DropDown;

            return box;
        }
    }
}
