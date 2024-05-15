using System.Collections;
using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace KanjiYomi
{
    /// <summary>
    /// �v���C���[���Ǘ�����C���X�^���X�N���X
    /// </summary>
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

        //--�^�C�����C����
        [SerializeField]
        PlayableDirector cameraDirector;
        public TimelineAsset[] timelineAssets;
        bool directorStop;//�^�C�����C���̏I�����Ǘ�����u�[���^
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
            cameraDirector.played += Director_Played;//�^�C�����C���J�n���Ɏ��s����鏈����o�^
            cameraDirector.stopped += Director_Stopped;//�^�C�����C���I�����Ɏ��s����鏈����o�^
        }
        private void OnEnable()
        {
            SetDefaultPlayerLife();
        }

        //�v���C���[�̃��C�t�������l�ɂ��郁�\�b�h
        public void SetDefaultPlayerLife()
        {
            PlayerLife = MaxPlayerLife; // �������C�t��ݒ�
        }

        // �v���C���[�̃��C�t�����炷���\�b�h
        public void DecreasePlayerLife()
        {
            PlayerLife--; // ���C�t�����炷
        }

        //�w��̃^�C�����C�����Đ����郁�\�b�h
        public void PlayCameraMove(TimelineAsset timelineAsset)
        {
            cameraDirector.Play(timelineAsset);
            
        }

        //�^�C�����C���J�n���Ɏ��s����郁�\�b�h
        void Director_Played(PlayableDirector obj)
        {
            directorStop = false;//�^�C�����C���̏I�����Ǘ�����u�[����OFF
            playerInput.OffInputField();//�C���v�b�g�t�B�[���h��OFF
        }

        //�^�C�����C���I�����Ɏ��s����郁�\�b�h
        void Director_Stopped(PlayableDirector obj)
        {
            directorStop = true;//�^�C�����C���̏I�����Ǘ�����u�[����ON
            playerInput.OnInputField();//�C���v�b�g�t�B�[���h��ON

        }

        //�v���C���[�̍U�����s�����\�b�h
        public void PlayerAttack(float duration)
        {
            StartCoroutine(GunAction(duration));
        }

        //�K���A�N�V�������s���R���[�`��
        IEnumerator GunAction(float duration)
        {
            impulseSource.GenerateImpulse();//��ʂ�h�炷
            AuidoManager.Instance.PlaySound_SE(gunFireClip);//�d�Ί퉹�̍Đ�
            foreach (ParticleSystem particle in muzzleParticle)//�}�Y���G�t�F�N�g�̍Đ�
            {
                particle.Play();
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(duration);//�w��b�ҋ@

            foreach (ParticleSystem particle in muzzleParticle)//�}�Y���G�t�F�N�g�̏I��
            {
                particle.Stop();
            }
        }

    }
}
