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
            UpdateGameState(GameState.Title);
        }

        public void UpdateGameState(GameState newState)
        {
            //gameState = newState;
            if (gameState == newState)return;
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
            Debug.Log($"<color=red>{gameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.Playng);
        }

        private void HandleTitle()
        {
            Debug.Log($"<color=red>{gameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.Title);
        }

        private void HandleGameClear()
        {
            Debug.Log($"<color=red>{gameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.GameClear);
        }

        private void HandleGameOver()
        {
            Debug.Log($"<color=red>{gameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.GameOver);
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
    /// �F
    /// </summary>
    public enum TextColor : uint
    {
        RED = 0xFF0000FF,    // ��
        GRN = 0x00FF00FF,    // ��
        BLR = 0x0000FFFF,    // ��
        ORANGE = 0xFFA500FF, // �I�����W
        YLW = 0xFFFF00FF,    // ���F
        PURPLE = 0x800080FF, // ��
        CYAN = 0x00FFFFFF,   // �V�A��
        MAGENTA = 0xFF00FFFF, // �}�[���^
        LIME = 0x32CD32FF,    // ���C��
        NAVY = 0x000080FF,    // �l�C�r�[
        WHITE = 0xFFFFFFFF,  // ��
        BLACK = 0x000000FF,   // ��
    }
}
