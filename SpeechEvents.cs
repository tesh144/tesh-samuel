using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class NewSpeechEvents
{
    public string speechTitle;
    public UnityEvent speechEvt;
}
public class SpeechEvents : MonoBehaviour
{
    public List<NewSpeechEvents> newSpeechEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
