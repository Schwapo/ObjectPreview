#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class ObjectPreviewAttributeDrawer<T> : OdinAttributeDrawer<ObjectPreviewAttribute, T>
    where T : Object
{
    private ValueResolver<Texture2D> previewResolver;
    private bool allowSceneObjects;
    private float height;
    private ObjectFieldAlignment alignment;

    protected override void Initialize()
    {
        var resolvedString = Attribute.PreviewGetter.IsNullOrWhitespace()
            ? "@$value.preview"
            : Attribute.PreviewGetter;

        previewResolver = ValueResolver.Get<Texture2D>(Property, resolvedString);
        allowSceneObjects = Property.GetAttribute<Sirenix.OdinInspector.AssetsOnlyAttribute>() == null;

        height = Attribute.Height == 0f
            ? GlobalConfig<GeneralDrawerConfig>.Instance.SquareUnityObjectFieldHeight
            : Attribute.Height;

        alignment = !Attribute.AlignmentHasValue
            ? GlobalConfig<GeneralDrawerConfig>.Instance.SquareUnityObjectAlignment
            : (ObjectFieldAlignment)Attribute.Alignment;
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        if (Attribute.PreviewGetterHasValue)
        {
            ValueResolver.DrawErrors(previewResolver);
        }

        var rect = EditorGUILayout.GetControlRect(label != null, height);
        var dragAndDropId = DragAndDropUtilities.GetDragAndDropId(rect);

        if (label != null)
        {
            rect = EditorGUI.PrefixLabel(rect, dragAndDropId, label);
        }

        rect = alignment switch
        {
            ObjectFieldAlignment.Left => rect.AlignLeft(rect.height),
            ObjectFieldAlignment.Center => rect.AlignCenter(rect.height),
            _ => rect.AlignRight(rect.height),
        };

        var value = ValueEntry.SmartValue;

        DrawDropZone(rect, value, null, dragAndDropId);

        value = (T)DragAndDropUtilities.DropZone(rect, value, typeof(T), dragAndDropId);

        value = (T)DragAndDropUtilities.ObjectPickerZone(
            rect, value, typeof(T), allowSceneObjects, dragAndDropId);

        value = (T)DragAndDropUtilities.DragZone(
            rect, value, typeof(T), true, true, dragAndDropId);

        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            GUIUtility.keyboardControl = dragAndDropId;
            GUIUtility.hotControl = dragAndDropId;
        }
        
        DrawTooltip(rect);

        ValueEntry.SmartValue = value;
    }

    private void DrawTooltip(Rect rect)
    {
        var mousePosition = Event.current.mousePosition;

        if (rect.Contains(mousePosition))
        {
            var tooltipSize = GUI.skin.label.CalcSize(new GUIContent("", Attribute.Tooltip));
            var tooltipRect = new Rect(mousePosition, tooltipSize);
            EditorGUI.LabelField(tooltipRect, new GUIContent("", Attribute.Tooltip));
        }
    }

    private void DrawDropZone(Rect rect, object value, GUIContent label, int id)
    {
        var isDragging = DragAndDropUtilities.IsDragging;

        if (Event.current.type != EventType.Repaint)
        {
            return;
        }

        var obj = value as Object;
        var objFieldThumb = EditorStyles.objectFieldThumb;

        var on = GUI.enabled && DragAndDropUtilities.HoveringAcceptedDropZone == id
            && rect.Contains(Event.current.mousePosition) && isDragging;

        objFieldThumb.Draw(rect, GUIContent.none, id, on);

        if (EditorGUI.showMixedValue)
        {
            GUI.Label(rect, "—", SirenixGUIStyles.LabelCentered);
        }
        else if (obj)
        {
            var texture = previewResolver.GetValue();

            if (texture == null)
            {
                texture = GUIHelper.GetAssetThumbnail(
                    obj, obj.GetType(), preferObjectPreviewOverFileIcon: true);
            }

            rect = rect.Padding(2f);
            var rectSize = Mathf.Min(rect.width, rect.height);

            EditorGUI.DrawTextureTransparent(
                rect.AlignCenter(rectSize, rectSize), texture, ScaleMode.ScaleToFit);

            if (label != null)
            {
                rect = rect.AlignBottom(16f);
                GUI.Label(rect, label, EditorStyles.label);
            }
        }
    }
}
#endif
