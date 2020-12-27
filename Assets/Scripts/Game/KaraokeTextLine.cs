using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaraokeTextLine : MonoBehaviour
{
    public Text baseText;
    public Text overlayText;
    public RectTransform maskRectTrans;

    public int textFillIdx;

    // Start is called before the first frame update
    void Start()
    {
        textFillIdx = 0;
    }
    /// <summary>
    /// 设置文本状态
    /// </summary>
    /// <param name="textLine">当前歌词的文本内容</param>
    /// <param name="maskRightPos">遮罩右位置</param>
    /// <param name="textPos">本文填充到的位置</param>
    public void SetTextState(string textLine,float maskRightPos,int textPos)
    {
        //白字
        baseText.text = textLine;
        //蓝字
        overlayText.text = textLine;
        //遮罩位置设置
        maskRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,maskRightPos);
        //记录一下当前填充状态
        textFillIdx = textPos;
    }
    //文本填充方法
    public bool FillkaraokeSection(string lyric,float eventPercent)
    {
        bool finishFilled = false;
        if (textFillIdx>=0)
        {

        }
        int endIdx = textFillIdx + (lyric.Length - 1);
        UICharInfo[] charInfos = baseText.cachedTextGenerator.GetCharactersArray();
        if (charInfos.Length>0)
        {
            //比例系数(从像素转换成以Unity为单位的长度)
            float scaleOffset = 1 / baseText.pixelsPerUnit;
            //以像素为单位的遮罩填充长度转换为以Unity长度为单位的变量
            float startPos = charInfos[textFillIdx].cursorPos.x * scaleOffset;
            if (endIdx>=charInfos.Length)
            {
                //填充完成
                return true;
            }
            float endPos = (charInfos[endIdx].cursorPos.x + charInfos[endIdx].charWidth) * scaleOffset;
            float maskRightPosX = Mathf.Lerp(startPos,endPos,eventPercent);
            maskRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,maskRightPosX);
            if (eventPercent>=1)
            {
                textFillIdx = endIdx + 1;
            }
            finishFilled = true;
        }
        return finishFilled;
    }
    //判断当前填充是否完成
    public bool IsLineFull()
    {
        return textFillIdx >= baseText.text.Length;
    }

    public void ClearText()
    {
        baseText.text = overlayText.text = "";
    }
}
