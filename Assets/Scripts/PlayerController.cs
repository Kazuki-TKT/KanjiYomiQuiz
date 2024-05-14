using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace KanjiYomi
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        //プレイヤーがもつライフの最大値
        private int maxPlayerLife = 3;
        public int MaxPlayerLife { get => maxPlayerLife; }

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

        //マズルエフェクト
        [SerializeField]
        ParticleSystem[] muzzleParticle;
        //画面揺れ
        [SerializeField]
        CinemachineImpulseSource impulseSource;
        //重火器音
        public AudioClip gunFireClip;

        //タイムライン類
        [SerializeField]
        PlayableDirector cameraDirector;
        public TimelineAsset[] timelineAssets;
        bool directorStop;
        public bool DirectorStop { get => directorStop; }

        //プレイヤーがミスした時のアニメーション
        public PlayerMissAnswerAnimation answerMissAnimation;

        //インプットフィールド
        [SerializeField]
        PlayerInputFieldGUI playerInput;
       

        // プレイヤーのライフが変更されたときに通知するイベント
        public static event Action<int> OnPlayerLifeChanged;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            cameraDirector.played += Director_Played;
            cameraDirector.stopped += Director_Stopped;
        }
        private void OnEnable()
        {
            playerLife = maxPlayerLife;
        }
        public void SetDefaultPlayerLife()
        {
            PlayerLife = MaxPlayerLife; // 初期ライフを設定
        }

        // プレイヤーのライフを減らすメソッド
        public void DecreasePlayerLife()
        {
            PlayerLife--; // ライフを減らす
        }

        public void PlayCameraMove(TimelineAsset timelineAsset)
        {
            cameraDirector.Play(timelineAsset);
            
        }
        void Director_Played(PlayableDirector obj)
        {
            directorStop = false;
            playerInput.OffInputField();
        }

        void Director_Stopped(PlayableDirector obj)
        {
            directorStop = true;
            playerInput.OnInputField();

        }
        public void PlayerAttack(float duration)
        {
            StartCoroutine(GunAction(duration));
        }

        IEnumerator GunAction(float duration)
        {
            impulseSource.GenerateImpulse();
            AuidoManager.Instance.PlaySound_SE(gunFireClip);
            foreach (ParticleSystem particle in muzzleParticle)
            {
                particle.Play();
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(duration);

            foreach (ParticleSystem particle in muzzleParticle)
            {
                particle.Stop();
            }
        }

    }
}
