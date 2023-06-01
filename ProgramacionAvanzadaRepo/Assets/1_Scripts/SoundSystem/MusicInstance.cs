using UnityEngine;

public class MusicInstance 
{
    int _volumen;

    public AudioClip _audioclip;
    public MusicInstance(SoundScriptable soundScr)
    {
        _volumen = soundScr.volumen;
        _audioclip = soundScr.audioclip;
    }

}
