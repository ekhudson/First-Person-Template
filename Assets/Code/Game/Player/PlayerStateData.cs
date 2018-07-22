using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStateData
{
    [SerializeField]
    private List<PlayerSubjectState> m_DiscoveredSubjects = new List<PlayerSubjectState>();
    [SerializeField]
    private List<PlayerClueState> m_DiscoveredClues = new List<PlayerClueState>();

    public List<PlayerSubjectState> DiscoveredSubjects
    {
        get
        {
            return m_DiscoveredSubjects;
        }
    }

    public List<PlayerClueState> DiscoveredClues
    {
        get
        {
            return m_DiscoveredClues;
        }
    }

    public bool TryAddNewSubject(SubjectDatabase.SubjectType subjectType, string subjectKey)
    {
        return TryAddNewSubject(subjectType, subjectKey, false);
    }

    public bool TryAddNewSubject(SubjectDatabase.SubjectType subjectType, string subjectKey, bool silentAdd)
    {
        if(m_DiscoveredSubjects.Exists(x => x.SubjectType == subjectType && x.SubjectKey == subjectKey))
        {
            return false;
        }
        else
        {
            m_DiscoveredSubjects.Add(new PlayerSubjectState(subjectType, subjectKey));

            if (!silentAdd)
            {
                EventManager.Instance.Post(new SubjectDiscoveredEvent(this, subjectType, subjectKey));
            }

            return true;
        }
    }

    public bool TryAddNewClue(string clueKey)
    {
        if (m_DiscoveredClues.Exists(x => x.ClueKey == clueKey))
        {
            return false;
        }
        else
        {
            m_DiscoveredClues.Add(new PlayerClueState(clueKey));
            return true;
        }
    }

    public PlayerSubjectState GetSubjectState(string pairedSubjectKey)
    {
        SubjectDatabase.SubjectType subjectType = SubjectDatabase.SubjectType.CHARACTER;
        string subjectKey = string.Empty;
        SubjectKeyParser.ParseSubjectPair(pairedSubjectKey, out subjectType, out subjectKey);
        return GetSubjectState(subjectType, subjectKey);
    }

    public PlayerSubjectState GetSubjectState(SubjectDatabase.SubjectType subjectType, string subjectKey)
    {
        try
        {
            return m_DiscoveredSubjects.Find(x => x.SubjectType == subjectType && x.SubjectKey == subjectKey);
        }
        catch
        {
            return null;
        }
    }

    public PlayerClueState GetClueState(string clueKey)
    {
        try
        {
            return m_DiscoveredClues.Find(x => x.ClueKey == clueKey);
        }
        catch
        {
            return null;
        }
    }
}
