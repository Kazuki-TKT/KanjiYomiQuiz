using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

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

        //CancellationTokenSource cts;
        void Start()
        {
            impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = animationTime;
        }

        public void PlayMissAnimation(CancellationToken token)
        {
            MissAnimation(token).Forget();
        }
        
        public void StopMissAnimation()
        {
            //cts?.Cancel();
        }
        private void NotifyMissAnimationComplete()
        {
            // イベントが登録されているかを確認してから通知
            OnMissAnimationComplete?.Invoke();
        }
        public async UniTask<bool>  MissAnimation(CancellationToken token)
        {
            bool end;
            foreach(ParticleSystem particleSystem in smokeParticle)
            {
                particleSystem.Play();
            }
            impulseSource.GenerateImpulse();
            damageCanvasGroup.DOFade(1, animationTime).SetEase(Ease.Flash,5);
            await UniTask.Delay(TimeSpan.FromSeconds(animationTime),cancellationToken:token);
            damageCanvasGroup.DOFade(0, 0.2f);
            foreach (ParticleSystem particleSystem in smokeParticle)
            {
                particleSystem.Stop();
            }
            NotifyMissAnimationComplete();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            end = true;
            return end;
        }
        
    }
}
