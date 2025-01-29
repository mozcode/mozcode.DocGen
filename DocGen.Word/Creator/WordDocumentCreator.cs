using System;
using System.IO;
using DocGen.Abstract.Interface.Content;
using DocGen.Abstract.Interface.Content.Concrete;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocGen.Word.Utility;
using DocumentFormat.OpenXml;
using DocGen.Abstract.Domain.Table;

namespace DocGen.Word.Creator
{
    public class WordDocumentCreator : IDocumentCreator
    {
        public void CreateDocument(IDocumentContent documentContent)
        {
            CreateDocument(documentContent, new DocumentCreateOptions());
        }

        public void CreateDocument(IDocumentContent documentContent, DocumentCreateOptions options)
        {
            // Örnek: Word çıktısını masaüstüne kaydedelim
            string outputPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "CompleteSampleWord.docx");

            using var wordDoc = WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document);
            var mainPart = wordDoc.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = new Body();

            // Sayfa yönü
            var sectionProps = new SectionProperties();
            if (options.IsLandscape)
            {
                var pageSize = new PageSize { Width = 16840, Height = 11900, Orient = PageOrientationValues.Landscape };
                sectionProps.Append(pageSize);
            }
            else
            {
                var pageSize = new PageSize { Width = 11900, Height = 16840 };
                sectionProps.Append(pageSize);
            }
            body.Append(sectionProps);

            mainPart.Document.Append(body);

            // 1) Header
            RenderSubContentAsTable(body, documentContent.MetaHeaderContent);

            // 2) Body
            RenderBodyContent(body, documentContent.BodyContent);

            // 3) Footer
            RenderSubContentAsTable(body, documentContent.MetaFooterContent);

            // 4) Signature
            RenderSignature(body, documentContent.MetaSignatureContent);

