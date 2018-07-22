using UnityEngine;
using System.Collections;

public class DialogueEvent : EventBase
{
    public enum DialogueEventTypes
    {
        NEW_LINE,
        LAST_LINE,
        END_CONVERSATION,
    }

    public readonly string DialogueText = string.Empty;
    public readonly DialogueEventTypes DialogueEventType = DialogueEventTypes.NEW_LINE;
    public readonly NPCController TalkingNPC;

    public DialogueEvent(object sender, DialogueEventTypes eventType, string dialogueText, NPCController talkingNPC) : base(Vector3.zero, sender)
    {
        DialogueText = dialogueText;
        DialogueEventType = eventType;
        TalkingNPC = talkingNPC;
    }
}