using DocGen.Abstract.Interface.Settings;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace DocGen.Pdf.Settings
{
    /// <summary>
    /// PDF'e özgü font ayarları. 
    /// </summary>
    public class PdfFontSettings : IFontSettings
    {
        public string FontColor { get; set; } = "#000000";
        public double FontSize { get; set; } = 12.0;
        public bool FontBold { get; set; }
        public bool FontItalic { get; set; }
        public bool FontUnderline { get; set; }
        public string Justification { get; set; } = "left";

        /// <summary>
        /// İsteğe bağlı olarak iText7'de kullanılacak PdfFont bilgisini dönmek üzere 
        /// bir metot eklenebilir. 
        /// IFontSettings’in ‘ConvertToLibrarySpecificFormat’ zorunluluğuna yönelik basit bir örnek.
        /// </summary>
        public object ConvertToLibrarySpecificFormat()
        {
            // Bu örnekte, iText7'de PDF çıktısı için
            // StandardFonts.HELVETICA, HELVETICA_BOLD, vb. seçimini yapıyoruz.
            // Gerçek projede TTF dosyaları ile createFont() yapılabilir.
            
            string baseFont = StandardFonts.HELVETICA; 
            if (FontBold && FontItalic) 
                baseFont = StandardFonts.HELVETICA_BOLDOBLIQUE;
            else if (FontBold) 
                baseFont = StandardFonts.HELVETICA_BOLD;
            else if (FontItalic) 
                baseFont = StandardFonts.HELVETICA_OBLIQUE;

            // iText7'de PDF font yaratma
            PdfFont pdfFont = PdfFontFactory.CreateFont(baseFont);
            return pdfFont;
        }
    }
}
