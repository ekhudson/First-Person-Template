using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    private List<GameObject> m_GameObjectsInVolume = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;

        if (!m_GameObjectsInVolume.Contains(otherGameObject))
        {
            m_GameObjectsInVolume.Add(otherGameObject);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        GameObject otherGameObject = other.gameObject;

        if (m_GameObjectsInVolume.Contains(otherGameObject))
        {
            m_GameObjectsInVolume.Remove(otherGameObject);
        }
    }

    public GameObject[] GetGameObjectsInVolume()
    {
        return m_GameObjectsInVolume.ToArray();
    }

    public bool GameObjectIsInVolume(GameObject gameObject)
    {
        return m_GameObjectsInVolume.Contains(gameObject);
    }
}

