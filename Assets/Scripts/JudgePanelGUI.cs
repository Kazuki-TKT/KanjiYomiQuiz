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
            // QuestionGameController�̃C�x���g���w��
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;

        }
        private void OnDestroy()
        {
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
        }

        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            //Debug.Log(newJudge);
            // newJudge�̒l�ɉ����ď����𕪊�
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
        /// �p�l���Ƀf�[�^���Z�b�g����
        /// </summary>
        void SetQuestionDataText(QuestionData questionData)
        {
            //inputFieldCanvasGroup.alpha = 0;
            string questionString = "";
            string correctString = "";

            //�`�e������Ȃ��ŕ���
            if (questionData.adjective)
            {
                //GUI�p��string�ő������s��
                questionString = $"<color=#{TextColor.ORANGE:X}>{questionData.Question.Replace(questionData.AdjectiveString, "")}</color>" +
                $"<color=#{TextColor.BLACK:X}><size=40%>{questionData.AdjectiveString}</size></color>";

                correctString = $"<color=#{TextColor.ORANGE:X}>{questionData.Correct}</color>" +
                $"<color=#{TextColor.BLACK:X}>{questionData.AdjectiveString}</color>";
            }
            else
            {
                //GUI�p��string�ő������s��
                questionString = $"<color=#{TextColor.ORANGE:X}>{questionData.Question}</color>";
                correctString = $"<color=#{TextColor.ORANGE:X}>{questionData.Correct}</color>";
            }

            //�����ƕs������GUI�ɖ����Z�b�g
            foreach(TextMeshProUGUI textMeshProUGUI in questionText)
            {
                textMeshProUGUI.text = questionString;
            }
            //�����ƕs������GUI�ɓ������Z�b�g
            foreach (TextMeshProUGUI textMeshProUGUI in correctText)
            {
                textMeshProUGUI.text = correctString;
            }
            //�����ƕs������GUI�ɐ��������Z�b�g
            foreach (TextMeshProUGUI textMeshProUGUI in descriptionText)
            {
                textMeshProUGUI.text = questionData.Description;
            }
        }


    }
}
