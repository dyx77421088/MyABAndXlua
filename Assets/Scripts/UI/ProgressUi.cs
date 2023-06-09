using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressUi : MonoBehaviour
{
    public Slider slider;
    public Text progressText;
    public GameObject loading;

    private static ProgressUi instance;
    private ProgressUi() { }
    public static ProgressUi Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // loading 文字循环播放放大
        loading.transform.DOScale(Vector3.one * 1.1f, 1).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// 设置当前的进度
    /// </summary>
    public void SetProgress(float progress)
    {
        slider.value = progress;
    }

    public void SetProgressText(string downloadSpeed, string size)
    {
        progressText.text = downloadSpeed + "/S " + size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
