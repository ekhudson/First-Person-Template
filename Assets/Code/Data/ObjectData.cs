using UnityEngine;

[System.Serializable]
public class ObjectData : BaseData
{
    [SerializeField]
    private string m_ObjectName = string.Empty;

    [SerializeField]
    [TextArea(3, 10)]
    private string m_Description = string.Empty;

    public string Description { get { return m_Description; } }

    public override string NotificationString()
    {
        return m_ObjectName;
    }
}
