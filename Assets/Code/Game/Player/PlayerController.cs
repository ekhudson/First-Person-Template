using UnityEngine;

public class PlayerController : CustomCharacterController 
{
    private vp_FPController mFirstPersonController;
    private PlayerInteractionComponent mInteractionController;
    [SerializeField]
    private vp_FPCamera mCameraController;

    private static PlayerController mGlobalPlayerReference;

    public static PlayerController GlobalPlayerReference
    {
        get
        {
            return mGlobalPlayerReference;
        }
    }

    public vp_FPController FirstPersonController {  get { return mFirstPersonController; } }
    public PlayerInteractionComponent InteractionController { get { return mInteractionController; } }
    public vp_FPCamera CameraController { get { return mCameraController; } }

    private void Start()
    {
        mFirstPersonController = GetComponent<vp_FPController>();
        mInteractionController = GetComponent<PlayerInteractionComponent>();
        mGlobalPlayerReference = this;
    }
}
