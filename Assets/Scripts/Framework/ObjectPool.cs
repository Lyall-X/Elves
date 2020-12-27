using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPool
{
    GameObject GetObj(string objName);

    void Recycle(GameObject obj, Action resetMethod = null);
}

public class GameObjectPool:IPool
{
    private ResFactory<GameObject> mFactory;

    private Stack<GameObject> mCacheStack;
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="resFactory"></param>
    /// <param name="objName"></param>
    /// <param name="initCount"></param>
    public GameObjectPool(ResFactory<GameObject> resFactory,string objName,int initCount=0)
    {
        mFactory = resFactory;
        mCacheStack = new Stack<GameObject>(); 
        for (int i = 0; i < initCount; i++)
        {
            mCacheStack.Push(GetObj(objName));
        }
    }
    /// <summary>
    /// 获取游戏物体对象
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public GameObject GetObj(string objName)
    {
        GameObject obj;
        if (mCacheStack.Count>0)
        {
            obj = mCacheStack.Pop();
        }
        else
        {
            obj = GameObject.Instantiate(mFactory.GetRes(objName));
        }
        obj.SetActive(true);
        return obj;
    }
    /// <summary>
    /// 回收不使用的对象到对象池
    /// </summary>
    /// <param name="obj"></param>
    public void Recycle(GameObject obj,Action resetMethod=null)
    {
        if (resetMethod!=null)
        {
            resetMethod();
        }
        obj.SetActive(false);
        mCacheStack.Push(obj);
    }
}
