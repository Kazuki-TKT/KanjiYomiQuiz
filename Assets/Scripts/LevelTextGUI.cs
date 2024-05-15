using UnityEngine;
using DG.Tweening;
using TMPro;


namespace KanjiYomi
{
    /// <summary>
    /// 問題の難易度が変わった際にテキストを表示するクラス
    /// </summary>
    public class LevelTextGUI : MonoBehaviour
    {
        //各レベルごとのTMP
        public TextMeshProUGUI level1Text, level2Text, level3Text, level4Text;
        
        //時間
        public float duration;
        
        //効果音
        public AudioClip onTextClip;

        /// <summary>
        /// レベルごとのテキストを表示するメソッド
        /// </summary>
        /// <param name="count">正答数</param>
        public void OnDisplayLevelText(int count)
        {
            switch (count)
            {
                case 0://レベル1
                    Initialize(level1Text);
                    TextAnimation(level1Text);
                    break;
                case 3://レベル2
                    Initialize(level2Text);
                    TextAnimation(level2Text);
                    break;
                case 6://レベル3
                    Initialize(level3Text);
                    TextAnimation(level3Text);
                    break;
                case 9://レベル4(最終問題)
                    Initialize(level4Text);
                    TextAnimation(level4Text);
                    break;
            }
            AuidoManager.Instance.PlaySound_SE(onTextClip);
        }

        //レベルテキストのアニメーション
        void TextAnimation(TextMeshProUGUI tmpro)
        {
            Debug.Log("Level");
            // 文字間隔を開ける
            DOTween.To(() => tmpro.characterSpacing, value => tmpro.characterSpacing = value, 10, duration)
                .SetEase(Ease.OutQuart);

            // フェード
            DOTween.Sequence()
                .Append(tmpro.DOFade(1, duration / 4))
                .AppendInterval(duration / 2)
                .Append(tmpro.DOFade(0, duration / 4));
        }

        //テキストの初期化
        private void Initialize(TextMeshProUGUI tmpro)
        {
            tmpro.DOFade(0, 0);//透明化
            tmpro.characterSpacing = -50;//スペースを縮める
        }
    }
}
