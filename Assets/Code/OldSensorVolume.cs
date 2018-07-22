using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class OldSensorVolume<T> : MonoBehaviour where T : MonoBehaviour 
{
    private List<T> mObjectList = new List<T>();
    private List<T> mToBeRemovedObjects = new List<T>();
    private SphereCollider mSphereCollider;

    private const float kScrubTimeInterval = 0.5f;

    public List<T> ObjectList
    {
        get
        {
            return mObjectList;
        }
    }

    public T[] ObjectListAsArray
    {
        get
        {
            return mObjectList.ToArray();
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
        T obj = other.GetComponent<T>();

        if (obj == null)
        {
            return;
        }
        else if (!mObjectList.Contains(obj))
        {
            mObjectList.Add(obj);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        T obj = other.GetComponent<T>();

        if (obj == null)
        {
            return;
        }
        else if (mObjectList.Contains(obj))
        {
            mObjectList.Remove(obj);
        }
    }

    IEnumerator ScrubListForNulls()
    {
        while (true)
        {
            foreach (T obj in mObjectList)
            {
                if (obj != null)
                {
                    continue;
                }
                else
                {
                    mToBeRemovedObjects.Add(obj);
                }
            }

            foreach (T obj in mToBeRemovedObjects)
            {
                mObjectList.Remove(obj);
            }

            mToBeRemovedObjects.Clear();

            yield return new WaitForSeconds(kScrubTimeInterval);
        }
    }
}
