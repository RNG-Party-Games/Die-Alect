using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    public static Ambience instance;
    public AudioClip day, night;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn() {
        GetComponent<Animator>().Play("FadeIn");
    }

    public void FadeOut() {
        GetComponent<Animator>().Play("FadeOut");
    }

    public void Day() {
        GetComponent<Animator>().Play("AmbienceDay");
    }

    public void Night() {
        GetComponent<Animator>().Play("AmbienceNight");
    }

    public void SwapToDay() {
        GetComponent<AudioSource>().clip = day;
        GetComponent<AudioSource>().Play();
    }

    public void SwapToNight() {
        GetComponent<AudioSource>().clip = night;
        GetComponent<AudioSource>().Play();

    }
}
