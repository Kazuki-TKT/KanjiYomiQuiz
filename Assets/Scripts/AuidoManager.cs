using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KanjiYomi
{
    /// <summary>
    /// �T�E���h�֘A���Ǘ�����V���O���g���p�^�[���̃N���X
    /// </summary>
    public class AuidoManager : MonoBehaviour
    {
        public static AuidoManager Instance;

        //�I�[�f�B�I�~�L�T�[�O���[�v�iMaster,BGM,SE�j
        public AudioMixerGroup masterMixerGroup, bgmMixerGroup, seMixerGroup;

        // �����Đ��\�ȃI�[�f�B�I�\�[�X�̍ő吔
        public int maxAudioSources = 8; 

        //BGM��SE�̃I�[�f�B�I�\�[�X
        [SerializeField]
        AudioSource audioSources_BGM;
        private List<AudioSource> audioSources_SE;

        //BGM�̃f�[�^
        [SerializeField]
        List<BGMData> bgmDatas;

        //�S�{�^���ɔ��f������SE�̃N���b�v(�N���b�N�A�G���^�[)
        public AudioClip buttonClickSE,buttonEnterSE;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            //BGM�p�̃I�[�f�B�I�~�L�T�[�O���[�v���w��
            audioSources_BGM.outputAudioMixerGroup = bgmMixerGroup;

            // SEList�̏�����
            audioSources_SE = new List<AudioSource>();
            // �ŏ��ɍő吔����SE�̃I�[�f�B�I�\�[�X�𐶐�
            for (int i = 0; i < maxAudioSources; i++)
            {
                audioSources_SE.Add(gameObject.AddComponent<AudioSource>());
            }

            foreach (AudioSource source in audioSources_SE)
            {
                source.outputAudioMixerGroup = seMixerGroup; // SE�p�̃I�[�f�B�I�~�L�T�[�O���[�v���w��
                source.playOnAwake = false;//PlayOnAwake���I�t
            }
        }

        /// <summary>
        /// BGM��炷���\�b�h
        /// </summary>
        /// <param name="bgm">BGM�̃^�C�v</param>
        public void PlaySound_BGM(BGMData.BGM bgm)
        {
            var data = GetBGMData(bgm);
            audioSources_BGM.clip = data.audioClip;
            audioSources_BGM.volume = data.volume;
            audioSources_BGM.Play();
        }

        /// <summary>
        /// BGM��Fade�`�F���W���Ė炷���\�b�h
        /// </summary>
        /// <param name="bgm">BGM�̃^�C�v</param>
        public void FadeChangeBGM(BGMData.BGM bgm)
        {
            var data = GetBGMData(bgm);
            audioSources_BGM.DOFade(0, 0.5f).OnComplete(() => {
                audioSources_BGM.clip = data.audioClip;
                audioSources_BGM.volume = data.volume;
                audioSources_BGM.DOFade(data.volume, 0.2f);
            });
        }

        /// <summary>
        /// SE��炷���\�b�h
        /// </summary>
        public void PlaySound_SE(AudioClip clip, float volume = 1f)
        {
            // �g�p�\�ȃI�[�f�B�I�\�[�X��T��
            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                availableSource.clip = clip;
                availableSource.volume = volume;
                availableSource.Play();
            }
            else
            {
                // �V�����I�[�f�B�I�\�[�X���쐬���čĐ�
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume;
                newSource.Play();
                audioSources_SE.Add(newSource);
            }
        }

        /// <summary>
        /// �g�p�\�ȃI�[�f�B�I�\�[�X��T���Ă����\�b�h
        /// </summary>
        /// <returns></returns>
        private AudioSource GetAvailableAudioSource()
        {
            foreach (AudioSource source in audioSources_SE)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }
            return null;
        }

        /// <summary>
        /// �w�肵��BGM�̃^�C�v�̃f�[�^���擾���ĕԂ����\�b�h
        /// </summary>
        BGMData GetBGMData(BGMData.BGM bgm)
        {
            BGMData data = bgmDatas.Find(data => data.bgm == bgm);
            return data;
        }

        //Master��Volume���Z�b�g���郁�\�b�h
        public void SetMasterVolume(float volume)
        {
            masterMixerGroup.audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
        //Master��BGM���Z�b�g���郁�\�b�h
        public void SetBGMVolume(float volume)
        {
            bgmMixerGroup.audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        }
        //Master��SE���Z�b�g���郁�\�b�h
        public void SetSEVolume(float volume)
        {
            seMixerGroup.audioMixer.SetFloat("SE", Mathf.Log10(volume) * 20);
        }

        //Master�̃~�L�T�[�O���[�v�̒l��Ԃ����\�b�h
        public float GetMasterVolume()
        {
            float value;
            if (masterMixerGroup.audioMixer.GetFloat("Master", out value))
            {
                return Mathf.Pow(10, value / 20);
            }
            return 0.0001f;
        }

        //BGM�̃~�L�T�[�O���[�v�̒l��Ԃ����\�b�h
        public float GetBGMVolume()
        {
            float value;
            if (bgmMixerGroup.audioMixer.GetFloat("BGM", out value))
            {
                return Mathf.Pow(10, value / 20);
            }
            return 0.0001f;
        }

        //SE�̃~�L�T�[�O���[�v�̒l��Ԃ����\�b�h
        public float GetSEVolume()
        {
            float value;
            if (seMixerGroup.audioMixer.GetFloat("SE", out value))
            {
                return Mathf.Pow(10, value / 20);
            }
            return 0.0001f;
        }
    }

    /// <summary>
    /// BGM�̃f�[�^���܂Ƃ߂��N���X
    /// </summary>
    [System.Serializable]
    public class BGMData
    {
        //BGM�̃^�C�v�̗񋓌^
      public enum BGM
        {
            Title,
            Playng,
            LastBattle,
            GameClear,
            GameOver,
        }

        public BGM bgm;//BGM�̃^�C�v
        public AudioClip audioClip;//BGM�N���b�v
        public float volume;//�X�̃{�����[����ݒ��

    }
}
