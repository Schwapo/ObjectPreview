#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObjectPreviewAttributeDrawer<T> : OdinAttributeDrawer<ObjectPreviewAttribute, T> where T : Object
{
    private ValueResolver<object> previewResolver;
    private ValueResolver<IEnumerable<T>> selectablesResolver;
    private bool allowSceneObjects;
    private float height;
    private ObjectFieldAlignment alignment;
    private GenericSelector<T> selector;

    protected override void Initialize()
    {
        var resolvedString = Attribute.Preview.IsNullOrWhitespace()
            ? "@$value.preview"
            : Attribute.Preview;

        previewResolver = ValueResolver.Get<object>(Property, resolvedString);
        selectablesResolver = ValueResolver.Get<IEnumerable<T>>(Property, Attribute.Selectables);
        allowSceneObjects = Property.GetAttribute<Sirenix.OdinInspector.AssetsOnlyAttribute>() == null;

        height = Attribute.Height == 0f
            ? GlobalConfig<GeneralDrawerConfig>.Instance.SquareUnityObjectFieldHeight
            : Attribute.Height;

        alignment = !Attribute.AlignmentHasValue
            ? GlobalConfig<GeneralDrawerConfig>.Instance.SquareUnityObjectAlignment
            : (ObjectFieldAlignment)Attribute.Alignment;

        selector = new GenericSelector<T>("", false, item => item.name, selectablesResolver.GetValue());
        selector.SelectionConfirmed += selection => Property.ValueEntry.WeakSmartValue = selection.FirstOrDefault();
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        if (Attribute.PreviewHasValue)
        {
            ValueResolver.DrawErrors(previewResolver);
        }

        if (Attribute.SelectablesHasValue)
        {
            ValueResolver.DrawErrors(selectablesResolver);
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

        if (Attribute.SelectablesHasValue)
        {
            if (HoveringOverRect(rect))
            {
                if (GUI.Button(rect.AlignBottom(15f), "Select", SirenixGUIStyles.TagButton))
                {
                    selector.ShowInPopup();
                }
            }
        }
        else
        {
            value = (T)DragAndDropUtilities.DropZone(rect, value, typeof(T), dragAndDropId);
            value = (T)DragAndDropUtilities.ObjectPickerZone(rect, value, typeof(T), allowSceneObjects, dragAndDropId);
            value = (T)DragAndDropUtilities.DragZone(rect, value, typeof(T), true, true, dragAndDropId);
        }

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
        if (HoveringOverRect(rect))
        {
            var tooltipSize = GUI.skin.label.CalcSize(new GUIContent("", Attribute.Tooltip));
            var tooltipRect = new Rect(Event.current.mousePosition, tooltipSize);
            EditorGUI.LabelField(tooltipRect, new GUIContent("", Attribute.Tooltip));
        }
    }

    private bool HoveringOverRect(Rect rect) => rect.Contains(Event.current.mousePosition);

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
            var previewObject = previewResolver.GetValue() as Object;

            var previewTexture = previewObject == null
                ? GUIHelper.GetAssetThumbnail(obj, obj.GetType(), true)
                : GUIHelper.GetAssetThumbnail(previewObject, previewObject.GetType(), true);
                
            previewTexture.filterMode = Attribute.FilterMode;

            rect = rect.Padding(2f);
            var rectSize = Mathf.Min(rect.width, rect.height);

            EditorGUI.DrawTextureTransparent(
                rect.AlignCenter(rectSize, rectSize), previewTexture, ScaleMode.ScaleToFit);

            if (label != null)
            {
                rect = rect.AlignBottom(16f);
                GUI.Label(rect, label, EditorStyles.label);
            }
        }
    }
}
#endif
