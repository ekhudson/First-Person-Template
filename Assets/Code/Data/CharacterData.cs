using UnityEngine;

[System.Serializable]
public class CharacterData : BaseData
{
    [SerializeField]
    private string m_FirstName = string.Empty;

    [SerializeField]
    private string m_LastName = string.Empty;

    [SerializeField]
    private string m_Age = string.Empty;

    [SerializeField]
    private string m_Citizenship = string.Empty;

    [SerializeField]
    private string m_Occupation = string.Empty;

    [SerializeField]
    [TextArea(3, 10)]
    private string m_Description = string.Empty;

    public string FirstName { get { return m_FirstName; } }
    public string LastName {  get { return m_LastName; } }
    public string Age { get { return m_Age; } }
    public string Citizenship { get { return m_Citizenship; } }
    public string Occupation { get { return m_Occupation; } }
    public string Description { get { return m_Description; } }

    public override string NotificationString()
    {
        return m_FirstName + " " + m_LastName;
    }
}