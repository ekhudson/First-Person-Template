using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class CustomSensorVolume : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_IgnoreLayers;

    private Dictionary<int, List<GameObject>> mObjectDictionary = new Dictionary<int, List<GameObject>>();
    private SphereCollider mSphereCollider;
    private List<KeyValuePair<int, GameObject>> mObjectsToRemove = new List<KeyValuePair<int, GameObject>>();

    private const float kScrubTimeInterval = 0.5f;

    public Dictionary<int, List<GameObject>> CurrentObjectDict
    {
        get
        {
            return mObjectDictionary;
        }
    }

    public GameObject[] GetObjectsOfLayer(int layer)
    {
        if (mObjectDictionary.ContainsKey(layer))
        {
            return mObjectDictionary[layer].ToArray();
        }
        else
        {
            return null;
        }
    }

    public void Start()
    {
        mSphereCollider = GetComponent<SphereCollider>();

        if (!mSphereCollider.isTrigger)
        {
            mSphereCollider.isTrigger = true;
        }

        StartCoroutine(ScrubListForNulls());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            return;
        }

        if (m_IgnoreLayers == (m_IgnoreLayers | (1 << other.gameObject.layer)))
        {
            return;
        }

        int layer = other.gameObject.layer;
        GameObject obj = other.gameObject;

        if (mObjectDictionary.ContainsKey(layer))
        {
            mObjectDictionary[layer].Add(obj);
        }
        else
        {
            mObjectDictionary.Add(layer, new List<GameObject>() { obj });
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other == null)
        {
            return;
        }

        if (m_IgnoreLayers == (m_IgnoreLayers | (1 << other.gameObject.layer)))
        {
            return;
        }

        int layer = other.gameObject.layer;
        GameObject obj = other.gameObject;

        if (mObjectDictionary.ContainsKey(layer))
        {
            if (mObjectDictionary[layer].Contains(obj))
            {
                mObjectDictionary[layer].Remove(obj);
            }
        }
    }

    IEnumerator ScrubListForNulls()
    {
        while (true)
        {
            foreach (KeyValuePair<int, List<GameObject>> layerList in mObjectDictionary)
            {
                foreach(GameObject obj in layerList.Value)
                {
                    if (obj != null)
                    {
                        continue;
                    }
                    else
                    {
                        mObjectsToRemove.Add(new KeyValuePair<int, GameObject>(layerList.Key, obj));
                    }
                }    
            }

            foreach (KeyValuePair<int, GameObject> objectToRemove in mObjectsToRemove)
            {
                try
                {
                    mObjectDictionary[objectToRemove.Key].Remove(objectToRemove.Value);
                }
                catch
                {
                    Debug.Log("Tried to remove an object but it wasn't in the dictionary", this);
                }
            }

            mObjectsToRemove.Clear();

            yield return new WaitForSeconds(kScrubTimeInterval);
        }
    }
}
