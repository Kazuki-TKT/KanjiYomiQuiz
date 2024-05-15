using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace KanjiYomi
{
    /// <summary>
    /// 漢字当てゲーム時に使用するインプットフィールドを管理するクラス
    /// </summary>
    public class PlayerInputFieldGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        //インプットフィールドを子に持つオブジェクト
        [SerializeField]
        GameObject playerInputFieldObject;

        //インプットフィールド
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
            //イベント解除
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
        }

        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            // newJudgeの値に応じて処理を分岐
            switch (newJudge)
            {
                case QuestionGameController.Judge.Correct:
                case QuestionGameController.Judge.Miss:
                    OffInputField();//
                    break;
                case QuestionGameController.Judge.Initial:
                    //正答数とフラグにより処理を変更
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
        /// プレイヤーが打ちこんだ値が問題の答えと合っているか確かめるメソッド
        /// </summary>
        public void CheckAnswer()
        {
            questionGameController.playerAnswer = QuestionManager.Instance.CheckQuestion(QuestionManager.Instance.CurrentData, PlayerInputField);// プレイヤーが打ちこんだ値が問題の答えと合っているか確かめる
            if (!questionGameController.playerAnswer)// 間違っていた場合インプットフィールドの文字を削除する
            {
                PlayerInputField.text = "";
                PlayerInputField.ActivateInputField();
            }
        }
       
        /// <summary>
        /// プレイヤー用のインプットフィールドをOffにするメソッド
        /// </summary>
        public void OffInputField()
        {
            EventSystem.current.SetSelectedGameObject(null);
            playerInputFieldObject.SetActive(false);
        }

        /// <summary>
        /// プレイヤー用のインプットフィールドをOnにするメソッド
        /// </summary>
        public void OnInputField()
        {
            playerInputFieldObject.SetActive(true);
            playerInputField.ActivateInputField();
            playerInputField.text = "";
        }
    }
}
