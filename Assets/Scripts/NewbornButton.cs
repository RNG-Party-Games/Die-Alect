using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewbornButton : MonoBehaviour
{
    public Sprite selected, unselected;
    public Person.PersonClass pclass;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter() {
        GetComponent<SpriteRenderer>().sprite = selected;
    }

    public void OnMouseExit() {
        GetComponent<SpriteRenderer>().sprite = unselected;
    }
    public void OnMouseUpAsButton() {
        float newbornprof = transform.parent.transform.parent.GetComponent<Person>().GetProficiency();
        GameInformation.instance.Spawn(pclass, transform.parent.transform.parent.position, newbornprof);
        Destroy(transform.parent.transform.parent.gameObject);
    }
}
