using System;
using System.Collections.Generic;
using DocGen.Abstract.Application.Formatter;
using DocGen.Abstract.Domain.RichText;
using DocGen.Abstract.Interface.Content;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace DocGen.Pdf.Renderer
{
    /// <summary>
    /// Handles PDF rendering of document sections 
    /// (header, body, footer, signature) via iText.
    /// 
    /// Uses SetProperty(Property.BOLD, true) etc. 
    /// to apply bold/italic/underline when Style or extension methods 
    /// like .SetBold() are unavailable.
    /// </summary>
    public class PdfDocumentRenderer
    {
        private readonly IDocumentContent _docContent;
        private readonly RichTextParser _richTextParser;

        /// <summary>
        /// Constructs the renderer with a DocumentContent and a RichTextParser instance.
        /// </summary>
        public PdfDocumentRenderer(IDocumentContent docContent, RichTextParser richTextParser)
        {
            _docContent = docContent;
            _richTextParser = richTextParser;
        }

        /// <summary>
        /// Renders the header portion into the given iText Document.
        /// (Simplified or to be filled with table logic.)
        /// </summary>
        public void RenderHeader(Document doc)
        {
            var header = _docContent.MetaHeaderContent;
            if (header == null) return;

            // Example usage; you might have table logic here.
            // doc.Add(new Paragraph("Header Example..."));
        }

        /// <summary>
        /// Renders the body sections (rich text) into the given iText Document,
        /// using SetProperty to apply bold/italic/underline from parsed elements.
        /// </summary>
        public void RenderBody(Document doc)
        {
            var body = _docContent.BodyContent;
            if (body == null) return;

            // Example: iterate each "BodySection"
            foreach (var section in body.BodySections)
            {
                // Use RichTextParser to parse "section.RichTextContent" into styled tokens
                var parsedElements = _richTextParser.ParseRichText(section.RichTextContent);

                // Create a new Paragraph for this section content
                var paragraph = new Paragraph();

                // For each parsed element, apply styles via SetProperty calls
                foreach (var pe in parsedElements)
                {
                    var txt = new Text(pe.Text);

                    if (pe.IsBold)
                    {
                        // BOLD
                        //txt.SetProperty(Property.BOLD, true);
                    }
                    if (pe.IsItalic)
                    {
                        // ITALIC
                        //txt.SetProperty(Property.ITALIC, true);
                    }
                    if (pe.IsUnderline)
                    {
                        // UNDERLINE
                        txt.SetProperty(Property.UNDERLINE, true);
                    }

                    // Add text to the paragraph
                    paragraph.Add(txt);
                }

                // Finally, add this paragraph to the document
                doc.Add(paragraph);
            }
        }

        /// <summary>
        /// Renders the footer portion into the given iText Document.
        /// (Simplified or to be filled with table logic.)
        /// </summary>
        public void RenderFooter(Document doc)
        {
            var footer = _docContent.MetaFooterContent;
            if (footer == null) return;

            // doc.Add(new Paragraph("Footer Example..."));
        }

        /// <summary>
        /// Renders the signature portion into the given iText Document.
        /// (Simplified or to be filled with table logic.)
        /// </summary>
        public void RenderSignature(Document doc)
        {
            var signature = _docContent.MetaSignatureContent;
            if (signature == null) return;

            // doc.Add(new Paragraph("Signature Example..."));
        }

        /// <summary>
        /// Renders all sections in order: Header, Body, Footer, Signature.
        /// </summary>
        public void RenderAllSections(Document doc)
        {
            RenderHeader(doc);
            RenderBody(doc);
            RenderFooter(doc);
            RenderSignature(doc);
        }
    }
}
