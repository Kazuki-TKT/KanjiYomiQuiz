using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KanjiYomi
{
    public class NumberOfCorrectAnswersGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        [SerializeField]
        List<GameObject> coverBleckImageList = new List<GameObject>();

        private void Awake()
        {
            // QuestionGameControllerのイベントを購読
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        }
        private void OnDestroy()
        {
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        void GameManagerOnGameStateChanged(GameState state)
        {
            if (state == GameState.Playing)
            {
                foreach (GameObject gameObject in coverBleckImageList)
                {
                    gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 正解するごとにアイコンを覆うイメージを非表示にする
        /// </summary>
        /// <param name="newJudge"></param>
        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            // newJudgeの値に応じて処理を分岐
            switch (newJudge)
            {
                case QuestionGameController.Judge.Correct:
                    foreach (GameObject gameObject in coverBleckImageList)
                    {
                        if (gameObject.activeSelf)
                        {
                            gameObject.SetActive(false);
                            break;
                        }
                    }
               break;
            }
        }
    }
}
