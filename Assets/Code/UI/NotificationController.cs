using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_NotificationPanel;
    [SerializeField]
    private Text m_NotificationText;
    [SerializeField]
    private AnimationCurve m_NotificationEntranceCurve;
    [SerializeField]
    private AnimationCurve m_NotificationExitCurve;
    [SerializeField]
    private Vector2 m_OffscreenPosition = Vector2.zero;
    [SerializeField]
    private Vector2 m_OnScreenPosition = Vector2.zero;
    [SerializeField]
    private float m_AnimInTime = 1f;
    [SerializeField]
    private float m_AnimOutTime = 1f;
    [SerializeField]
    private float m_HoldTime = 4f;

    private float mCurrentTime = 0f;
    private States mState = States.IDLE;
    
    private enum States
    {
        IDLE,
        ANIM_IN,
        HOLD,
        ANIM_OUT,
    }

    private void Start()
    {
        m_NotificationPanel.anchoredPosition = m_OffscreenPosition;
        EventManager.Instance.AddHandler<SubjectDiscoveredEvent>(OnSubjectDiscoveredEvent);
    }

    private void OnSubjectDiscoveredEvent(object sender, SubjectDiscoveredEvent subjectDiscoveredEvent)
    {
        string notificationString = GameDataManager.Instance.SubjectDatabase.Data.RetrieveSubject<BaseData>(subjectDiscoveredEvent.SubjectType, subjectDiscoveredEvent.SubjectKey).NotificationString();
        m_NotificationText.text = notificationString;
        mCurrentTime = 0f;
        mState = States.ANIM_IN;
    }

    private void Update()
    {
        switch(mState)
        {
            case States.IDLE:

            break;

            case States.ANIM_IN:

                mCurrentTime = Mathf.Clamp(mCurrentTime += Time.deltaTime, 0f, m_AnimInTime);

                m_NotificationPanel.anchoredPosition = Vector2.Lerp(m_OffscreenPosition, m_OnScreenPosition, m_NotificationEntranceCurve.Evaluate(mCurrentTime / m_AnimInTime));
                
                if (mCurrentTime >= m_AnimInTime)
                {
                    mCurrentTime = 0f;
                    mState = States.HOLD;
                    return;
                }

            break;

            case States.HOLD:

                mCurrentTime = Mathf.Clamp(mCurrentTime += Time.deltaTime, 0f, m_HoldTime);

                if (mCurrentTime >= m_HoldTime)
                {
                    mCurrentTime = 0f;
                    mState = States.ANIM_OUT;
                    return;
                }

            break;

            case States.ANIM_OUT:

                mCurrentTime = Mathf.Clamp(mCurrentTime += Time.deltaTime, 0f, m_AnimOutTime);

                m_NotificationPanel.anchoredPosition = Vector2.Lerp(m_OnScreenPosition, m_OffscreenPosition, m_NotificationEntranceCurve.Evaluate(mCurrentTime / m_AnimOutTime));

                if (mCurrentTime >= m_AnimOutTime)
                {
                    mCurrentTime = 0f;
                    mState = States.IDLE;
                    return;
                }

                break;
        }
    }
}
