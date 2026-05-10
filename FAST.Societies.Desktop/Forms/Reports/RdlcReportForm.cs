using System.Data;
using System.Text;
using FAST.Societies.Desktop.BLL;
using Microsoft.Reporting.WinForms;

namespace FAST.Societies.Desktop.Forms.Reports;

public partial class RdlcReportForm : Form
{
    private readonly string _reportType;
    private readonly ReportViewer _viewer = new() { Dock = DockStyle.Fill };

    public RdlcReportForm(string reportType)
    {
        _reportType = reportType;
        InitializeComponent();
        Load += (_, _) => BuildReport();
    }

    private void InitializeComponent()
    {
        Text = $"{_reportType} Report - FAST Societies Management System";
        Width = 1200;
        Height = 750;
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 9);
        Controls.Add(_viewer);
    }

    private void BuildReport()
    {
        try
        {
            var table = GetDataTable();
            if (table.Rows.Count == 0)
            {
                MessageBox.Show("No data available for this report.", "Empty Report");
                return;
            }

            var dsName = "DataSet1";
            var rdl = BuildRdlcDefinition(table, dsName, _reportType);

            _viewer.ProcessingMode = ProcessingMode.Local;
            _viewer.LocalReport.DataSources.Clear();
            
            using (var reader = new StringReader(rdl))
            {
                _viewer.LocalReport.LoadReportDefinition(reader);
            }
            
            _viewer.LocalReport.DataSources.Add(new ReportDataSource(dsName, table));
            _viewer.RefreshReport();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating report: {ex.Message}\n\n{ex.InnerException?.Message}", "Report Error");
        }
    }

    private DataTable GetDataTable()
    {
        try
        {
            var dt = new DataTable("ReportTable");
            if (_reportType == "Students")
            {
                dt.Columns.Add("StudentId", typeof(int));
                dt.Columns.Add("FullName", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                var rows = new StudentBLL().GetAll();
                if (rows != null)
                {
                    foreach (var r in rows) dt.Rows.Add(r.StudentId, r.FullName ?? "", r.Email ?? "");
                }
                return dt;
            }

            if (_reportType == "Societies")
            {
                dt.Columns.Add("SocietyId", typeof(int));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Category", typeof(string));
                dt.Columns.Add("IsActive", typeof(bool));
                var rows = new SocietyBLL().GetAll();
                if (rows != null)
                {
                    foreach (var r in rows) dt.Rows.Add(r.SocietyId, r.Name ?? "", r.Category ?? "", r.IsActive);
                }
                return dt;
            }

            dt.Columns.Add("EventId", typeof(int));
            dt.Columns.Add("SocietyId", typeof(int));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("EventDate", typeof(DateTime));
            dt.Columns.Add("Status", typeof(string));
            var events = new EventBLL().GetUpcoming();
            if (events != null)
            {
                foreach (var e in events) dt.Rows.Add(e.EventId, e.SocietyId, e.Title ?? "", e.EventDate, e.Status ?? "");
            }
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error loading data: {ex.Message}", ex);
        }
    }

    private static string BuildRdlcDefinition(DataTable table, string dataSetName, string title)
    {
        try
        {
            if (table == null || table.Columns.Count == 0)
            {
                throw new Exception("Invalid data table for report generation.");
            }

            var fields = new StringBuilder();
            foreach (DataColumn c in table.Columns)
            {
                fields.Append($@"<Field Name=""{c.ColumnName}""><DataField>{c.ColumnName}</DataField><rd:TypeName>{GetTypeName(c.DataType)}</rd:TypeName></Field>");
            }

            var headerCells = new StringBuilder();
            var detailCells = new StringBuilder();
            var widths = new StringBuilder();
            var colWidth = Math.Max(0.5, 6.5 / Math.Max(1, table.Columns.Count));
            foreach (DataColumn c in table.Columns)
            {
                var displayName = c.ColumnName.Replace("_", " ");
                headerCells.Append($@"<TablixCell><CellContents><Textbox Name=""H_{c.ColumnName}""><CanGrow>true</CanGrow><Paragraphs><Paragraph><TextRuns><TextRun><Value>{displayName}</Value><Style><FontWeight>Bold</FontWeight><FontSize>10pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs><rd:DefaultName>H_{c.ColumnName}</rd:DefaultName><Style><Border><Color>DarkGray</Color><Style>Solid</Style></Border><BackgroundColor>LightGray</BackgroundColor><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>");
                detailCells.Append($@"<TablixCell><CellContents><Textbox Name=""D_{c.ColumnName}""><CanGrow>true</CanGrow><Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!{c.ColumnName}.Value</Value><Style><FontSize>9pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs><rd:DefaultName>D_{c.ColumnName}</rd:DefaultName><Style><Border><Color>Gainsboro</Color><Style>Solid</Style></Border><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>");
                widths.Append($@"<TablixColumn><Width>{colWidth:0.00}in</Width></TablixColumn>");
            }

            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<Report xmlns=""http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition"" xmlns:rd=""http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"">
  <AutoRefresh>0</AutoRefresh>
  <DataSources>
    <DataSource Name=""DataSource1"">
      <ConnectionProperties><DataProvider>System.Data.DataSet</DataProvider><ConnectString>/* Local Connection */</ConnectString></ConnectionProperties>
      <rd:DataSourceID>9c5c2ec0-0000-0000-0000-000000000001</rd:DataSourceID>
    </DataSource>
  </DataSources>
  <DataSets>
    <DataSet Name=""{dataSetName}"">
      <Fields>{fields}</Fields>
      <Query><DataSourceName>DataSource1</DataSourceName><CommandText>/* Local Query */</CommandText></Query>
    </DataSet>
  </DataSets>
  <Body>
    <ReportItems>
      <Textbox Name=""ReportTitle""><CanGrow>true</CanGrow><Paragraphs><Paragraph><TextRuns><TextRun><Value>{title} Report - FAST Societies Management System</Value><Style><FontSize>14pt</FontSize><FontWeight>Bold</FontWeight><Color>Navy</Color></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs><rd:DefaultName>ReportTitle</rd:DefaultName><Top>0in</Top><Left>0in</Left><Height>0.35in</Height><Width>6.5in</Width><Style /></Textbox>
      <Tablix Name=""Tablix1"">
        <TablixBody>
          <TablixColumns>{widths}</TablixColumns>
          <TablixRows>
            <TablixRow><Height>0.25in</Height><TablixCells>{headerCells}</TablixCells></TablixRow>
            <TablixRow><Height>0.22in</Height><TablixCells>{detailCells}</TablixCells></TablixRow>
          </TablixRows>
        </TablixBody>
        <TablixColumnHierarchy><TablixMembers>{string.Concat(Enumerable.Repeat("<TablixMember />", table.Columns.Count))}</TablixMembers></TablixColumnHierarchy>
        <TablixRowHierarchy><TablixMembers><TablixMember /><TablixMember><Group Name=""DetailGroup"" /></TablixMember></TablixMembers></TablixRowHierarchy>
        <DataSetName>{dataSetName}</DataSetName>
        <Top>0.5in</Top><Left>0in</Left><Height>0.47in</Height><Width>6.5in</Width><Style />
      </Tablix>
    </ReportItems>
    <Height>2.5in</Height>
    <Style />
  </Body>
  <Width>6.8in</Width>
  <Page>
    <PageHeight>11in</PageHeight><PageWidth>8.5in</PageWidth>
    <LeftMargin>0.5in</LeftMargin><RightMargin>0.5in</RightMargin><TopMargin>0.5in</TopMargin><BottomMargin>0.5in</BottomMargin>
    <Style />
  </Page>
  <rd:ReportUnitType>Inch</rd:ReportUnitType>
  <rd:ReportID>9c5c2ec0-0000-0000-0000-000000000002</rd:ReportID>
</Report>";
        }
        catch (Exception ex)
        {
            throw new Exception($"Error building report definition: {ex.Message}", ex);
        }
    }

    private static string GetTypeName(Type t)
    {
        if (t == typeof(int)) return "System.Int32";
        if (t == typeof(DateTime)) return "System.DateTime";
        if (t == typeof(bool)) return "System.Boolean";
        return "System.String";
    }
}
