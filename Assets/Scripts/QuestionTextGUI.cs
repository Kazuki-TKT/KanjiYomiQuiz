using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace KanjiYomi
{
    public class QuestionTextGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        //���̃e�L�X�g���q�Ɏ��e�Q�[���I�u�W�F�N�g
        [SerializeField]
        GameObject questionTextObject, questionTextAdjectiveObject;

        //��L2���܂Ƃ߂��L�����o�X�O���[�v
        CanvasGroup canvasGroup;

        //���̃e�L�X�g��\��������TMP
        [SerializeField]
        TextMeshProUGUI kanjiText, kanjiTextAdjective, adjectiveText;

        //�X�P�[���̒l��ݒ�
        [SerializeField]
        Vector3 defaltScale,targetScale;

        private Tweener scaleUpTween;

        //�p�[�e�B�N��(Hit)
        [SerializeField]
        ParticleSystem hitParticle;
        private void Awake()
        {
            // QuestionGameController�̃C�x���g���w��
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;

        }
        void Start()
        {
            //defaultTransform = gameObject.transform;
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
            // newJudge�̒l�ɉ����ď����𕪊�
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
        /// ��蕶���Z�b�g���郁�\�b�h
        /// </summary>
        public void SetQuesitionText(float time, QuestionData questionData)
        {
            SetQuestionDataText(questionData);
            canvasGroup.DOFade(1, 0.2f);
            scaleUpTween = gameObject.transform.DOScale(targetScale, time);
        }

        //�L�����Z��
        void CancelFadeIn()
        {
            if (scaleUpTween != null)
            {
                scaleUpTween.Kill(); // Tween���L�����Z��
            }
        }
        /// <summary>
        /// ��蕶�����Z�b�g���郁�\�b�h
        /// </summary>
        public void ResetQuesitionText()
        {
            canvasGroup.alpha = 0;
            gameObject.transform.localScale = defaltScale;
            questionTextObject.SetActive(false);
            questionTextAdjectiveObject.SetActive(false);
            kanjiText.text = kanjiTextAdjective.text = adjectiveText.text = "";
        }

        void SetQuestionDataText(QuestionData questionData)
        {
            //�`�e������Ȃ��ŕ���
            if (questionData.adjective)
            {
                //����GUI�Ƀf�[�^���Z�b�g
                questionTextAdjectiveObject.SetActive(true);
                kanjiTextAdjective.text = questionData.Question;
                adjectiveText.text = questionData.AdjectiveString;
            }
            else
            {
                //����GUI�Ƀf�[�^���Z�b�g
                questionTextObject.SetActive(true);
                kanjiText.text = questionData.Question;
            }
        }
    }
}
