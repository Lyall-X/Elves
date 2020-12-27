using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;

public class StartPanel : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        //transform.Find("Btn_Start").gameObject.SetActive(false);
        //transform.Find("text").gameObject.SetActive(false);
        
        button = transform.Find("Btn_Start").GetComponent<Button>();
        button.onClick.AddListener(EnterGame);
        Tween buttonTween = DOTween.To(() => button.GetComponent<Text>().color,
            toColor => button.GetComponent<Text>().color = 
            toColor,new Color(0,0,0,0),2f
            );
        buttonTween.SetLoops(-1).SetEase(Ease.InOutQuart);
        Invoke("StopMovie", 9);
    }

    private void EnterGame()
    {
        GameManager.Instance.ShowMask(GameManager.Instance.EnterGame);
        Invoke("DestroySelf", 2f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void StopMovie()
    {
        transform.Find("Btn_Start").gameObject.SetActive(true);
        transform.Find("text").gameObject.SetActive(true);
    }
}
