using UnityEngine;

public class NotebookCategoryDataAsset : ScriptableObject
{
    [SerializeField]
    private string m_CategoryDisplayNameSingular;
    [SerializeField]
    private string m_CategoryDisplayNamePlural;

    public string CategoryDisplayNameSingular {  get { return m_CategoryDisplayNameSingular; } }
    public string CategoryDisplayNamePlural { get { return m_CategoryDisplayNamePlural; } }
}
