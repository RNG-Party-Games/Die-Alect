using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;

public class Person : MonoBehaviour
{
    public float proficiency, food, energy, productivity, social;
    public enum PersonClass { Civilian, Innkeeper, Merchant, Blacksmith, Linguist, Newborn }
    public PersonClass pclass;
    public Sprite head;
    public Sprite[] minisprites;
    public Animator sprite;
    public GameObject highlight;
    public Animator emotion;
    public float tickTime = 1;
    public float foodLoss, energyLoss, productivityLoss, socialLoss, profLoss;
    public float timeBorn;
    float lastTick;
    bool dragging;
    AI ai;
    int framesdown = 0;
    float parentProficiency = 0;
    float originalSpeed = 1;
    float startedDragging;
    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<AI>();
        lastTick = Time.time;
        timeBorn = Time.time;
        originalSpeed = ai.maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        sprite.SetBool("Walking", (ai.velocity.magnitude > 0));
        if(ai.velocity.x > 0 && sprite.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
            sprite.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(ai.velocity.x < 0 || sprite.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            sprite.GetComponent<SpriteRenderer>().flipX = false;
        }
        if(dragging) {
            var mouse = Input.mousePosition;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            transform.position = GameInformation.instance.ClosestWalkable(mouse);
            if(Input.GetMouseButtonUp(0)) {
                sprite.Play("Idle");
                dragging = false;
                ai.isStopped = false;
                GetComponent<BoxCollider2D>().enabled = true;
                Drop();
                GameInformation.instance.SetDrag(null);
            }
        }
    }

    private void FixedUpdate() {
        if(Time.time - lastTick > tickTime) {
            lastTick = Time.time;
            food -= foodLoss;
            energy -= energyLoss;
            productivity -= productivityLoss;
            social -= socialLoss;
            proficiency -= profLoss;
            food = Mathf.Clamp(food, 0.0f, 1.0f);
            energy = Mathf.Clamp(energy, 0.0f, 1.0f);
            productivity = Mathf.Clamp(productivity, 0.0f, 1.0f);
            social = Mathf.Clamp(social, 0.0f, 1.0f);
            proficiency = Mathf.Clamp(proficiency, 0.0f, 1.0f);
            StatManager.instance.DisplayPopulation();
        }
    }

    public float GetProficiency() {
        return proficiency;
    }

    public void BoostProficiency(float boost) {
        proficiency += boost;
        proficiency = Mathf.Clamp(proficiency, 0.0f, 1.0f);
    }

    public void ReachGoal() {
        ai.maxSpeed = originalSpeed;
        sprite.Play("Idle");
        if(food >= 0.75f && energy >= 0.75f && productivity >= 0.75f && social >= 0.75f) {
            float x = transform.position.x + Random.Range(-1.0f, 1.0f);
            float y = transform.position.y + Random.Range(-1.0f, 1.0f);
            Vector3 toSpawnAt = GameInformation.instance.ClosestPlaceable(new Vector3(x, y, -5));
            GameInformation.instance.Spawn(PersonClass.Newborn, toSpawnAt, proficiency*0.5f);
            food = 0.25f;
            energy = 0.25f;
            productivity = 0.25f;
            social = 0.25f;
        }
    }

    public void Interact(AI.AIGoal goal) {
        Person interactwith = GameInformation.instance.GetRandomPerson(goal);
        Debug.Log("Interacting with " + interactwith.name);
        if(interactwith != null) {
            emotion.Play("Exclamation");
            if (goal == AI.AIGoal.Inn) {
                energy = 1.0f;
            }
            else if (goal == AI.AIGoal.Shop) {
                food = 1.0f;
            }
            if (goal == AI.AIGoal.Smithy) {
                productivity = 1.0f;
            }
            else if (goal == AI.AIGoal.Apartment1) {
                social = 1.0f;
            }
            else if (goal == AI.AIGoal.Apartment2) {
                social = 1.0f;
            }
            GameInformation.instance.StartBattle(this, interactwith, goal);
        }
    }

    public void OnMouseEnter() {
        highlight.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void OnMouseExit() {
        if(!dragging) {
            highlight.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void OnMouseDown() {
        UpdateFrame();
    }

    public void UpdateFrame() {
        StatManager.instance.DisplayStats(this);
    }

    public void OnMouseDrag() {
        ++framesdown;
        if(framesdown >= 6 && pclass != PersonClass.Newborn) {
            StartDragging();
        }
    }
    public void OnMouseUp() {
        framesdown = 0;
    }

    public void Drop() {
        PopupPerson popup = GameInformation.instance.GetHoveringPopup();
        if (popup != null && popup.GetPerson() == null) {
            popup.SetPerson(this);
            Deactivate();
        }
        else {
            var mouse = Input.mousePosition;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            Vector3 newposition = GameInformation.instance.ClosestPlaceable(mouse);
            if (!GetComponent<Collider2D>().OverlapPoint(newposition)) {
                highlight.GetComponent<SpriteRenderer>().enabled = false;
            }
            transform.position = newposition;
        }
        if(Time.time - startedDragging > 2) {
            ai.maxSpeed = originalSpeed * 2;
        }
    }

    public void StartDragging() {
        if (!dragging) {
            startedDragging = Time.time;
        }
        sprite.Play("Drag");
        UpdateFrame();
        ai.isStopped = true;
        dragging = true;
        highlight.GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GameInformation.instance.SetDrag(this);
    }
    public PersonClass GetClass() {
        return pclass;
    }

    public Sprite GetHead() {
        return head;
    }

    public float GetFood() {
        return food;
    }

    public float GetEnergy() {
        return energy;
    }

    public float GetProductivity() {
        return productivity;
    }

    public float GetSocial() {
        return social;
    }

    public Sprite GetMinisprite(bool selected) {
        if(selected) {
            return minisprites[0];
        }
        return minisprites[1];
    }

    public bool IsDragging() {
        return dragging;
    }

    public void Deactivate() {
        GetComponent<BoxCollider2D>().enabled = false;
        sprite.gameObject.SetActive(false);
        highlight.SetActive(false);
        ai.enabled = false;
    }

    public void Reactivate() {
        sprite.gameObject.SetActive(true);
        highlight.SetActive(true);
        ai.enabled = true;
    }

    public float GetBorn() {
        return timeBorn;
    }
}
