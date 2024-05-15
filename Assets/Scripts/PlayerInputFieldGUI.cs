using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace KanjiYomi
{
    /// <summary>
    /// �������ăQ�[�����Ɏg�p����C���v�b�g�t�B�[���h���Ǘ�����N���X
    /// </summary>
    public class PlayerInputFieldGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        //�C���v�b�g�t�B�[���h���q�Ɏ��I�u�W�F�N�g
        [SerializeField]
        GameObject playerInputFieldObject;

        //�C���v�b�g�t�B�[���h
        TMP_InputField playerInputField;
        public TMP_InputField PlayerInputField { get => playerInputField; }

        private void Awake()
        {
            // QuestionGameController�̃C�x���g���w��
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;

        }
        void Start()
        {
            if (playerInputFieldObject != null)
                playerInputField = playerInputFieldObject.GetComponent<TMP_InputField>();
        }

        private void OnDestroy()
        {
            //�C�x���g����
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
        }

        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            // newJudge�̒l�ɉ����ď����𕪊�
            switch (newJudge)
            {
                case QuestionGameController.Judge.Correct:
                case QuestionGameController.Judge.Miss:
                    OffInputField();//
                    break;
                case QuestionGameController.Judge.Initial:
                    //�������ƃt���O�ɂ�菈����ύX
                    if ((questionGameController.CorrectAnswerCount == 0 && !questionGameController.level1Flag) ||
                        (questionGameController.CorrectAnswerCount == 3 && !questionGameController.level2Flag) ||
                        (questionGameController.CorrectAnswerCount == 6 && !questionGameController.level3Flag) ||
                       (questionGameController.CorrectAnswerCount == 9 && !questionGameController.level4Flag))
                    {
                        OffInputField();
                    }
                    else
                    {
                        OnInputField();
                    }
                    break;
            }
        }

        /// <summary>
        /// �v���C���[���ł����񂾒l�����̓����ƍ����Ă��邩�m���߂郁�\�b�h
        /// </summary>
        public void CheckAnswer()
        {
            questionGameController.playerAnswer = QuestionManager.Instance.CheckQuestion(QuestionManager.Instance.CurrentData, PlayerInputField);// �v���C���[���ł����񂾒l�����̓����ƍ����Ă��邩�m���߂�
            if (!questionGameController.playerAnswer)// �Ԉ���Ă����ꍇ�C���v�b�g�t�B�[���h�̕������폜����
            {
                PlayerInputField.text = "";
                PlayerInputField.ActivateInputField();
            }
        }
       
        /// <summary>
        /// �v���C���[�p�̃C���v�b�g�t�B�[���h��Off�ɂ��郁�\�b�h
        /// </summary>
        public void OffInputField()
        {
            EventSystem.current.SetSelectedGameObject(null);
            playerInputFieldObject.SetActive(false);
        }

        /// <summary>
        /// �v���C���[�p�̃C���v�b�g�t�B�[���h��On�ɂ��郁�\�b�h
        /// </summary>
        public void OnInputField()
        {
            playerInputFieldObject.SetActive(true);
            playerInputField.ActivateInputField();
            playerInputField.text = "";
        }
    }
}
