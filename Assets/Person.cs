using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;

public class Person : MonoBehaviour
{
    public float proficiency;
    public Transform target;
    public Animator sprite;
    public GameObject highlight, battleanim;
    AIPath ai;
    bool dragging;
    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<AIPath>();
        ai.destination = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        sprite.SetBool("Walking", (ai.velocity.magnitude > 0));
        if(dragging) {
            var mouse = Input.mousePosition;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            mouse.z = 0;
            transform.position = mouse;
        }
    }

    public float GetProficiency() {
        return proficiency;
    }

    public void Interact() {
        GameObject newbattleanim = Instantiate(battleanim, transform.position, Quaternion.identity);
    }

    public void OnMouseOver() {
        highlight.SetActive(true);
    }

    public void OnMouseExit() {
        highlight.SetActive(false);
    }

    public void OnMouseDown() {
        ai.isStopped = true;
        dragging = true;
    }
    public void OnMouseUp() {
        dragging = false;
        Interact();
    }
}
