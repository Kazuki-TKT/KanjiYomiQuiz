using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;


namespace KanjiYomi
{
    public class LevelTextGUI : MonoBehaviour
    {
        public TextMeshProUGUI level1Text, level2Text, level3Text, level4Text;
        public float duration;
        public AudioClip onTextClip;
        public void OnDisplayLevelText(int count)
        {
            switch (count)
            {
                case 0:
                    Initialize(level1Text);
                    TextAnimation(level1Text);
                    break;
                case 3:
                    Initialize(level2Text);
                    TextAnimation(level2Text);
                    break;
                case 6:
                    Initialize(level3Text);
                    TextAnimation(level3Text);
                    break;
                case 9:
                    Initialize(level4Text);
                    TextAnimation(level4Text);
                    break;
            }
            AuidoManager.Instance.PlaySound_SE(onTextClip);
        }

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

        private void Initialize(TextMeshProUGUI tmpro)
        {
            tmpro.DOFade(0, 0);
            tmpro.characterSpacing = -50;
        }
    }
}
