using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ObjectOverride;

    [SerializeField]
    private UnityEvent m_OnInteractEvent;

    private Mesh mMesh;
    private Renderer mRenderer;
    private SubjectComponent mSubjectComponentReference;

    public Mesh GetMesh()
    {
        if (m_ObjectOverride != null)
        {
            mMesh = m_ObjectOverride.GetComponent<MeshFilter>().mesh;
        }
        else if (mMesh == null)
        {
            mMesh = GetComponent<MeshFilter>().mesh;
        }

        return mMesh;
    }

    public Renderer GetRenderer()
    {
        if (m_ObjectOverride != null)
        {
            mRenderer = m_ObjectOverride.GetComponent<Renderer>();
        }
        else if(mRenderer == null)
        {
            mRenderer = GetComponent<Renderer>();
        }

        return mRenderer;
    }

    public SubjectComponent SubjectComponentReference
    {
        get
        {
            return mSubjectComponentReference;
        }
    }

    protected virtual void Start()
    {
        mSubjectComponentReference = GetComponent<SubjectComponent>();
    }

    public virtual void OnInteract()
    {
        m_OnInteractEvent.Invoke();
    }
}