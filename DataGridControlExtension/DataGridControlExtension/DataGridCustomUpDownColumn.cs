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
    public class DataGridCustomUpDownColumn : DataGridCustomColumnBase
    {
        Decimal _nullValue = -1;
        
        public override object NullValue
        {
            get { return _nullValue; }
            set {
                if (!(value is Decimal))
                {
                    throw new ArgumentException("Value sould be of type Decimal for this property.");
                }
                
                if ((Decimal)value != _nullValue)
                {
                    _nullValue = (Decimal)value;
                    this.Owner.Invalidate();
                }
            }
        }

        // Let's add this so user can access control
        public virtual NumericUpDown NumericUpDown
        {
            get { return this.HostedControl as NumericUpDown; }
        }

        protected override string GetBoundPropertyName()
        {
            return "Value";                                         // We'll bind to "Value" property.
        }

        protected override Control CreateHostedControl()
        {
            NumericUpDown nud = new NumericUpDown();                // Just create new control.
            nud.Minimum = -1;

            return nud;
        }
    }
}
