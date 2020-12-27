using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class LevelThree : Level
{

    private GameObject bottomGo;
    private GameObject topGo;

    private ForgeMask forgeMask;

    public LevelThree(int goalScore) : base(goalScore)
    {

    }

    public override void InitLevel()
    {
        base.InitLevel();
        forgeMask = gameManager.LoadUI(StringManager.forgeMask).GetComponent<ForgeMask>();
    }

    public override void ExitLevel()
    {
        base.ExitLevel();
        gameManager.RecycleObj(StringManager.forgeMask, forgeMask.gameObject);
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
            bottomGo.transform.position = new Vector3(-4.3f, -3f, 0);
            topGo.transform.position = new Vector3(9.5f, -3f, 0);
            noteLanes[i].Initialize(this, i + 1, topGo.transform, bottomGo.transform);
            noteLanes[i].transform.position = bottomGo.transform.position + new Vector3(1.04f, 0, 0);
        }
        score = 0;
    }

    public override void MatchEvt(KoreographyEvent evt)
    {
        int noteID = evt.GetIntValue();
        for (int i = 0; i < noteLanes.Count; i++)
        {
            LaneController lane = noteLanes[i];
            if (noteID == 3)
            {
                noteID = 1;
            }
            if (lane.DoesMatch(noteID))
            {
                lane.AddEventToLane(evt);
                break;
            }
        }
    }

    private bool showForge;
    private bool showMask;

    public override void HandleLevelLogic()
    {
        if (hasEnter)
        {
            //musicPlayer.Stop();
            if (!musicPlayer.IsPlaying)
            {
                gameManager.ShowMask(CompleteLevel);
                hasEnter = false;
            }
            //检测是否显示雾效
            if (!showForge)
            {
                if (gameManager.GetMusicSample() >= 1613969)
                {
                    forgeMask.ShowForge();
                    showForge = true;
                }
            }
            //检测是否显示黑遮罩效果
            else if (!showMask)
            {
                if (gameManager.GetMusicSample() >= 3728041)
                {
                    forgeMask.ShowForgeMask();
                    showMask = true;
                }
            }
            for (int i = 0; i < noteLanes.Count; i++)
            {
                noteLanes[i].CheckSpawnNext();
            }
        }
    }

    public void HideMask()
    {
        forgeMask.HideForgeAndMask();
    }
}