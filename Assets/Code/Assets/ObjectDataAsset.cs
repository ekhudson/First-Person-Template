using UnityEngine;

public class ObjectDataAsset : ScriptableObject
{
    [SerializeField]
    private ObjectData m_Data;

    public ObjectData Data { get { return m_Data; } }
}
