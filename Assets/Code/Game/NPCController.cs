using UnityEngine;
using UnityEngine.Events;

public class NPCController : CustomCharacterController 
{
    public enum NPCStates
    {
        IDLE,
        TALKING_TO_PLAYER,
        TALKING_NON_INTERRUPTIBLE,
        TALKING_INTERRUPTIBLE,
    }

    public EntityController EntityToLookAt;
    public bool MoveToPlayer = false;
    public bool LookAtPlayer = true;
    public float DistanceToLookAtPlayer = 10f;
    public float TestMaxDistanceFromPlayer = 5f;
    public float BodyTurnSpeed = 4f;
    public float HeadTurnSpeed = 6f;
    public float AngleLook = 0.4f;
    public float DistanceLook = 10f;
    public float AngleTurn = 0.2f;
    public float DistanceTurn = 5f;
    public float AngleToMoveForward = 0.8f;
    public Transform HeadTransform;
    public Transform BodyTransform;
    public Texture2D IdleFaceTexture;
    public Texture2D TalkingFaceTexture;
    public float TalkSpeed = 1f;
    public string DialogueStartNode = "Start";

    private UnityEngine.AI.NavMeshAgent mNavMeshAgent;
    private Vector3 mIdealTurnRotation = Vector3.forward;
    private Quaternion mIdleHeadRotation = Quaternion.identity;
    private EntityController mCurrentEntityToLookAt;
    private NPCStates mCurrentState = NPCStates.IDLE;
    private bool mMouthOpen = false;
    private float mCurrentTalkCycle = 0f;

    [SerializeField]
    private UnityEvent m_OnStartWalking;
    [SerializeField]
    private UnityEvent m_OnStopWalking;

    public NPCStates CurrentState
    {
        get
        {
            return mCurrentState;
        }
    }

    private void Start()
    {
        mNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        mIdealTurnRotation = transform.forward;
        mIdleHeadRotation = HeadTransform.localRotation;
        mCurrentEntityToLookAt = EntityToLookAt;
    }

    public void FixedUpdate()
    {
        Vector3 directionToPlayer = PlayerController.GlobalPlayerReference.transform.position - transform.position;
        Vector3 directionToPlayerNorm = directionToPlayer.normalized;
        float distanceToPlayer = directionToPlayer.magnitude;

        Quaternion currentRot = transform.rotation;

        if (distanceToPlayer < DistanceToLookAtPlayer && LookAtPlayer)
        {
            if (mCurrentEntityToLookAt != PlayerController.GlobalPlayerReference)
            {
                mCurrentEntityToLookAt = PlayerController.GlobalPlayerReference;
            }
        }
        else if (EntityToLookAt != null)
        {
            mCurrentEntityToLookAt = EntityToLookAt;
        }
        else
        {
            mCurrentEntityToLookAt = null;
        }

        if (mCurrentEntityToLookAt != null)
        {
            Vector3 directionToTarget = mCurrentEntityToLookAt.transform.position - transform.position;
            Vector3 directionToTargetNormalized = directionToTarget.normalized;
            float dotToTarget = Vector3.Dot(transform.forward, directionToTargetNormalized);
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget > DistanceLook && distanceToTarget > DistanceTurn)
            {
                RotateToIdle();
            }
            else
            {
                if (dotToTarget > AngleTurn && distanceToTarget < DistanceTurn)
                {
                    Quaternion desiredRot = Quaternion.LookRotation(mCurrentEntityToLookAt.transform.position - transform.position);

                    Quaternion newRot = Quaternion.Slerp(currentRot, desiredRot, Time.deltaTime * BodyTurnSpeed);

                    transform.rotation = newRot;
                }

                if (dotToTarget > AngleLook && distanceToTarget < DistanceLook)
                {
                    Vector3 lookVector = directionToTargetNormalized;
                    lookVector.y = 0f;

                    Quaternion lookRot = Quaternion.LookRotation(lookVector, Vector3.up);

                    HeadTransform.rotation = Quaternion.Slerp(HeadTransform.rotation, lookRot, Time.deltaTime * HeadTurnSpeed);
                }
                else if (distanceToTarget > DistanceTurn)
                {
                    HeadTransform.localRotation = Quaternion.Slerp(HeadTransform.localRotation, mIdleHeadRotation, Time.deltaTime * HeadTurnSpeed);
                }
            }           
        }
        else
        {
            RotateToIdle();

            if (mCurrentState == NPCStates.TALKING_TO_PLAYER)
            {
                EndConversation();
            }
        }

        if ((distanceToPlayer > TestMaxDistanceFromPlayer) && (MoveToPlayer) && (mNavMeshAgent.isOnNavMesh))
        {
            mNavMeshAgent.SetDestination(PlayerController.GlobalPlayerReference.transform.position);
            mNavMeshAgent.isStopped = false;
            m_OnStartWalking.Invoke();
        }
        else if (mNavMeshAgent.isOnNavMesh)
        {
            mNavMeshAgent.isStopped = true;
            m_OnStopWalking.Invoke();
        }

        if (mCurrentState == NPCStates.TALKING_TO_PLAYER)
        {
            TalkUpdate();
        }
    }

    public void TalkUpdate()
    {
        if (mCurrentTalkCycle > TalkSpeed)
        {
            mCurrentTalkCycle = 0f;

            if (mMouthOpen)
            {
                HeadTransform.GetComponent<MeshRenderer>().material.mainTexture = IdleFaceTexture;
                mMouthOpen = false;
            }
            else
            {
                HeadTransform.GetComponent<MeshRenderer>().material.mainTexture = TalkingFaceTexture;
                mMouthOpen = true;
            }
        }

        mCurrentTalkCycle += Time.deltaTime;
    }

    public void RotateToIdle()
    {
        if (mCurrentState == NPCStates.TALKING_TO_PLAYER)
        {
            EndConversation();
        }

        Quaternion currentRot = transform.rotation;

        Quaternion desiredRot = Quaternion.LookRotation(mIdealTurnRotation);

        Quaternion newRot = Quaternion.Lerp(currentRot, desiredRot, Time.deltaTime * BodyTurnSpeed);

        transform.rotation = newRot;

        HeadTransform.localRotation = Quaternion.Slerp(HeadTransform.localRotation, mIdleHeadRotation, Time.deltaTime * HeadTurnSpeed);
    }

    public void TalkTo()
    {
        if (mCurrentState == NPCStates.TALKING_TO_PLAYER)
        {
            return;
        }

        mCurrentState = NPCStates.TALKING_TO_PLAYER;
        mCurrentTalkCycle = 0f;
        mMouthOpen = false;
    }

    public void EndConversation()
    {
        StopTalkState();
        //EventManager.Instance.Post(new DialogueEvent(this, DialogueEvent.DialogueEventTypes.END_CONVERSATION, string.Empty, this));
    }

    public void StopTalkState()
    {
        mCurrentState = NPCStates.IDLE;
        mMouthOpen = false;
        mCurrentTalkCycle = 0f;
        HeadTransform.GetComponent<MeshRenderer>().material.mainTexture = IdleFaceTexture;
    }
}
