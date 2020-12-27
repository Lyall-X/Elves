using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using System;
using System.IO;
/// <summary>
/// 资源管理者，管理所有所有物体的对象池，负责加载保存相应的资源
/// </summary>
public class ResourceManager
{
    private GameManager gameManager;
    public ResourceManager()
    {
        gameManager = GameManager.Instance;
        InitFactory();
        InitPoolDic();
    }

    #region 资源管理
    //资源工厂
    public ResFactory<Sprite> spritesFactory;
    public ResFactory<GameObject> prefabsFactory;
    public ResFactory<AudioClip> audioClipFactory;
    public ResFactory<Koreography> koreoFactory;
    /// <summary>
    /// 初始化资源工厂
    /// </summary>
    public void InitFactory()
    {
        spritesFactory = new ResFactory<Sprite>();
        spritesFactory.InitFactory(
            (resName)=>
            {
                if (spritesFactory.resDic.ContainsKey(resName))
                {
                    return spritesFactory.resDic[resName];
                }
                else
                {
                    return Resources.Load<Sprite>(StringManager.spritesResPath+resName);
                }
            }
            );
        prefabsFactory = new ResFactory<GameObject>();
        prefabsFactory.InitFactory(
             (resName) =>
             {
                 if (prefabsFactory.resDic.ContainsKey(resName))
                 {
                     return prefabsFactory.resDic[resName];
                 }
                 else
                 {
                     return Resources.Load<GameObject>(StringManager.prefabsResPath + resName);
                 }
             }
            );
        audioClipFactory = new ResFactory<AudioClip>();
        audioClipFactory.InitFactory(
           (resName) =>
           {
               if (audioClipFactory.resDic.ContainsKey(resName))
               {
                   return audioClipFactory.resDic[resName];
               }
               else
               {
                   return Resources.Load<AudioClip>(StringManager.audioclipResPath + resName);
               }
           }
            );
        koreoFactory = new ResFactory<Koreography>();
        koreoFactory.InitFactory(
             (resName) =>
             {
                 if (koreoFactory.resDic.ContainsKey(resName))
                 {
                     return koreoFactory.resDic[resName];
                 }
                 else
                 {
                     return Resources.Load<Koreography>(StringManager.koreoResPath + resName);
                 }
             }
            );
    }
    /// <summary>
    /// 获取相应资源
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public Sprite GetSpriteRes(string resName)
    {
        return spritesFactory.GetRes(resName);
    }
    public AudioClip GetAudioclipRes(string resName)
    {
        return audioClipFactory.GetRes(resName);
    }
    public GameObject GetPrefabRes(string resName)
    {
        return prefabsFactory.GetRes(resName);
    }
    public Koreography GetKoreoRes(string resName)
    {
        return koreoFactory.GetRes(resName);
    }
    #endregion

    #region 对象池
    //对象池字典
    public Dictionary<string, GameObjectPool> poolDic;
    /// <summary>
    /// 对象池字典的初始化方法
    /// </summary>
    public void InitPoolDic()
    {
        poolDic = new Dictionary<string, GameObjectPool>();
    }
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj(string objName,int initCount=0)
    {
        if (!poolDic.ContainsKey(objName))
        {
            poolDic.Add(objName,new GameObjectPool(prefabsFactory,objName,initCount));
        }
        return poolDic[objName].GetObj(objName);
    }
    /// <summary>
    /// 回收对象
    /// </summary>
    public void RecycleObj(string objName,GameObject obj,Action resetMethod=null)
    {
        if (!poolDic.ContainsKey(objName))
        {
            //Debug.Log("没有该类型的游戏物体对象池");
            return;
        }
        poolDic[objName].Recycle(obj,resetMethod);
    }
    #endregion
}
