using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonUtils
{
    /// <summary>
    /// ͨ��jsonת����list��������message��status
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
    /// ͨ��jsonת����list
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
    /// ͨ��jsonת����list��
    /// ��json���ж��Ƿ���pd[i]������о�ת����to���ͣ�һ�����ڶ�̬
    /// </summary>
    /// <typeparam name="T">����Ҫת��������</typeparam>
    /// <param name="text">json�ı�</param>
    /// <param name="pd">�����ж�</param>
    /// <param name="to">��Ҫת��������</param>
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
    /// ֻ���message��status
    /// </summary>
    /// <param name="text">json</param>
    /// <param name="message">��Ϣ</param>
    /// <param name="status">״̬</param>
    public static void GetStatusAndMessage(string text, out string message, out int status)
    {
        JObject jo = JObject.Parse(text);
        message = jo["message"].ToString();
        status = int.Parse(jo["status"].ToString());
    }

    /// <summary>
    /// ���һ��objct
    /// </summary>
    /// <typeparam name="T">����</typeparam>
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
