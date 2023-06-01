using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{

    public SoundScriptable[] musicArray;
    public List<MusicInstance> allMusic = new List<MusicInstance>();

    AudioSource audiosource;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        musicArray = Resources.LoadAll<SoundScriptable>("Music");

        for (int i = 0; i < musicArray.Length; i++)
        {
            allMusic.Add(new MusicInstance(musicArray[i]));
        }

        audiosource.clip = allMusic[0]._audioclip;
        PlayMusic();
        StartCoroutine(NextMusic(allMusic[0]._audioclip.length));
    }

    void PlayMusic()
    {
        audiosource.time = 0;
        audiosource.Play();
    }

    int indexMusic;

    IEnumerator NextMusic(float timeSong)
    {
        yield return new WaitForSecondsRealtime(timeSong);
        Debug.Log("Next Music!!");
        indexMusic++;
        if (indexMusic == allMusic.Count) indexMusic = 0;
        audiosource.clip = allMusic[indexMusic]._audioclip;
        PlayMusic();
        StartCoroutine(NextMusic(allMusic[indexMusic]._audioclip.length));
    }
}
