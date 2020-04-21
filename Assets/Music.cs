using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music instance;
    public AudioClip overworld, battle;
    AudioSource source;
    float overworldTime;
    bool goingToOverworld = false;
    // Start is called before the first frame update
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = overworld;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattleMusic() {
        goingToOverworld = false;
        GetComponent<Animator>().Play("FadeOut");
    }

    public void StartOverworldMusic() {
        goingToOverworld = true;
        GetComponent<Animator>().Play("FadeOut");
    }

    public void StartNewMusic() {
        if(goingToOverworld) {
            source.time = overworldTime;
            source.clip = overworld;
        }
        else {
            overworldTime = source.time;
            source.time = 0;
            source.clip = battle;
        }
        source.Play();
    }
}
