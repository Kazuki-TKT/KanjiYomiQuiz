using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace KanjiYomi
{
    /// <summary>
    /// �Q�[�����Ǘ�����V���O���g���p�^�[���̃N���X
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        //���݂̃X�e�[�g���i�[����
        public GameState currentGameState;

        /// <summary>
        /// �Q�[���̏�Ԃ��ύX���ꂽ�Ƃ��ɒʒm���󂯎�邽�߂̃C�x���g
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
            UpdateGameState(GameState.Title);//�Q�[���J�n���̃X�e�[�g��Title
        }

        /// <summary>
        /// �Q�[���̃X�e�[�g��ύX���郁�\�b�h
        /// </summary>
        /// <param name="newState">�V�����Q�[���X�e�[�g</param>
        public void UpdateGameState(GameState newState)
        {
            if (currentGameState == newState)return;//���݂̃X�e�[�g�Ɠ����ꍇ�̓��^�[��
            currentGameState = newState;//���݂̃X�e�[�g�ɐV�����X�e�[�g����
            switch (newState)//�X�e�[�g�ɂ���ĕ��򏈗�
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
            OnGameStateChanged?.Invoke(newState);//�o�^�����C�x���g�̎��s

        }

        //Playing���Ɏ��s����郁�\�b�h
        private void HandlePlaying()
        {
            Debug.Log($"<color=red>{currentGameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.Playng);
        }

        //Playing���Ɏ��s����郁�\�b�h
        private void HandleTitle()
        {
            Debug.Log($"<color=red>{currentGameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.Title);
        }

        //GameClear���Ɏ��s����郁�\�b�h
        private void HandleGameClear()
        {
            Debug.Log($"<color=red>{currentGameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.GameClear);
        }

        //GameOver���Ɏ��s����郁�\�b�h
        private void HandleGameOver()
        {
            Debug.Log($"<color=red>{currentGameState}</color>");
            AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.GameOver);
        }
    }

    /// <summary>
    /// �Q�[���̃X�e�[�g
    /// </summary>
    public enum GameState
    {
        Title,//�^�C�g��
        Playing,//�v���C���O
        GameOver,//�Q�[���I�[�o�[
        GameClear//�Q�[���N���A
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
