using System.Collections.Generic;

namespace DocGen.Abstract.Interface.Content;

/// <summary>
/// Parses a rich text string into a list of formatted elements.
/// </summary>
public interface IContentFormatter<T>
{
    List<T> ParseRichText(string richText);
}