using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class LevelFour : Level
{
    public bool gameStart;

    private GameObject bottomGo;
    private GameObject topGo;

    private GameObject[] aliens;

    private GameObject karaokeTextGo;

    public LevelFour(int goalScore) : base(goalScore)
    {

    }

    public override void InitLevel()
    {
        base.InitLevel();
        aliens = new GameObject[5];
        for (int i = 0; i < aliens.Length; i++)
        {
            aliens[i] = gameManager.GetObj(StringManager.aliens + (i + 1).ToString());
        }
    }

    public override void ExitLevel()
    {
        base.ExitLevel();
        gameStart = false;
        gameManager.RecycleObj(StringManager.targetTransform, topGo);
        gameManager.RecycleObj(StringManager.targetTransform, bottomGo);
        for (int i = 0; i < aliens.Length; i++)
        {
            gameManager.RecycleObj(StringManager.aliens + (i + 1).ToString(), aliens[i]);
        }
    }

    protected override void EnterLevel(object obj)
    {
        noteLanes.Add(gameManager.GetObj(StringManager.laneController).AddComponent<SoundNoteController>());
        noteLanes.Add(gameManager.GetObj(StringManager.laneController).AddComponent<SoundNoteController>());
        bottomGo = gameManager.GetObj(StringManager.targetTransform);
        topGo = gameManager.GetObj(StringManager.targetTransform);
        bottomGo.transform.position = new Vector3(-4.3f, -2, 0);
        topGo.transform.position = new Vector3(10, -2, 0);
        for (int i = 0; i < noteLanes.Count; i++)
        {
            noteLanes[i].Initialize(this, i + 1, topGo.transform, bottomGo.transform);
            noteLanes[i].transform.position = new Vector3(0, -2, 0);
        }
        score = 0;
        gameStart = true;
        karaokeTextGo = gameManager.LoadUI(StringManager.karaokeText);
    }

    public override void HandleLevelLogic()
    {
        if (hasEnter)
        {
            //判断当前游戏是否结束
            //musicPlayer.Stop();
            if (!musicPlayer.IsPlaying)
            {
                //当前游戏结束，音乐停止了
                gameManager.ShowMask(CompleteLevel);
                karaokeTextGo.transform.Find("KaraokeUI").GetComponent<KaraokeController>().ClearText();
                gameManager.RecycleObj(StringManager.karaokeText, karaokeTextGo);
                hasEnter = false;
            }
            //是否生成新音符
            for (int i = 0; i < noteLanes.Count; i++)
            {
                noteLanes[i].CheckSpawnNext();
            }
        }
    }
}