using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using AppWeather.Models;
using AppWeather.Services.Interfaces;

namespace AppWeather.Services.Implementations
{
    public class DocxExporter : IExporter
    {
        private const int TotalTableWidth = 5000;
        private const int BorderSize = 6;
        private const string HeadingFontSize = "32";
        private const string DefaultFontSize = "24";

        public void Export(ExportData data, string filePath)
        {
            using var doc = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
            var main = doc.AddMainDocumentPart();
            main.Document = new Document(new Body());
            var body = main.Document.Body ?? throw new InvalidOperationException("Не вдалося створити Body у DOCX-документі.");

            if (!string.IsNullOrWhiteSpace(data.Title))
                body.Append(MakeParagraph(data.Title, bold: true, size: HeadingFontSize));

            if (data.Headers.Count > 0 && data.Rows.Count > 0)
                body.Append(CreateTable(data.Headers, data.Rows));

            foreach (var value in data.AdditionalValues)
                body.Append(MakeParagraph(value));

            main.Document.Save();
        }

        private Table CreateTable(List<string> headers, List<List<string>> rows)
        {
            var table = new Table(
                new TableProperties(
                    new TableWidth { Width = TotalTableWidth.ToString(), Type = TableWidthUnitValues.Pct },
                    CreateStandardBorders()
                )
            );

            int columnCount = headers.Count;
            int columnWidth = TotalTableWidth / columnCount;

            table.Append(MakeRowWithWidths(columnWidth, headers.ToArray()));

            foreach (var row in rows)
            {
                table.Append(MakeRowWithWidths(columnWidth, row.ToArray()));
            }

            return table;
        }

        private TableBorders CreateStandardBorders() => new TableBorders(
            new TopBorder { Val = BorderValues.Single, Size = BorderSize },
            new BottomBorder { Val = BorderValues.Single, Size = BorderSize },
            new LeftBorder { Val = BorderValues.Single, Size = BorderSize },
            new RightBorder { Val = BorderValues.Single, Size = BorderSize },
            new InsideHorizontalBorder { Val = BorderValues.Single, Size = BorderSize },
            new InsideVerticalBorder { Val = BorderValues.Single, Size = BorderSize }
        );

        private Paragraph MakeParagraph(string text, bool bold = false, string size = DefaultFontSize)
        {
            var runProps = new RunProperties();
            if (bold) {
                runProps.Append(new Bold());
            }
            runProps.Append(new FontSize { Val = size });

            return new Paragraph(
                new Run(runProps, new Text(text))
            );
        }

        private TableRow MakeRowWithWidths(int colWidth, params string[] cells)
        {
            var row = new TableRow();
            foreach (var cellText in cells)
            {
                var cell = new TableCell(
                    new TableCellProperties(
                        new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = colWidth.ToString() }
                    ),
                    new Paragraph(new Run(new Text(cellText)))
                );
                row.Append(cell);
            }
            return row;
        }
    }
}
