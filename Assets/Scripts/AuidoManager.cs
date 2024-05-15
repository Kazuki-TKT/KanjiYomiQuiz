using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KanjiYomi
{
    /// <summary>
    /// サウンド関連を管理するシングルトンパターンのクラス
    /// </summary>
    public class AuidoManager : MonoBehaviour
    {
        public static AuidoManager Instance;

        //オーディオミキサーグループ（Master,BGM,SE）
        public AudioMixerGroup masterMixerGroup, bgmMixerGroup, seMixerGroup;

        // 同時再生可能なオーディオソースの最大数
        public int maxAudioSources = 8; 

        //BGMとSEのオーディオソース
        [SerializeField]
        AudioSource audioSources_BGM;
        private List<AudioSource> audioSources_SE;

        //BGMのデータ
        [SerializeField]
        List<BGMData> bgmDatas;

        //全ボタンに反映させるSEのクリップ(クリック、エンター)
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

        /// <summary>
        /// BGMを鳴らすメソッド
        /// </summary>
        /// <param name="bgm">BGMのタイプ</param>
        public void PlaySound_BGM(BGMData.BGM bgm)
        {
            var data = GetBGMData(bgm);
            audioSources_BGM.clip = data.audioClip;
            audioSources_BGM.volume = data.volume;
            audioSources_BGM.Play();
        }

        /// <summary>
        /// BGMをFadeチェンジして鳴らすメソッド
        /// </summary>
        /// <param name="bgm">BGMのタイプ</param>
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
        /// SEを鳴らすメソッド
        /// </summary>
        public void PlaySound_SE(AudioClip clip, float volume = 1f)
        {
            // 使用可能なオーディオソースを探す
            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                availableSource.clip = clip;
                availableSource.volume = volume;
                availableSource.Play();
            }
            else
            {
                // 新しいオーディオソースを作成して再生
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume;
                newSource.Play();
                audioSources_SE.Add(newSource);
            }
        }

        /// <summary>
        /// 使用可能なオーディオソースを探してすメソッド
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
        /// 指定したBGMのタイプのデータを取得して返すメソッド
        /// </summary>
        BGMData GetBGMData(BGMData.BGM bgm)
        {
            BGMData data = bgmDatas.Find(data => data.bgm == bgm);
            return data;
        }

        //MasterのVolumeをセットするメソッド
        public void SetMasterVolume(float volume)
        {
            masterMixerGroup.audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
        //MasterのBGMをセットするメソッド
        public void SetBGMVolume(float volume)
        {
            bgmMixerGroup.audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        }
        //MasterのSEをセットするメソッド
        public void SetSEVolume(float volume)
        {
            seMixerGroup.audioMixer.SetFloat("SE", Mathf.Log10(volume) * 20);
        }

        //Masterのミキサーグループの値を返すメソッド
        public float GetMasterVolume()
        {
            float value;
            if (masterMixerGroup.audioMixer.GetFloat("Master", out value))
            {
                return Mathf.Pow(10, value / 20);
            }
            return 0.0001f;
        }

        //BGMのミキサーグループの値を返すメソッド
        public float GetBGMVolume()
        {
            float value;
            if (bgmMixerGroup.audioMixer.GetFloat("BGM", out value))
            {
                return Mathf.Pow(10, value / 20);
            }
            return 0.0001f;
        }

        //SEのミキサーグループの値を返すメソッド
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
    /// BGMのデータをまとめたクラス
    /// </summary>
    [System.Serializable]
    public class BGMData
    {
        //BGMのタイプの列挙型
      public enum BGM
        {
            Title,
            Playng,
            LastBattle,
            GameClear,
            GameOver,
        }

        public BGM bgm;//BGMのタイプ
        public AudioClip audioClip;//BGMクリップ
        public float volume;//個々のボリュームを設定可

    }
}
