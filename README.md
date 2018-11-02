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

## Source
[.NET Compact Framework Team](https://blogs.msdn.microsoft.com/netcfteam/2006/04/25/net-compact-framework-v2-service-pack-1-data-grid-control-enhancements/)
