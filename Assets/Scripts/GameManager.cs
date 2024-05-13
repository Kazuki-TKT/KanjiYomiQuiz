using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace KanjiYomi
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameState gameState;

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
            UpdateGameState(GameState.Playing);
        }

        public void UpdateGameState(GameState newState)
        {
            gameState = newState;

            switch (newState)
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

            OnGameStateChanged?.Invoke(newState);
        }


        private void HandlePlaying()
        {
        }

        private void HandleTitle()
        {
        }

        private void HandleGameClear()
        {

        }

        private void HandleGameOver()
        {

        }
    }

    public enum GameState
    {
        Title,
        Playing,
        GameOver,
        GameClear
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
