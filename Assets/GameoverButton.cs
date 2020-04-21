using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameoverButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Sprite unselected, selected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Image>().sprite = unselected;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("Enter");
        GetComponent<Image>().sprite = selected;
    }

    public void OnPointerClick(PointerEventData eventData) {
        GameInformation.instance.Pause(false);
        SceneManager.LoadScene("Game");
    }

    public void OnPointerUp(PointerEventData eventData) {
    }

    public void OnPointerDown(PointerEventData eventData) {
    }
}
