using UnityEngine;

[System.Serializable]
public class PlayerClueState
{
    [SerializeField]
    private string m_ClueKey;

    public string ClueKey { get { return m_ClueKey; } }

    public PlayerClueState(string clueKey)
    {
        m_ClueKey = clueKey;
    }
}
