using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "soundScr", menuName = "ProgramacionAvanzadaRepo/soundScr", order = 5)]

public class SoundScriptable : ScriptableObject
{
    public int volumen = 100;
    public AudioClip audioclip;

}
