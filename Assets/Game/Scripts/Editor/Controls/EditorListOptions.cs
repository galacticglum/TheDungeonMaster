using System;

[Flags]
public enum EditorListOptions
{
    None = 0,
    ShowSize = 1,
    ShowLabel = 2,
    ShowElementLabels = 4,
    Buttons = 8,

    Default = ShowSize | ShowLabel | ShowElementLabels,
    NoElementLabels = ShowSize | ShowLabel,
    All = Default | Buttons
}
