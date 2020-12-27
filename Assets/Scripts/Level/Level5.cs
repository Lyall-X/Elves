using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class LevelFive : Level
{
    public LevelFive(int goalScore) : base(goalScore)
    {

    }

    public override void InitLevel()
    {
        gameManager.StopMusic();
        base.InitLevel();

        //加载背景乐
        gameManager.PlaySound("level5/bg_1");
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
                EnterCurrentLevel(true, "level5/music_1", "1");
                Koreographer.Instance.UnregisterForEvents("1_1", UpdateMethod);
                Koreographer.Instance.RegisterForEventsWithTime("1_1", UpdateMethod);

                
                break;
            case 1:
                Koreographer.Instance.UnregisterForEvents("1_1", UpdateMethod);

                EnterCurrentLevel(false, "level5/music_2", "2");
                Koreographer.Instance.UnregisterForEvents("2_1", UpdateMethod);
                Koreographer.Instance.RegisterForEventsWithTime("2_1", UpdateMethod);

                
                //PlayMusic(StringManager.mainBG, true);
                //musicPlayer.LoadSong(gameManager.GetKoreoRes("MainBG"), 0, false);
                break;
            case 2:
                Koreographer.Instance.UnregisterForEvents("2_1", UpdateMethod);

                EnterCurrentLevel(false, "level5/music_3", "3");
                Koreographer.Instance.UnregisterForEvents("3_1", UpdateMethod);
                Koreographer.Instance.RegisterForEventsWithTime("3_1", UpdateMethod);
                break;
            case 3:
                Koreographer.Instance.UnregisterForEvents("3_1", UpdateMethod);

                EnterCurrentLevel(false, "level5/music_4", "4");
                Koreographer.Instance.UnregisterForEvents("4_1", UpdateMethod);
                Koreographer.Instance.RegisterForEventsWithTime("4_1", UpdateMethod);
                break;
            case 4:
                Koreographer.Instance.UnregisterForEvents("4_1", UpdateMethod);
                gameManager.Send("ShowCurrentLevelInformation");
                break;
            default:
                break;
        }

        base.GoNextLevel();
    }
}