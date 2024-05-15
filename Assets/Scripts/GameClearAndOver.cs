using DG.Tweening;
using TMPro;
using UnityEngine;


namespace KanjiYomi
{
    /// <summary>
    /// GameState���Q�[���N���A�ƃQ�[���I�[�o�[�ɂȂ������Ɏ��s����N���X
    /// </summary>
    public class GameClearAndOver : MonoBehaviour
    {
        //�Q�[���N���A�ƃQ�[���I�[�o�[�p�̃I�u�W�F�N�g
        public GameObject gameClearObject, gameOverObject;
        //�Q�[���N���A�ƃQ�[���I�[�o�[�p��TMP
        public TextMeshProUGUI gameClearText, gameOverText;

        private void Awake()
        {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;//�o�^
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;//����
        }

        /// <summary>
        /// �X�e�[�g�ύX���ɍs���郁�\�b�h
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

        //�����̃A�j���[�V����
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
