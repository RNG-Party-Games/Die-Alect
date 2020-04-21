using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<BattleFrame> frames;
    public Animator speedlines, wordframe, battletransition;
    float timeFrameAdded, frameFrequency, timeBattleStarted;
    Person interactor, interactee;
    int framesActive = 0;
    string phrase;
    AI.AIGoal currentgoal;
    bool controllingemployee;
    public TextMeshProUGUI endText, endText2;
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.instance.GetBattleState() == InputManager.BattleState.Active && Time.unscaledTime - timeFrameAdded > frameFrequency) {
            AddNewFrame();
        }
    }

    public void EndBattle() {
        Music.instance.StartOverworldMusic();
        for (int i = 0; i < frames.Count; ++i) {
            if (i < framesActive) {
                frames[i].GetComponent<Animator>().Play("FadeOut");
            }
        }
        framesActive = 0;
        float timetook = Time.unscaledTime - timeBattleStarted;
        float score = -0.1f + (phrase.Length / timetook) * 0.05f;
        Debug.Log(phrase.Length / timetook);
        if (currentgoal == AI.AIGoal.Inn && interactor.GetClass() == Person.PersonClass.Innkeeper
            || currentgoal == AI.AIGoal.Smithy && interactor.GetClass() == Person.PersonClass.Blacksmith
            || currentgoal == AI.AIGoal.Shop && interactor.GetClass() == Person.PersonClass.Merchant
            || currentgoal == AI.AIGoal.Apartment1 && interactor.GetClass() == Person.PersonClass.Civilian
            || currentgoal == AI.AIGoal.Apartment2 && interactor.GetClass() == Person.PersonClass.Civilian) {
            score += Mathf.Abs(score*0.5f);
        }
        if (interactor.GetClass() == Person.PersonClass.Linguist || interactee.GetClass() == Person.PersonClass.Linguist) {
            score += Mathf.Abs(score*0.25f);
        }
        interactee.BoostProficiency(score);
        if (controllingemployee) {
            interactee.GetComponent<AI>().Decide();
        }
        else {
            interactor.GetComponent<AI>().Decide();
        }
        string verbText = " were able to communicate", reactText = "";
        if (score < 0) {
            reactText = " but were not happy about it.";
        }
        else if (score < 0.05) {
            reactText = " but got kind of frustrated.";
        }
        else if (score < 0.1) {
            reactText = ". And it was alright, maybe a 6/10.";
        }
        else if(score < 0.15) {
            reactText = " and it was quite efficient!";
        }
        else{
            reactText = " and it was a textbook exchange!";
        }
        endText.text = "The " + StatManager.instance.classnames[(int)interactee.GetClass()] + " and " 
            + StatManager.instance.classnames[(int)interactor.GetClass()] + " " + verbText + reactText;
        float formattedscore = Mathf.Abs(Mathf.Ceil(score * 100));
        if (formattedscore < 0) {
            endText2.text = "- " + formattedscore + "% proficiency";
        }
        else {
            endText2.text = "+ " + formattedscore + "% proficiency";
        }
        battletransition.Play("BattleEnd");
        speedlines.Play("LinesFadeOut");
        wordframe.Play("FrameFadeOut");
    }

    public void AddNewFrame() {
        if(framesActive != 0) {
            GetComponent<Animator>().Play("Shake");
        }
        if(framesActive < frames.Count) {
            frames[framesActive].gameObject.SetActive(true);
            frames[framesActive].SetPeople(interactor.pclass, interactee.pclass);
            frames[framesActive].GetComponent<Animator>().Play("FadeIn");
            framesActive++;
            timeFrameAdded = Time.unscaledTime;
        }
    }

    public void SetBattleParameters(Person a, Person b, AI.AIGoal goal) {
        currentgoal = goal;
        controllingemployee = false;
        if(b.GetProficiency() > a.GetProficiency()) {
            controllingemployee = true;
            interactor = b;
            interactee = a;
        }
        else {
            interactor = a;
            interactee = b;
        }
        phrase = GameInformation.instance.GetRandomPhrase(goal);
        InputManager.instance.SetInputParameters(phrase, interactor.GetProficiency(), interactee.GetProficiency());
        frameFrequency = phrase.Length / 5;
    }

    public void Begin() {
        timeBattleStarted = Time.unscaledTime;
        InputManager.instance.Begin();
        speedlines.Play("LinesFadeIn");
        wordframe.Play("FrameFadeIn");
        AddNewFrame();
    }
}
