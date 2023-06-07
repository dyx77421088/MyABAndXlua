using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GameApp : MonoBehaviour
{
    public static Dictionary<string, GameObject> goDict = new Dictionary<string, GameObject>();

    private string webVersion = RequestConfig.versionPath;
    private string localVersion = FileUtils.versionPath;

    private RequestUtils reqUtils;
    //private string version = FileUtils.versionPath;
    public void EnterGame()
    {
        reqUtils = RequestUtils.Instance;
        reqUtils.GetText(webVersion, GetVersion);
    }

    private void GetVersion(string versionText)
    {
    Debug.Log(versionText);
        if (versionText != FileUtils.GetTextByPath(localVersion))
        {
            Debug.Log("版本号不同，需要更新....");
            reqUtils.GetText(RequestConfig.GetVersionJsonPath(versionText), GetUpdataJson);
            //FileUtils.WriteTextByPath("version.txt", versionText);
        }
        else
        {
            EnterGameScene();
        }
    }

    /// <summary>
    /// 请求成功后获得的json(版本更新需要更新哪些ab的json）
    /// </summary>
    /// <param name="updataJson">json</param>
    private void GetUpdataJson(string updataJson)
    {
        List<AssetBundleModel> abs = JsonUtils.GetList<AssetBundleModel>(updataJson);
        
        foreach (AssetBundleModel ab in abs)
        {
            reqUtils.GetAssetBundle(ab.Name, ab.Path, SetAssetBundle);
        }
    }

    /// <summary>
    /// 从网上获得ab包之后的处理工作
    /// </summary>
    /// <param name="ab"></param>
    private void SetAssetBundle(string name, AssetBundle ab)
    {
        GameObject go = ab.LoadAsset<GameObject>(name);
        goDict.Add(name, go);

        Instantiate(go);
    }

    public GameObject GetGameObject(string name)
    {
        return goDict[name];
    }

    private void EnterGameScene()
    {
        Debug.Log("进入游戏");
    }

}
