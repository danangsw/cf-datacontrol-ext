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
using DataGridColumnExtensions;

namespace DataGridColumnExtensions
{
    // We'll inherit from DataGridTextBoxColumn, not from DataGridColumnStyle to ensure desktop compatibility.
    // Since some abstract methods are not availible on NETCF's DataGridColumnStyle, it's not possible to override them.
    // Thus attempt to run this code on desktop would fail as these abstract methods won't have implementation at runtime.
    
    public class DataGridCustomColumnBase : DataGridTextBoxColumn
    {

        #region Privates

        private StringFormat _stringFormat = null;                                      // Actual string format we'll use to draw string.

        private DataGrid _owner = null;                                                 // Grid this column belongs to.
        private int _columnOrdinal = -1;                                                // Our ordinal in the grid.
        
        private Control _hostedControl = null;                                          // Column's hosted control (e.g. TextBox).
        private Rectangle _bounds = Rectangle.Empty;                                    // Last known bounds of hosted control.

        private bool _readOnly = false;                                                 // Set if column is read only.

        private Color _alternatingBackColor = SystemColors.Window;                      // Back color for odd numbered rows
        private SolidBrush _alternatingBrush = null;                                    // Brush to use for odd numbered rows
        
        #endregion

        #region Public properties

        
        // Color for odd numbered rows. Has no effect untill set.
        public Color AlternatingBackColor
        {
            get 
            {
                return _alternatingBackColor;                                   
            }
            set
            {
                if(_alternatingBackColor != value)                                      // Setting new color?
                {
                    if (this._alternatingBrush != null)                                 // See if got brush
                    {
                        this._alternatingBrush.Dispose();                               // Yes, get rid of it.
                    }
                    
                    this._alternatingBackColor = value;                                 // Set new color

                    this._alternatingBrush = new SolidBrush(value);                     // Create new brush.

                    this.Invalidate();
                }
            }
        }

        // Sets horisontal aligment in the cell. We don't have separate variable for this, we'll keep it in StringFormat instead.
        public virtual HorizontalAlignment Alignment
        {
            get 
            {
                return (this.StringFormat.Alignment == StringAlignment.Center) ? HorizontalAlignment.Center :
                       (this.StringFormat.Alignment == StringAlignment.Far) ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            }
            set
            {
                if (this.Alignment != value)                                    // New aligment?
                {
                    this.StringFormat.Alignment = (value == HorizontalAlignment.Center) ? StringAlignment.Center :
                                                  (value == HorizontalAlignment.Right) ? StringAlignment.Far : StringAlignment.Near;
                                                                                // Set it.
                    Invalidate();                                               // Aligment just changed, repaint.
                }
            }
        }
        
        // Determines if column is read only or not.
        public virtual bool ReadOnly
        {
            get
            {
                return this._readOnly;
            }
            set
            {
                if (this._readOnly != value)                                    // New value?
                {
                    this._readOnly = value;                                     // Yes, store it.
                    this.Invalidate();                                          // Update grid.
                }
            }
        }

        // Use this to set text formatting in the grid including aligment. 
        // Note: grid needs to be invalidated if format is changed.
        public StringFormat StringFormat
        {
            get
            {   
                if (null == _stringFormat)                                              // No format yet?
                {
                    _stringFormat = new StringFormat();                                 // Create one.
                    
                    this.Alignment = HorizontalAlignment.Left;                          // And set default aligment.
                }
                
                return _stringFormat;                                                   // Return our format
            }
         }

        // Gets or sets null value. This values would be shown if data in the data source is null.
        // If data in the grid is set to this value, null would be pushed to data source.
        // Our base class is text oriented, so we'll use NullText for that.
        public virtual object NullValue
        {
            get 
            { 
                return this.NullText;
            }
            set 
            { 
                this.NullText = value.ToString(); 
            }
        }

        // Returns column's ordinal in the grid.
        public int ColumnOrdinal
        {
            get
            {
                if ((_columnOrdinal == -1) && (this.Owner != null))                     // Parent is set but ordinal is not?
                {
                    foreach (DataGridTableStyle table in this.Owner.TableStyles)        // Check all tables.
                    {
                        this._columnOrdinal = table.GridColumnStyles.IndexOf(this);     // Get our index.

                        if (this._columnOrdinal != -1) break;                           // Exit if found.
                    }
                }

                return _columnOrdinal;    
            }
        }

        // Gets or sets DataGrid we're part of. Can be set only onece.
        public DataGrid Owner
        {
            get 
            {
                if (null == _owner)
                {
                    throw new InvalidOperationException("DataGrid owner of this ColumnStyle must be set prior to this operation.");
                }
                return _owner; 
            }
            set
            {
                if (null != _owner)
                {
                    throw new InvalidOperationException("DataGrid owner of this ColumnStyle is already set.");
                }

                _owner = value;
            }
        }

        // Gets hosted control.
        public Control HostedControl
        {
            get
            {
                if ((null == this._hostedControl) && (this.Owner != null))              // If not created and have owner...
                {                                                                       
                    this._hostedControl = this.CreateHostedControl();                   // Create hosted control.

                    this._hostedControl.Visible = false;                                // Hide it.
                    this._hostedControl.Name = this.HeaderText;
                    this._hostedControl.Font = this.Owner.Font;                         // Set up control's font to match grid's font.

                    this.Owner.Controls.Add(this._hostedControl);                       // Add it to grid's conrtols.

                    this._hostedControl.DataBindings.Add(this.GetBoundPropertyName(), Owner.DataSource, this.MappingName, true, DataSourceUpdateMode.OnValidation, this.NullValue);
                                                                                        // Set up data binding so contol would get data from data source.

                    // Now we need to hook into grid's horisontal scroll event so we could move hosted control as user scrolls.
                    // To do so we'll look for HScrollBar control owned by the groid. We'll grab the first one we found.

                    HScrollBar horisonal = null;                                        // Assume no ScrollBar found.
                        
                    foreach (Control c in this.Owner.Controls)                          // For each controls owned by grid...
                    {
                        if ((horisonal = c as HScrollBar) != null)                      // See if it's HScrollBar
                        {
                            horisonal.ValueChanged += new EventHandler(gridScrolled);
                                                                                        // Got it. Hook into ValueChanged event.
                            break;                                                      // We're done. Terminate.
                         }
                    }
                }


                return _hostedControl;
            }
        }


