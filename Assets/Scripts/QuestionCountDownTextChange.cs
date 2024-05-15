using UnityEngine;
using TMPro;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// 問題の制限時間が迫った時に、文字を変更し、赤い点滅を行うクラス
    /// アニメーションクリップで使用
    /// </summary>
    public class QuestionCountDownTextChange : MonoBehaviour
    {
        //カウントダウン用のテキスト
        TextMeshProUGUI countDownText;

        //点滅用のキャンバスグループ
        [SerializeField]
        CanvasGroup flashRedCanvas;

        //点滅時に鳴らすアラート音
        public AudioClip alertClip;

        void Start()
        {
            countDownText = GetComponentInChildren<TextMeshProUGUI>();
        }

        //デフォルトの値をテキストに設定
        public void DefaultText()
        {
            countDownText.text = 5.ToString();
        }
        //テキストの値を変更
        public void ChangeText(int num)
        {
            countDownText.text = num.ToString();
        }
        //画面を赤く点滅させる
        public void Flash()
        {
            flashRedCanvas.DOFade(1, 0.1f).OnComplete(() => flashRedCanvas.DOFade(0, 0.4f));
            AuidoManager.Instance.PlaySound_SE(alertClip);
        }
    }
}
