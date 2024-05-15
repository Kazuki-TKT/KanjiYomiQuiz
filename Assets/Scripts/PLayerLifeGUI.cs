using UnityEngine;
using TMPro;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// �v���C���[�̎c�胉�C�t��\������N���X
    /// </summary>
    public class PLayerLifeGUI : MonoBehaviour
    {
        //�v���C���[�̎c�胉�C�t��\������TMP
        [SerializeField]
        TextMeshProUGUI playerLifeText;

        //�A�j���[�V��������
        public float effectDuration = 0.5f;

        //�w��X�P�[��
        public float scaleMultiplier = 1.2f;

        //���C�t��������SE
        public AudioClip decreaseLifeClip;

        private void Awake()
        {
            PlayerController.OnPlayerLifeChanged += ChangePlayetLifeText;//�C�x���g���w��
        }

        private void OnDestroy()
        {
            //����
            PlayerController.OnPlayerLifeChanged -= ChangePlayetLifeText;
        }

        //�v���C���[�̃��C�t�ω����Ƀe�L�X�g��ύX���郁�\�b�h
        void ChangePlayetLifeText(int playerLife)
        {
            if (PlayerController.Instance.PlayerLife == PlayerController.Instance.MaxPlayerLife)//�v���C���[�̃��C�t���ő�l�̎�
            {
                playerLifeText.text = playerLife.ToString();
            }
            else//�v���C���[�̃��C�t������
            {
                AuidoManager.Instance.PlaySound_SE(decreaseLifeClip);//�����p��SE
                playerLifeText.rectTransform.DOScale(Vector3.one * scaleMultiplier, effectDuration / 2f)
            .SetEase(Ease.OutBack) // �e�L�X�g�̃X�P�[�����傫���Ȃ�
            .OnComplete(() =>
            {
                // �X�P�[�����傫���Ȃ�����A�e�L�X�g��ύX���Č��̃T�C�Y�ɖ߂�
                playerLifeText.text = playerLife.ToString();
                playerLifeText.rectTransform.DOScale(Vector3.one, effectDuration / 2f)
                    .SetEase(Ease.InBack); // ���̃X�P�[���ɖ߂�܂�
            });
            }
        }
    }
}
