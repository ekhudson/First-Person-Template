using UnityEngine;
using UnityEditor;

public class DataAssetUtility : ScriptableObject
{
    [MenuItem("Assets/Create/Create Data Asset/SubjectDatabaseAsset")]
    public static void CreateSubjectDatabaseAsset()
    {
        ScriptableObjectUtility.CreateAsset<SubjectDatabaseAsset>();
    }

    [MenuItem("Assets/Create/Create Data Asset/CharacterDataAsset")]
    public static void CreateCharacterDataAsset()
    {
        ScriptableObjectUtility.CreateAsset<CharacterDataAsset>();
    }

    [MenuItem("Assets/Create/Create Data Asset/ObjectDataAsset")]
    public static void CreateObjectDataAsset()
    {
        ScriptableObjectUtility.CreateAsset<ObjectDataAsset>();
    }

    [MenuItem("Assets/Create/Create Data Asset/LocationDataAsset")]
    public static void CreateLocationDataAsset()
    {
        ScriptableObjectUtility.CreateAsset<LocationDataAsset>();
    }

    [MenuItem("Assets/Create/Create Data Asset/Notebook Category")]
    public static void CreateNotebookCategoryAsset()
    {
        ScriptableObjectUtility.CreateAsset<NotebookCategoryDataAsset>();
    }

    [MenuItem("Assets/Create/Create Data Asset/Clue Database Asset")]
    public static void CreateClueDatabaseAsset()
    {
        ScriptableObjectUtility.CreateAsset<ClueDatabaseDataAsset>();
    }
}