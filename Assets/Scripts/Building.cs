using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingPopup popup;
    public Transform entrance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetEntrance() {
        return entrance;
    }

    public BuildingPopup GetPopup() {
        return popup;
    }

    public void OnMouseEnter() {
        popup.GetComponent<Animator>().SetBool("IsHoveringBuilding", true);
    }

    public void OnMouseExit() {
        popup.GetComponent<Animator>().SetBool("IsHoveringBuilding", false);
    }
}
