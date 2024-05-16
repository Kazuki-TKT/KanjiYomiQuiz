using UnityEngine;
using DG.Tweening;
using TMPro;

namespace KanjiYomi
{
    /// <summary>
    /// 問題文を表示させるクラス
    /// </summary>
    public class QuestionTextGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        //問題のテキストを子に持つ親ゲームオブジェクト
        [SerializeField]
        GameObject questionTextObject, questionTextAdjectiveObject;

        //上記2つをまとめたキャンバスグループ
        CanvasGroup canvasGroup;

        //問題のテキストを表示させるTMP
        [SerializeField]
        TextMeshProUGUI kanjiText, kanjiTextAdjective, adjectiveText,furiganaText,furiganaAdjectiveText;

        //スケールの値を設定
        [SerializeField]
        Vector3 defaltScale,targetScale;

        private Tweener scaleUpTween;

        //パーティクル(Hit)
        [SerializeField]
        ParticleSystem hitParticle;

        private void Awake()
        {
            // QuestionGameControllerのイベントを購読
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;

        }
        void Start()
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            questionTextObject.SetActive(false);
            questionTextAdjectiveObject.SetActive(false);
        }

        private void OnDestroy()
        {
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
        }

        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            ResetQuesitionText();
            // newJudgeの値に応じて処理を分岐
            switch (newJudge)
            {
                case QuestionGameController.Judge.Correct:
                    hitParticle.Play();
                    CancelFadeIn();
                    break;
                case QuestionGameController.Judge.Miss:
                    CancelFadeIn();
                    break;
            }
        }
       

        /// <summary>
        /// 問題文をセットするメソッド
        /// </summary>
        public void SetQuesitionText(float time, QuestionData questionData)
        {
            SetQuestionDataText(questionData);
            canvasGroup.DOFade(1, 0.2f);
            scaleUpTween = gameObject.transform.DOScale(targetScale, time);
        }

        //キャンセル
        void CancelFadeIn()
        {
            if (scaleUpTween != null)
            {
                scaleUpTween.Kill(); // Tweenをキャンセル
            }
        }

        /// <summary>
        /// 問題文をリセットするメソッド
        /// </summary>
        public void ResetQuesitionText()
        {
            canvasGroup.alpha = 0;
            gameObject.transform.localScale = defaltScale;
            questionTextObject.SetActive(false);
            questionTextAdjectiveObject.SetActive(false);
            kanjiText.text = kanjiTextAdjective.text = adjectiveText.text = furiganaAdjectiveText.text= furiganaText.text="";
        }

        void SetQuestionDataText(QuestionData questionData)
        {
            //形容詞ありなしで分岐
            if (questionData.adjective)
            {
                //問題のGUIにデータをセット
                questionTextAdjectiveObject.SetActive(true);
                kanjiTextAdjective.text = questionData.Question;
                adjectiveText.text = questionData.AdjectiveString;
                for (int i = 0; i < questionData.Correct[0].Length; i++)
                {
                    furiganaAdjectiveText.text += "●";
                }
            }
            else
            {
                //問題のGUIにデータをセット
                questionTextObject.SetActive(true);
                kanjiText.text = questionData.Question;
                for (int i = 0; i < questionData.Correct[0].Length; i++)
                {
                    furiganaText.text += "●";
                }
            }
        }
    }
}
