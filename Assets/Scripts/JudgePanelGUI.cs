using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KanjiYomi
{
    public class JudgePanelGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;
        [SerializeField]
        PlayerMissAnswerAnimation playerMissAnswerAnimation;
        [SerializeField]
        GameObject correctPanelObjects,missPanelObjects, inputFieldObject;
        [SerializeField]
        TextMeshProUGUI[] questionText, correctText, descriptionText;

        public AudioClip correctClip, missClip;
        private void Awake()
        {
            // QuestionGameController�̃C�x���g���w��
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;
            GameManager.OnGameStateChanged += HandleGameStateChanged;
            playerMissAnswerAnimation.OnMissAnimationComplete+=()=> missPanelObjects.SetActive(true);

        }
        private void OnDestroy()
        {
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
            GameManager.OnGameStateChanged -= HandleGameStateChanged;
            playerMissAnswerAnimation.OnMissAnimationComplete -= () => missPanelObjects.SetActive(true);
        }
        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.GameOver||
                state == GameState.GameClear)
            {
                correctPanelObjects.SetActive(false);
                missPanelObjects.SetActive(false);
            }
        }
        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            //Debug.Log(newJudge);
            // newJudge�̒l�ɉ����ď����𕪊�
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

        IEnumerator WaitDisplayPanel(GameObject gameObject,float duration, QuestionGameController.Judge newJudge)
        {
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(true);
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
