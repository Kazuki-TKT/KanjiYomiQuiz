using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace KanjiYomi
{
    /// <summary>
    /// 3,2,1,GOのテキストアニメーションを行うクラス
    /// </summary>
    public class StartCountDown : MonoBehaviour
    {
        //アニメーション用のTMP
        public TextMeshProUGUI countdownText;

        //色
        private Color[] countdownColors = { Color.red, Color.green, Color.blue, Color.yellow };

        //カウント音、GO音
        public AudioClip countClip, goClip;

        //カウントが終了したかどうかの真偽地
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
                countdownText.color = countdownColors[3 - i]; // カウントダウンに応じて色を変更
                countdownText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce); // バウンドでスケールアップ
                countdownText.DOFade(1, 0.5f);
                yield return new WaitForSeconds(0.5f); // 待機
                countdownText.rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack); // バウンドでスケールダウン
                countdownText.DOFade(0, 0.5f);
                yield return new WaitForSeconds(0.5f);  // 待機
            }
            AuidoManager.Instance.PlaySound_SE(goClip);
            countdownText.text = "GO!";
            countdownText.color = countdownColors[3]; // "GO!"の色を設定
            countdownText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce); // 最後にスケールアップ
            countdownText.DOFade(1, 0.5f); // フェードイン
            yield return new WaitForSeconds(1f);
            countdownText.text = "";
            isCountdownFinished = true;
        }
    }
}
