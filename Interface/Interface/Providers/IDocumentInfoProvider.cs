using System.Collections.Generic;
using DocGen.Abstract.Interface.Content;

namespace DocGen.Abstract.Interface.Providers;

/// <summary>
/// A provider interface that abstracts away 
/// the underlying DocumentContent structure.
/// </summary>
public interface IDocumentInfoProvider
{
    IDocumentSection GetHeader();
    IDocumentSection GetFooter();
    IDocumentSection GetBody();
    IDocumentSection GetSignature();

    IEnumerable<IDocumentSection> GetAllSections();
}
