using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLanch : MonoBehaviour
{
    private GameApp app;
    private void Awake()
    {
        #region ��ʼ����Ϸ��ܣ���Դ��������������������
        #endregion

        app = gameObject.AddComponent<GameApp>();
        GameStart();
    }

    public void GameStart()
    {
        // ������

        // ������Ϸ
        app.EnterGame();
    }
}
