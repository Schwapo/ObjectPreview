using Sirenix.OdinInspector;
using System;
using System.Diagnostics;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public class ObjectPreviewAttribute : Attribute
{
    public float Height { get; set; }
    public string Tooltip { get; set; }
    public FilterMode FilterMode { get; set; }
    public bool PreviewHasValue { get; private set; }
    public bool SelectablesHasValue { get; private set; }
    public bool AlignmentHasValue { get; private set; }

    private string preview;
    private string selectables;
    private ObjectFieldAlignment alignment;

    public string Preview
    {
        get => preview;
        set
        {
            preview = value;
            PreviewHasValue = true;
        }
    }

    public string Selectables
    {
        get => selectables;
        set
        {
            selectables = value;
            SelectablesHasValue = true;
        }
    }

    public ObjectFieldAlignment Alignment
    {
        get => alignment;
        set
        {
            alignment = value;
            AlignmentHasValue = true;
        }
    }
}
