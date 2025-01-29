using DocGen.Abstract.Interface.Content;
using DocGen.Abstract.Interface.Content.Concrete;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using DocGen.Pdf.Utility;
using DocGen.Abstract.Interface.Content.Table;
using iText.Kernel.Font;
using iText.Bouncycastleconnector;

namespace DocGen.Pdf.Creator
{
    public class PdfDocumentCreator : IDocumentCreator
    {
        public void CreateDocument(IDocumentContent documentContent)
        {
            CreateDocument(documentContent, new DocumentCreateOptions());
        }

        public void CreateDocument(IDocumentContent documentContent, DocumentCreateOptions options)
        {
            // PDF çıktısını kaydetme yolu
            string outputPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "CompleteSamplePdf.pdf");

            // PdfWriter yapılandırması
            var writerProperties = new WriterProperties();

            using var pdfWriter = new PdfWriter(outputPath, writerProperties);
            using var pdfDoc = new PdfDocument(pdfWriter);

            // Sayfa yatay mı?
            if (options.IsLandscape)
            {
                pdfDoc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
            }

            using var doc = new Document(pdfDoc);

            // Arka plan resmi ekleme
            if (options.AddBackgroundImage && !string.IsNullOrEmpty(options.BackgroundImagePath))
            {
                var imgData = iText.IO.Image.ImageDataFactory.Create(options.BackgroundImagePath);
                var img = new Image(imgData).SetFixedPosition(0, 0).SetAutoScale(true);
                doc.Add(img);
            }

            // İçeriği ekleyelim
            RenderSubContent(doc, documentContent.MetaHeaderContent);
            RenderBodyContent(doc, documentContent.BodyContent);
            RenderSubContent(doc, documentContent.MetaFooterContent);
            RenderSignatureContent(doc, documentContent.MetaSignatureContent);

            // PDF kaydedilir, doc kapatılır
        }

        private void RenderSubContent(Document doc, ISubContent subContent)
        {
            if (subContent == null || subContent.TablesRows == null) return;

            // Tabloları ekle
            foreach (var row in subContent.TablesRows)
            {
                if (row.TableCells == null || row.TableCells.Count == 0) continue;

                int colCount = row.TableCells.Count;
                var table = new Table(colCount).SetWidth(UnitValue.CreatePercentValue(100));

                row.ValidateColumnWidths();
                var widths = row.ColumnWidths;
                for (int i = 0; i < colCount; i++)
                {
                    if (widths != null && widths.Count == colCount)
                    {
                        //table.SetRelativeColumnWidth(i, (float)widths[i]);
                    }
                }

                foreach (var cell in row.TableCells)
                {
                    var pdfCell = RenderPdfCell(cell);
                    table.AddCell(pdfCell);
                }
                doc.Add(table);
            }
        }

        private void RenderBodyContent(Document doc, BodyContent bodyContent)
        {
            if (bodyContent == null) return;

            // BodySections içindeki her başlık ve alt başlık
            foreach (var section in bodyContent.BodySections)
            {
                // Başlık
                var headingPara = RenderParagraph(
                    section.Title,
                    isBold: true,
                    isItalic: false,
                    isUnderline: false,
                    colorHex: "#000000",
                    fontSize: GetFontSizeByLevel(section.HierarchyLevel));

                doc.Add(headingPara);

                // TitleNotes
                if (section.TitleNotes != null)
                {
                    foreach (var note in section.TitleNotes)
                    {
                        var notePara = RenderParagraph(note, false, true, false, "#444444", 10f);
                        doc.Add(notePara);
                    }
                }

                // İçerik (RichTextContent) -> Basit yaklaşımda direkt paragraf
                var bodyPara = RenderParagraph(
                    section.RichTextContent,
                    isBold: section.FontSettings.FontBold,
                    isItalic: section.FontSettings.FontItalic,
                    isUnderline: section.FontSettings.FontUnderline,
                    colorHex: section.FontSettings.FontColor,
                    fontSize: (float)section.FontSettings.FontSize,
                    justification: section.FontSettings.Justification);
                doc.Add(bodyPara);

                // SectionNotes
                if (section.SectionNotes != null)
                {
                    foreach (var note in section.SectionNotes)
                    {
                        var notePara = RenderParagraph(note, false, true, false, "#777777", 9f);
                        doc.Add(notePara);
                    }
                }
            }
        }

