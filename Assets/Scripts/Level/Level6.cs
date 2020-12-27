using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class LevelSix : Level
{
    public LevelSix(int goalScore) : base(goalScore)
    {

    }

    public override void InitLevel()
    {
        base.InitLevel();
    }

    public override void ExitLevel()
    {
        base.ExitLevel();
        gameManager.Send("ShowPanel");
    }

    protected override void EnterLevel(object obj)
    {
        base.EnterLevel(obj);
    }

    public override void MatchEvt(KoreographyEvent evt)
    {
        base.MatchEvt(evt);
    }
    public override void HandleLevelLogic()
    {
        base.HandleLevelLogic();
    }


    public override void GoNextLevel()
    {
        switch (currentMusicIndex)
        {
            case 0:
                EnterCurrentLevel(false, "level6/music_1", "1");
                Koreographer.Instance.UnregisterForEvents("1_1", UpdateMethod);
                Koreographer.Instance.RegisterForEventsWithTime("1_1", UpdateMethod);

                //加载背景乐
                gameManager.PlaySound("level6/bg_1");

                break;
            case 1:
                Koreographer.Instance.UnregisterForEvents("1_1", UpdateMethod);

                EnterCurrentLevel(false, "level6/music_2", "2");
                Koreographer.Instance.UnregisterForEvents("2_1", UpdateMethod);
                Koreographer.Instance.RegisterForEventsWithTime("2_1", UpdateMethod);
                break;
            case 2:
                Koreographer.Instance.UnregisterForEvents("2_1", UpdateMethod);

                EnterCurrentLevel(false, "level6/music_3", "3");
                Koreographer.Instance.UnregisterForEvents("3_1", UpdateMethod);
                Koreographer.Instance.RegisterForEventsWithTime("3_1", UpdateMethod);
                break;
            case 3:
                Koreographer.Instance.UnregisterForEvents("3_1", UpdateMethod);
                gameManager.Send("ShowPanel");
                break;
            default:
                break;
        }

        base.GoNextLevel();
    }
}