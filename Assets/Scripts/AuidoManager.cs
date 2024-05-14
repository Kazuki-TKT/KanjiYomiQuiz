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

        public int maxAudioSources = 8; // 同時再生可能なオーディオソースの最大数

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

            //BGM用のオーディオミキサーグループを指定
            audioSources_BGM.outputAudioMixerGroup = bgmMixerGroup;

            // SEListの初期化
            audioSources_SE = new List<AudioSource>();
            // 最初に最大数分のSEのオーディオソースを生成
            for (int i = 0; i < maxAudioSources; i++)
            {
                audioSources_SE.Add(gameObject.AddComponent<AudioSource>());
            }

            foreach (AudioSource source in audioSources_SE)
            {
                source.outputAudioMixerGroup = seMixerGroup; // SE用のオーディオミキサーグループを指定
                source.playOnAwake = false;//PlayOnAwakeをオフ
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
            // 使用可能なオーディオソースを探します
            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                // オーディオを再生します
                availableSource.clip = clip;
                availableSource.volume = volume;
                availableSource.Play();
            }
            else
            {
                // 新しいオーディオソースを作成して再生します
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume;
                newSource.Play();
                audioSources_SE.Add(newSource);
            }
        }

        public void StopAllSounds()
        {
            // 全てのオーディオを停止します
            foreach (AudioSource source in audioSources_SE)
            {
                source.Stop();
            }
        }

        private AudioSource GetAvailableAudioSource()
        {
            // 使用可能なオーディオソースを探して返します
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
