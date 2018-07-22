using UnityEngine;
using System.Collections;

public class PickupObject : InteractableObject
{
    public float SlowsPlayerByPercent = 0f;
    public Vector3 PutdownNormal = Vector3.up; //What normal is required to put the object down? ie. Does it only go down on a flat surface (Vector3.up), or does it stick to a wall (Vector3.forward)
    public float PutdownDotThreshold = 0.90f;

    private Rigidbody mRigidbody = null;
    private Vector3 mOriginalPosition = Vector3.zero;
    private Quaternion mOriginalRotation = Quaternion.identity;
    private Vector3 mPickupPosition = Vector3.zero;
    private Quaternion mPickupRotation = Quaternion.identity;

    private MeshFilter mMeshFilter = null;
    //private Color mOriginalColor = Color.white;
    private Collider mCollider;

    private const float kPickupAlpha = 1f;

    public Mesh PickupMesh
    {
        get
        {
            if (mMeshFilter == null)
            {
                mMeshFilter = GetComponent<MeshFilter>();
            }

            return mMeshFilter.mesh;
        }
    }

    public Vector3 OriginalPosition
    {
        get
        {
            return mOriginalPosition;
        }
    }

    public Quaternion OriginalRotation
    {
        get
        {
            return mOriginalRotation;
        }
    }

    public Vector3 PickupPosition
    {
        get
        {
            return mPickupPosition;
        }
    }

    public Quaternion PickupRotation
    {
        get
        {
            return mPickupRotation;
        }
    }

    protected override void Start()
    {
        base.Start();
        mRigidbody = GetComponent<Rigidbody>();
        mOriginalPosition = transform.position;
        mOriginalRotation = transform.rotation;
        mCollider = GetComponent<Collider>();
        base.Start();
    }

    public override void OnInteract()
    {
        Pickup();
        base.OnInteract();
    }

    public void Pickup()
    {
        mRigidbody.isKinematic = true;
        //mCollider.isTrigger = true;
        mCollider.enabled = false;
        mPickupPosition = transform.position;
        mPickupRotation = transform.rotation;
    }

    public void PlaceDown(Vector3 position, Quaternion rotation)
    {
        mRigidbody.isKinematic = false;
        //mCollider.isTrigger = false;
        mCollider.enabled = true;
        transform.position = position;
        transform.rotation = rotation;
    }

    public void Drop()
    {
        Throw(Vector3.zero);
    }

    public void Throw(Vector3 throwForce)
    {
        mRigidbody.isKinematic = false;
        //mCollider.isTrigger = false;
        mCollider.enabled = true;
        mRigidbody.AddForce(throwForce, ForceMode.Force);
    }
}
