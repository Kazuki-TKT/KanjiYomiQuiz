using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

namespace KanjiYomi
{
    public class PlayerInputFieldGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        [SerializeField]
        GameObject playerInputFieldObject;

        TMP_InputField playerInputField;
        public TMP_InputField PlayerInputField { get => playerInputField; }

        private void Awake()
        {
            // QuestionGameControllerのイベントを購読
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;

        }
        void Start()
        {
            if (playerInputFieldObject != null)
                playerInputField = playerInputFieldObject.GetComponent<TMP_InputField>();
        }

        private void OnDestroy()
        {
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
        }
        public void CheckAnswer()
        {
            questionGameController.playerAnswer = QuestionManager.Instance.CheckQuestion(QuestionManager.Instance.CurrentData, PlayerInputField);
            if (!questionGameController.playerAnswer)
            {
                PlayerInputField.text = "";
                PlayerInputField.ActivateInputField();
            }
        }
        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            // newJudgeの値に応じて処理を分岐
            switch (newJudge)
            {
                case QuestionGameController.Judge.Correct:
                case QuestionGameController.Judge.Miss:
                    EventSystem.current.SetSelectedGameObject(null);
                    playerInputFieldObject.SetActive(false);
                    break;
                case QuestionGameController.Judge.Initial:
                    playerInputFieldObject.SetActive(true);
                    playerInputField.ActivateInputField();
                    playerInputField.text = "";
                    break;
            }
        }

    }
}
