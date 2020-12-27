using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    protected GameManager gameManager;

    public int BtnIndex = 0;

    public ElementButton()
    {
        gameManager = GameManager.Instance;
    }

    public void Start()
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //print("按下！！！！" + BtnIndex);
        gameManager.DownButton(BtnIndex);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        //print("抬起！！！！" + BtnIndex);
        gameManager.UpButton(BtnIndex);
    }
}
