using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace KanjiYomi
{
    public class JudgePanelGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;
        [SerializeField]
        GameObject correctPanelObjects,wrongPanelObjects, inputFieldObject;
        [SerializeField]
        TextMeshProUGUI[] questionText, correctText, descriptionText;

        private void Awake()
        {
            // QuestionGameControllerのイベントを購読
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;

        }
        private void OnDestroy()
        {
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
        }

        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            //Debug.Log(newJudge);
            // newJudgeの値に応じて処理を分岐
            switch (newJudge)
            {
                case QuestionGameController.Judge.Correct:
                    SetQuestionDataText(QuestionManager.Instance.CurrentData);
                    correctPanelObjects.SetActive(true);
                    break;
                case QuestionGameController.Judge.Wrong:
                    SetQuestionDataText(QuestionManager.Instance.CurrentData);
                    wrongPanelObjects.SetActive(true);
                    break;
                case QuestionGameController.Judge.Initial:
                    correctPanelObjects.SetActive(false);
                    wrongPanelObjects.SetActive(false);
                    break;
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
                $"<color=#{TextColor.BLACK:X}><size=40%>{questionData.AdjectiveString}</size></color>";

                correctString = $"<color=#{TextColor.ORANGE:X}>{questionData.Correct}</color>" +
                $"<color=#{TextColor.BLACK:X}>{questionData.AdjectiveString}</color>";
            }
            else
            {
                //GUI用にstringで装飾を行う
                questionString = $"<color=#{TextColor.ORANGE:X}>{questionData.Question}</color>";
                correctString = $"<color=#{TextColor.ORANGE:X}>{questionData.Correct}</color>";
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
