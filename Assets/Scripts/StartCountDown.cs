using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;

namespace KanjiYomi
{
    public class StartCountDown : MonoBehaviour
    {
        public TextMeshProUGUI countdownText;

        private Color[] countdownColors = { Color.red, Color.green, Color.blue, Color.yellow };

        public AudioClip countClip, goClip;

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
