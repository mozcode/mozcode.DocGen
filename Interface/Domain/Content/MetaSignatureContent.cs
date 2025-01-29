using System.Collections.Generic;
using DocGen.Abstract.Domain.Table;
using DocGen.Abstract.Interface.Content;
using DocGen.Abstract.Interface.Settings;

namespace DocGen.Abstract.Domain.Content;

/// <summary>
/// Represents the signature portion of a document,
/// storing table rows and optional font settings.
/// Implements IDocumentSection for usage in DocumentContentInfoProvider.
/// 
/// Yeni eklenen "PlaceAtBottom" özelliği sayesinde 
/// çıktı üretim katmanı, bu değeri dikkate alarak imza bölümünü
/// sayfanın en altına (örneğin koordinat bazlı) yerleştirebilir.
/// </summary>
public class MetaSignatureContent : SubContent, IDocumentSection
{
    /// <summary>
    /// Constructor allowing immediate assignment of tablesRows and fontSettings.
    /// </summary>
    public MetaSignatureContent(List<TablesRow> tablesRows, IFontSettings? fontSettings = null)
    {
        this.TablesRows = tablesRows;
        this.FontSettings = fontSettings;
    }

    /// <summary>
    /// İsteğe bağlı: İmzanın fiziksel olarak sayfa sonuna konulup konulmayacağını belirtir.
    /// Varsayılan olarak true (sayfanın altına).
    /// </summary>
    public bool PlaceAtBottom { get; set; } = true;

    /// <summary>
    /// Re-expose TablesRows with 'new' keyword so it can be assigned (set).
    /// </summary>
    public new List<TablesRow> TablesRows
    {
        get => base.TablesRows;
        set => base.TablesRows = value;
    }

    /// <summary>
    /// Re-expose FontSettings with 'new' keyword so it can be assigned (set).
    /// </summary>
    public new IFontSettings? FontSettings
    {
        get => base.FontSettings;
        set => base.FontSettings = value;
    }

    /// <summary>
    /// Signature typically has no sub-sections, so return an empty list.
    /// </summary>
    public IEnumerable<IDocumentSection> GetSubSections()
    {
        return new List<IDocumentSection>();
    }
}
