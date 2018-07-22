using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClueDatabaseDataAsset))]
public class ClueDataEditor : Editor
{
    private ClueDatabaseDataAsset mTarget = null;

    public ClueDatabaseDataAsset Target
    {
        get
        {
            if (mTarget == null)
            {
                mTarget = (target as ClueDatabaseDataAsset);
            }

            return mTarget;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        string testString = Target.Data.Clues[0].ClueString;
        ClueSubjectLink[] links = SubjectKeyParser.ParseClueText(testString);

        foreach(ClueSubjectLink link in links)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(link.LinkText);
            GUILayout.Label(link.SubjectKey);
            GUILayout.Label(link.SubjectPosition.ToString());
            GUILayout.EndHorizontal();

            switch(link.SubjectType)
            {
                case SubjectDatabase.SubjectType.CHARACTER:

                    CharacterData data = Target.SubjectDatabaseReference.Data.RetrieveSubject<CharacterData>(link.SubjectType, link.SubjectKey);
                    GUILayout.Label(data.Description);

                    break;
                case SubjectDatabase.SubjectType.LOCATION:

                    LocationData locData = Target.SubjectDatabaseReference.Data.RetrieveSubject<LocationData>(link.SubjectType, link.SubjectKey);
                    GUILayout.Label(locData.Description);

                    break;
                case SubjectDatabase.SubjectType.OBJECT:

                    ObjectData objData = Target.SubjectDatabaseReference.Data.RetrieveSubject<ObjectData>(link.SubjectType, link.SubjectKey);
                    GUILayout.Label(objData.Description);

                    break;
            }

            
        }
    }
}
