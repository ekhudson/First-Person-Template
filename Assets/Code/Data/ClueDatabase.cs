using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ClueDatabase
{
    [SerializeField]
    private List<ClueData> m_Clues = new List<ClueData>();

    public List<ClueData> Clues { get { return m_Clues; } }
}
