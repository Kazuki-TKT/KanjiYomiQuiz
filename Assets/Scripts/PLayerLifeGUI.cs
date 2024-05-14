using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace KanjiYomi
{
    public class PLayerLifeGUI : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI playerLifeText;

        public float effectDuration = 0.5f;
        public float scaleMultiplier = 1.2f;

        public AudioClip decreaseLifeClip;
        private void Awake()
        {
            PlayerController.OnPlayerLifeChanged += ChangePlayetLifeText;
        }

        private void OnDestroy()
        {
            PlayerController.OnPlayerLifeChanged -= ChangePlayetLifeText;
        }

        void ChangePlayetLifeText(int playerLife)
        {
            if (PlayerController.Instance.PlayerLife == PlayerController.Instance.MaxPlayerLife)
            {
            
            playerLifeText.text = playerLife.ToString();

            }
            else
            {
                AuidoManager.Instance.PlaySound_SE(decreaseLifeClip);
                playerLifeText.rectTransform.DOScale(Vector3.one * scaleMultiplier, effectDuration / 2f)
            .SetEase(Ease.OutBack) // テキストのスケールが大きくなります
            .OnComplete(() =>
            {
            // スケールが大きくなった後、テキストを変更して元のサイズに戻します
            playerLifeText.text = playerLife.ToString();
                playerLifeText.rectTransform.DOScale(Vector3.one, effectDuration / 2f)
                    .SetEase(Ease.InBack); // 元のスケールに戻ります
            });
            }
        }
    }
}
