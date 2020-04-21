using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public static StatManager instance;
    public TextMeshProUGUI personclass, proficiency, popproficiency;
    public Image head, hunger, energy, productivity, social;
    public Image hungerbar, energybar, productivitybar, socialbar;
    public Sprite[] smileys;
    public string[] classnames = { "Civilian", "Innkeeper", "Merchant", "Blacksmith", "Linguist", "Newborn" };
    Person p = null;
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    void Start()
    {
        DisplayPopulation();
    }

    // Update is called once per frame
    void Update()
    {
        if(p != null) {
            DisplayStats(p);
        }
    }

    public void DisplayStats(Person p) {
        this.p = p;
        head.sprite = p.GetHead();
        head.enabled = true;
        personclass.text = classnames[(int) p.GetClass()];
        proficiency.text = Mathf.Ceil(p.GetProficiency()*100) + "%";
        hunger.sprite = (p.GetFood() > .66) ? smileys[0] : ((p.GetFood() > .33) ? smileys[1] : smileys[2]);
        energy.sprite = (p.GetEnergy() > .66) ? smileys[0] : ((p.GetEnergy() > .33) ? smileys[1] : smileys[2]);
        productivity.sprite = (p.GetProductivity() > .66) ? smileys[0] : ((p.GetProductivity() > .33) ? smileys[1] : smileys[2]);
        social.sprite = (p.GetSocial() > .66) ? smileys[0] : ((p.GetSocial() > .33) ? smileys[1] : smileys[2]);
    }

    public void DisplayPopulation() {
        List<Person> ppl = GameInformation.instance.GetPopulation();
        float hungersum = 0, energysum = 0, productivitysum = 0, socialsum = 0;
        int proficiency = 0;
        foreach(Person p in ppl) {
            hungersum += p.GetFood();
            energysum += p.GetEnergy();
            productivitysum += p.GetProductivity();
            socialsum += p.GetSocial();
            if(p.GetProficiency() >= .5) {
                proficiency++;
            }
        }
        hungerbar.GetComponent<RectTransform>().anchorMax = new Vector2((hungersum / ppl.Count), 1.0f);
        energybar.GetComponent<RectTransform>().anchorMax = new Vector2((energysum / ppl.Count), 1.0f);
        productivitybar.GetComponent<RectTransform>().anchorMax = new Vector2((productivitysum / ppl.Count), 1.0f);
        socialbar.GetComponent<RectTransform>().anchorMax = new Vector2((socialsum / ppl.Count), 1.0f);
        popproficiency.text = proficiency + "/" + ppl.Count;
        if(proficiency <= 0) {
            GameInformation.instance.Gameover();
        }
    }
}
