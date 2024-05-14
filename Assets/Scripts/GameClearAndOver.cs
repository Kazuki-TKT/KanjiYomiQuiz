using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace KanjiYomi
{
    public class GameClearAndOver : MonoBehaviour
    {
        public GameObject gameClearObject, gameOverObject;
        public TextMeshProUGUI gameClearText, gameOverText;

        private void Awake()
        {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }
        void GameManagerOnGameStateChanged(GameState state)
        {
            if (state == GameState.GameOver) {
                gameOverObject.SetActive(true);
                AnimateGameClear(gameOverText);

            } else if (state == GameState.GameClear)
            {
                gameClearObject.SetActive(true);
                AnimateGameClear(gameClearText);
            }
            else
            {
                gameOverObject.SetActive(false);
                gameClearObject.SetActive(false);

            }
        }
        public void AnimateGameClear(TextMeshProUGUI text)
        {
            text.rectTransform.localScale = Vector3.zero;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(text.DOFade(1, 1f)) // �t�F�[�h�C��
                    .Join(text.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce)) // �X�P�[���A�b�v
                    .AppendInterval(1f) // �\������
                    .Append(text.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetEase(Ease.InOutElastic)) // �����傫������
                    .Append(text.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutElastic)); // ���ɖ߂�
        }
    }
}
