namespace DocGen.Abstract.Application.Helpers
{
    using System;
    using DocGen.Abstract.Interface.Providers;
    using DocGen.Abstract.Interface.Content;
    using DocGen.Abstract.Domain.Content;
    using DocGen.Abstract.Domain.Table;
    using DocGen.Abstract.Interface.FactorySettings;

    /// <summary>
    /// Example helper class that demonstrates 
    /// how to access sections via IDocumentInfoProvider,
    /// and do something useful with them.
    /// </summary>
    public class DocumentSectionHelper
    {
        private readonly IDocumentInfoProvider _docInfo;
        private readonly IFontSettingsFactory _fontSettingsFactory;

        public DocumentSectionHelper(IDocumentInfoProvider docInfoProvider, IFontSettingsFactory fontSettingsFactory)
        {
            _docInfo = docInfoProvider;
            _fontSettingsFactory = fontSettingsFactory;
        }

        /// <summary>
        /// E.g. add a row to the header with "Logo" text.
        /// </summary>
        public void DoSomethingWithHeader()
        {
            var header = _docInfo.GetHeader();
            // Genelde MetaHeaderContent veya ISubContent olabilir.
            if (header is MetaHeaderContent metaHeader)
            {
                // Yeni bir satır ekleyelim
                var newRow = new TablesRow();
                newRow.ColumnWidths = new System.Collections.Generic.List<int> { 100 }; // tek sütun, %100
                // Hücre oluştur
                var textCell = new TableCellText
                {
                    Content = "LOGO or Header Info"
                };
                newRow.TableCells.Add(textCell);

                metaHeader.TablesRows.Add(newRow);
                Console.WriteLine("Added a new row to header with 'LOGO or Header Info'.");
            }
        }

        /// <summary>
        /// E.g. add the current date/time to the footer as a separate row.
        /// </summary>
        public void DoSomethingWithFooter()
        {
            var footer = _docInfo.GetFooter();
            if (footer is MetaFooterContent metaFooter)
            {
                var newRow = new TablesRow();
                newRow.ColumnWidths = new System.Collections.Generic.List<int> { 100 };
                var textCell = new TableCellText
                {
                    Content = $"Page 1 - {DateTime.Now.ToShortDateString()}"
                };
                newRow.TableCells.Add(textCell);

                metaFooter.TablesRows.Add(newRow);
                Console.WriteLine("Added date/time info to footer.");
            }
        }

        /// <summary>
        /// E.g. read all BodySections, print them out, or add a new sub-section
        /// </summary>
        public void DoSomethingWithBody()
        {
            var body = _docInfo.GetBody();
            // Genelde BodyContent
            if (body is DocGen.Abstract.Interface.Content.Concrete.BodyContent bodyContent)
            {
                // Her bir BodySection'ı gezelim
                foreach (var section in bodyContent.BodySections)
                {
                    Console.WriteLine($"Found BodySection: '{section.Title}' [Level: {section.HierarchyLevel}]");
                }

                var newSection = new DocGen.Abstract.Interface.Content.Concrete.BodySection(
                    fontSettingsFactory: _fontSettingsFactory,
                    title: "Ek Bilgi",
                    titleNotes: new System.Collections.Generic.List<string>(),
                    sectionNotes: new System.Collections.Generic.List<string>(),
                    richTextContent: "Buraya ek paragraf bilgisi.",
                    hierarchyLevel: 2,
                    subBodySections: new System.Collections.Generic.List<DocGen.Abstract.Interface.Content.IBodySection>(),
                    numberingType: DocGen.Abstract.Constants.TitleNumberingType.Numeric
                );
                bodyContent.BodySections.Add(newSection);
                Console.WriteLine("Added a new BodySection titled 'Ek Bilgi'.");
            }
        }
    }
}
