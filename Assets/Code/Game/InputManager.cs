using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField]
    private float m_SensitivityX = 1f;
    [SerializeField]
    private float m_SensitivityY = 1f;
    [SerializeField]
    private bool m_InvertY = false;
    [SerializeField]
    private bool m_InvertX = false;

    private float xAxis = 0f;
    private float yAxis = 0f;
    private float xAxisRaw = 0f;
    private float yAxisRaw = 0f;

    public float XAxis { get { return xAxis; } }
    public float YAxis { get { return yAxis; } }
    public float XAxisRaw { get { return xAxisRaw; } }
    public float YAxisRaw { get { return yAxisRaw; } }

    private void LateUpdate()
    {
        xAxisRaw = Input.GetAxis("Mouse X");
        yAxisRaw = Input.GetAxis("Mouse Y");

        xAxis = (xAxisRaw * m_SensitivityX) * (m_InvertX ? -1 : 1);
        yAxis = (yAxisRaw * m_SensitivityY) * (m_InvertY ? -1 : 1);
    }
}
