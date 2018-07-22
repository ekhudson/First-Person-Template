using UnityEngine;


public class ToggleGameObjects : TriggerableComponent
{
    public enum ToggleType
    {
        ENABLE,
        DISABLE,
        TOGGLE,
    }

    public GameObject[] GameObjectsToToggle;
    public ToggleType Type = ToggleType.TOGGLE;

    protected override void OnTriggered()
    {
        foreach (GameObject obj in GameObjectsToToggle)
        {
            if (obj == null)
            {
                continue;
            }
            else
            {
                ToggleGameObject(obj);
            }
        }
    }

    private void ToggleGameObject(GameObject obj)
    {
        if (Type == ToggleType.ENABLE)
        {
            obj.SetActive(true);
        }
        else if (Type == ToggleType.DISABLE)
        {
            obj.SetActive(false);
        }
        else if (Type == ToggleType.TOGGLE)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
