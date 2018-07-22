using UnityEngine;

public class InteractableDoor : InteractableObject
{
    private enum DoorStartStates
    {
        START_OPEN,
        START_CLOSED,
    }

    [SerializeField]
    private DoorStartStates m_StartingState = DoorStartStates.START_CLOSED;
    [SerializeField]
    private Animator m_AnimatorReference;

    protected override void Start()
    {
        if (m_AnimatorReference == null)
        {
            enabled = false;
            return;
        }

        //mAnimatorReference.SetTrigger(m_StartingState == DoorStartStates.START_OPEN ? "Start Open" : "Start Closed");
    }

    public override void OnInteract()
    {
        if (m_AnimatorReference == null)
        {
            return;
        }

        m_AnimatorReference.SetBool("Player Outside", CheckIfPlayerIsOutside());
        m_AnimatorReference.SetTrigger("Interact");

        base.OnInteract();
    }

    private bool CheckIfPlayerIsOutside()
    {
        Vector3 directionFromPlayer = PlayerController.GlobalPlayerReference.transform.position - transform.position;
        directionFromPlayer.Normalize();

        return Vector3.Dot(transform.forward, directionFromPlayer) < 0;
    }
}
