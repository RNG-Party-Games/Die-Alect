using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public AudioClip boat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Arrive() {
        GameInformation.instance.BoatArrive();
    }

    public void BoatSFX() {
        GameInformation.instance.PlaySFX(boat, 0.7f);
    }
}
