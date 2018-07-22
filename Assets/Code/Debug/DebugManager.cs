using UnityEngine;
using System.Collections;

public class DebugManager : Singleton<DebugManager>
{
    [SerializeField]
    private bool m_ShowFPS = false;

    [SerializeField]
    private KeyCode m_ShowFPSKeyCode = KeyCode.Slash;

    private float mSmoothedDeltaTime = 0f;

    private void Update()
    {
        if (Input.GetKeyUp(m_ShowFPSKeyCode))
        {
            m_ShowFPS = !m_ShowFPS;
        }

        mSmoothedDeltaTime = 0.9f * mSmoothedDeltaTime + 0.1f * Time.deltaTime;
    }

    private void OnGUI()
    {
        if (m_ShowFPS)
        {
            float smoothedFrameRate = 1f / mSmoothedDeltaTime;

            if (smoothedFrameRate < 10f)
            {
                GUI.color = Color.red;
            }
            else if (smoothedFrameRate < 29f)
            {
                GUI.color = Color.yellow;
            }
            else if (smoothedFrameRate < 59f)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.blue;
            }

            GUILayout.Label(string.Format("Clamped FPS: {0:0.0}", 1f / Time.deltaTime));
            GUILayout.Label(string.Format("Unity Smoothed FPS: {0:0.0}", 1f / Time.smoothDeltaTime));
            GUILayout.Label(string.Format("BBI Smoothed FPS: {0:0.0}", smoothedFrameRate));
            GUI.color = Color.white;           
        }
    }
}
