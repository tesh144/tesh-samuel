using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class NewSpeech
{
    public string speechTitle;
    public List<SpeechClip> subbedClips;
}

[System.Serializable]
public class SpeechClip
{
    public AudioClip clip;
    [TextArea (1 , 10)]
    [FoldoutGroup("Subtitle Settings")]
    public string subtitle;
    [Space]
    [FoldoutGroup("Subtitle Settings")]
    public Text speechText;
}
[System.Serializable]
public class SpeechManager : MonoBehaviour
{
    [TabGroup("Speech Set-up")]
    public SpeechEvents speechEvents;
    [TabGroup("Speech Set-up")]
    public List<NewSpeech> speechEv;

    [TabGroup("Audio / visual Set-up")]
    public AudioSource AS;
    //[TabGroup("Audio / visual Set-up")]
    //public Text speechText;
    [TabGroup("Audio / visual Set-up")]
    public Animator speechAnimator;

    [TabGroup("Debug Values")]
    [ReadOnly] public NewSpeech speechPlaying;
    [TabGroup("Debug Values")]
    [ReadOnly] public int speechInt;
    [TabGroup("Debug Values")]
    [ReadOnly] public bool isPlaying;

    private void OnDisable()
    {
        StopReading();
    }

    public void StartRepeating()
    {
        InvokeRepeating("CheckForClipChange", 2f, 0.75f);
        //Debug.Log("Scanning On");
    }

    public void StopReading()
    {
        CancelInvoke("CheckForClipChange");
        //Debug.Log("Scanning Off");
    }

    void CheckForClipChange()
    {
        if (isPlaying)
        {
            if (!AS.isPlaying)
            {
                speechInt++;
                
                if (speechInt < speechPlaying.subbedClips.Count)
                {
                    AS.Stop();
                    AS.clip = speechPlaying.subbedClips[speechInt].clip;
                    speechPlaying.subbedClips[speechInt].speechText.text = speechPlaying.subbedClips[speechInt].subtitle;
                    AS.Play();

                    Debug.Log("Speech int: " + speechInt);

                    if (speechAnimator != null)
                        speechAnimator.SetTrigger("start");
                }
                else if(speechInt >= speechPlaying.subbedClips.Count)
                {
                    foreach(NewSpeechEvents nse in speechEvents.newSpeechEvent)
                    {
                        if(nse.speechTitle == speechPlaying.speechTitle && isPlaying)
                        {
                            nse.speechEvt.Invoke();
                            StopReading();
                        }
                    }
                    isPlaying = false;
                    //speechInt = 0;
                }
            }
        }
    }

    public void Skip()
    {
        speechInt = speechPlaying.subbedClips.Count;
        AS.Stop();
    }

    public void playclip(string eventString)
    {
        StartRepeating();
        foreach (NewSpeech ns in speechEv)
        {
            if(ns.speechTitle == eventString)
            {
                speechPlaying = ns;
                speechInt = 0;
                AS.clip = ns.subbedClips[speechInt].clip;
                speechPlaying.subbedClips[speechInt].speechText.text = ns.subbedClips[speechInt].subtitle;

                AS.Play();

                isPlaying = true;
                return;
            }
        }
    }
}
