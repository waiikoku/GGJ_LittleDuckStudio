using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public abstract class XmlData<T> where T : class
{
    public static void Save(string path, T data, Action<bool> callback = null)
    {
        try
        {
            XmlSerializer serializer = CreateSerializer();
            FileStream stream = CreateStream(path, FileMode.Create); //???????????????????????????????????
            serializer.Serialize(stream, data); //?????????? T ?????????????????????Xml
            stream.Close();//???????????
            callback?.Invoke(true);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            callback?.Invoke(false);
            throw;
        }

    }

    public static T Load(string path, Action<bool> callback = null)
    {
        if (File.Exists(path) == false)
        {
            return null;
        }
        try
        {
            XmlSerializer serializer = CreateSerializer();
            FileStream stream = CreateStream(path, FileMode.Open); //??????????????????????????????????
            T data = serializer.Deserialize(stream) as T; //??????????????????Xml?????????????? T
            stream.Close(); //??????????Xml??????????????? T
            callback?.Invoke(true);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            callback?.Invoke(false);
            return null;
            throw;
        }
    }

    private static XmlSerializer CreateSerializer()
    {
        return new XmlSerializer(typeof(T));
    }

    private static FileStream CreateStream(string fileName, FileMode mode)
    {
        return new FileStream(fileName, mode);
    }
}
