using UnityEditor;

[CustomPropertyDrawer(typeof(CharacterDatabaseDictionary))]
[CustomPropertyDrawer(typeof(ObjectDatabaseDictionary))]
[CustomPropertyDrawer(typeof(LocationDatabaseDictionary))]
public class AnySerializedDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
{

}
