using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

namespace KanjiYomi
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        //�v���C���[�������C�t�̍ő�l
        const int MAX_PLAYER_LIFE = 3;

        /// <summary>
        /// �v���C���[�̃��C�t
        /// </summary>
        int playerLife;
        public int PlayerLife
        {
            get => playerLife;
            private set
            {
                playerLife = value;
                OnPlayerLifeChanged?.Invoke(playerLife); // ���C�t���ύX���ꂽ�Ƃ��ɃC�x���g�𔭐�������
            }
        }

        // �v���C���[�̃��C�t���ύX���ꂽ�Ƃ��ɒʒm����C�x���g
        public static event Action<int> OnPlayerLifeChanged;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            SetDefaultPlayerLife();
        }

        public void SetDefaultPlayerLife()
        {
            PlayerLife = MAX_PLAYER_LIFE; // �������C�t��ݒ�
        }

        // �v���C���[�̃��C�t�����炷���\�b�h
        public void DecreasePlayerLife()
        {
            PlayerLife--; // ���C�t�����炷
        }

    }
}
