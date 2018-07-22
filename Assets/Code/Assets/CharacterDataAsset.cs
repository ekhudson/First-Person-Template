using UnityEngine;

public class CharacterDataAsset : ScriptableObject
{
    [SerializeField]
    private CharacterData m_Data;

    public CharacterData Data { get { return m_Data; } }
}
