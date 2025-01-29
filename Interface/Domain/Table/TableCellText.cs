﻿using DocGen.Abstract.Interface.Content;
using DocGen.Abstract.Interface.Settings;

namespace DocGen.Abstract.Domain.Table;

/// <summary>
/// Represents a text-based table cell with optional font settings.
/// </summary>
public class TableCellText : BaseTableCell
{
    public TableCellText()
    {
        Content = string.Empty;
    }

    public string Content { get; set; }
}
