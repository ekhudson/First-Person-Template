using UnityEngine;

public class LocationDataAsset : ScriptableObject
{
    [SerializeField]
    private LocationData m_Data;

    public LocationData Data { get { return m_Data; } }
}
