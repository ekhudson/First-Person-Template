using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueAsset
{
    public string[] Lines = new string[0];

    private int mCurrentLineIndex = 0;

    public string GetCurrentLine()
    {
        return Lines[mCurrentLineIndex];
    }

    public int RemainingNumberOfLines()
    {
        return (Lines.Length - 1) - mCurrentLineIndex;
    }

    public void IncrementLines()
    {
        mCurrentLineIndex = Mathf.Clamp(mCurrentLineIndex + 1, 0, Lines.Length - 1);
    }

    public void ResetLines()
    {
        mCurrentLineIndex = 0;
    }
}

