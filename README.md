# ObjectPreview

#### Draws object previews similar to Odin's [PreviewField attribute], but with more configurability. (Requires [Odin Inspector])

### Installation
Simply put the downloaded ObjectPreview folder in your project
and start using the attribute as shown in the examples.
You can move the files, but make sure that `ObjectPreviewAttribute.cs`
is not in an editor folder or it will be removed during build, causing errors.

### Examples
```CSharp
// SomeScriptableObject.cs
public class SomeScriptableObject : ScriptableObject
{
    public Texture2D preview;
}

// SomeMonoBehaviour.cs
public class SomeMonoBehaviour : MonoBehaviour
{
    // We don't provide a preview string to resolve, so it looks for a field named "preview" on the target object first
    // If that also fails, the object's default asset thumbnail is used.
    [ObjectPreview]
    public SomeScriptableObject SomeSO;

    // We pass a property name as the value for the Preview parameter.
    // We then use the returned value of this property as our preview image.
    [ObjectPreview(Preview = nameof(GameObjectPreview))]
    public GameObject SomeObject;

    // We pass a method name as the value for the Selectables parameter.
    // We then use the returned value of this method as our selectable values.
    [ObjectPreview(Selectables = nameof(GetSelectables))]
    public Texture2D SomeOtherObject;

    private Texture2D GameObjectPreview => 
        Resources.Load<Texture2D>("Odin Inspector Logo");

    private IEnumerable<Texture2D> GetSelectables() => 
        AssetDatabase
            .FindAssets("t:Texture2D")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<Texture2D>);
}
```

### Parameters
Parameter   | Description                                                                             | Default                           | Type
----------- | --------------------------------------------------------------------------------------- | --------------------------------- | ----------------------
Height      | Determines the width and height of the square preview                                   | Configurable in Odin preferences. | [float]
Alignment   | Determines the position of the preview image                                            | Configurable in Odin preferences. | [ObjectFieldAlignment]
FilterMode  | Determines the filter mode that is used when the preview image is displayed             | [FilterMode.Point]                | [FilterMode]
Tooltip     | Determines the tooltip that is displayed when you move the mouse over the preview image | [null]                            | [string]
Selectables | A resolved string that determines which values can be selected for this preview field. Setting this parameter automatically adds a selection button that allows you to select one of the selectable values. Setting this will also disable drag and drop to ensure that only selectable values are set. | [null] | [string] / [ValueResolver]
Preview     | A Resolved string that determines the image that gets used as a preview. If no value is provided it will first try to find a property called preview on the target object if that also fails it will use the assets default thumbnail. | "@$value.preview" | [string] / [ValueResolver]

[Odin Inspector]: https://odininspector.com/
[ValueResolver]: https://odininspector.com/documentation/sirenix.odininspector.editor.valueresolvers.valueresolver-1
[PreviewField Attribute]: https://odininspector.com/attributes/preview-field-attribute

[float]: https://docs.microsoft.com/bs-latn-ba/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
[string]: https://docs.microsoft.com/bs-latn-ba/dotnet/csharp/language-reference/builtin-types/reference-types#the-string-type
[null]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null
[FilterMode]: https://docs.unity3d.com/ScriptReference/FilterMode.html
[FilterMode.Point]: https://docs.unity3d.com/ScriptReference/FilterMode.Point.html
[ObjectFieldAlignment]: https://www.odininspector.com/documentation/sirenix.odininspector.objectfieldalignment
