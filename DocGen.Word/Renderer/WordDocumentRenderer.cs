using DocGen.Abstract.Interface.Content;
using DocGen.Abstract.Interface.Content.Concrete;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using DocGen.Abstract.Application.Formatter; // If needed for rich text parse
using DocGen.Abstract.Domain.RichText;
using static System.Net.Mime.MediaTypeNames;

namespace DocGen.Word.Renderer
{
    /// <summary>
    /// Handles rendering of header, body, footer, signature 
    /// into an OpenXML WordprocessingDocument.
    /// </summary>
    public class WordDocumentRenderer
    {
        private readonly IDocumentContent _docContent;
        private readonly RichTextParser _richTextParser;
        // or some IContentFormatter<ParsedElement>

        public WordDocumentRenderer(IDocumentContent docContent, RichTextParser parser)
        {
            _docContent = docContent;
            _richTextParser = parser;
        }

        public void RenderHeader(MainDocumentPart mainPart)
        {
            var header = _docContent.MetaHeaderContent;
            if (header == null) return;
            // Create or retrieve header part in Word, add tables...
        }

        public void RenderBody(MainDocumentPart mainPart)
        {
            var body = _docContent.BodyContent;
            if (body == null) return;

            // Suppose mainPart.Document.Body is your entry. 
            var docBody = mainPart.Document.Body;

            foreach (var section in body.BodySections)
            {
                var parsedElements = _richTextParser.ParseRichText(section.RichTextContent);

                // For each parsed element, create a Run / Paragraph
                var paragraph = new Paragraph();
                var run = new Run();

                foreach (var pe in parsedElements)
                {
                    var text = new DocumentFormat.OpenXml.Wordprocessing.Text(pe.Text);

                    if (pe.IsBold) run.AppendChild(new Bold());
                    if (pe.IsItalic) run.AppendChild(new Italic());
                    if (pe.IsUnderline) run.AppendChild(new Underline());

                    run.AppendChild(text);
                }
                paragraph.AppendChild(run);
                docBody.AppendChild(paragraph);
            }
        }

        public void RenderFooter(MainDocumentPart mainPart)
        {
            var footer = _docContent.MetaFooterContent;
            if (footer == null) return;
            // Similar approach for footers...
        }

        public void RenderSignature(MainDocumentPart mainPart)
        {
            var signature = _docContent.MetaSignatureContent;
            if (signature == null) return;
            // Maybe a separate signature part or just appended to docBody
        }

        public void RenderAllSections(MainDocumentPart mainPart)
        {
            RenderHeader(mainPart);
            RenderBody(mainPart);
            RenderFooter(mainPart);
            RenderSignature(mainPart);
        }
    }
}
