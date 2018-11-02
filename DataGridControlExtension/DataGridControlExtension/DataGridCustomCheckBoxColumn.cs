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


namespace DataGridColumnExtensions
{
    public class DataGridCustomCheckBoxColumn : DataGridCustomColumnBase
    {
        private object _nullValue = DBNull.Value;
        
        // Set up owr own NullValue. Let's use DBNull.Value by default.
        public override object NullValue
        {
            get { return _nullValue;  }
            set {
                if (value != _nullValue)
                {
                    _nullValue = value;
                    this.Invalidate();
                }
            }
        }

        // Let's add this so user can access 
        public virtual CheckBox CheckBox
        {
            get { return this.HostedControl as CheckBox; }
        }

        protected override string GetBoundPropertyName()
        {
            return "CheckState";                                                    // We'll bind to CheckState property.
        }

        protected override Control CreateHostedControl()
        {
            CheckBox box = new CheckBox();                                          // Create CheckBox
            
            box.ThreeState = true;                                                  // Make it show 3 states.

            return box;
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            Object cellData;                                                    // Object to show in the cell 

            DrawBackground(g, bounds, rowNum, backBrush);                       // Draw cell background

            cellData = this.PropertyDescriptor.GetValue(source.List[rowNum]);   // Get data for this cell from data source.

            if((cellData != null) && (cellData != this.NullValue) && (cellData is IConvertible))
            {                                                                   // Data is IConvertale and not NULL?
                cellData = ((IConvertible)cellData).ToBoolean(this.FormatInfo); // Go ahead and convert it to boolean
            }
            
            DrawCheckBox(g, bounds, (cellData is Boolean )? ((bool)cellData ? CheckState.Checked : CheckState.Unchecked) : CheckState.Indeterminate);
                                                                                // Draw the checkbox according to the data in data source.

            this.updateHostedControl();                                         // Have to do that.
        }
 

        private void DrawCheckBox(Graphics g, Rectangle bounds, CheckState state)
        {
            // If I Were A Painter... That would look way better. Sorry.

            int size;
            int boxTop;
                
            size = bounds.Size.Height < bounds.Size.Width ? bounds.Size.Height : bounds.Size.Width;
            size = size > ((int)g.DpiX / 7) ? ((int)g.DpiX / 7) : size;

            boxTop = bounds.Y + (bounds.Height - size) / 2;
            
            using (Pen p = new Pen(this.Owner.ForeColor)) 
            {
                g.DrawRectangle(p, bounds.X, boxTop, size, size);
            }

            if (state != CheckState.Unchecked) 
            {
                using (Pen p = new Pen(state == CheckState.Indeterminate ? SystemColors.GrayText : SystemColors.ControlText)) 
                {
                    g.DrawLine(p, bounds.X, boxTop, bounds.X + size, boxTop + size);
                    g.DrawLine(p, bounds.X, boxTop + size, bounds.X + size, boxTop);
                }
            }
        }
    }
}
