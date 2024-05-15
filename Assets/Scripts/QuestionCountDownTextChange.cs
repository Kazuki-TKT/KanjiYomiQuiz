using UnityEngine;
using TMPro;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// ���̐������Ԃ����������ɁA������ύX���A�Ԃ��_�ł��s���N���X
    /// �A�j���[�V�����N���b�v�Ŏg�p
    /// </summary>
    public class QuestionCountDownTextChange : MonoBehaviour
    {
        //�J�E���g�_�E���p�̃e�L�X�g
        TextMeshProUGUI countDownText;

        //�_�ŗp�̃L�����o�X�O���[�v
        [SerializeField]
        CanvasGroup flashRedCanvas;

        //�_�Ŏ��ɖ炷�A���[�g��
        public AudioClip alertClip;

        void Start()
        {
            countDownText = GetComponentInChildren<TextMeshProUGUI>();
        }

        //�f�t�H���g�̒l���e�L�X�g�ɐݒ�
        public void DefaultText()
        {
            countDownText.text = 5.ToString();
        }
        //�e�L�X�g�̒l��ύX
        public void ChangeText(int num)
        {
            countDownText.text = num.ToString();
        }
        //��ʂ�Ԃ��_�ł�����
        public void Flash()
        {
            flashRedCanvas.DOFade(1, 0.1f).OnComplete(() => flashRedCanvas.DOFade(0, 0.4f));
            AuidoManager.Instance.PlaySound_SE(alertClip);
        }
    }
}
