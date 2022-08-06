using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentences
{
    [TextArea]
    public string sentence;
    public SentencesType sentencesType;
    public VoiceName voiceName;

}

public enum SentencesType
{
    WaitForInput, WaitForVoice, WaitForVerticalSlide, WaitForHorizontalSlide
}

public enum VoiceName
{
    Matthew, Amy, Brian, Camila
}