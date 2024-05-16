using System;
using System.Threading;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// 問題を間違えた時に行うアニメーション
    /// </summary>
    public class PlayerMissAnswerAnimation : MonoBehaviour
    {
        // デリゲートの定義
        public delegate void MissAnimationCompleteHandler();

        // デリゲートのイベント
        public event MissAnimationCompleteHandler OnMissAnimationComplete;

        //アニメーション時間
        public float animationTime;

        //揺れ
        [SerializeField]
        CinemachineImpulseSource impulseSource;

        //煙
        [SerializeField]
        ParticleSystem[] smokeParticle;

        //ダメージ演出
        [SerializeField]
        CanvasGroup damageCanvasGroup;

        //効果音
        public AudioClip damageClip, steamClip;
        void Start()
        {
            impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = animationTime;
        }

        //ミスアニメーションを行うメソッド
        public void PlayMissAnimation(CancellationToken token)
        {
            MissAnimation(token).Forget();
        }

        private void NotifyMissAnimationComplete()
        {
            // イベントが登録されているかを確認してから通知
            OnMissAnimationComplete?.Invoke();
        }

        //ミスアニメーションのメソッド
        public async UniTask<bool>  MissAnimation(CancellationToken token)
        {
            bool end=false;

            AuidoManager.Instance.PlaySound_SE(damageClip);//ダメージ音
            AuidoManager.Instance.PlaySound_SE(steamClip);//スチーム音

            foreach (ParticleSystem particleSystem in smokeParticle)//スチームエフェクトの再生
            {
                particleSystem.Play();
            }
            impulseSource.GenerateImpulse();//画面を揺らす
            _ = damageCanvasGroup.DOFade(1, animationTime).SetEase(Ease.Flash,5);//画面を赤く点滅

            //--アニメーション時間待機
            await UniTask.Delay(TimeSpan.FromSeconds(animationTime),cancellationToken:token);

            _ = damageCanvasGroup.DOFade(0, 0.2f);

            foreach (ParticleSystem particleSystem in smokeParticle)//スチームエフェクトの終了
            {
                particleSystem.Stop();
            }

            NotifyMissAnimationComplete();//終了を通知

            //--0.5秒待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

            end = true;
            return end;//アニメーション終了を返す
        }
        
    }
}
