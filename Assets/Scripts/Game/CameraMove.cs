using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 摄像机跟随以及更换移动背景图片
/// </summary>
public class CameraMove : MonoBehaviour
{
    public GameObject[] bgGos;
    private int bgIndex;
    private Vector3[] initPos;
    private Vector3 cameraInitPos;
    private GameObject LevelBG;
    private GameManager gameManager;

    //开始墙体
    private GameObject beginPic;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        LevelBG = gameManager.GetObj(StringManager.LevelBG);
        beginPic = GameObject.FindGameObjectWithTag("BG_Begin");

        bgGos = new GameObject[2];
        initPos = new Vector3[2];
        for (int i = 0; i < 2; i++)
        {
            bgGos[i] = GameObject.FindGameObjectWithTag("BG_" + i.ToString());
            initPos[i] = bgGos[i].transform.position;
        }
        gameManager.Register("PlayerRun", PlayerRun);
        gameManager.Register("ResetCamera", ResetCamera);
        cameraInitPos = transform.position;
    }
    /// <summary>
    /// 开始更新背景图片
    /// </summary>
    private void PlayerRun(object boRun)
    {
        if ((bool)boRun)
        {
            CancelInvoke("UpdateBGPos");
            InvokeRepeating("UpdateBGPos", 0f, 0.01f);
        }
        else
        {
            CancelInvoke("UpdateBGPos");
        }
    }
    /// <summary>
    /// 更新背景图片位置
    /// </summary>
    private void UpdateBGPos()
    {
        bool reset = false;
        for (int i = 0; i < bgGos.Length; i++)
        {
            bgGos[i].transform.position -= new Vector3(0.02f, 0, 0);
            if ((int)bgGos[1].transform.position.x == (int)initPos[0].x)
            {
                reset = true;
                beginPic.SetActive(false);
            }
        }
        if (reset)
        {
            for (int i = 0; i < bgGos.Length; i++)
            {
                bgGos[i].transform.position = initPos[i];
            }
        }
    }
    /// <summary>
    /// 重置摄像机以及背景图片状态
    /// </summary>
    private void ResetCamera(object obj=null)
    {
        CancelInvoke();
        bgIndex = 0;
        for (int i = 0; i < bgGos.Length; i++)
        {
            bgGos[i].transform.position = initPos[i];
        }
        transform.SetParent(null);
        transform.position = cameraInitPos;
        gameManager.RecycleObj(StringManager.LevelBG, LevelBG);
        gameManager.UnRegister("PlayerRun", PlayerRun);
        gameManager.UnRegister("ResetCamera", ResetCamera);
        Destroy(this);
    }
}
