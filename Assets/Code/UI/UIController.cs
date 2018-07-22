using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [Header("Crosshair")]
    [SerializeField]
    private Image m_Crosshair;
    [SerializeField]
    private Sprite m_DefaultCrosshair;
    [SerializeField]
    private Sprite m_NotebookCrosshair;
    [SerializeField]
    private Text m_MouseOverTextInfoField;

    private const string m_UnidentifiedMouseOverText = "?";

    public static string UnidentifiedMouseOverText
    {
        get
        {
            return m_UnidentifiedMouseOverText;
        }
    }

    private void SetCrosshairSprite(Sprite sprite)
    {
        m_Crosshair.sprite = sprite;
    }

    public void SetMouseOverTextInfo(bool enabled, string textToShow)
    {
        m_MouseOverTextInfoField.enabled = enabled;
        m_MouseOverTextInfoField.text = textToShow;
    }

    public void SetNotebookOpen()
    {
        SetCrosshairSprite(m_NotebookCrosshair);
    }

    public void SetNotebookClosed()
    {
        SetCrosshairSprite(m_DefaultCrosshair);
    }
}
