using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class KaraokeController : MonoBehaviour
{
    public List<KaraokeTextLine> textLines;
    private Animator animator;
    private int lineAddIdx;

    // Start is called before the first frame update
    void Start()
    {
        lineAddIdx = 0;
        Koreographer.Instance.RegisterForEventsWithTime(StringManager.lyricEventID,OnLyricEvent);
        Koreographer.Instance.RegisterForEvents(StringManager.lineEventID,OnFeedLine);
        animator = GetComponent<Animator>();
    }
    //歌词填充事件
    private void OnLyricEvent(KoreographyEvent koreoEvt,int sampleTime,int sampleDelta,DeltaSlice slice)
    {
        string lyric = koreoEvt.GetTextValue();
        float percent = koreoEvt.GetEventDeltaAtSampleTime(sampleTime);
        for (int i = 0; i < textLines.Count; i++)
        {
            KaraokeTextLine curLine = textLines[i];
            if (!curLine.IsLineFull())
            {
                curLine.FillkaraokeSection(lyric,percent);
                break;
            }
        }
    }
    //歌词行更新事件
    private void OnFeedLine(KoreographyEvent koreoEvt)
    {
        string lyrics = koreoEvt.GetTextValue();
        textLines[lineAddIdx].SetTextState(lyrics,0,0);
        if (lineAddIdx==textLines.Count-1)
        {
            animator.SetTrigger("Feed");
        }
        else
        {
            lineAddIdx++;
        }
    }
    //文本动画播放完（整体上移之后）执行的回调方法
    private void OnLineFeedComplete()
    {
        for (int i = 0; i < textLines.Count-1; i++)
        {
            KaraokeTextLine copyTarget = textLines[i + 1];
            textLines[i].SetTextState(copyTarget.baseText.text,copyTarget.maskRectTrans.rect.width,copyTarget.textFillIdx);
        }
    }
    //文本清除方法
    public void ClearText()
    {
        for (int i = 0; i < textLines.Count; i++)
        {
            textLines[i].ClearText();
        }
    }
}
