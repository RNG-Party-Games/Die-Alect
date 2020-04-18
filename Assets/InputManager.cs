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
    public TextMeshProUGUI phrase1, phrase2;
    public float timeLastClarification, timeRequired;
    public Person target, user;
    string possibleNonsenseChars = "abcdefghijklmnopqrstuvwxyz";
    public string phrase, inputphrase, nonsensephrase;
    public List<bool> isNonsense;
    public GameObject battleCanvas;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        timeLastClarification = Time.time;
        for(int i = 0; i < phrase.Length; ++i) {
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
        phrase1.text = nonsensephrase;
    }

    // Update is called once per frame
    void Update() {
        if(state == BattleState.Active) {
            if (Input.GetButtonDown("Backspace")) {
                inputphrase = inputphrase.Substring(0, inputphrase.Length - 1);
            }
            else {
                string inputThisFrame = inputThisFrame = Regex.Replace(Input.inputString, "[^ a-zA-Z0-9!\\[\\]@#\\$%\\^&\\*\\(\\)=_\\+\\.,\\?'&%~:;\\/-]", "");
                inputphrase += inputThisFrame;
            }
            phrase2.text = inputphrase;
            UpdateNonsense();
        }
    }

    public void UpdateNonsense() {
        if (isNonsense.Contains(true) && Time.time - timeLastClarification > timeRequired) {
            int randomIndex = Random.Range(0, nonsensephrase.Length);
            while (!isNonsense[randomIndex]) {
                randomIndex = Random.Range(0, nonsensephrase.Length);
                Debug.Log(randomIndex + " = " + isNonsense[randomIndex]);
            }
            nonsensephrase = nonsensephrase.Substring(0, randomIndex) + phrase[randomIndex] + nonsensephrase.Substring(randomIndex + 1);
            isNonsense[randomIndex] = false;
            timeLastClarification = Time.time;
            phrase1.text = nonsensephrase;
        }
    }

    public void Begin() {
        state = BattleState.Active;
        battleCanvas.SetActive(true);
    }
}
