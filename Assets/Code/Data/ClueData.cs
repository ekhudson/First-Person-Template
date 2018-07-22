using UnityEngine;

[System.Serializable]
public class ClueData
{
    [SerializeField]
    private string m_ClueID = string.Empty;
    [SerializeField]
    [TextArea(3,3)]
    private string m_ClueString = string.Empty;

    public string ClueID { get { return m_ClueID; } }
    public string ClueString { get { return m_ClueString; } }
}
