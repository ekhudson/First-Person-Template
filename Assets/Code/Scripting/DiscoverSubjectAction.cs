using UnityEngine;
using SimpleScript;

public class DiscoverSubjectAction : ActionBase
{
    [SerializeField]
    private SubjectDatabase.SubjectType m_SubjectType = SubjectDatabase.SubjectType.CHARACTER;
    [SerializeField]
    private string m_SubjectKey = string.Empty;

    [SerializeField]
    [Tooltip("If true, supresses the discovery notification from appearing")]
    private bool m_SilentDiscovery = false;

    protected override void ActionLogic()
    {
        GameDataManager.Instance.PlayerState.TryAddNewSubject(m_SubjectType, m_SubjectKey, m_SilentDiscovery);
    }
}
