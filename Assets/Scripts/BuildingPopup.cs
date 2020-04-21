using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPopup : MonoBehaviour
{
    public int maxPeople;
    public List<PopupPerson> people;
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
        for(int i = 0; i < people.Count; ++i) {
            people[i].UpdateSprite();
        }
    }

    public List<Person> GetPeople() {
        List<Person> ppl = new List<Person>();
        foreach(PopupPerson p in people) {
            if(p.GetPerson() != null) {
                ppl.Add(p.GetPerson());
            }
        }
        return ppl;
    }

    public void OnMouseEnter() {
        GetComponent<Animator>().SetBool("IsHoveringPopup", true);
    }

    public void OnMouseExit() {
        GetComponent<Animator>().SetBool("IsHoveringPopup", false);
    }
}
