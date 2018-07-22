using UnityEngine;

public class SubjectComponent : MonoBehaviour
{
    [SerializeField]
    private string m_PairedSubjectKey = string.Empty;

    private SubjectDatabase.SubjectType mSubjectType = SubjectDatabase.SubjectType.CHARACTER;
    private string mSubjectKey = string.Empty;

    public string PairedSubjectKey { get { return m_PairedSubjectKey; } }

    public SubjectDatabase.SubjectType SubjectType
    {
        get
        {
            ParseCurrentKey();
            return mSubjectType;
        }
    }

    public string SubjectKey
    {
        get
        {
            ParseCurrentKey();
            return mSubjectKey;
        }
    }

    private void ParseCurrentKey()
    {
        SubjectKeyParser.ParseSubjectPair(m_PairedSubjectKey, out mSubjectType, out mSubjectKey);
    }
}
