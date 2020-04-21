using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleFrame : MonoBehaviour
{
    public Animator player, enemy;
    public Image[] playeremotes, enemyemotes;
    public Sprite[] bgs;
    public int frameIndex;
    // Start is called before the first frame update
    void OnEnable() {
        foreach (Image p in playeremotes) {
            p.enabled = false;
        }
        foreach (Image e in enemyemotes) {
            e.enabled = false;
        }
        int index = Random.Range(0, bgs.Length);
        int playeremotion = Random.Range(0, playeremotes.Length);
        int enemyemotion = Random.Range(0, enemyemotes.Length);
        GetComponent<Image>().sprite = bgs[index];
        if (frameIndex != 0) {
            playeremotes[playeremotion].enabled = true;
            enemyemotes[enemyemotion].enabled = true;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetPeople(Person.PersonClass playerclass, Person.PersonClass enemyclass) {
        switch (playerclass) {
            case Person.PersonClass.Civilian:
                player.Play("CivFight");
                break;
            case Person.PersonClass.Innkeeper:
                player.Play("InnFight");
                break;
            case Person.PersonClass.Blacksmith:
                player.Play("BlacksmithFight");
                break;
            case Person.PersonClass.Merchant:
                player.Play("MerchFight");
                break;
            case Person.PersonClass.Linguist:
                player.Play("LinguistFight");
                break;
        }
        switch (enemyclass) {
            case Person.PersonClass.Civilian:
                enemy.Play("CivEnemy");
                break;
            case Person.PersonClass.Innkeeper:
                enemy.Play("InnEnemy");
                break;
            case Person.PersonClass.Blacksmith:
                enemy.Play("BlacksmithEnemy");
                break;
            case Person.PersonClass.Merchant:
                enemy.Play("MerchEnemy");
                break;
            case Person.PersonClass.Linguist:
                enemy.Play("LinguistEnemy");
                break;
        }
    }

    public void DisableFrame() {
        gameObject.SetActive(false);
    }
}
