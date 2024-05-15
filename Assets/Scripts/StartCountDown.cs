using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace KanjiYomi
{
    /// <summary>
    /// 3,2,1,GO�̃e�L�X�g�A�j���[�V�������s���N���X
    /// </summary>
    public class StartCountDown : MonoBehaviour
    {
        //�A�j���[�V�����p��TMP
        public TextMeshProUGUI countdownText;

        //�F
        private Color[] countdownColors = { Color.red, Color.green, Color.blue, Color.yellow };

        //�J�E���g���AGO��
        public AudioClip countClip, goClip;

        //�J�E���g���I���������ǂ����̐^�U�n
        private bool isCountdownFinished = false;
        public bool IsCountdownFinished { get => isCountdownFinished; }

        public IEnumerator StartTextCountdown()
        {
            isCountdownFinished = false;
            countdownText.rectTransform.localScale = Vector3.zero;
            yield return new WaitForSeconds(0.5f);
            for (int i = 3; i > 0; i--)
            {
                AuidoManager.Instance.PlaySound_SE(countClip);
                countdownText.text = i.ToString();
                countdownText.color = countdownColors[3 - i]; // �J�E���g�_�E���ɉ����ĐF��ύX
                countdownText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce); // �o�E���h�ŃX�P�[���A�b�v
                countdownText.DOFade(1, 0.5f);
                yield return new WaitForSeconds(0.5f); // �ҋ@
                countdownText.rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack); // �o�E���h�ŃX�P�[���_�E��
                countdownText.DOFade(0, 0.5f);
                yield return new WaitForSeconds(0.5f);  // �ҋ@
            }
            AuidoManager.Instance.PlaySound_SE(goClip);
            countdownText.text = "GO!";
            countdownText.color = countdownColors[3]; // "GO!"�̐F��ݒ�
            countdownText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce); // �Ō�ɃX�P�[���A�b�v
            countdownText.DOFade(1, 0.5f); // �t�F�[�h�C��
            yield return new WaitForSeconds(1f);
            countdownText.text = "";
            isCountdownFinished = true;
        }
    }
}
