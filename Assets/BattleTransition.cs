using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTransition : MonoBehaviour
{
    public GameObject battlescene;
    public AudioClip swoosh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndBattleTransition() {
        battlescene.SetActive(false);
        GameInformation.instance.EndBattle();
    }

    public void StartBattle() {
        battlescene.SetActive(true);
        BattleManager.instance.Begin();
    }

    public void Swoosh() {
        GameInformation.instance.PlaySFX(swoosh, 0.5f);
    }
}
