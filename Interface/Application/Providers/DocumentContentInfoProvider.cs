using System.Collections.Generic;
using DocGen.Abstract.Interface.Content;
using DocGen.Abstract.Interface.Providers;

namespace DocGen.Abstract.Application.Providers;

/// <summary>
/// Concrete implementation of IDocumentInfoProvider
/// that uses a DocumentContent internally.
/// 
/// NOT: Artık GetAllSections() içinde MetaSignatureContent yer almıyor.
/// Böylece imza alanı, liste sırayla elde edilen bölümler içinde
/// görünmemiş olur ve imza alanı fiziksel olarak (koordine dayalı) 
/// en alt kısma yerleştirilebilir.
/// </summary>
public class DocumentContentInfoProvider : IDocumentInfoProvider
{
    private readonly IDocumentContent _documentContent;

    public DocumentContentInfoProvider(IDocumentContent documentContent)
    {
        _documentContent = documentContent;
    }

    /// <summary>
    /// Header (Üst Bilgi) kısmını döndürür.
    /// </summary>
    public IDocumentSection GetHeader()
    {
        return _documentContent.MetaHeaderContent;
    }

    /// <summary>
    /// Footer (Alt Bilgi) kısmını döndürür.
    /// </summary>
    public IDocumentSection GetFooter()
    {
        return _documentContent.MetaFooterContent;
    }

    /// <summary>
    /// Body (içerik) kısmını döndürür.
    /// </summary>
    public IDocumentSection GetBody()
    {
        return _documentContent.BodyContent;
    }

    /// <summary>
    /// İmza alanını döndürür.
    /// </summary>
    public IDocumentSection GetSignature()
    {
        return _documentContent.MetaSignatureContent;
    }

    /// <summary>
    /// Tüm bölümleri varsayılan sırasıyla döndürür.
    /// DİKKAT: Artık MetaSignatureContent burada listelenmiyor.
    /// Fiziksel olarak sayfanın en altına konumlandırılması bekleniyor.
    /// </summary>
    public IEnumerable<IDocumentSection> GetAllSections()
    {
        yield return _documentContent.MetaHeaderContent;
        yield return _documentContent.BodyContent;
        yield return _documentContent.MetaFooterContent;

        // Artık:
        // yield return _documentContent.MetaSignatureContent; 
        // satırı kaldırıldı.
    }
}