using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInteractionComponent : MonoBehaviour 
{
    [System.Serializable]
    public class ObjectSearchSettings
    {
        public int Layer = 0;
        public float MinDistance = 0f;
        public float MaxDistance = 0f;
        [Range(-1f, 1f)]
        public float SearchDotProductThreshold = 0.5f;
        public float ScreenSpaceSearchRadius = 10f;

        public float MinDistanceSqr
        {
            get
            {
                return MinDistance * MinDistance;
            }
        }

        public float MaxDistanceSqr
        {
            get
            {
                return MaxDistance * MaxDistance;
            }
        }
    }
    [Header("References")]
    [SerializeField]
    private UIController m_UIControllerReference;
    [SerializeField]
    private CustomSensorVolume m_PlayerSensorVolume;

    [Header("General Player Settings")]
    public Camera PlayerCamera;
    public float HighlightScaleFactor = 1.1f;
    public Transform RightHandTransform;
    public Vector3 RightHandOffset = Vector3.zero;
    [Tooltip("Distance directly in front of the camera to hold an object while examining it")]
    public float ExamineOffset = 1f;
    public float ExamineRotationSpeed = 1f;

    [Header("Pickup Settings")]
    public ObjectSearchSettings PickupSearchSettings;
    public MeshFilter ObjectHighlighter;
    public Material InteractHighlightMaterial;
    public Material PutdownValidHighlightMaterial;
    public Material PutdownNotValidHighlightMaterial;
    public LayerMask PutdownLayerMask = new LayerMask();
    public LayerMask PutdownBlockMask = new LayerMask();
    public float PutdownRayDistance = 1f;

    [Header("Throw Settings")]
    public float MinThrowForce = 2f;
    public float MaxThrowForce = 5f;
    public float MaxThrowChargeTime = 2f;
    public float PullbackDistance = 1f;
    public float ThrowRaycastDistance = 30f;
    public LayerMask ThrowRaycastMask;

    [Header("Character Interaction Settings")]
    public Image TalkBubbleImage;
    public Vector3 TalkBubbleScreenOffset = Vector3.zero;
    public ObjectSearchSettings CharacterSearchSettings;
    [Range(-1f, 1f)]
    public float TalkHeadThreshold = 0.5f;

    [Header("Key Bindings")]
    public KeyCode Run = KeyCode.LeftShift;
    public KeyCode Crouch = KeyCode.LeftControl;

    private InteractableObject mCurrentNearestInteractable;
    private PickupObject mCurrentNearestPickup;
    private PickupObject mCurrentHeldPickup;
    private bool mExaminingObject = false;
    //private Vector3 mInitialPlayerExamViewDirection = Vector3.zero;
    private Vector3 mExamRotation = Vector3.zero;
    private Transform mTransform;
    private float mLastClosestPickupDistanceSquared = float.MaxValue;
    private Vector3 mRightHandOriginalLocalPosition = Vector3.zero;
    private NPCController mCurrentNearestCharacter = null;
    private bool mCurrentlyInConversation = false;
    private Ray mPutdownRay = new Ray();
    private RaycastHit mPutdownHit = new RaycastHit();
    private Matrix4x4 mPutdownMatrix;
    private bool mHaveCurrentValidPutdownPosition = false;
    private Vector3 mCurrentPutdownPosition = Vector3.zero;
    private Quaternion mCurrentPutdownRotation = Quaternion.identity;
    private float mCurrentThrowChargeTime = 0f;
    private Vector2 mCentreScreenVector = new Vector2(0.5f, 0.5f);
    private InteractableObject[] mCurrentNearInteractableObjects;   

    public bool CurrentlyExaminingObject
    {
        get
        {
            return mExaminingObject;
        }
    }

    public InteractableObject[] CurrentNearInteractableObjects
    {
        get
        {
            return mCurrentNearInteractableObjects;
        }
    }

    private void Start()
    {
        ObjectHighlighter.gameObject.SetActive(false);
        mTransform = transform;
        mRightHandOriginalLocalPosition = RightHandTransform.localPosition;
    }

    public void Update()
    {
        mCurrentNearInteractableObjects = new InteractableObject[0];

        if (mCurrentHeldPickup != null)
        {
            DoPutDownCheck(out mCurrentPutdownPosition, out mCurrentPutdownRotation, out mHaveCurrentValidPutdownPosition);
        }
    }

    public void UpdatePickupPosition()
    {
        if (!mExaminingObject)
        {
            mCurrentHeldPickup.transform.position = RightHandTransform.transform.position;
            mCurrentHeldPickup.transform.localRotation = RightHandTransform.transform.rotation;
        }
        else
        {
            mCurrentHeldPickup.transform.position = MainCameraController.Instance.MainCameraReference.transform.position + (MainCameraController.Instance.MainCameraReference.transform.forward * ExamineOffset);
        }
    }

    public void LateUpdate()
    {
        if (mCurrentHeldPickup != null)
        {
            UpdatePickupPosition();
        }

        SearchForCharacters();

        if (mCurrentNearestCharacter == null) //Nearby talkable characters takes precedence over picking objects up
        {
            SearchForInteractablesAndPickups();
        }

        GetInput();
    }

    private void SearchForCharacters()
    {
        mCurrentNearestCharacter = FindNearestObjectOfType<NPCController>(m_PlayerSensorVolume, CharacterSearchSettings);

        if (mCurrentNearestCharacter != null)
        {
            if (mCurrentNearestCharacter.CurrentState == NPCController.NPCStates.TALKING_TO_PLAYER || mCurrentNearestCharacter.CurrentState == NPCController.NPCStates.TALKING_NON_INTERRUPTIBLE)
            {
                //TalkBubbleImage.gameObject.SetActive(false);
            }
            else
            {
                ShowTextBubbleAtCharacter(mCurrentNearestCharacter);
                HighlightUtility.HighlightObject(mCurrentNearestCharacter.gameObject);
            }
        }
        //else if (TalkBubbleImage.gameObject.activeSelf)
        //{
        //    TalkBubbleImage.gameObject.SetActive(false);
        //}
    }

    private void SearchForInteractablesAndPickups()
    {
        mCurrentNearestInteractable = FindNearestObjectOfType<InteractableObject>(m_PlayerSensorVolume, PickupSearchSettings);
        mCurrentNearestPickup = mCurrentNearestInteractable is PickupObject ? mCurrentNearestInteractable as PickupObject : null;

        if (mCurrentNearestInteractable != null)
        {
            HighlightUtility.HighlightObject(mCurrentNearestInteractable.gameObject, mCurrentNearestInteractable.GetRenderer().bounds.center, InteractHighlightMaterial);

            if (mCurrentNearestInteractable.SubjectComponentReference != null)
            {
                bool identified = GameDataManager.Instance.PlayerState.GetSubjectState(mCurrentNearestInteractable.SubjectComponentReference.PairedSubjectKey) != null;
                string displayString = identified ? GameDataManager.Instance.SubjectDatabase.Data.RetrieveSubject<BaseData>(mCurrentNearestInteractable.SubjectComponentReference.PairedSubjectKey).NotificationString() : UIController.UnidentifiedMouseOverText;
                m_UIControllerReference.SetMouseOverTextInfo(true, displayString);
            }
        }
        else
        {
            m_UIControllerReference.SetMouseOverTextInfo(false, string.Empty);
        }
    }

    private void GetExaminationInput()
    {
        Vector3 up = MainCameraController.Instance.MainCameraReference.transform.up;
        Vector3 right = MainCameraController.Instance.MainCameraReference.transform.right;

        float xAmount = 0f;
        float yAmount = 0f;

        xAmount += Input.GetKey(KeyCode.A) ? 1f : 0f;
        xAmount += Input.GetKey(KeyCode.D) ? -1f : 0f;
        yAmount += Input.GetKey(KeyCode.W) ? 1f : 0f;
        yAmount += Input.GetKey(KeyCode.S) ? -1f : 0f;

        xAmount = Mathf.Clamp(xAmount, -1f, 1f) * (ExamineRotationSpeed * Time.deltaTime);
        yAmount = Mathf.Clamp(yAmount, -1f, 1f) * (ExamineRotationSpeed * Time.deltaTime);

        //mCurrentHeldPickup.transform.Rotate(up, xAmount, Space.World);
        //mCurrentHeldPickup.transform.Rotate(right, yAmount, Space.World);

        mExamRotation.x += xAmount;
        mExamRotation.y += yAmount;

        if (mExamRotation.x > 360f)
        {
            mExamRotation.x = 0f;
        }
        else if (mExamRotation.x < 0f)
        {
            mExamRotation.x = 360f;
        }

        if (mExamRotation.y > 360)
        {
            mExamRotation.y = 0f;
        }
        else if (mExamRotation.y < 0f)
        {
            mExamRotation.y = 360f;
        }

        mCurrentHeldPickup.transform.rotation = MainCameraController.Instance.MainCameraReference.transform.rotation;
        mCurrentHeldPickup.transform.Rotate(MainCameraController.Instance.MainCameraReference.transform.right, mExamRotation.y, Space.World);
        mCurrentHeldPickup.transform.Rotate(MainCameraController.Instance.MainCameraReference.transform.up, mExamRotation.x, Space.World);

        //mCurrentHeldPickup.transform.rotation *= Quaternion.FromToRotation(mInitialPlayerExamViewDirection, MainCameraController.Instance.MainCameraReference.transform.forward);
    }

    public void GetInput()
    {
        //MOUSE 2
        if (mCurrentHeldPickup != null && Input.GetMouseButton(1))
        {
            GetExaminationInput();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (mCurrentHeldPickup == null)
            {
                MainCameraController.Instance.EnableZoom(true);
                PlayerController.GlobalPlayerReference.FirstPersonController.SetState("Zoomed", true);
            }
            else
            {
                StartExaminingObject();
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (mCurrentHeldPickup == null)
            {
                MainCameraController.Instance.EnableZoom(false);
                PlayerController.GlobalPlayerReference.FirstPersonController.SetState("Zoomed", false);
            }
            else
            {
                StopExaminingObject();
            }
        }

        //TODO: Interact and Pickup are different paths right now. Should probably JUST be Interact, which, for Pickups, picks them up
        //MOUSE 1
        if (Input.GetMouseButtonUp(0))
        {
            if (mCurrentNearestCharacter != null && mCurrentNearestCharacter.CurrentState == NPCController.NPCStates.IDLE)
            {
                mCurrentNearestCharacter.TalkTo();
            }
            else if (mCurrentNearestInteractable != null && mCurrentNearestInteractable != mCurrentNearestPickup && !mExaminingObject)
            {
                mCurrentNearestInteractable.OnInteract();
            }
            else if (mCurrentNearestPickup != null && mCurrentHeldPickup == null)
            {
                mCurrentHeldPickup = mCurrentNearestPickup;
                mCurrentNearestPickup = null;
                mCurrentHeldPickup.OnInteract();

                //TODO: Get objects to coordinate with new uFPS states
                //mRigidbodyFirstPersonController.movementSettings.MovementDebuffPercent = mCurrentHeldPickup.SlowsPlayerByPercent;
            }
            else if (mCurrentHeldPickup != null && !mExaminingObject)
            {
                if (mHaveCurrentValidPutdownPosition)
                {
                    mCurrentHeldPickup.PlaceDown(mCurrentPutdownPosition, mCurrentPutdownRotation);
                    //mRigidbodyFirstPersonController.movementSettings.MovementDebuffPercent = 0f;
                    mCurrentThrowChargeTime = 0f;
                    mCurrentHeldPickup = null;
                }
                else
                {
                    if (mCurrentThrowChargeTime > 0 && mCurrentHeldPickup != null)
                    {
                        mCurrentHeldPickup.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 1f);

                        Vector3 throwDirection = GetThrowDirection();

                        mCurrentHeldPickup.Throw(throwDirection * Mathf.Lerp(MinThrowForce, MaxThrowForce, mCurrentThrowChargeTime / MaxThrowChargeTime));
                        //mRigidbodyFirstPersonController.movementSettings.MovementDebuffPercent = 0f;
                        mCurrentHeldPickup = null;
                    }

                    RightHandTransform.localPosition = mRightHandOriginalLocalPosition;
                    mCurrentThrowChargeTime = 0f;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (mCurrentHeldPickup != null && !mExaminingObject)
            {
                if (mCurrentThrowChargeTime < MaxThrowChargeTime)
                {
                    mCurrentThrowChargeTime += Time.deltaTime;
                }
                else if (mCurrentThrowChargeTime > MaxThrowChargeTime)
                {
                    mCurrentThrowChargeTime = MaxThrowChargeTime;
                }

                RightHandTransform.localPosition = mRightHandOriginalLocalPosition + (-Vector3.forward * Mathf.Lerp(0f, PullbackDistance, mCurrentThrowChargeTime / MaxThrowChargeTime));
            }
        }

        GetRunInput();
        GetCrouchInput();
    }

    private void GetRunInput()
    {
        PlayerController.GlobalPlayerReference.FirstPersonController.SetState("Running", Input.GetKey(Run));
        PlayerController.GlobalPlayerReference.CameraController.SetState("Running", Input.GetKey(Run));
    }

    private void GetCrouchInput()
    {
        PlayerController.GlobalPlayerReference.FirstPersonController.SetState("Crouching", Input.GetKey(Crouch));
        PlayerController.GlobalPlayerReference.CameraController.SetState("Crouching", Input.GetKey(Crouch));
    }

    private void SetInConversation(bool inConversation)
    {
        mCurrentlyInConversation = inConversation;

        if (mCurrentlyInConversation)
        {
            PlayerController.GlobalPlayerReference.FirstPersonController.SetState("InConversation", true);
        }
        else
        {
            PlayerController.GlobalPlayerReference.FirstPersonController.SetState("InConversation", false);
        }
    }

    private void ShowTextBubbleAtCharacter(NPCController npcController)
    {
        if (npcController != null)
        {
            TalkBubbleImage.gameObject.SetActive(true);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(npcController.HeadTransform.position);
            screenPos += TalkBubbleScreenOffset;
            TalkBubbleImage.rectTransform.position = screenPos;
        }
    }

    public T FindNearestObjectOfType<T>(CustomSensorVolume sensorVolume, ObjectSearchSettings searchSettings) where T : MonoBehaviour
    {
        GameObject[] nearbyCandidates = sensorVolume.GetObjectsOfLayer(searchSettings.Layer);

        if (nearbyCandidates == null || nearbyCandidates.Length == 0)
        {
            return null;
        }

        List<T> nearbyObjects = new List<T>();

        for(int i = 0; i < nearbyCandidates.Length; i++)
        {
            T component = nearbyCandidates[i].GetComponent<T>();

            if (component != null)
            {
                nearbyObjects.Add(component);
            }           
        }

        //TODO: This is hacky and just for gizmos. Need to get this out of here. - Yes, but how?
        if (typeof(T) == typeof(InteractableObject))
        {
            mCurrentNearInteractableObjects = nearbyObjects.ToArray() as InteractableObject[];
        }

        Vector3 directionFromPlayer = Vector3.zero;
        Vector2 viewportPos = Vector2.zero;
        mLastClosestPickupDistanceSquared = float.MaxValue;

        T currentNearestObject = null;

        foreach (T obj in nearbyObjects)
        {
            //This is a mess. We're doing this twice; here and in IsPositionValidForInteraction
            if (obj == null || !PositionIsValidForInteraction(obj.GetComponent<Collider>(), searchSettings))
            {
                continue;
            }

            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (obj.GetComponent<Collider>().Raycast(ray, out hit, searchSettings.MaxDistance))
            {
                //We are looking directly at this object
                currentNearestObject = obj;
                break;
            }
            else //see if we're looking sort of close to the object
            {
                viewportPos = PlayerCamera.WorldToViewportPoint(obj.transform.position);
                float viewportDistanceSquared = (viewportPos - mCentreScreenVector).sqrMagnitude;

                if (viewportDistanceSquared < mLastClosestPickupDistanceSquared)
                {
                    mLastClosestPickupDistanceSquared = viewportDistanceSquared;
                    currentNearestObject = obj;
                }
            }            
        }

        return currentNearestObject;
    }

    public void HighlightNearbyPickups()
    {
        InteractableObject obj = FindNearestObjectOfType<InteractableObject>(m_PlayerSensorVolume, PickupSearchSettings);

        if (obj != mCurrentNearestPickup)
        {
            if (obj is PickupObject)
            {
                mCurrentNearestPickup = (PickupObject)FindNearestObjectOfType<InteractableObject>(m_PlayerSensorVolume, PickupSearchSettings);
            }
        }

        if (mCurrentNearestPickup != null)
        {
            HighlightUtility.HighlightAtPosition(mCurrentNearestPickup.PickupMesh, mCurrentNearestPickup.transform.position, mCurrentNearestPickup.transform.rotation, mCurrentNearestPickup.transform.lossyScale);  
        }
    }

    public void HighlightNearbyObjects<T>(CustomSensorVolume sensor, ObjectSearchSettings searchSettings) where T : MonoBehaviour
    {
        T nearestObj = null;

        nearestObj = FindNearestObjectOfType<T>(sensor, searchSettings);

        if (nearestObj != null)
        {
            HighlightUtility.HighlightAtPosition(mCurrentNearestPickup.PickupMesh, mCurrentNearestPickup.transform.position, mCurrentNearestPickup.transform.rotation, mCurrentNearestPickup.transform.lossyScale);
        }
    }

    public Vector3 GetThrowDirection()
    {
        Vector3 throwDirection = (Camera.main.transform.forward);

        Vector3 throwPoint = GetThrowPoint();

        throwDirection = (throwPoint - Camera.main.transform.position).normalized;

        return throwDirection;
    }

    public Vector3 GetThrowPoint()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, ThrowRaycastDistance, ThrowRaycastMask))
        {
            return hit.point;
        }
        else
        {
            return (Camera.main.transform.position + (Camera.main.transform.forward * ThrowRaycastDistance));
        }
    }

    public void DoPutDownCheck(out Vector3 putdownPosition, out Quaternion putdownRotation, out bool haveCurrentValidPutdownPosition)
    {
        mPutdownRay = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);
        putdownPosition = Vector3.zero;
        putdownRotation = Quaternion.identity;
        haveCurrentValidPutdownPosition = false;
        Material materialToUse = null;

        if (Physics.Raycast(mPutdownRay, out mPutdownHit, PutdownRayDistance, PutdownLayerMask))
        {
            float normalDot = Vector3.Dot(mCurrentHeldPickup.PutdownNormal, mPutdownHit.normal);
            putdownPosition = mPutdownHit.point;
            putdownPosition += (mPutdownHit.normal * (mCurrentHeldPickup.GetMesh().bounds.extents.y * mCurrentHeldPickup.transform.lossyScale.y));
            putdownRotation = Quaternion.FromToRotation(Vector3.up, mPutdownHit.normal);
            mPutdownMatrix = Matrix4x4.TRS(putdownPosition, putdownRotation, mCurrentHeldPickup.transform.lossyScale);

            if (normalDot >= mCurrentHeldPickup.PutdownDotThreshold) //TODO: Figure out a way to determine when it would be bad to place the object
            {  
                haveCurrentValidPutdownPosition = true;
                materialToUse = PutdownValidHighlightMaterial;
            }
            else
            {
                haveCurrentValidPutdownPosition = false;
            }
        }

        if (materialToUse != null)
        {
            HighlightUtility.HighlightAtPosition(mCurrentHeldPickup.GetMesh(), materialToUse, putdownPosition, putdownRotation, mCurrentHeldPickup.transform.lossyScale);
        }
    }

    public void OnDrawGizmos()
    {
        Vector3 throwPoint = GetThrowPoint();

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(throwPoint, 0.1f);
        Gizmos.color = Color.white;

        if (mCurrentNearInteractableObjects == null || mCurrentNearInteractableObjects.Length <= 0)
        {
            return;
        }

        foreach(InteractableObject obj in mCurrentNearInteractableObjects)
        {
            if (PositionIsValidForInteraction(obj.GetComponent<Collider>(), PickupSearchSettings))
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.gray;
            }

            Gizmos.DrawWireCube(obj.GetRenderer().bounds.center, obj.GetRenderer().bounds.size);
        }

        Gizmos.color = Color.cyan;

        if (mCurrentHeldPickup != null)
        {
            Gizmos.DrawWireCube(mCurrentPutdownPosition, mCurrentHeldPickup.GetRenderer().bounds.size);
        }

        Gizmos.color = Color.white;
    }

    public bool PositionIsValidForInteraction(Collider col, ObjectSearchSettings searchSettings)
    {
        Vector3 directionFromPlayer = col.transform.position - mTransform.position;
        float distanceFromPlayerSquared = directionFromPlayer.sqrMagnitude;

        if (distanceFromPlayerSquared > searchSettings.MaxDistanceSqr)
        {
            return false;
        }

        if (Vector3.Dot(PlayerCamera.transform.forward, directionFromPlayer.normalized) <= searchSettings.SearchDotProductThreshold)
        {
            return false;
        }

        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (col.Raycast(ray, out hit, searchSettings.MaxDistance))
        {
            return true;
        }

        Vector2 viewportPos = PlayerCamera.WorldToViewportPoint(col.transform.position);
        float viewportDistanceSquared = (viewportPos - mCentreScreenVector).sqrMagnitude;

        if (viewportDistanceSquared >= searchSettings.ScreenSpaceSearchRadius)
        {
            return false;
        }

        return true;
    }

    private void StartExaminingObject()
    {
        mExaminingObject = true;
        mCurrentHeldPickup.transform.rotation = MainCameraController.Instance.MainCameraReference.transform.rotation;
        //mInitialPlayerExamViewDirection = MainCameraController.Instance.MainCameraReference.transform.forward;
        mExamRotation = Vector3.zero;
        PlayerController.GlobalPlayerReference.FirstPersonController.SetState("Inspecting", true);
        GameDataManager.Instance.PlayerState.TryAddNewSubject(mCurrentHeldPickup.SubjectComponentReference.SubjectType, mCurrentHeldPickup.SubjectComponentReference.SubjectKey);
    }

    private void StopExaminingObject()
    {
        mExaminingObject = false;
        PlayerController.GlobalPlayerReference.FirstPersonController.SetState("Inspecting", false);
    }
}
