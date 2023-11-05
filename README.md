# StateManager

StateManager is a plugin library that provides more classes for modifying states of interactables and usables. Simply pass in the barricade/item and modify the properties you wish to change, then use the `Apply` method.

The following code clears an `InteractableStorage` of its items.
```csharp
//Pass in a BarricadeDrop.
var stateReadWrite = new InteractableStorageState((BarricadeDrop)barricade);
//Modify the properties you wish to change.
stateReadWrite.Items.clear();
//Apply the changes.
stateReadWrite.Apply();
```
