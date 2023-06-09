using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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
            instance ??= new GameObject("requestUtils").AddComponent<RequestUtils>();
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
    IEnumerator IEGetText(string textName, Action<string> resp, string extension = ".txt")
    {
        UnityWebRequest request = UnityWebRequest.Get(RequestConfig.url + textName + extension);
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
    public void GetAssetBundleToUnityWebRequest(string abName, string abPath, Action<string, MyRequestInfoModel> resp)
    {
        StartCoroutine(IEGetAssetBundleToUnityWebRequest(abName, abPath, resp));
    }
    /// <summary>
    /// �������󲢷���һ�� UnityWebRequest
    /// </summary>
    /// <param name="textName">�����ab��</param>
    /// <param name="resp"></param>
    /// <returns></returns>
    IEnumerator IEGetAssetBundleToUnityWebRequest(string abName, string abPath, Action<string, MyRequestInfoModel> resp, string extension = ".ab")
    {
        string url = RequestConfig.url + abPath;
        if (url.LastIndexOf(extension) == -1) { url += extension; }

        //using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url))
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            ulong size = 0, preSize = 0;
            float js = 0; // ��ʱ
            MyRequestInfoModel reModel = new MyRequestInfoModel();
            while (!request.isDone)
            {
                size = request.downloadedBytes;
                js += Time.deltaTime;
                if (reModel.Size == 0)
                {
                    reModel.Size = UnityWebRequestHeadUtils.GetResponseHeaderLength(request);
                }
                if (js >= 0.5f) // 0.5���ͳ���ٶ�
                {
                    reModel.DownloadSpeed = (long)((size - preSize) / js);
                    preSize = size;
                    js = 0;
                }
                reModel.DownloadProgress = request.downloadProgress;
                //Debug.Log(size + ", " + preSize);
                //Debug.Log("�ܴ�СΪ:" + DataUtils.GetDataSize(UnityWebRequestHeadUtils.GetResponseHeaderLength(request)));
                //Debug.Log("��ǰ����Ϊ" + request.downloadProgress + "�ٶȣ�" + (size - preSize) / Time.deltaTime);
                resp(abName, reModel);
                yield return null;
            }
            yield return new WaitUntil(() => request.isDone);
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("����ʧ��: " + request.error);
            }
            else
            {
                Debug.Log("���سɹ�!");
                //reModel.Ab = DownloadHandlerAssetBundle.GetContent(request);
                //File.WriteAllBytes($"{ FileUtils.downPath}/{name}.ab ", request.downloadHandler.data);

                reModel.Data = request.downloadHandler.data;
                reModel.Ab = AssetBundle.LoadFromMemory(reModel.Data);
                reModel.DownloadProgress = request.downloadProgress;
                resp(abName, reModel);
            }
        }
    }
}
