using Sirenix.OdinInspector;
using System;
using System.Diagnostics;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public class ObjectPreviewAttribute : Attribute
{
    public float Height { get; set; }
    public string Tooltip { get; set; }
    public bool PreviewGetterHasValue { get; private set; }
    public bool SelectableObjectsGetterHasValue { get; private set; }
    public bool AlignmentHasValue { get; private set; }

    private string previewGetter;
    private string selectableObjectsGetter;
    private ObjectFieldAlignment alignment;

    public string PreviewGetter
    {
        get => previewGetter;
        set
        {
            previewGetter = value;
            PreviewGetterHasValue = true;
        }
    }

    public string SelectableObjectsGetter
    {
        get => selectableObjectsGetter;
        set
        {
            selectableObjectsGetter = value;
            SelectableObjectsGetterHasValue = true;
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

    public ObjectPreviewAttribute() { }

    public ObjectPreviewAttribute(
        string previewGetter = "", string tooltip = "")
        => (PreviewGetter, Tooltip) = (previewGetter, tooltip);

    public ObjectPreviewAttribute(
        float height, string previewGetter = "", string tooltip = "")
        => (Height, PreviewGetter, Tooltip) = (height, previewGetter, tooltip);

    public ObjectPreviewAttribute(
        ObjectFieldAlignment alignment, string previewGetter = "", string tooltip = "")
        => (Alignment, PreviewGetter, Tooltip) = (alignment, previewGetter, tooltip);

    public ObjectPreviewAttribute(
        float height, ObjectFieldAlignment alignment, string previewGetter = "", string tooltip = "")
        => (Height, Alignment, PreviewGetter, Tooltip) = (height, alignment, previewGetter, tooltip);
}
