using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KanjiYomi
{
    public class AuidoManager : MonoBehaviour
    {
        public static AuidoManager Instance;

        [SerializeField]
        AudioMixerGroup masterMixerGroup, bgmMixerGroup, seMixerGroup;

        public int maxAudioSources = 8; // �����Đ��\�ȃI�[�f�B�I�\�[�X�̍ő吔

        [SerializeField]
        AudioSource audioSources_BGM;
        private List<AudioSource> audioSources_SE;

        [SerializeField]
        List<BGMData> bgmDatas;

        public AudioClip buttonSE;
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

        public void PlaySound_BGM(BGMData.BGM bgm)
        {
            var data = GetBGMData(bgm);
            audioSources_BGM.clip = data.audioClip;
            audioSources_BGM.volume = data.volume;
            audioSources_BGM.Play();
        }

        public void FadeChangeBGM(BGMData.BGM bgm)
        {
            var data = GetBGMData(bgm);
            audioSources_BGM.DOFade(0, 0.5f).OnComplete(() => {
                audioSources_BGM.clip = data.audioClip;
                audioSources_BGM.volume = data.volume;
                audioSources_BGM.DOFade(data.volume, 0.2f);
            });
        }

        public void PlaySound_SE(AudioClip clip, float volume = 1f)
        {
            // �g�p�\�ȃI�[�f�B�I�\�[�X��T���܂�
            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                // �I�[�f�B�I���Đ����܂�
                availableSource.clip = clip;
                availableSource.volume = volume;
                availableSource.Play();
            }
            else
            {
                // �V�����I�[�f�B�I�\�[�X���쐬���čĐ����܂�
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume;
                newSource.Play();
                audioSources_SE.Add(newSource);
            }
        }

        public void StopAllSounds()
        {
            // �S�ẴI�[�f�B�I���~���܂�
            foreach (AudioSource source in audioSources_SE)
            {
                source.Stop();
            }
        }

        private AudioSource GetAvailableAudioSource()
        {
            // �g�p�\�ȃI�[�f�B�I�\�[�X��T���ĕԂ��܂�
            foreach (AudioSource source in audioSources_SE)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }
            return null;
        }

        BGMData GetBGMData(BGMData.BGM bgm)
        {
            BGMData data = bgmDatas.Find(data => data.bgm == bgm);
            return data;
        }
    }

    [System.Serializable]
    public class BGMData
    {
      public enum BGM
        {
            Title,
            Playng,
            LastBattle,
            GameClear,
            GameOver,
        }

        public BGM bgm;
        public AudioClip audioClip;
        public float volume;

    }
}
