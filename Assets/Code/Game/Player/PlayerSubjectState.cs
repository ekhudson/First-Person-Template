using UnityEngine;

[System.Serializable]
public class PlayerSubjectState
{
    public enum SubjectStates
    {
        NEUTRAL,
        CLEARED,
        SUSPICIOUS,
        EVIDENCE,
    }

    [SerializeField]
    private SubjectDatabase.SubjectType m_SubjectType;
    [SerializeField]
    private string m_SubjectKey;
    [SerializeField]
    private SubjectStates m_SubjectState = SubjectStates.NEUTRAL;

    public SubjectDatabase.SubjectType SubjectType { get { return m_SubjectType; } }
    public string SubjectKey { get { return m_SubjectKey; } }

    public SubjectStates SubjectState
    {
        get
        {
            return m_SubjectState;
        }
        set
        {
            m_SubjectState = value;
        }
    }

    public PlayerSubjectState(SubjectDatabase.SubjectType subjectType, string subjectKey)
    {
        m_SubjectType = subjectType;
        m_SubjectKey = subjectKey;
    }
}
