# ObjectPreview

### Requires [Odin Inspector]

Allows object previews to be drawn similar to Odin's [PreviewField Attribute],
but with a [ValueResolver] to get a different custom preview than the one provided by Unity.

### Examples
```CSharp
public class SomeScriptableObject : ScriptableObject
{
    public Texture2D preview;
    public string someData;
}

public class SomeMonoBehaviour : MonoBehaviour
{
    // If you dont provide a string to resolve it will
    // first look for a field named preview on the target object
    // if it cant be found it will try to get a default asset thumbnail
    [ObjectPreview]
    public NewScriptableObject SomeSO;

    // Trys to get an asset preview by calling GameObjectPreview
    // You can also pass property names and a lot of other powerful stuff
    // Visit Odin's website to see what you can do with value resolvers
    [ObjectPreview(Preview = nameof(GameObjectPreview))]
    public GameObject SomeObject;

    // Providing a value to the Selectables parameter will change the
    // behaviour of the select button to draw a dropdown with all the values
    // provided by the resolved value instead of opening an asset selector
    // !!! Note that drag and drop is disabled in this case to prevent
    // you from dragging a value inside the field that is not selectable !!!
    [ObjectPreview(Selectables = nameof(Selectables))]
    public Texture2D SomeOtherObject;

    // All available parameters
    [ObjectPreview(Height = 80f, Alignment = ObjectFieldAlignment.Right, Tooltip = "Tooltip",
    Preview = nameof(GameObjectPreview), Selectables = nameof(Selectables), FilterMode = FilterMode.Point)]
    public Texture2D AllParameters;

    private Texture2D GameObjectPreview
        => Resources.Load<Texture2D>("Odin Inspector Logo");

    private IEnumerable<Texture2D> Selectables
        => AssetDatabase
            .FindAssets("t:Texture2D")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<Texture2D>);
}
```

### Usage
Simply put the downloaded ObjectPreview folder in your project
and start using the attribute as shown in the examples.
You can move the files, but make sure that `ObjectPreviewAttribute.cs`
is not in an editor folder or it will be removed during build, causing errors.

[Odin Inspector]: https://odininspector.com/
[ValueResolver]: https://odininspector.com/documentation/sirenix.odininspector.editor.valueresolvers.valueresolver-1
[PreviewField Attribute]: https://odininspector.com/attributes/preview-field-attribute
