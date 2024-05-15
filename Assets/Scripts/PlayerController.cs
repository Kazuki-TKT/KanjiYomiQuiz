using System.Collections;
using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace KanjiYomi
{
    /// <summary>
    /// プレイヤーを管理するインスタンスクラス
    /// </summary>
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

        //--タイムライン類
        [SerializeField]
        PlayableDirector cameraDirector;
        public TimelineAsset[] timelineAssets;
        bool directorStop;//タイムラインの終了を管理するブール型
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
            cameraDirector.played += Director_Played;//タイムライン開始時に実行される処理を登録
            cameraDirector.stopped += Director_Stopped;//タイムライン終了時に実行される処理を登録
        }
        private void OnEnable()
        {
            SetDefaultPlayerLife();
        }

        //プレイヤーのライフを初期値にするメソッド
        public void SetDefaultPlayerLife()
        {
            PlayerLife = MaxPlayerLife; // 初期ライフを設定
        }

        // プレイヤーのライフを減らすメソッド
        public void DecreasePlayerLife()
        {
            PlayerLife--; // ライフを減らす
        }

        //指定のタイムラインを再生するメソッド
        public void PlayCameraMove(TimelineAsset timelineAsset)
        {
            cameraDirector.Play(timelineAsset);
            
        }

        //タイムライン開始時に実行されるメソッド
        void Director_Played(PlayableDirector obj)
        {
            directorStop = false;//タイムラインの終了を管理するブールをOFF
            playerInput.OffInputField();//インプットフィールドをOFF
        }

        //タイムライン終了時に実行されるメソッド
        void Director_Stopped(PlayableDirector obj)
        {
            directorStop = true;//タイムラインの終了を管理するブールをON
            playerInput.OnInputField();//インプットフィールドをON

        }

        //プレイヤーの攻撃を行うメソッド
        public void PlayerAttack(float duration)
        {
            StartCoroutine(GunAction(duration));
        }

        //ガンアクションを行うコルーチン
        IEnumerator GunAction(float duration)
        {
            impulseSource.GenerateImpulse();//画面を揺らす
            AuidoManager.Instance.PlaySound_SE(gunFireClip);//重火器音の再生
            foreach (ParticleSystem particle in muzzleParticle)//マズルエフェクトの再生
            {
                particle.Play();
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(duration);//指定秒待機

            foreach (ParticleSystem particle in muzzleParticle)//マズルエフェクトの終了
            {
                particle.Stop();
            }
        }

    }
}
