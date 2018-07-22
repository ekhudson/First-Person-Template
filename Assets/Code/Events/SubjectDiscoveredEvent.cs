using UnityEngine;

public class SubjectDiscoveredEvent : EventBase
{
    public readonly SubjectDatabase.SubjectType SubjectType = SubjectDatabase.SubjectType.CHARACTER;
    public readonly string SubjectKey = string.Empty;

    public SubjectDiscoveredEvent(object sender, SubjectDatabase.SubjectType subjectType, string subjectKey) : base(Vector3.zero, sender)
    {
        SubjectType = subjectType;
        SubjectKey = subjectKey;
    }
}
