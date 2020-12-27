using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
/// <summary>
/// 处理UI相关的加载，回收，存贮当前显示面板，回收以及遮罩的处理
/// </summary>
public class ControlManager
{
    private GameManager gameManager;
    private Transform eleOneTrans;
    private Transform eleTwoTrans;
    private Transform eleThreeTrans;
    private Transform spaceTrans;
    private Transform canvasTrans;
    private int[] btnPressStatus;
    private int pressIndex = -1;
    private Dictionary<string, bool> btnTranDic;
    public ControlManager()
    {
        gameManager = GameManager.Instance;
        btnPressStatus = new int[10];
        InitControlPanel();
    }

    /// <summary>
    /// 加载UI的方法
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public void InitControlPanel()
    {
        eleOneTrans = gameManager.GetObj(StringManager.control + 0).transform;
        eleTwoTrans = gameManager.GetObj(StringManager.control + 1).transform;
        eleThreeTrans = gameManager.GetObj(StringManager.control + 2).transform;
        spaceTrans = gameManager.GetObj(StringManager.control + 9).transform;
        btnTranDic = new Dictionary<string, bool>();
        btnTranDic.Add("A", false);
        btnTranDic.Add("S", false);
        btnTranDic.Add("D", false);
        btnTranDic.Add("O", true);
        
        canvasTrans = GameObject.Find("ControlBtn").transform;

        canvasTrans.gameObject.SetActive(false);
        eleOneTrans.gameObject.SetActive(false);
        eleTwoTrans.gameObject.SetActive(false);
        eleThreeTrans.gameObject.SetActive(false);
        spaceTrans.gameObject.SetActive(false);
        //eleOneTrans.SetSiblingIndex(0);
    }

    public void ShowControlPanel()
    {
        canvasTrans.gameObject.SetActive(true);
        //eleOneTrans.SetSiblingIndex(0);
        foreach (string key in btnTranDic.Keys)
        {
            if (key == "A")
                eleOneTrans.gameObject.SetActive(btnTranDic[key]);
            if (key == "S")
                eleTwoTrans.gameObject.SetActive(btnTranDic[key]);
            if (key == "D")
                eleThreeTrans.gameObject.SetActive(btnTranDic[key]);
            if (key == "O")
                spaceTrans.gameObject.SetActive(btnTranDic[key]);
        }

    }

    public void HideControlPanel()
    {
        canvasTrans.gameObject.SetActive(false);
        eleOneTrans.gameObject.SetActive(false);
        eleTwoTrans.gameObject.SetActive(false);
        eleThreeTrans.gameObject.SetActive(false);
        spaceTrans.gameObject.SetActive(false);
    }




    /// <summary>
    /// 回收控制面板的方法
    /// </summary>
    /// <param name="panelName"></param>
    public void RecycleControlPanel()
    {
        gameManager.RecycleObj(StringManager.control + 0, eleOneTrans.gameObject);
        gameManager.RecycleObj(StringManager.control + 1, eleTwoTrans.gameObject);
        gameManager.RecycleObj(StringManager.control + 2, eleThreeTrans.gameObject);
        gameManager.RecycleObj(StringManager.control + 9, spaceTrans.gameObject);
    }

    public void DownButton(int btn_index)
    {
        if (pressIndex == -1)
        {
            btnPressStatus[btn_index] = 1;
            pressIndex = btn_index;
        }
    }

    public void UpButton(int btn_index)
    {
        if (pressIndex == btn_index)
        {
            btnPressStatus[btn_index] = 0;
            pressIndex = -1;
        }
    }

    public bool CheckButtonPressed(int btn_index)
    {
        return btnPressStatus[btn_index] == 1;
    }

    public void SetControlBtnVisible(string BtnName, bool visible)
    {
        if (btnTranDic.ContainsKey(BtnName))
        {
            btnTranDic[BtnName] = visible;
            ShowControlPanel();
        }
    }
}
