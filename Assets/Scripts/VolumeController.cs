using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KanjiYomi
{
    /// <summary>
    /// ゲームの音量を変化させるクラス。スライダーに付与
    /// </summary>
    public class VolumeController : MonoBehaviour
    {
        //各スライダー(Master,BGM,SE)
        [SerializeField]
        private Slider masterVolumeSlider, bgmVolumeSlider, seVolumeSlider;

        private void Start()
        {
            //Sliderにメソッドを設定
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
            seVolumeSlider.onValueChanged.AddListener(SetSEVolume);

            // 初期値の設定（必要に応じて調整）
            masterVolumeSlider.value = AuidoManager.Instance.GetMasterVolume();
            bgmVolumeSlider.value = AuidoManager.Instance.GetBGMVolume();
            seVolumeSlider.value = AuidoManager.Instance.GetSEVolume();
        }

        //MasterMixerの値を変化させる
        private void SetMasterVolume(float volume)
        {
            AuidoManager.Instance.SetMasterVolume(volume);
        }

        //BGMMixerの値を変化させる
        private void SetBGMVolume(float volume)
        {
            AuidoManager.Instance.SetBGMVolume(volume);
        }

        //SEMixerの値を変化させる
        private void SetSEVolume(float volume)
        {
            AuidoManager.Instance.SetSEVolume(volume);
        }
    }
}
