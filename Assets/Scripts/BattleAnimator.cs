using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimator : MonoBehaviour
{
    Vector3 originalPos;
    bool movingToCenter = false;
    float startedMoving;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play("BattleStart");
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(movingToCenter) {
            transform.position = Vector3.Lerp(originalPos, Vector3.zero, (Time.time - startedMoving)*1.8f);
        }
    }

    public void MoveToCenter() {
        startedMoving = Time.time;
        movingToCenter = true;
    }

    public void Kill() {
        Destroy(this.gameObject);
    }
}
