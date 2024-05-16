using System.Collections;
using TMPro;
using UnityEngine;

namespace KanjiYomi
{
    /// <summary>
    /// ���̐����A�s�������̃I�u�W�F�N�g�𑀍삷��N���X
    /// </summary>
    public class JudgePanelGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        //�v���C���[�s�������̃A�j���[�V����
        [SerializeField]
        PlayerMissAnswerAnimation playerMissAnswerAnimation;

        //�����A�s�������̃I�u�W�F�N�g�B�C���v�b�g�t�B�[���h�̃I�u�W�F�N�g
        [SerializeField]
        GameObject correctPanelObjects,missPanelObjects, inputFieldObject;

        //���A�����A������\������TMP
        [SerializeField]
        TextMeshProUGUI[] questionText, correctText, descriptionText;

        //�����A�s��������SE
        public AudioClip correctClip, missClip;

        private void Awake()
        {
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;// QuestionGameController�̃C�x���g���w��
            GameManager.OnGameStateChanged += HandleGameStateChanged;// QuestionGameController�̃C�x���g���w��
            playerMissAnswerAnimation.OnMissAnimationComplete += HandleMissAnimationComplete;// PlayerMissAnswerAnimation�̃C�x���g���w��

        }

        private void OnDestroy()
        {
            //����
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
            GameManager.OnGameStateChanged -= HandleGameStateChanged;
            playerMissAnswerAnimation.OnMissAnimationComplete -= HandleMissAnimationComplete;
        }


        private void HandleMissAnimationComplete()
        {
            missPanelObjects.SetActive(true);
        }
        //�Q�[���X�e�[�g�ɂ���ĕ��򂷂郁�\�b�h
        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.GameOver||
                state == GameState.GameClear)
            {
                correctPanelObjects.SetActive(false);
                missPanelObjects.SetActive(false);
            }
        }

        //�����A�s�����A���̑��ɂ���ĕ��򂷂郁�\�b�h
        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
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

        //�w��b�҂��Ă���I�u�W�F�N�g�Ɖ���炷�R���[�`��
        IEnumerator WaitDisplayPanel(GameObject gameObject,float duration, QuestionGameController.Judge newJudge)
        {
            Debug.Log($"WaitDisplayPanel started for {newJudge}, waiting for {duration} seconds.");
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(true);
            // newJudge�̒l�ɉ����ď����𕪊�
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
                $"<color=#{TextColor.WHITE:X}><size=40%>{questionData.AdjectiveString}</size></color>";

                correctString = $"<color=#{TextColor.ORANGE:X}>{questionData.Correct[0]}</color>" +
                $"<color=#{TextColor.WHITE:X}>{questionData.AdjectiveString}</color>";
            }
            else
            {
                //GUI�p��string�ő������s��
                questionString = $"<color=#{TextColor.ORANGE:X}>{questionData.Question}</color>";
                correctString = $"<color=#{TextColor.ORANGE:X}>{questionData.Correct[0]}</color>";
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
