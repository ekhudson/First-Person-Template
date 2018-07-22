using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerableComponent : MonoBehaviour
{
    public enum TriggerType
    {
        OnAwake,
        OnStart,
        OnUpdate,
        OnEnable,
        OnDisable,
        OnDestroy,
        TriggerEnter,
        TriggerExit,
        CollisionEnter,
        CollisionExit,
        ForceTriggerOnly,
    }

    [SerializeField]
    private TriggerType m_TriggerOn = TriggerType.OnStart;

    [SerializeField]
    private bool m_OnlyTriggersOnce = false;

    [SerializeField]
    private bool m_DebugMode = false;

    [SerializeField]
    private UnityEvent m_OnTriggered;

    private bool m_HasBeenTriggered = false;

    protected virtual void OnTriggered()
    {
        m_OnTriggered.Invoke();
    }

    private void Awake()
    {
        if (ShouldTrigger(TriggerType.OnAwake))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void Start()
    {
        if (ShouldTrigger(TriggerType.OnStart))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void OnEnable()
    {
        if (ShouldTrigger(TriggerType.OnEnable))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void OnDisable()
    {
        if (ShouldTrigger(TriggerType.OnDisable))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void OnDestroy()
    {
        if (ShouldTrigger(TriggerType.OnDestroy))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void Update()
    {
        if (ShouldTrigger(TriggerType.OnUpdate))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void OnTriggerEnter()
    {
        if (ShouldTrigger(TriggerType.TriggerEnter))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void OnTriggerExit()
    {
        if (ShouldTrigger(TriggerType.TriggerExit))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void OnCollisionEnter()
    {
        if (ShouldTrigger(TriggerType.CollisionEnter))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private void OnCollisionExit()
    {
        if (ShouldTrigger(TriggerType.CollisionExit))
        {
            m_HasBeenTriggered = true;
            OnTriggered();
        }
    }

    private bool ShouldTrigger(TriggerType typeCheck)
    {
        if ((m_TriggerOn != typeCheck))
        {
            return false;
        }

        if (m_DebugMode)
        {
            Debug.Log("Checking if should trigger on " + typeCheck.ToString());
        }

        if (m_HasBeenTriggered && m_OnlyTriggersOnce)
        {
            if (m_DebugMode)
            {
                Debug.Log("Already triggered and only triggers once");
            }

            return false;
        }

        if (m_DebugMode)
        {
            Debug.Log("Triggering");
        }

        return true;
    }

    public void ForceTrigger(bool ignoreNumberOfUses)
    {
        if (ignoreNumberOfUses || (m_HasBeenTriggered && !m_OnlyTriggersOnce))
        {
            OnTriggered();
        }
    }
}