using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class LevelTwo : Level
{
    private GameObject noteCreatorGo;
    private GameObject topGo;
    private GameObject bottomGo;

    public LevelTwo(int goalScore) : base(goalScore)
    {

    }

    public override void InitLevel()
    {
        base.InitLevel();
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.AddComponent<CameraMove>();
        mainCamera.transform.SetParent(gameManager.GetPlayerTrans());
    }

    public override void ExitLevel()
    {
        base.ExitLevel();
        gameManager.Send("ResetCamera");
        gameManager.InitPlayer();
        gameManager.RecycleObj(StringManager.targetTransform, topGo);
        gameManager.RecycleObj(StringManager.targetTransform, bottomGo);
    }

    protected override void EnterLevel(object obj)
    {
        noteLanes.Add(gameManager.GetObj(StringManager.laneController).AddComponent<LaneController>());
        for (int i = 0; i < noteLanes.Count; i++)
        {
            bottomGo = gameManager.GetObj(StringManager.targetTransform);
            topGo = gameManager.GetObj(StringManager.targetTransform);
            bottomGo.transform.position = new Vector3(-5.3f, 0, 0);
            topGo.transform.position = new Vector3(10, 0, 0);
            noteLanes[i].Initialize(this, 1, topGo.transform, bottomGo.transform);
            noteLanes[i].transform.position = bottomGo.transform.position + new Vector3(1.04f, 0, 0);
        }
        score = 0;
    }


}