using System;
using System.Threading;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

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

        //�A�j���[�V��������
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

        //���ʉ�
        public AudioClip damageClip, steamClip;
        void Start()
        {
            impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = animationTime;
        }

        //�~�X�A�j���[�V�������s�����\�b�h
        public void PlayMissAnimation(CancellationToken token)
        {
            MissAnimation(token).Forget();
        }

        private void NotifyMissAnimationComplete()
        {
            // �C�x���g���o�^����Ă��邩���m�F���Ă���ʒm
            OnMissAnimationComplete?.Invoke();
        }

        //�~�X�A�j���[�V�����̃��\�b�h
        public async UniTask<bool>  MissAnimation(CancellationToken token)
        {
            bool end=false;

            AuidoManager.Instance.PlaySound_SE(damageClip);//�_���[�W��
            AuidoManager.Instance.PlaySound_SE(steamClip);//�X�`�[����

            foreach (ParticleSystem particleSystem in smokeParticle)//�X�`�[���G�t�F�N�g�̍Đ�
            {
                particleSystem.Play();
            }
            impulseSource.GenerateImpulse();//��ʂ�h�炷
            _ = damageCanvasGroup.DOFade(1, animationTime).SetEase(Ease.Flash,5);//��ʂ�Ԃ��_��

            //--�A�j���[�V�������ԑҋ@
            await UniTask.Delay(TimeSpan.FromSeconds(animationTime),cancellationToken:token);

            _ = damageCanvasGroup.DOFade(0, 0.2f);

            foreach (ParticleSystem particleSystem in smokeParticle)//�X�`�[���G�t�F�N�g�̏I��
            {
                particleSystem.Stop();
            }

            NotifyMissAnimationComplete();//�I����ʒm

            //--0.5�b�ҋ@
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

            end = true;
            return end;//�A�j���[�V�����I����Ԃ�
        }
        
    }
}
