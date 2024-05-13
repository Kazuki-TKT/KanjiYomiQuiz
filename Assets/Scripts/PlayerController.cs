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
        //プレイヤーがもつライフの最大値
        const int MAX_PLAYER_LIFE = 3;

        /// <summary>
        /// プレイヤーのライフ
        /// </summary>
        int playerLife;
        public int PlayerLife
        {
            get => playerLife;
            private set
            {
                playerLife = value;
                OnPlayerLifeChanged?.Invoke(playerLife); // ライフが変更されたときにイベントを発生させる
            }
        }

        // プレイヤーのライフが変更されたときに通知するイベント
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
            PlayerLife = MAX_PLAYER_LIFE; // 初期ライフを設定
        }

        // プレイヤーのライフを減らすメソッド
        public void DecreasePlayerLife()
        {
            PlayerLife--; // ライフを減らす
        }

    }
}
