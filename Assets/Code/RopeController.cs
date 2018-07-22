using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class RopeController : MonoBehaviour 
{
    public Transform StartAnchor;
    public Transform EndAnchor;
    public float RopeWidth = 1;
    public int RopeSegments = 5;
    public float RopeSlack = 1f;
    public bool DrawGizmos = false;

    [Range(0f, 1f)]
    public float CentreRelativeOffset = 0.5f;

    //private float mRopeDistance = 0f;
    private Vector3 mRopeCentre = Vector3.zero;

    private LineRenderer mLineRenderer;

    public void Start()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        CreateRope();
    }

    public void CreateRope()
    {
        Vector3 ropeDirection = EndAnchor.transform.position - StartAnchor.transform.position;
        float ropeLength = ropeDirection.magnitude;
        ropeDirection.Normalize();

        //mRopeDistance = ropeLength;

        float segmentLength = ropeLength / RopeSegments;

        mLineRenderer.positionCount = (RopeSegments + 1);

        mRopeCentre = StartAnchor.position + (ropeDirection * (ropeLength * CentreRelativeOffset));
        mRopeCentre += new Vector3(0, RopeSlack, 0);

        for (int i = 0; i <= RopeSegments; i++)
        {
            Vector3 pos = StartAnchor.position + (ropeDirection * (i * segmentLength));

            float time = (float)i / (float)RopeSegments;
            float ang = time * 180f;

            pos.y += RopeSlack * (Mathf.Sin(ang * Mathf.Deg2Rad));

            mLineRenderer.SetPosition(i, pos);
        }

        mLineRenderer.startWidth = mLineRenderer.endWidth = RopeWidth;
    }

    private void OnDrawGizmosSelected()
    {
        if (!DrawGizmos)
        {
            return;
        }

        //Gizmos.DrawSphere(StartAnchor.position + Handle01, 2f);
        //Gizmos.DrawSphere(EndAnchor.position + Handle02, 2f);
        Gizmos.DrawSphere(mRopeCentre, 3f);
        Gizmos.DrawSphere(StartAnchor.position, 2f);
        Gizmos.DrawSphere(EndAnchor.position, 2f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(StartAnchor.position, mRopeCentre);
        Gizmos.DrawLine(EndAnchor.position, mRopeCentre);
        Gizmos.color = Color.white;
    }

    public void Update()
    {
      //  if (UpdateRope)
       // {
            CreateRope();
           // UpdateRope = false;
        //}
    }
}
