using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SubjectDatabase
{
    public enum SubjectType
    {
        CHARACTER,
        OBJECT,
        LOCATION,
    }

    [SerializeField] private CharacterDatabaseDictionary m_CharacterDatabaseDict;
    [SerializeField] private ObjectDatabaseDictionary m_ObjectDatabaseDict;
    [SerializeField] private LocationDatabaseDictionary m_LocationDatabaseDict;

    public T RetrieveSubject<T>(string pairedSubjectKey) where T : BaseData
    {
        SubjectType subjectType = SubjectType.CHARACTER;
        string subjectKey = string.Empty;
        SubjectKeyParser.ParseSubjectPair(pairedSubjectKey, out subjectType, out subjectKey);
        return RetrieveSubject<T>(subjectType, subjectKey);
    }

    public T RetrieveSubject<T>(SubjectType subjectType, string subjectKey) where T : BaseData
    {
        switch(subjectType)
        {
            case SubjectType.CHARACTER:

                if (m_CharacterDatabaseDict.ContainsKey(subjectKey))
                {
                    return m_CharacterDatabaseDict[subjectKey].Data as T;
                }

                break;

            case SubjectType.OBJECT:

                if (m_ObjectDatabaseDict.ContainsKey(subjectKey))
                {
                    return m_ObjectDatabaseDict[subjectKey].Data as T;
                }

                break;

            case SubjectType.LOCATION:

                if (m_LocationDatabaseDict.ContainsKey(subjectKey))
                {
                    return m_LocationDatabaseDict[subjectKey].Data as T;
                }

                break;
        }

        return null;
    }
}
