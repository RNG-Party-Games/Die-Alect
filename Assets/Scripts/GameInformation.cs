using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class GameInformation : MonoBehaviour
{
    public static GameInformation instance;
    public List<Person> people;
    public List<Building> buildings;
    public List<GameObject> peopleTypes;
    public AstarPath astar;
    public Collider2D world, walkable, placeable, boatarea;
    public int personPlane = -5;
    public int day;
    public TextMeshProUGUI dayText;
    public GameObject battlescene;
    public List<string> phrases, innphrases, smithyphrases, shopphrases, socialphrases, news;
    Person dragging = null;
    PopupPerson currentpopup;
    bool paused;
    public BattleTransition battletransition;
    public Animator boat;
    public float newsInterval = 60.0f, charInterval = 0.1f, downInterval = 20.0f;
    float lastNews, lastChar;
    public TextMeshProUGUI ticker;
    public Animator tickerAnim;
    string currentnews;
    int currentnewsindex;
    int charmax = 27;
    public GameObject SFX;
    public GameObject gameoverFrame;
    public Transform dock;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start() {
        dayText.text = "0";
        ShowNews("Arrenjeese is dying! Place citizens into buildings to start getting interactions! Interactions give valuable language proficiency.");
    }

    // Update is called once per frame
    void Update() {
        if (currentnews != null && currentnewsindex < currentnews.Length + charmax && Time.time - lastChar > charInterval) {
            if (currentnewsindex >= currentnews.Length && currentnewsindex - currentnews.Length < charmax) {
                ticker.text += "\u00A0";
            }
            else {
                ticker.text += currentnews[currentnewsindex];
            }
            if (ticker.text.Length > charmax) {
                ticker.text = ticker.text.Substring(1);
            }
            ++currentnewsindex;
            lastChar = Time.time;
        }
        else if (currentnews != null && currentnewsindex >= currentnews.Length + charmax) {
            currentnews = null;
            tickerAnim.Play("TickerDown");
        }
        if (Time.time - lastNews > newsInterval) {
            int index = Random.Range(0, news.Count);
            ShowNews(news[index]);
        }
    }

    public Vector3 ClosestWalkable(Vector3 position) {
        Vector3 closestPoint = world.ClosestPoint(position);
        closestPoint.z = personPlane;
        return closestPoint;
    }

    public Vector3 ClosestPlaceable(Vector3 position) {
        Vector3 closestPoint = placeable.ClosestPoint(position);
        closestPoint.z = personPlane;
        return closestPoint;
    }

    public Vector3 GetWalkablePoint() {
        Bounds bounds = walkable.bounds;
        return new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y),
        personPlane
        );
    }

    public Vector3 GetEntrances(int index) {
        return buildings[index].GetEntrance().position;
    }

    public List<Person> GetPopulation() {
        return people;
    }

    public void AdvanceDay() {
        day++;
        dayText.text = "" + day;
    }

    public void SetDrag(Person p) {
        dragging = p;
    }

    public Person GetDrag() {
        return dragging;
    }

    public void SetHoveringPopup(PopupPerson popup) {
        currentpopup = popup;
    }

    public PopupPerson GetHoveringPopup() {
        return currentpopup;
    }

    public Person GetRandomPerson(AI.AIGoal goal) {
        BuildingPopup building;
        if (goal == AI.AIGoal.Inn) {
            building = buildings[0].GetPopup();
        }
        else if (goal == AI.AIGoal.Smithy) {
            building = buildings[1].GetPopup();
        }
        else if (goal == AI.AIGoal.Shop) {
            building = buildings[2].GetPopup();
        }
        else if (goal == AI.AIGoal.Apartment1) {
            building = buildings[3].GetPopup();
        }
        else {
            building = buildings[4].GetPopup();
        }
        List<Person> potentialPeople = building.GetPeople();
        //if(potentialPeople.Count > 0) {
        //    int index = Random.Range(0, potentialPeople.Count);
        //    return potentialPeople[index];
        //}
        if (potentialPeople.Count > 0) {
            return building.GetPeople()[0];
        }
        return null;
    }

    public int GetAmountOfPeopleIn(AI.AIGoal goal) {
        BuildingPopup building;
        if (goal == AI.AIGoal.Inn) {
            building = buildings[0].GetPopup();
        }
        else if (goal == AI.AIGoal.Smithy) {
            building = buildings[1].GetPopup();
        }
        else if (goal == AI.AIGoal.Shop) {
            building = buildings[2].GetPopup();
        }
        else if (goal == AI.AIGoal.Apartment1) {
            building = buildings[3].GetPopup();
        }
        else {
            building = buildings[4].GetPopup();
        }
        List<Person> potentialPeople = building.GetPeople();
        return potentialPeople.Count;
    }

    public void Spawn(Person.PersonClass pclass, Vector3 v, float prof) {
        GameObject toSpawn;
        if (pclass == Person.PersonClass.Civilian) {
            toSpawn = peopleTypes[0];
        }
        else if (pclass == Person.PersonClass.Innkeeper) {
            toSpawn = peopleTypes[1];
        }
        else if (pclass == Person.PersonClass.Blacksmith) {
            toSpawn = peopleTypes[2];
        }
        else if (pclass == Person.PersonClass.Merchant) {
            toSpawn = peopleTypes[3];
        }
        else if (pclass == Person.PersonClass.Linguist) {
            toSpawn = peopleTypes[4];
        }
        else {
            toSpawn = peopleTypes[5];
        }

        GameObject newPerson = Instantiate(toSpawn, v, Quaternion.identity);
        Person newp = newPerson.GetComponent<Person>();
        newp.BoostProficiency(prof);
        people.Add(newp);
    }

    public void StartBattle(Person p1, Person p2, AI.AIGoal goal) {
        Pause(true);
        Music.instance.StartBattleMusic();
        Ambience.instance.FadeOut();
        battletransition.GetComponent<Animator>().Play("BattleStart");
        BattleManager.instance.SetBattleParameters(p1, p2, goal);
    }

    public void EndBattle() {
        Pause(false);
        Ambience.instance.FadeIn();
    }

    public string GetRandomPhrase(AI.AIGoal goal) {
        List<string> potentialPhrases = new List<string>(phrases);
        if (goal == AI.AIGoal.Inn) {
            potentialPhrases.AddRange(innphrases);
        }
        else if (goal == AI.AIGoal.Smithy) {
            potentialPhrases.AddRange(smithyphrases);
        }
        else if (goal == AI.AIGoal.Shop) {
            potentialPhrases.AddRange(shopphrases);
        }
        else if (goal == AI.AIGoal.Apartment1 || goal == AI.AIGoal.Apartment2) {
            potentialPhrases.AddRange(socialphrases);
        }
        int index = Random.Range(0, potentialPhrases.Count);
        return potentialPhrases[index];
    }

    public bool IsPaused() {
        return paused;
    }

    public void Pause(bool pause) {
        paused = pause;
        if (paused) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }

    public void Boat() {
        boat.Play("BoatIn");
    }

    public void BoatArrive() {
        Bounds bounds = boatarea.bounds;
        int pplToSpawn = Mathf.Clamp(2 + (int) Mathf.Ceil(day / 3), 2, 6);
        for(int i = 0; i < pplToSpawn; ++i) {
            Vector3 spawnpoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            personPlane
            );
            Person.PersonClass pclass = (Person.PersonClass)Random.Range(0, 5);
            Spawn(pclass, spawnpoint, 0);
        }
    }

    public void ShowNews(string s) {
        currentnews = s;
        currentnewsindex = 0;
        tickerAnim.Play("Ticker");
        lastNews = lastChar = Time.time;
    }

    public void PlaySFX(AudioClip clip, float vol) {
        GameObject sfx = Instantiate(SFX);
        sfx.GetComponent<AudioSource>().clip = clip;
        sfx.GetComponent<AudioSource>().volume = vol;
        sfx.GetComponent<AudioSource>().Play();
    }

    public void Gameover() {
        Pause(true);
        gameoverFrame.SetActive(true);
    }

    public Vector3 GetDock() {
        return dock.position;
    }

    public void KillPerson(Person p) {
        people.Remove(p);
        Destroy(p.gameObject);
    }
}
