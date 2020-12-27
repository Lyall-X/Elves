using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IResFactory<T>
{
    void InitFactory(Func<string,T> factoryMehtod);

    T GetRes(string resName);
}

public class ResFactory<T> : IResFactory<T>
{
    public Dictionary<string, T> resDic;

    private Func<string, T> mFactoryMethod;

    public T GetRes(string resName)
    {
        return mFactoryMethod(resName);
        //if (resDic.ContainsKey(resName))
        //{
        //    return resDic[resName];
        //}
        //else
        //{
        //    return Resources.Load<T>(resName);
        //}
    }

    public void InitFactory(Func<string, T> factoryMehtod)
    {
        resDic = new Dictionary<string, T>();
        mFactoryMethod = factoryMehtod;
    }
}

//public interface IResFactory
//{
//    void InitFactory();

//    object GetRes(string resName);
//}

//public class ResFactory : IResFactory
//{
//    public Dictionary<string, object> resDic;

//    public FactoryType factoryType;

//    public object GetRes(string resName)
//    {
//        if (resDic.ContainsKey(resName))
//        {
//            return resDic[resName];
//        }
//        else
//        {
//            //return Resources.Load<object>(resName);
//            return Resources.Load(resName);
//        }
//    }

//    public void InitFactory()
//    {
//        resDic = new Dictionary<string, object>();
//    }
//}
