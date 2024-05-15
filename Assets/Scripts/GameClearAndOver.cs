using DG.Tweening;
using TMPro;
using UnityEngine;


namespace KanjiYomi
{
    /// <summary>
    /// GameStateがゲームクリアとゲームオーバーになった時に実行するクラス
    /// </summary>
    public class GameClearAndOver : MonoBehaviour
    {
        //ゲームクリアとゲームオーバー用のオブジェクト
        public GameObject gameClearObject, gameOverObject;
        //ゲームクリアとゲームオーバー用のTMP
        public TextMeshProUGUI gameClearText, gameOverText;

        private void Awake()
        {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;//登録
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;//解除
        }

        /// <summary>
        /// ステート変更時に行われるメソッド
        /// </summary>
        void GameManagerOnGameStateChanged(GameState state)
        {
            //GameClear
            if (state == GameState.GameClear)
            {
                gameClearObject.SetActive(true);
                AnimateGameClear(gameClearText);
            }
            //GameOver
            else if (state == GameState.GameOver)
            {
                gameOverObject.SetActive(true);
                AnimateGameClear(gameOverText);

            }
            //Other
            else
            {
                gameOverObject.SetActive(false);
                gameClearObject.SetActive(false);
            }
        }

        //文字のアニメーション
        public void AnimateGameClear(TextMeshProUGUI text)
        {
            text.rectTransform.localScale = Vector3.zero;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(text.DOFade(1, 1f)) // フェードイン
                    .Join(text.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce)) // スケールアップ
                    .AppendInterval(1f) // 表示時間
                    .Append(text.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetEase(Ease.InOutElastic)) // 少し大きくする
                    .Append(text.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutElastic)); // 元に戻す
        }
    }
}