        #endregion

        #region Methids to be overriden

        // Returns name of the property on hosted control we're going to bind to, e.g. "Text" on TextBox.
        protected virtual string GetBoundPropertyName()
        {
            throw new InvalidOperationException("User must override GetBoundPropertyName()");
        }

        // Creates hosted control and sets it's properties as needed.
        protected virtual Control CreateHostedControl()
        {
            throw new InvalidOperationException("User must override CreateHostedControl()");
        }

        #endregion

        #region Protected methods

        // Would reposition, hide and show hosted control as needed.
        protected void updateHostedControl()
        {
            Rectangle selectedBounds = this.Owner.GetCellBounds(this.Owner.CurrentCell.RowNumber, this.Owner.CurrentCell.ColumnNumber);
                                                                                    // Get selected cell bounds.
            
            // We only need to show hosted control if column is not read only, 
            // selected cell is in our column and not occluded by anything.
            
            if (!this.ReadOnly && (this.ColumnOrdinal == this.Owner.CurrentCell.ColumnNumber) &&
                 this.Owner.HitTest(selectedBounds.Left, selectedBounds.Top).Type == DataGrid.HitTestType.Cell &&
                 this.Owner.HitTest(selectedBounds.Right, selectedBounds.Bottom).Type == DataGrid.HitTestType.Cell )
            {                                                                   
                    if (selectedBounds != this._bounds)                             // See if control bounds are already set.
                    {                                                               
                        this._bounds = selectedBounds;                              // Store last bounds. Note we can't use control's bounds 
                                                                                    // as some controls are not resizable and would change bounds as they pleased.
                        this.HostedControl.Bounds = selectedBounds;                 // Update control bounds.
                        
                        this.HostedControl.Focus();
                        this.HostedControl.Update();                                // And update control now so it looks better visually.
                    }

                    if (!this.HostedControl.Visible)                                // If control is not yet visible...
                    {
                        this.HostedControl.Show();                                  // Show it
                        this.HostedControl.Focus();
                    }
            } 
            else if (this.HostedControl.Visible)                                    // Hosted control should not be visible. Check if it is.
            {
                this.HostedControl.Hide();                                          // Hide it.
            }

        }
        
        // We'll override cell painting - new feature introduced in NETCF V2 SP1.
        // This implementation is sutable for all controls representing data as text. Should be overriden for others.
        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            RectangleF textBounds;                                              // Bounds of text 
            Object cellData;                                                    // Object to show in the cell 

            DrawBackground(g, bounds, rowNum, backBrush);                       // Draw cell background

            bounds.Inflate(-2, -2);                                             // Shrink cell by couple pixels for text.

            textBounds = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                                                                                // Set text bounds.
            cellData = this.PropertyDescriptor.GetValue(source.List[rowNum]);   // Get data for this cell from data source.

            g.DrawString(FormatText(cellData), this.Owner.Font, foreBrush, textBounds, this.StringFormat);
                                                                                // Render contents 
            this.updateHostedControl();                                         // Update floating hosted control.
        }

        protected virtual void DrawBackground(Graphics g, Rectangle bounds, int rowNum, Brush backBrush)
        {
            Brush background = backBrush;                                       // Use default brush by... hmm... default.
                
            if((null != background) && ((rowNum & 1) != 0) && !Owner.IsSelected(rowNum))
            {                                                                   // If have alternating brush, row is odd and not selected...
                background = _alternatingBrush;                                 // Then use alternating brush.
            }

            g.FillRectangle(background, bounds);                                // Draw cell background
        }

        #endregion

        // Converts data from data source to string according to formatting set.
        protected virtual String FormatText(Object cellData)
        {
            String cellText;                                                    // Formatted text.

            if ((null == cellData) || (DBNull.Value == cellData))               // See if data is null
            {                                                                   
                cellText = this.NullText;                                       // It's null, so set it to NullText.
            }
            else if (cellData is IFormattable)                                  // Is data IFormattable?
            {
                cellText = ((IFormattable)cellData).ToString(this.Format, this.FormatInfo);
                                                                                // Yes, format it.
            }
            else if (cellData is IConvertible)                                  // May be it's IConvertible?
            {
                cellText = ((IConvertible)cellData).ToString(this.FormatInfo);  // We'll take that, no problem.
            }
            else
            {
                cellText = cellData.ToString();                                 // At this point we'll give up and simply call ToString()
            }

            return cellText;                                                    
        }

        // Invalidates grid so changes can be reflected.
        protected void Invalidate() 
        {
            if (this.Owner != null)                                             // Got parent?
            {
                this.Owner.Invalidate();                                        // Repaint it.
            }
        }

        #region Private methods
        
        // Event handler for horizonta scrolling.
        private void gridScrolled(Object sender, EventArgs e)
        {
            updateHostedControl();                                              // We need to update hosted control so it would move as grid is scrolled.
        }
        
        #endregion
    }
}
