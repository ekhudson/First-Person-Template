using UnityEngine;

[System.Serializable]
public class ClueSubjectLink
{
    private string m_LinkText = string.Empty;
    private SubjectDatabase.SubjectType m_SubjectType = SubjectDatabase.SubjectType.CHARACTER;
    private string m_SubjectKey = string.Empty;
    private int m_SubjectPosition = 0;

    public string LinkText { get { return m_LinkText; } }
    public SubjectDatabase.SubjectType SubjectType { get { return m_SubjectType; } }
    public string SubjectKey { get { return m_SubjectKey; } }
    public int SubjectPosition { get { return m_SubjectPosition; } }

    public ClueSubjectLink(string linkText, SubjectDatabase.SubjectType subjectType, string subjectKey, int position)
    {
        m_LinkText = linkText;
        m_SubjectType = subjectType;
        m_SubjectKey = subjectKey;
        m_SubjectPosition = position;
    }
}
