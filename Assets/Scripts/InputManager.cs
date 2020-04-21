using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public enum BattleState { Active, Inactive };
    public BattleState state;
    public TextMeshProUGUI phrase1, phrase2, prof1, prof2;
    public float timeLastClarification, timeRequired;
    public Person target, user;
    string possibleNonsenseChars = "abcdefghijklmnopqrstuvwxyz";
    public string phrase, inputphrase, nonsensephrase;
    public List<bool> isNonsense;
    public GameObject battleCanvas;
    float proficiency, enemyproficiency;
    public List<AudioClip> vowelSFX, consonantSFX, punctuationSFX;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(state == BattleState.Active && phrase == inputphrase) {
            Debug.Log(phrase);
            Debug.Log(inputphrase);
            phrase = "";
            inputphrase = "";
            nonsensephrase = "";
            UpdateInput();
            state = BattleState.Inactive;
            BattleManager.instance.EndBattle();
        }
        if(state == BattleState.Active) {
            if (Input.GetButtonDown("Backspace")) {
                inputphrase = inputphrase.Substring(0, inputphrase.Length - 1);
                int index = Random.Range(0, punctuationSFX.Count);
                GameInformation.instance.PlaySFX(punctuationSFX[index], 1);
            }
            else {
                string inputThisFrame = inputThisFrame = Regex.Replace(Input.inputString, "[^ a-zA-Z0-9!\\[\\]@#\\$%\\^&\\*\\(\\)=_\\+\\.,\\?'&%~:;\\/-]", "");
                inputphrase += inputThisFrame;
                if (inputThisFrame != "") {
                    if ("aeiou".Contains((""+inputThisFrame[0]).ToLower())) {
                        int index = Random.Range(0, vowelSFX.Count);
                        GameInformation.instance.PlaySFX(vowelSFX[index], 1);
                    }
                    else if("bcdfghjklmnpqrstvwxyz".Contains((""+inputThisFrame[0]).ToLower())) {
                        int index = Random.Range(0, consonantSFX.Count);
                        GameInformation.instance.PlaySFX(consonantSFX[index], 1);
                    }
                    else {
                        int index = Random.Range(0, punctuationSFX.Count);
                        GameInformation.instance.PlaySFX(punctuationSFX[index], 1);
                    }
                }
            }
            UpdateNonsense();
            UpdateInput();
            phrase2.fontSize = phrase1.fontSize;
        }
    }

    public void UpdateInput() {
        phrase2.text = inputphrase;
    }

    public void DisplayNonsense() {
        string s = "";
        for(int i = 0; i < nonsensephrase.Length; ++i) {
            if(isNonsense[i]) {
                s += "<color=#33625F>" + nonsensephrase[i] + "</color>";
            }
            else {
                s += nonsensephrase[i];
            }
        }
        phrase1.text = s;
    }

    void UpdateNonsense() {
        if (isNonsense.Contains(true) && Time.unscaledTime - timeLastClarification > timeRequired) {
            int randomIndex = Random.Range(0, nonsensephrase.Length);
            while (!isNonsense[randomIndex]) {
                randomIndex = Random.Range(0, nonsensephrase.Length);
            }
            nonsensephrase = nonsensephrase.Substring(0, randomIndex) + phrase[randomIndex] + nonsensephrase.Substring(randomIndex + 1);
            isNonsense[randomIndex] = false;
            timeLastClarification = Time.unscaledTime;
            DisplayNonsense();
        }
    }

    public void Begin() {
        Debug.Log("Beginning battle!");
        GenerateNonsense();
        state = BattleState.Active;
        battleCanvas.SetActive(true);
    }

    public void SetInputParameters(string newphrase, float prof, float enemyprof) {
        Debug.Log("Setting input parameters.");
        phrase = newphrase;
        proficiency = prof;
        enemyproficiency = enemyprof;
        float diffInProficiency = proficiency - enemyproficiency;
        diffInProficiency = Mathf.Clamp(diffInProficiency, 0.1f, 1.0f);
        timeRequired = Mathf.Abs(diffInProficiency);
        prof1.text = Mathf.Ceil(prof*100) + "%";
        prof2.text = Mathf.Ceil(enemyprof*100) + "%";
    }

    public void GenerateNonsense() {
        timeLastClarification = Time.unscaledTime;
        isNonsense = new List<bool>();
        for (int i = 0; i < phrase.Length; ++i) {
            if (possibleNonsenseChars.Contains("" + char.ToLower(phrase[i]))) {
                int randomCharIndex = Random.Range(0, 26);
                char randomChar = possibleNonsenseChars[randomCharIndex];
                if (char.IsUpper(phrase[i])) {
                    randomChar = char.ToUpper(randomChar);
                }
                nonsensephrase += randomChar;
                isNonsense.Add(true);
            }
            else {
                nonsensephrase += phrase[i];
                isNonsense.Add(false);
            }
        }
        DisplayNonsense();
    }

    public BattleState GetBattleState() {
        return state;
    }
}
