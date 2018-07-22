using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class SubjectKeyParser : MonoBehaviour
{
    private const string kLinkPattern = @"</?[^>]+>";
    private static char[] kLinkTrimChars = { '<', '>' };
    private const string kSubjectPairPattern = @"{/?[^}]+}";
    private static char[] kSubjectPairTrimChars = { '{', '}' };
    private const char kSeparatorChar = ':';
    private const string kCharacterTypeString = "Character";
    private const string kObjectTypeString = "Object";
    private const string kLocationTypeString = "Location";

    public static ClueSubjectLink[] ParseClueText(string input)
    {
        MatchCollection matches = Regex.Matches(input, kLinkPattern);

        ClueSubjectLink[] links = new ClueSubjectLink[matches.Count];

        for(int i = 0; i < matches.Count; i++)
        {
            string linkText = matches[i].Value.Trim(kLinkTrimChars);
            string pairText = Regex.Match(linkText, kSubjectPairPattern).Value;
            linkText = linkText.Replace(pairText, "");
            pairText = pairText.Trim(kSubjectPairTrimChars);
            SubjectDatabase.SubjectType subjectType = SubjectDatabase.SubjectType.CHARACTER;
            ParseSubjectPair(pairText, out subjectType, out pairText);
            links[i] = new ClueSubjectLink(linkText, subjectType, pairText, matches[i].Index);
        }

        return links;
    }

    public static void ParseSubjectPair(string input, out SubjectDatabase.SubjectType type, out string key)
    {
        string[] splitStrings = input.Split(kSeparatorChar);

        string classString = splitStrings[0];
        type = SubjectDatabase.SubjectType.CHARACTER; //default

        switch (classString)
        {
            case kCharacterTypeString:
                type = SubjectDatabase.SubjectType.CHARACTER;
            break;

            case kObjectTypeString:
                type = SubjectDatabase.SubjectType.OBJECT;
            break;

            case kLocationTypeString:
                type = SubjectDatabase.SubjectType.LOCATION;
            break;
        }

        key = splitStrings.Length == 2 ? splitStrings[1] : string.Empty;
    }
}
