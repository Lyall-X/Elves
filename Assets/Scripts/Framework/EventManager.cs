using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 消息机制
/// 1.同模块上下级之间进行访问，可用可不用，老师推荐不用。消息机制的一个特点是谁订阅，谁会收到消息
/// 没有订阅就不会，如果我们代码开发内容很多很复杂，那么我们后期维护不方便查找相关引用。
/// 2.跨模块访问调用管理者相关方法，可用可不用，推荐不用。因为GameManager里提供了必要的外部可访问
/// 接口方法，直接调用管理里边的方法就可以了。封装的很好，访问者不需要去认识除了GameManager以外的对象
/// 3.跨模块同级或者下层级之间互相调用，要使用。防止耦合性太高。
/// </summary>
class EventManager
{         
    private Dictionary<string, Action<object>> mRegisterMsgs;
    public EventManager()
    {
        mRegisterMsgs = new Dictionary<string, Action<object>>();
    }

    public void Register(string msgName,Action<object> onMsgReceived)
    {
        //if (!mRegisterMsgs.ContainsKey(msgName))
        //{
        //    mRegisterMsgs.Add(msgName,onMsgReceived);
        //}
        //else
        //{
        //    mRegisterMsgs[msgName] += onMsgReceived;
        //}
        if (!mRegisterMsgs.ContainsKey(msgName))
        {
            mRegisterMsgs.Add(msgName, _=> { });
        }
        mRegisterMsgs[msgName] += onMsgReceived;
    }

    public void UnRegister(string msgName, Action<object> onMsgReceived)
    {
        if (mRegisterMsgs.ContainsKey(msgName))
        {
            mRegisterMsgs[msgName] -= onMsgReceived;
        }
    }

    public void Send(string msgName,object data)
    {
        if (mRegisterMsgs.ContainsKey(msgName))
        {
            mRegisterMsgs[msgName](data);
        }
    }
}

#region 特异性消息管理
public struct TalkPanelMsg
{
    public string[] talks;
    public bool[] talkOrder;
    public bool captionBetrayal;
    public int enterGame;
}

public struct TalkIndexMsg
{
    public bool ifAdd;
    public int addCount;
}
#endregion

