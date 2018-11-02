# .Net Compact Framework 3.5 SP 1 Data Grid control enhancements
> .Net Compact Framework 3.5 SP 1 Data Grid control enhancements.

Data Grid control available on Compact Framework is extensively used to represent various data. It is fast as only visible rows are retrieved from data source and relatively easy to use.

However, Compact Framework version of the Data Grid is not as extensible as its desktop counterpart and lacking some key features developers needed to create modern application.

Among most requested features is the ability to format data displayed by Data Grid, check box column availability and inability to use different colors for odd and even rows (even though very complex solution for last one is available here).

Compact framework V2 Service Pack 1 introduces minor, but very useful extensions to address these issues either directly (e.g. data formatting) or by allowing customer to perform custom cell drawings to achieve desired results.

## Methods & Properties

Let's take a look at these additions:

| Class Name        | Method or Property           | Functionality  |
| ----- |-------------| ------------- |
| DataGridColumnStyle      | public virtual PropertyDescriptor PropertyDescriptor { set; get; } | Property descriptor allows access to this column data in the data source so it could be processed as needed. |
| DataGridColumnStyle      | protected internal virtual void Paint(Graphics g,Rectangle bounds,CurrencyManager source,int rowNum,Brush backBrush,Brush foreBrush,bool alignToRight);      |   Protected Paint() method is called as cell is drawn. Overriding it allows representing data from data source in any way imaginable. Colors, fonts, formatting, alignment, pictures and more - anything is possible as long as it can be painted. CurrencyManager and row number along with PropertyDescriptor allows fetching data from data source to convert and format the way you want. |
| DataGridTextBoxColumn | public string Format { set; get; }      |   Format string to be used to format data in the column. To be used the same way as on desktop or in simple data binding. |
| DataGridTextBoxColumn | public IFormatProvider FormatInfo { set; get; } | Format provider to be used to format data in the column. To be used the same way as on desktop or in simple data binding. |

Format related properties are very easy to use, they can be set via designers the same way it's done on the desktop.

Overriding Paint() is not hard as well, but requires some coding. To illustrate how that can be done, I've created small sample which shows sample data in several custom columns.

## Code sample

See on the code sample below:

