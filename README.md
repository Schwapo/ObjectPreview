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
    // If you dont provide a string to resolve then it will
    // first look for a field named preview on the target object
    // if it cant be found it will try to get a default asset thumbnail
    [ObjectPreview]
    public SomeScriptableObject someSO;

    // Trys to get an asset preview by calling GameObjectPreview
    // You can also pass property names and alot of other powerful stuff
    // Visit Odin's website to see what you can do with value resolvers
    [ObjectPreview("@GameObjectPreview()")]
    public GameObject someObject;

    // All available parameters
    [ObjectPreview(height: 80f, alignment: ObjectFieldAlignment.Right, 
    previewGetter: "@GameObjectPreview()", tooltip: "Tooltip")]
    public GameObject someOtherObject;

    private Texture2D GameObjectPreview => Resources.Load<Texture2D>("Odin Inspector Logo");
}
```

### Usage
Simply put the downloaded ObjectPreviewAttribute folder in your project
and start using the attribute as shown in the examples.
You can move the files, but make sure that `ObjectPreviewAttribute.cs`
is not in an editor folder or it will be removed during build, causing errors.

[Odin Inspector]: https://odininspector.com/
[ValueResolver]: https://odininspector.com/documentation/sirenix.odininspector.editor.valueresolvers.valueresolver-1
[PreviewField Attribute]: https://odininspector.com/attributes/preview-field-attribute
