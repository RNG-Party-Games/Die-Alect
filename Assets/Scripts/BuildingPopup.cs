using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPopup : MonoBehaviour
{
    public int maxPeople;
    public PopupPerson person;
    // Start is called before the first frame update
    void Start()
    {
        SetSprites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprites() {
        person.UpdateSprite();
    }

    public Person GetPerson() {
        return person.GetPerson();
    }

    public void OnMouseEnter() {
        GetComponent<Animator>().SetBool("IsHoveringPopup", true);
    }

    public void OnMouseExit() {
        GetComponent<Animator>().SetBool("IsHoveringPopup", false);
    }
}