```C#
private void SetupTableStyles()
        {
            var alternatingColor = System.Drawing.Color.LightGray;
            DataTable vehicle = this.dataSource.Tables[1];

            // ID Column 
            var dataGridCustomColumn0  = new DataGridCustomTextBoxColumn();
            dataGridCustomColumn0.Owner = this.dataGrid1;
            dataGridCustomColumn0.Format = "0##";
            dataGridCustomColumn0.FormatInfo = CultureInfo.InvariantCulture;
            dataGridCustomColumn0.HeaderText = vehicle.Columns[0].ColumnName;
            dataGridCustomColumn0.MappingName = vehicle.Columns[0].ColumnName;
            dataGridCustomColumn0.Width = this.dataGrid1.Width * 10 / 100;       // 10% of the grid size
            dataGridCustomColumn0.AlternatingBackColor = alternatingColor;
            dataGridCustomColumn0.ReadOnly = true;

            this.DataGridTableStyle1.GridColumnStyles.Add(dataGridCustomColumn0);

            // Make column
            var dataGridCustomColumn1 = new DataGridCustomTextBoxColumn();
            dataGridCustomColumn1.Owner = this.dataGrid1;
            dataGridCustomColumn1.HeaderText = vehicle.Columns[1].ColumnName;
            dataGridCustomColumn1.MappingName = vehicle.Columns[1].ColumnName;
            dataGridCustomColumn1.NullText = "-Unknown-";
            dataGridCustomColumn1.Width = this.dataGrid1.Width * 10 / 100;       // 10% of the grid size
            dataGridCustomColumn1.Alignment = HorizontalAlignment.Center;
            dataGridCustomColumn1.AlternatingBackColor = alternatingColor;

            this.DataGridTableStyle1.GridColumnStyles.Add(dataGridCustomColumn1);

            // Mileage column
            var dataGridCustomColumn2 = new DataGridCustomUpDownColumn();
            dataGridCustomColumn2.Owner = this.dataGrid1;
            dataGridCustomColumn2.Format = "N2";
            dataGridCustomColumn2.FormatInfo = CultureInfo.InvariantCulture;
            dataGridCustomColumn2.HeaderText = vehicle.Columns[2].ColumnName;
            dataGridCustomColumn2.MappingName = vehicle.Columns[2].ColumnName;
            dataGridCustomColumn2.NullText = "-Unknown-";
            dataGridCustomColumn2.Width = this.dataGrid1.Width * 10 / 100;         // 10% of the grid size
            dataGridCustomColumn2.Alignment = HorizontalAlignment.Right;
            dataGridCustomColumn2.AlternatingBackColor = alternatingColor;
            dataGridCustomColumn2.ReadOnly = true;

            this.DataGridTableStyle1.GridColumnStyles.Add(dataGridCustomColumn2);

            // Availability column
            var dataGridCustomColumn3 = new DataGridCustomCheckBoxColumn();
            dataGridCustomColumn3.Owner = this.dataGrid1;
            dataGridCustomColumn3.HeaderText = vehicle.Columns[3].ColumnName;
            dataGridCustomColumn3.MappingName = vehicle.Columns[3].ColumnName;
            dataGridCustomColumn3.NullText = "-";
            dataGridCustomColumn3.Width = this.dataGrid1.Width * 10 / 100;        // 10% of the grid size
            dataGridCustomColumn3.Alignment = HorizontalAlignment.Center;
            dataGridCustomColumn3.AlternatingBackColor = alternatingColor;

            this.DataGridTableStyle1.GridColumnStyles.Add(dataGridCustomColumn3);

            // Fuel Level column
            var dataGridCustomColumn4 = new DataGridCustomComboBoxColumn();
            dataGridCustomColumn4.Owner =this.dataGrid1;
            dataGridCustomColumn4.HeaderText = vehicle.Columns[4].ColumnName;
            dataGridCustomColumn4.MappingName = vehicle.Columns[4].ColumnName;
            dataGridCustomColumn4.NullText = "-Unknown-";
            dataGridCustomColumn4.Width = this.dataGrid1.Width * 10 / 100;
            dataGridCustomColumn4.AlternatingBackColor = alternatingColor;

            this.DataGridTableStyle1.GridColumnStyles.Add(dataGridCustomColumn4);

            // Last Used column
            var dataGridCustomColumn5 = new DataGridCustomDateTimePickerColumn();
            dataGridCustomColumn5.Owner = this.dataGrid1;
            dataGridCustomColumn5.HeaderText = vehicle.Columns[5].ColumnName;
            dataGridCustomColumn5.MappingName = vehicle.Columns[5].ColumnName;
            dataGridCustomColumn5.NullText = "-Unknown-";
            dataGridCustomColumn5.Width = this.dataGrid1.Width * 10 / 100;        // 10% of the grid size
            dataGridCustomColumn5.Alignment = HorizontalAlignment.Left;
            dataGridCustomColumn5.AlternatingBackColor = alternatingColor;

            // Price column
            var dataGridCustomColumn6 = new DataGridCustomDateTimePickerColumn();
            dataGridCustomColumn6.Owner = this.dataGrid1;
            dataGridCustomColumn6.Format = "#,#.00#";
            dataGridCustomColumn6.FormatInfo = CultureInfo.InvariantCulture;
            dataGridCustomColumn6.HeaderText = vehicle.Columns[6].ColumnName;
            dataGridCustomColumn6.MappingName = vehicle.Columns[6].ColumnName;
            dataGridCustomColumn6.NullText = "-Unknown-";
            dataGridCustomColumn6.Width = this.dataGrid1.Width * 10 / 100;        // 10% of the grid size
            dataGridCustomColumn6.Alignment = HorizontalAlignment.Right;
            dataGridCustomColumn6.AlternatingBackColor = alternatingColor;
            dataGridCustomColumn6.ReadOnly = true;

            this.DataGridTableStyle1.GridColumnStyles.Add(dataGridCustomColumn6);
         
            // Set grid
            this.DataGridTableStyle1.MappingName = vehicle.TableName;             // Setup table mapping name
            this.dataGrid1.DataSource = vehicle;                                  // Setup grid's data source
            this.dataGrid1.TableStyles.Add(this.DataGridTableStyle1);             // Add table style of datagrid
            
            ComboBox cb = (ComboBox) dataGridCustomColumn4.HostedControl;

            DataTable fuel = this.dataSource.Tables[0];                           // Set up data source

            cb.DataSource = fuel;                                                 // For combo box column
            cb.DisplayMember = fuel.Columns[0].ColumnName;
            cb.ValueMember = fuel.Columns[0].ColumnName;

            this.dataGrid1.CurrentRowIndex = 0;                                    // Move to the middle of the table
        }
```

## Sample screenshot
sample 1
![](sample1.png)

sample 2
![](sample2.png)

## Credit
[.NET Compact Framework Team](https://blogs.msdn.microsoft.com/netcfteam/2006/04/25/net-compact-framework-v2-service-pack-1-data-grid-control-enhancements/)

## System Requirement
1. .Net Compact Framework 3.5 Service Pack 1
2. Windows Mobile 5.0 or later
3. Microsoft Visual Studio 2008
