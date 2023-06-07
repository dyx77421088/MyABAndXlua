using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonUtils
{
    /// <summary>
    /// 通过json转换成list，并返回message和status
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text"></param>
    /// <param name="message"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static List<T> GetList<T>(string text, out string message, out int status)
    {
        List<T> list = new List<T>();
        JObject jo = JObject.Parse(text);
        message = jo["message"].ToString();
        status = int.Parse(jo["status"].ToString());

        JArray ja = JArray.Parse(jo["data"].ToString());
        foreach (JObject item in ja)
        {
            T p = item.ToObject<T>();
            list.Add(p);
        }
        return list;
    }
    /// <summary>
    /// 通过json转换成list
    /// </summary>
    public static List<T> GetList<T>(string text)
    {
        List<T> list = new List<T>();
        JObject jo = JObject.Parse(text);

        JArray ja = JArray.Parse(jo["data"].ToString());
        foreach (JObject item in ja)
        {
            T p = item.ToObject<T>();
            list.Add(p);
        }
        return list;
    }

    /// <summary>
    /// 通过json转换成list，
    /// 在json中判断是否有pd[i]，如果有就转换成to类型，一般用在多态
    /// </summary>
    /// <typeparam name="T">最终要转换的类型</typeparam>
    /// <param name="text">json文本</param>
    /// <param name="pd">条件判断</param>
    /// <param name="to">需要转换的类型</param>
    /// <returns></returns>
    public static List<T> GetList<T>(string text, string[] pd, Type[] to)
    {
        List<T> list = new List<T>();
        JObject jo = JObject.Parse(text);

        JArray ja = JArray.Parse(jo["data"].ToString());
        foreach (JObject item in ja)
        {
            bool isOk = false;
            for (int i = 0; i < pd.Length; ++i)
            {
                if (item[pd[i]] != null)
                {
                    list.Add((T)item.ToObject(to[i]));
                    isOk = true;
                    break;
                }
            }
            if (!isOk)
            {
                T p = item.ToObject<T>();
                list.Add(p);
            }
        }
        return list;
    }

    /// <summary>
    /// 只获得message和status
    /// </summary>
    /// <param name="text">json</param>
    /// <param name="message">消息</param>
    /// <param name="status">状态</param>
    public static void GetStatusAndMessage(string text, out string message, out int status)
    {
        JObject jo = JObject.Parse(text);
        message = jo["message"].ToString();
        status = int.Parse(jo["status"].ToString());
    }

    /// <summary>
    /// 获得一个objct
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="text">json</param>
    /// <returns></returns>
    public static T GetObject<T>(string text)
    {
        JObject jo = JObject.Parse(text);
        return jo["data"].ToObject<T>();
    }





    ////////////////////////////////json to string///////////////////////////////////////////////////

    public static string Info(int status, object info)
    {
        return "{\"status\":" + status + ",\"message\":\"" + info + "\"}";
    }

    public static string Info(int status, string message, object data, bool useSerialize = true)
    {
        return "{\"status\":" + status + ",\"message\":\"" + message + "\" " + ",\"data\":" +
               (useSerialize ? JsonConvert.SerializeObject(data) : data)
            + " }";
    }

    public static string Success(object data, bool useSerialize = true)
    {
        return "{\"status\":" + 200 + ",\"data\":" +
                (useSerialize ? JsonConvert.SerializeObject(data) : data)
            + "}";
    }
}
