using System;
using System.IO;
using DocGen.Abstract.Interface.Content;
using DocGen.Abstract.Interface.Content.Concrete;
using DocGen.Abstract.Domain.Content;
using DocGen.Abstract.Domain.Table;
using DocGen.Abstract.Application.Formatter;
using DocGen.Abstract.Interface.Providers;
using DocGen.Abstract.Interface.FactorySettings;
using DocGen.Abstract.Settings.Concrete;
using DocGen.Pdf.Creator;   // PdfDocumentCreator
using DocGen.Word.Creator;  // WordDocumentCreator
using DocGen.Abstract.Application.Providers;
using DocGen.Abstract.Application.Helpers;
using DocGen.Abstract.Interface.Settings;

namespace DocGen.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1) RichText metnini dışarıdan okuyalım (örnek: input.txt)
            string inputFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "input.txt");

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            string richText = File.ReadAllText(inputFilePath);

            // 2) RichText parse: basit bir örnek, 
            //    Abstract katmandaki RichTextParser'ı kullanıyoruz
            var parser = new RichTextParser();
            var parsedElements = parser.ParseRichText(richText);
            // Bu parsedElements => bold/italic/underline işaretli metin parçaları

            // 3) DocumentContent oluşturalım. 
            //    (BodyContent, MetaHeaderContent, MetaFooterContent, MetaSignatureContent + default font settings)
            var defaultFontSettings = new FontSettings
            {
                FontColor = "#000000",
                FontSize = 12.0,
                FontBold = false,
                FontItalic = false,
                FontUnderline = false,
                Justification = "left"
            };

            // Header (basit bir tablo) - Örnek
            var headerTableRow = new TablesRow
            {
                TableCells = new System.Collections.Generic.List<DocGen.Abstract.Interface.Content.Table.ITableCell>
                {
                    new TableCellText { Content = "Company Header" }
                },
                ColumnWidths = new System.Collections.Generic.List<int> { 100 }
            };
            var metaHeader = new MetaHeaderContent(
                new System.Collections.Generic.List<TablesRow> { headerTableRow },
                defaultFontSettings);

            // Footer (basit bir tablo) - Örnek
            var footerTableRow = new TablesRow
            {
                TableCells = new System.Collections.Generic.List<DocGen.Abstract.Interface.Content.Table.ITableCell>
                {
                    new TableCellText { Content = "Page 1 of 1" }
                },
                ColumnWidths = new System.Collections.Generic.List<int> { 100 }
            };
            var metaFooter = new MetaFooterContent(
                new System.Collections.Generic.List<TablesRow> { footerTableRow },
                defaultFontSettings);

            // Signature (boş tablo)
            var metaSignature = new MetaSignatureContent(
                new System.Collections.Generic.List<TablesRow>(),
                defaultFontSettings);

            // BodyContent 
            var bodySections = new System.Collections.Generic.List<IBodySection>();
            // Tek bir örnek BodySection oluşturalım: 
            // Parse ettiğimiz richText parçalarını bu BodySection'ın RichTextContent'ine koyuyoruz 
            // (basitçe birleştirerek).
            string joinedText = string.Join("", parsedElements.ConvertAll(e => e.Text));
            // Yukarıda, stil bilgileri BodySection'a verilebilir. 
            // Basitlik adına "stilleri" tek parça metin olarak koyduk.
            var bodySection = new BodySection(
                fontSettingsFactory: new NullFontSettingsFactory(),  // Örnek fabrika
                title: "Girilen İçerik",
                titleNotes: new System.Collections.Generic.List<string>(),
                sectionNotes: new System.Collections.Generic.List<string>(),
                richTextContent: joinedText,
                hierarchyLevel: 1,
                subBodySections: new System.Collections.Generic.List<IBodySection>(),
                numberingType: DocGen.Abstract.Constants.TitleNumberingType.Numeric
            );
            bodySections.Add(bodySection);

            var bodyContent = new BodyContent
            {
                BodySections = bodySections
            };
            // BodyContent'in de default ayarları var. Gerekirse set edebiliriz.

            // Tüm DocumentContent
            var documentContent = new DocumentContent(
                bodyContent: bodyContent,
                metaHeaderContent: metaHeader,
                metaFooterContent: metaFooter,
                metaSignatureContent: metaSignature,
                defaultFontSettings: defaultFontSettings
            );

            // 4) DocumentSectionHelper kullanarak header/footer/body üzerinde bir şeyler yapalım
            var docInfoProvider = new DocumentContentInfoProvider(documentContent);
            var fontSettingsFactory = new NullFontSettingsFactory();
            var helper = new DocumentSectionHelper(docInfoProvider, fontSettingsFactory);

            helper.DoSomethingWithHeader();
            helper.DoSomethingWithFooter();
            helper.DoSomethingWithBody();

            // 5) PDF ve Word oluşturma
            //    - PdfDocumentCreator
            //    - WordDocumentCreator
            var pdfCreator = new PdfDocumentCreator();
            var wordCreator = new WordDocumentCreator();

            // Seçenekler: mesela landscape istemiyoruz, background image yok
            var createOptions = new DocGen.Abstract.Interface.Content.DocumentCreateOptions
            {
                AddBackgroundImage = false,
                BackgroundImagePath = null,
                IsLandscape = false
            };

            // Oluştur
            pdfCreator.CreateDocument(documentContent, createOptions);
            wordCreator.CreateDocument(documentContent, createOptions);

            Console.WriteLine("PDF ve Word dökümanları oluşturuldu.");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Örnek bir 'Null' FontSettingsFactory
    /// Kod çalışsın diye basitlik adına eklendi.
    /// Gerçekte PdfFontSettingsFactory vb. kullanabilirsiniz.
    /// </summary>
    public class NullFontSettingsFactory : IFontSettingsFactory
    {
        public IFontSettings CreateFontSettings()
        {
            return new FontSettings
            {
                FontColor = "#000000",
                FontSize = 12.0,
                FontBold = false,
                FontItalic = false,
                FontUnderline = false,
                Justification = "left"
            };
        }
    }
}
