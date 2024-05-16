using System.Collections;
using TMPro;
using UnityEngine;

namespace KanjiYomi
{
    /// <summary>
    /// 問題の正解、不正解時のオブジェクトを操作するクラス
    /// </summary>
    public class JudgePanelGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        //プレイヤー不正解時のアニメーション
        [SerializeField]
        PlayerMissAnswerAnimation playerMissAnswerAnimation;

        //正解、不正解時のオブジェクト。インプットフィールドのオブジェクト
        [SerializeField]
        GameObject correctPanelObjects,missPanelObjects, inputFieldObject;

        //問題、答え、説明を表示するTMP
        [SerializeField]
        TextMeshProUGUI[] questionText, correctText, descriptionText;

        //正解、不正解時のSE
        public AudioClip correctClip, missClip;

        private void Awake()
        {
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;// QuestionGameControllerのイベントを購読
            GameManager.OnGameStateChanged += HandleGameStateChanged;// QuestionGameControllerのイベントを購読
            playerMissAnswerAnimation.OnMissAnimationComplete += HandleMissAnimationComplete;// PlayerMissAnswerAnimationのイベントを購読

        }

        private void OnDestroy()
        {
            //解除
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
            GameManager.OnGameStateChanged -= HandleGameStateChanged;
            playerMissAnswerAnimation.OnMissAnimationComplete -= HandleMissAnimationComplete;
        }


        private void HandleMissAnimationComplete()
        {
            missPanelObjects.SetActive(true);
        }
        //ゲームステートによって分岐するメソッド
        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.GameOver||
                state == GameState.GameClear)
            {
                correctPanelObjects.SetActive(false);
                missPanelObjects.SetActive(false);
            }
        }

        //正解、不正解、その他によって分岐するメソッド
        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            // newJudgeの値に応じて処理を分岐
            switch (newJudge)
            {
                case QuestionGameController.Judge.Correct:
                    SetQuestionDataText(QuestionManager.Instance.CurrentData);
                    StartCoroutine(WaitDisplayPanel(correctPanelObjects,1, newJudge));
                    break;
                case QuestionGameController.Judge.Miss:
                    SetQuestionDataText(QuestionManager.Instance.CurrentData);
                    StartCoroutine(WaitDisplayPanel(missPanelObjects, 2, newJudge));
                    break;
                case QuestionGameController.Judge.Initial:
                    correctPanelObjects.SetActive(false);
                    missPanelObjects.SetActive(false);
                    break;
            }
        }

        //指定秒待ってからオブジェクトと音を鳴らすコルーチン
        IEnumerator WaitDisplayPanel(GameObject gameObject,float duration, QuestionGameController.Judge newJudge)
        {
            Debug.Log($"WaitDisplayPanel started for {newJudge}, waiting for {duration} seconds.");
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(true);
            // newJudgeの値に応じて処理を分岐
            if (newJudge == QuestionGameController.Judge.Miss)
            {
               AuidoManager.Instance.PlaySound_SE(missClip);
            }
            else
            {
                AuidoManager.Instance.PlaySound_SE(correctClip);
            }
        }

        /// <summary>
        /// パネルにデータをセットする
        /// </summary>
        void SetQuestionDataText(QuestionData questionData)
        {
            //inputFieldCanvasGroup.alpha = 0;
            string questionString = "";
            string correctString = "";

            //形容詞ありなしで分岐
            if (questionData.adjective)
            {
                //GUI用にstringで装飾を行う
                questionString = $"<color=#{TextColor.ORANGE:X}>{questionData.Question.Replace(questionData.AdjectiveString, "")}</color>" +
                $"<color=#{TextColor.WHITE:X}><size=40%>{questionData.AdjectiveString}</size></color>";

                correctString = $"<color=#{TextColor.ORANGE:X}>{questionData.Correct[0]}</color>" +
                $"<color=#{TextColor.WHITE:X}>{questionData.AdjectiveString}</color>";
            }
            else
            {
                //GUI用にstringで装飾を行う
                questionString = $"<color=#{TextColor.ORANGE:X}>{questionData.Question}</color>";
                correctString = $"<color=#{TextColor.ORANGE:X}>{questionData.Correct[0]}</color>";
            }

            //正解と不正解のGUIに問題をセット
            foreach(TextMeshProUGUI textMeshProUGUI in questionText)
            {
                textMeshProUGUI.text = questionString;
            }
            //正解と不正解のGUIに答えをセット
            foreach (TextMeshProUGUI textMeshProUGUI in correctText)
            {
                textMeshProUGUI.text = correctString;
            }
            //正解と不正解のGUIに説明文をセット
            foreach (TextMeshProUGUI textMeshProUGUI in descriptionText)
            {
                textMeshProUGUI.text = questionData.Description;
            }
        }


    }
}