            mainPart.Document.Save();
        }

        private void RenderSubContentAsTable(Body docBody, ISubContent subContent)
        {
            if (subContent == null || subContent.TablesRows == null) return;

            foreach (var row in subContent.TablesRows)
            {
                if (row.TableCells == null || row.TableCells.Count == 0) continue;

                var table = new Table();

                // Tablo özellikleri
                var tblProps = new TableProperties(
                    new TableWidth { Type = TableWidthUnitValues.Pct, Width = "10000" }
                );
                table.Append(tblProps);

                row.ValidateColumnWidths();
                var widths = row.ColumnWidths;

                var tableRow = new TableRow();

                for (int i = 0; i < row.TableCells.Count; i++)
                {
                    var cell = row.TableCells[i];
                    var tableCell = new TableCell();

                    if (widths != null && i < widths.Count)
                    {
                        var cellProps = new TableCellProperties(
                            new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = (widths[i] * 100).ToString() }
                        );
                        tableCell.Append(cellProps);
                    }

                    if (cell is TableCellText textCell)
                    {
                        var para = new Paragraph();
                        var run = CreateStyledRun(textCell.Content, textCell.FontSettings);
                        para.Append(run);
                        tableCell.Append(para);
                    }
                    else
                    {
                        // Desteklenmeyen hücre tipi
                        var para = new Paragraph(new Run(new Text("Unsupported Cell Type")));
                        tableCell.Append(para);
                    }

                    tableRow.Append(tableCell);
                }

                table.Append(tableRow);
                docBody.Append(table);
            }
        }

        private void RenderBodyContent(Body docBody, BodyContent bodyContent)
        {
            if (bodyContent == null) return;

            // Her BodySection'u işliyoruz
            foreach (var section in bodyContent.BodySections)
            {
                // Başlık
                var headingP = new Paragraph();
                var headingRun = CreateStyledRun(section.Title, section.FontSettings,
                    overrideBold: true,
                    overrideFontSizeTwips: GetTitleFontSizeTwips(section.HierarchyLevel));
                headingP.Append(headingRun);
                docBody.Append(headingP);

                // TitleNotes
                if (section.TitleNotes != null)
                {
                    foreach (var note in section.TitleNotes)
                    {
                        var noteP = new Paragraph();
                        var noteRun = CreateStyledRun(note, section.FontSettings, overrideItalic: true, overrideFontSizeTwips: 20);
                        noteP.Append(noteRun);
                        docBody.Append(noteP);
                    }
                }

                // İçerik (RichTextContent)
                var contentP = new Paragraph();
                var contentRun = CreateStyledRun(section.RichTextContent, section.FontSettings);
                contentP.Append(contentRun);
                docBody.Append(contentP);

                // SectionNotes
                if (section.SectionNotes != null)
                {
                    foreach (var note in section.SectionNotes)
                    {
                        var noteP = new Paragraph();
                        var noteRun = CreateStyledRun(note, section.FontSettings, overrideItalic: true, overrideFontSizeTwips: 18);
                        noteP.Append(noteRun);
                        docBody.Append(noteP);
                    }
                }
            }
        }

        private void RenderSignature(Body docBody, ISubContent signatureContent)
        {
            if (signatureContent == null || signatureContent.TablesRows == null) return;

            // Birkaç satır boşluk
            docBody.Append(new Paragraph(new Run(new Text("\n\n"))));

            // Tablo formatında ekleyelim
            foreach (var row in signatureContent.TablesRows)
            {
                if (row.TableCells.Count == 0) continue;

                var table = new Table();
                var tblProps = new TableProperties(
                    new TableWidth { Type = TableWidthUnitValues.Pct, Width = "10000" }
                );
                table.Append(tblProps);

                row.ValidateColumnWidths();
                var widths = row.ColumnWidths;

                var tableRow = new TableRow();
                for (int i = 0; i < row.TableCells.Count; i++)
                {
                    var cell = row.TableCells[i];
                    var tableCell = new TableCell();

                    if (widths != null && i < widths.Count)
                    {
                        var cellProps = new TableCellProperties(
                            new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = (widths[i] * 100).ToString() }
                        );
                        tableCell.Append(cellProps);
                    }

                    if (cell is TableCellText textCell)
                    {
                        var para = new Paragraph();
                        var run = CreateStyledRun(textCell.Content, textCell.FontSettings);
                        para.Append(run);
                        tableCell.Append(para);
                    }
                    else
                    {
                        var para = new Paragraph(new Run(new Text("Unsupported Cell Type")));
                        tableCell.Append(para);
                    }

                    tableRow.Append(tableCell);
                }
                table.Append(tableRow);
                docBody.Append(table);
            }
        }

        /// <summary>
        /// Word'de run oluşturup font özelliklerini uygulama
        /// </summary>
        private Run CreateStyledRun(
            string text,
            Abstract.Interface.Settings.IFontSettings? fontSettings,
            bool? overrideBold = null,
            bool? overrideItalic = null,
            bool? overrideUnderline = null,
            int? overrideFontSizeTwips = null)
        {
            var run = new Run(new Text(text ?? ""));

            // Varsayılan ayar
            bool isBold = fontSettings?.FontBold ?? false;
            bool isItalic = fontSettings?.FontItalic ?? false;
            bool isUnderline = fontSettings?.FontUnderline ?? false;
            double fontSizePt = fontSettings?.FontSize ?? 12.0;
            string colorHex = fontSettings?.FontColor ?? "#000000";

            // Override’lar (Örneğin title'a özel boyut vb.)
            if (overrideBold.HasValue) isBold = overrideBold.Value;
            if (overrideItalic.HasValue) isItalic = overrideItalic.Value;
            if (overrideUnderline.HasValue) isUnderline = overrideUnderline.Value;

            int fontSizeTwips = (int)(fontSizePt * 2);
            if (overrideFontSizeTwips.HasValue) fontSizeTwips = overrideFontSizeTwips.Value;

            // RunProperties
            var rPr = new RunProperties();

            if (isBold) rPr.Append(new Bold());
            if (isItalic) rPr.Append(new Italic());
            if (isUnderline) rPr.Append(new Underline { Val = UnderlineValues.Single });

            // Renk 
            var normalizedColor = WordHelper.NormalizeHexColor(colorHex);
            rPr.Color = new Color { Val = normalizedColor };

            // Font size (Word, half-points)
            rPr.FontSize = new FontSize { Val = fontSizeTwips.ToString() };

            // Justification paragraf düzeyinde yapılır; 
            // run düzeyinde yok. Örneğin her paragraf eklerken paragraf'ın 
            // ParagraphProperties → Justification eklemeniz lazım.

            run.RunProperties = rPr;
            return run;
        }

        /// <summary>
        /// Başlık seviyesine göre pt * 2 = Twips
        /// Seviye1 => 16pt => "32"
        /// </summary>
        private int GetTitleFontSizeTwips(int level)
        {
            return level switch
            {
                1 => 32, // 16pt
                2 => 28, // 14pt
                3 => 24, // 12pt
                _ => 24
            };
        }
    }
}
