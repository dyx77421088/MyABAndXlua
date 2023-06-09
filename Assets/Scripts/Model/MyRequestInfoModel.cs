using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���request�Żص�һЩ���Ե�
/// </summary>
public class MyRequestInfoModel
{
    // ���ص��ļ��Ĵ�С (bytes)
    private long size;
    // ����
    private float downloadProgress;
    // ���ص��ٶ� ��bytes/s)
    private long downloadSpeed;
    // ab��
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
