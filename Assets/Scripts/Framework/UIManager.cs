using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 处理UI相关的加载，回收，存贮当前显示面板，回收以及遮罩的处理
/// </summary>
public class UIManager
{
    private GameManager gameManager;
    private GameObject currentPanel;
    private Transform canvasTrans;
    public UIManager()
    {
        gameManager = GameManager.Instance;
        canvasTrans = GameObject.Find("Canvas").transform;
        InitMask();
    }

    #region 遮罩
    private GameObject uiMaskGo;
    private Tween maskTween;
    private Image maskImage;
    /// <summary>
    /// 初始化遮罩
    /// </summary>
    private void InitMask()
    {
        uiMaskGo = gameManager.GetObj(StringManager.imgMask);
        CorrectPos(uiMaskGo.transform);
        maskImage = uiMaskGo.GetComponent<Image>();
        maskTween = DOTween.To(()=>maskImage.color,toColor=>maskImage.color=toColor,new Color(0,0,0,1),2f);
        maskTween.SetAutoKill(false);
        maskTween.Pause();
    }
    /// <summary>
    /// 显示遮罩
    /// </summary>
    /// <param name="tweenCallback"></param>
    public void ShowMask(TweenCallback tweenCallback)
    {
        //动画事件，必须得是正播之后才会去回调相应的方法，并且想要倒播得先正播一次。
        uiMaskGo.transform.SetSiblingIndex(10);
        maskTween.PlayForward();
        maskTween.OnComplete(tweenCallback);
    }
    /// <summary>
    /// 隐藏遮罩
    /// </summary>
    public void HideMask()
    {
        uiMaskGo.transform.SetSiblingIndex(10);
        maskTween.PlayBackwards();
    }

    #endregion
    /// <summary>
    /// 纠正位置的方法，把UI设置父对象以及状态重置
    /// </summary>
    /// <param name="uiTrans"></param>
    private void CorrectPos(Transform uiTrans)
    {
        uiTrans.SetParent(canvasTrans);
        uiTrans.localPosition = Vector3.zero;
        uiTrans.localScale = Vector3.one;
    }
    /// <summary>
    /// 加载UI的方法
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public GameObject LoadUI(string uiName)
    {
        GameObject uiGo = gameManager.GetObj(uiName);
        CorrectPos(uiGo.transform);
        return uiGo;
    }
    /// <summary>
    /// 加载面板的方法
    /// </summary>
    /// <param name="panelName"></param>
    public void LoadPanel(string panelName)
    {
        GameObject panelGo = gameManager.GetObj(panelName);
        CorrectPos(panelGo.transform);
        currentPanel = panelGo;
        currentPanel.SetActive(true);
    }
    /// <summary>
    /// 回收面板的方法
    /// </summary>
    /// <param name="panelName"></param>
    public void RecyclePanel(string panelName)
    {
        gameManager.RecycleObj(panelName,currentPanel);
    }
}
