using UnityEngine;
using TMPro;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// プレイヤーの残りライフを表示するクラス
    /// </summary>
    public class PLayerLifeGUI : MonoBehaviour
    {
        //プレイヤーの残りライフを表示するTMP
        [SerializeField]
        TextMeshProUGUI playerLifeText;

        //アニメーション時間
        public float effectDuration = 0.5f;

        //指定スケール
        public float scaleMultiplier = 1.2f;

        //ライフ減少時のSE
        public AudioClip decreaseLifeClip;

        private void Awake()
        {
            PlayerController.OnPlayerLifeChanged += ChangePlayetLifeText;//イベントを購買
        }

        private void OnDestroy()
        {
            //解除
            PlayerController.OnPlayerLifeChanged -= ChangePlayetLifeText;
        }

        //プレイヤーのライフ変化時にテキストを変更するメソッド
        void ChangePlayetLifeText(int playerLife)
        {
            if (PlayerController.Instance.PlayerLife == PlayerController.Instance.MaxPlayerLife)//プレイヤーのライフが最大値の時
            {
                playerLifeText.text = playerLife.ToString();
            }
            else//プレイヤーのライフ減少時
            {
                AuidoManager.Instance.PlaySound_SE(decreaseLifeClip);//減少用のSE
                playerLifeText.rectTransform.DOScale(Vector3.one * scaleMultiplier, effectDuration / 2f)
            .SetEase(Ease.OutBack) // テキストのスケールが大きくなる
            .OnComplete(() =>
            {
                // スケールが大きくなった後、テキストを変更して元のサイズに戻す
                playerLifeText.text = playerLife.ToString();
                playerLifeText.rectTransform.DOScale(Vector3.one, effectDuration / 2f)
                    .SetEase(Ease.InBack); // 元のスケールに戻ります
            });
            }
        }
    }
}
