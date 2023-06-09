using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存放request放回的一些属性的
/// </summary>
public class MyRequestInfoModel
{
    // 下载的文件的大小 (bytes)
    private long size;
    // 进度
    private float downloadProgress;
    // 下载的速度 （bytes/s)
    private long downloadSpeed;
    // ab包
    private AssetBundle ab;

    public MyRequestInfoModel(long size, float downloadProgress, long downloadSpeed, AssetBundle ab)
    {
        this.size = size;
        this.downloadProgress = downloadProgress;
        this.downloadSpeed = downloadSpeed;
        this.Ab = ab;
    }

    public MyRequestInfoModel()
    {
        
    }
    public long Size { get => size; set => size = value; }
    public float DownloadProgress { get => downloadProgress; set => downloadProgress = value; }
    public long DownloadSpeed { get => downloadSpeed; set => downloadSpeed = value; }
    public AssetBundle Ab { get => ab; set => ab = value; }

    public override string ToString()
    {
        return JsonUtils.Success(this);
    }
}
