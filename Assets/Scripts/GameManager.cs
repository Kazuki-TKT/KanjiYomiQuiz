using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace KanjiYomi
{
    /// <summary>
    /// ゲームを管理するシングルトンパターンのクラス
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        //現在のステートを格納する
        public GameState currentGameState;

        /// <summary>
        /// ゲームの状態が変更されたときに通知を受け取るためのイベント
        /// </summary>
        public static event Action<GameState> OnGameStateChanged;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            UpdateGameState(GameState.Title);//ゲーム開始時のステートはTitle
        }

        /// <summary>
        /// ゲームのステートを変更するメソッド
        /// </summary>
        /// <param name="newState">新しいゲームステート</param>
        public void UpdateGameState(GameState newState)
        {
            if (currentGameState == newState)return;//現在のステートと同じ場合はリターン
            currentGameState = newState;//現在のステートに新しいステートを代入
            switch (newState)//ステートによって分岐処理
            {
                case GameState.Title:
                    HandleTitle();
                    break;
                case GameState.Playing:
                    HandlePlaying();
                    break;
                case GameState.GameOver:
                    HandleGameOver();
                    break;
                case GameState.GameClear:
                    HandleGameClear();
                    break;
            }
            OnGameStateChanged?.Invoke(newState);//登録したイベントの実行

        }

        //Playing時に実行されるメソッド
        private void HandlePlaying()
        {
            Debug.Log($"<color=red>{currentGameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.Playng);
        }

        //Playing時に実行されるメソッド
        private void HandleTitle()
        {
            Debug.Log($"<color=red>{currentGameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.Title);
        }

        //GameClear時に実行されるメソッド
        private void HandleGameClear()
        {
            Debug.Log($"<color=red>{currentGameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.GameClear);
        }

        //GameOver時に実行されるメソッド
        private void HandleGameOver()
        {
            Debug.Log($"<color=red>{currentGameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.GameOver);
        }
    }

    /// <summary>
    /// ゲームのステート
    /// </summary>
    public enum GameState
    {
        Title,//タイトル
        Playing,//プレイング
        GameOver,//ゲームオーバー
        GameClear//ゲームクリア
    }

    /// <summary>
    /// 色
    /// </summary>
    public enum TextColor : uint
    {
        RED = 0xFF0000FF,    // 赤
        GRN = 0x00FF00FF,    // 緑
        BLR = 0x0000FFFF,    // 青
        ORANGE = 0xFFA500FF, // オレンジ
        YLW = 0xFFFF00FF,    // 黄色
        PURPLE = 0x800080FF, // 紫
        CYAN = 0x00FFFFFF,   // シアン
        MAGENTA = 0xFF00FFFF, // マゼンタ
        LIME = 0x32CD32FF,    // ライム
        NAVY = 0x000080FF,    // ネイビー
        WHITE = 0xFFFFFFFF,  // 白
        BLACK = 0x000000FF,   // 黒
    }
}
