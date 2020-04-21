using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupPerson : MonoBehaviour
{
    public Sprite empty;
    SpriteRenderer sprite;
    bool selected = false;
    Person p = null;
    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateSprite() {
        if(p == null) {
            sprite.sprite = empty;
        }
        else if(GameInformation.instance.GetDrag() == null){
            sprite.sprite = p.GetMinisprite(selected);
        }
        else {
            sprite.sprite = p.GetMinisprite(false);
        }
    }

    public void SetPerson(Person newp) {
        p = newp;
        UpdateSprite();
    }

    public Person GetPerson() {
        if(p != null)
        Debug.Log("returning " + p.name);
        return p;
    }

    public void OnMouseEnter() {
        transform.parent.GetComponent<Animator>().SetBool("IsHoveringPopup", true);
        GameInformation.instance.SetHoveringPopup(this);
        selected = true;
        UpdateSprite();
    }

    public void OnMouseExit() {
        transform.parent.GetComponent<Animator>().SetBool("IsHoveringPopup", false);
        GameInformation.instance.SetHoveringPopup(null);
        selected = false;
        UpdateSprite();
    }

    public void OnMouseDrag() {
        if(p != null) {
            p.Reactivate();
            p.StartDragging();
            p = null;
        }
    }
}
