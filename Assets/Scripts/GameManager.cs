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
