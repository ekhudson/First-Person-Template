using UnityEngine;
using UnityEngine.UI;

public class PersonDataEntry : MonoBehaviour
{
    [SerializeField]
    private Text m_RoleField;
    [SerializeField]
    private Text m_NameField;

    public void Initialize(PersonData personData)
    {
        m_RoleField.text = personData.Role;
        m_NameField.text = personData.Name;
    }
}
