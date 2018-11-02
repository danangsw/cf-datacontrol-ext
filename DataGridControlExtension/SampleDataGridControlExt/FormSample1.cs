using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataGridColumnExtensions;
using System.Globalization;

namespace SampleDataGridControlExt
{
    public partial class FormSample1 : Form
    {
        private DataSet dataSource;
        private DataGridTableStyle DataGridTableStyle1;
        public FormSample1()
        {
            InitializeComponent();
            this.DataGridTableStyle1 = new DataGridTableStyle();
        }

        private void FormSample1_Load(object sender, EventArgs e)
        {
            CreateDataTable(100);                               // Create DataSet with some big data
            SetupTableStyles();                                 // Setup custom table styles and bind to grid
        }

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
            dataGridCustomColumn1.Width = this.dataGrid1.Width * 10 / 100;       // 40% of the grid size
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
            dataGridCustomColumn2.Width = this.dataGrid1.Width * 10 / 100;         // 15% of the grid size
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
            dataGridCustomColumn4.Alignment = HorizontalAlignment.Center;
            dataGridCustomColumn4.AlternatingBackColor = alternatingColor;

            this.DataGridTableStyle1.GridColumnStyles.Add(dataGridCustomColumn4);

            // Last Used column
            var dataGridCustomColumn5 = new DataGridCustomDateTimePickerColumn();
            dataGridCustomColumn5.Owner = this.dataGrid1;
            dataGridCustomColumn5.Format = "G";
            dataGridCustomColumn5.FormatInfo = CultureInfo.InvariantCulture;
            dataGridCustomColumn5.HeaderText = vehicle.Columns[5].ColumnName;
            dataGridCustomColumn5.MappingName = vehicle.Columns[5].ColumnName;
            dataGridCustomColumn5.NullText = "-Unknown-";
            dataGridCustomColumn5.Width = this.dataGrid1.Width * 10 / 100;        // 30% of the grid size
            dataGridCustomColumn5.Alignment = HorizontalAlignment.Left;
            dataGridCustomColumn5.AlternatingBackColor = alternatingColor;

            this.DataGridTableStyle1.GridColumnStyles.Add(dataGridCustomColumn5);

            // Price column
            var dataGridCustomColumn6 = new DataGridCustomDateTimePickerColumn();
            dataGridCustomColumn6.Owner = this.dataGrid1;
            dataGridCustomColumn6.Format = "#,#.00#";
            dataGridCustomColumn6.FormatInfo = CultureInfo.InvariantCulture;
            dataGridCustomColumn6.HeaderText = vehicle.Columns[6].ColumnName;
            dataGridCustomColumn6.MappingName = vehicle.Columns[6].ColumnName;
            dataGridCustomColumn6.NullText = "-Unknown-";
            dataGridCustomColumn6.Width = this.dataGrid1.Width * 10 / 100;        // 30% of the grid size
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

        private void CreateDataTable(int p)
        {
            int records = p;
            this.dataSource = new DataSet();                            // Initiate dataset

            DataTable fuel = new DataTable("Fuel");                     // Initiate 'Items' table - that would be list of items to choose from for ComboBox column

            fuel.Columns.Add("Level", typeof(string));                  // Add 'Level' column 

            // And some rows - that would be user's choice
            fuel.Rows.Add("-Unknown-");
            fuel.Rows.Add("Empty");
            fuel.Rows.Add("Half");
            fuel.Rows.Add("Full");
            
            this.dataSource.Tables.Add(fuel);                           // Add 'Fuel' table to DataSet

            DataTable vehicle = new DataTable("Vehicle");               // Create 'Vehicle' table - that's the one to show in the grid

            // Add some columns to show in the grid

            vehicle.Columns.Add("ID", typeof(Int32));                   // Primary key
            vehicle.Columns.Add("Make", typeof(String));                // Vehicle make
            vehicle.Columns.Add("Mileage", typeof(Int32));              // Car's Mileage
            vehicle.Columns.Add("Availability", typeof(Boolean));       //Availability
            vehicle.Columns.Add("Fuel Level", typeof(String));          // Fuel Level
            vehicle.Columns.Add("Last Used", typeof(DateTime));         //Last used date
            vehicle.Columns.Add("Price", typeof(decimal));         //Last used date


            // Insert to rows
            for (int i = 0; i <= records; i++)
			{
                object available = new object();
                string fuelLvl = "-Unknown-";

                var switchOn = i%3;
                switch (switchOn)
	            {
                    case 0:
                        available = false;
                        fuelLvl = "Half";
                        break;
                    case 1:
                        available = true;
                        fuelLvl = "Full";
                        break;
                    case 2:
                        available = DBNull.Value;
                        fuelLvl = "Empty";
                        break;
		            default:
                        break;
	            }

                vehicle.Rows.Add(
                    i+1,
                    string.Format("Make #{0}", i),
                    (i == 0) ? (1 * 1000) : (i * 1000),
                    available,
                    fuelLvl,
                    DateTime.Now,
                    (i == 0) ? (1 * 1000000M) : (i * 1000000M));
			}

            this.dataSource.Tables.Add(vehicle);            // Add to data source 
        }
    }
}