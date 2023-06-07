using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestUtils : MonoBehaviour 
{
    private static RequestUtils instance;
    private RequestUtils() { }
    public static RequestUtils Instance
    {
        get
        {
            instance ??= GameObject.Find("GameApp").GetComponent<RequestUtils>();
            return instance;
        }
    }

    public void GetText(string text, Action<string> resp)
    {
        StartCoroutine(IEGetText(text, resp));
    }
    /// <summary>
    /// �������󲢷���һ��text
    /// </summary>
    /// <param name="textName">�����text��</param>
    /// <param name="resp"></param>
    /// <returns></returns>
    IEnumerator IEGetText(string textName, Action<string> resp)
    {
        UnityWebRequest request = UnityWebRequest.Get(RequestConfig.url + textName + ".txt");
        yield return request.SendWebRequest();

        resp(request.downloadHandler.text);
    }

    public void GetAssetBundle(string abName, string abPath, Action<string, AssetBundle> resp)
    {
        StartCoroutine(IEGetAssetBundle(abName, abPath, resp));
    }
    /// <summary>
    /// �������󲢷���һ��assetBundle
    /// </summary>
    /// <param name="textName">�����ab��</param>
    /// <param name="resp"></param>
    /// <returns></returns>
    IEnumerator IEGetAssetBundle(string abName, string abPath, Action<string, AssetBundle> resp, string extension = ".ab")
    {
        string url = RequestConfig.url + abPath;
        if (url.LastIndexOf(extension) == -1) { url += extension; }
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle( url );
        yield return request.SendWebRequest();

        resp(abName, DownloadHandlerAssetBundle.GetContent(request));
    }
}
