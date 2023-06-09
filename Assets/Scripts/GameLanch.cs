using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLanch : MonoBehaviour
{
    private GameApp app;
    private void Awake()
    {
        #region 初始化游戏框架；资源管理，声音管理，网络管理等
        #endregion

        app = gameObject.AddComponent<GameApp>();
        GameStart();
    }

    public void GameStart()
    {
        // 检查更新

        // 进入游戏
        app.EnterGame();
    }
}