        private void RenderSignatureContent(Document doc, ISubContent signatureContent)
        {
            if (signatureContent == null) return;

            // Birkaç boş satır verelim
            doc.Add(new Paragraph("\n\n"));

            // Ardından tablo formatında ekleyelim
            foreach (var row in signatureContent.TablesRows)
            {
                if (row.TableCells.Count == 0) continue;

                int colCount = row.TableCells.Count;
                var table = new Table(colCount).SetWidth(UnitValue.CreatePercentValue(100));

                row.ValidateColumnWidths();
                var widths = row.ColumnWidths;
                for (int i = 0; i < colCount; i++)
                {
                    if (widths != null && widths.Count == colCount)
                    {
                        //table.SetRelativeColumnWidth(i, (float)widths[i]);
                    }
                }

                foreach (var cell in row.TableCells)
                {
                    var pdfCell = RenderPdfCell(cell);
                    table.AddCell(pdfCell);
                }

                doc.Add(table);
            }
        }

        /// <summary>
        /// Bir tablo hücresini, <see cref="BaseTableCell"/> değerleriyle iText7'de oluşturur.
        /// </summary>
        private Cell RenderPdfCell(ITableCell cell)
        {
            if (cell is Abstract.Domain.Table.TableCellText textCell)
            {
                var cellParagraph = RenderParagraph(
                    textCell.Content,
                    textCell.FontSettings?.FontBold == true,
                    textCell.FontSettings?.FontItalic == true,
                    textCell.FontSettings?.FontUnderline == true,
                    textCell.FontSettings?.FontColor ?? "#000000",
                    (float)(textCell.FontSettings?.FontSize ?? 12),
                    textCell.FontSettings?.Justification ?? "left"
                );

                var pdfCell = new Cell()
                    .Add(cellParagraph)
                    .SetBorder(Border.NO_BORDER); // Örnek: border yok
                return pdfCell;
            }
            else
            {
                // Desteklenmeyen hücre tipi
                var pdfCell = new Cell()
                    .Add(new Paragraph("Unsupported Cell Type"))
                    .SetBorder(Border.NO_BORDER);
                return pdfCell;
            }
        }

        /// <summary>
        /// iText7 ile bir Paragraph oluştururken stil ayarlarını uygulayan yardımcı metot.
        /// </summary>
        private Paragraph RenderParagraph(
            string text,
            bool isBold,
            bool isItalic,
            bool isUnderline,
            string colorHex,
            float fontSize,
            string justification = "left")
        {
            // Font
            string baseFont = iText.IO.Font.Constants.StandardFonts.HELVETICA;
            if (isBold && isItalic) baseFont = iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLDOBLIQUE;
            else if (isBold) baseFont = iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD;
            else if (isItalic) baseFont = iText.IO.Font.Constants.StandardFonts.HELVETICA_OBLIQUE;

            var pdfFont = PdfFontFactory.CreateFont(baseFont);

            var paragraph = new Paragraph(text);
            paragraph.SetFont(pdfFont);
            paragraph.SetFontSize(fontSize);

            // Renk
            var rgbColor = PdfHelper.ConvertHexToColor(colorHex);
            paragraph.SetFontColor(rgbColor);

            // Alt çizgi
            if (isUnderline) paragraph.SetUnderline();

            // Hizalama
            switch (justification.ToLower())
            {
                case "center":
                    paragraph.SetTextAlignment(TextAlignment.CENTER);
                    break;
                case "right":
                    paragraph.SetTextAlignment(TextAlignment.RIGHT);
                    break;
                case "justified":
                    paragraph.SetTextAlignment(TextAlignment.JUSTIFIED);
                    break;
                default:
                    paragraph.SetTextAlignment(TextAlignment.LEFT);
                    break;
            }
            return paragraph;
        }

        private float GetFontSizeByLevel(int level)
        {
            return level switch
            {
                1 => 18f,
                2 => 16f,
                3 => 14f,
                _ => 12f
            };
        }
    }
}
