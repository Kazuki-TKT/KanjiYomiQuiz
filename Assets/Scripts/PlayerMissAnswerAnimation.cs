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
    /// �����ԈႦ�����ɍs���A�j���[�V����
    /// </summary>
    public class PlayerMissAnswerAnimation : MonoBehaviour
    {
        // �f���Q�[�g�̒�`
        public delegate void MissAnimationCompleteHandler();

        // �f���Q�[�g�̃C�x���g
        public event MissAnimationCompleteHandler OnMissAnimationComplete;

        public float animationTime;

        //�h��
        [SerializeField]
        CinemachineImpulseSource impulseSource;

        //��
        [SerializeField]
        ParticleSystem[] smokeParticle;

        //�_���[�W���o
        [SerializeField]
        CanvasGroup damageCanvasGroup;

        CancellationTokenSource cts;
        void Start()
        {
            impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = animationTime;
        }

        public void PlayMissAnimation()
        {
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            MissAnimation(token).Forget();
        }
        
        public void StopMissAnimation()
        {
            cts?.Cancel();
        }
        private void NotifyMissAnimationComplete()
        {
            // �C�x���g���o�^����Ă��邩���m�F���Ă���ʒm
            OnMissAnimationComplete?.Invoke();
        }
        async UniTask  MissAnimation(CancellationToken token)
        {
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
        }
        
    }
}
