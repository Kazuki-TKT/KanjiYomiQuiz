using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KanjiYomi
{
    /// <summary>
    /// �Q�[���̉��ʂ�ω�������N���X�B�X���C�_�[�ɕt�^
    /// </summary>
    public class VolumeController : MonoBehaviour
    {
        //�e�X���C�_�[(Master,BGM,SE)
        [SerializeField]
        private Slider masterVolumeSlider, bgmVolumeSlider, seVolumeSlider;

        private void Start()
        {
            //Slider�Ƀ��\�b�h��ݒ�
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
            seVolumeSlider.onValueChanged.AddListener(SetSEVolume);

            // �����l�̐ݒ�i�K�v�ɉ����Ē����j
            masterVolumeSlider.value = AuidoManager.Instance.GetMasterVolume();
            bgmVolumeSlider.value = AuidoManager.Instance.GetBGMVolume();
            seVolumeSlider.value = AuidoManager.Instance.GetSEVolume();
        }

        //MasterMixer�̒l��ω�������
        private void SetMasterVolume(float volume)
        {
            AuidoManager.Instance.SetMasterVolume(volume);
        }

        //BGMMixer�̒l��ω�������
        private void SetBGMVolume(float volume)
        {
            AuidoManager.Instance.SetBGMVolume(volume);
        }

        //SEMixer�̒l��ω�������
        private void SetSEVolume(float volume)
        {
            AuidoManager.Instance.SetSEVolume(volume);
        }
    }
}
