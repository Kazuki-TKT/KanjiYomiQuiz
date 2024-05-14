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
        //�v���C���[�������C�t�̍ő�l
        private int maxPlayerLife = 3;
        public int MaxPlayerLife { get => maxPlayerLife; }

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

        //�}�Y���G�t�F�N�g
        [SerializeField]
        ParticleSystem[] muzzleParticle;
        //��ʗh��
        [SerializeField]
        CinemachineImpulseSource impulseSource;
        //�d�Ί퉹
        public AudioClip gunFireClip;

        //�^�C�����C����
        [SerializeField]
        PlayableDirector cameraDirector;
        public TimelineAsset[] timelineAssets;
        bool directorStop;
        public bool DirectorStop { get => directorStop; }

        //�v���C���[���~�X�������̃A�j���[�V����
        public PlayerMissAnswerAnimation answerMissAnimation;

        //�C���v�b�g�t�B�[���h
        [SerializeField]
        PlayerInputFieldGUI playerInput;
       

        // �v���C���[�̃��C�t���ύX���ꂽ�Ƃ��ɒʒm����C�x���g
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
            PlayerLife = MaxPlayerLife; // �������C�t��ݒ�
        }

        // �v���C���[�̃��C�t�����炷���\�b�h
        public void DecreasePlayerLife()
        {
            PlayerLife--; // ���C�t�����炷
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
