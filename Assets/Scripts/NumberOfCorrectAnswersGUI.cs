using System.Collections.Generic;
using UnityEngine;

namespace KanjiYomi
{
    /// <summary>
    /// 画面左上のプレイヤーの正答数を表示するクラス
    /// </summary>
    public class NumberOfCorrectAnswersGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        //各UIを覆う黒いカバーイメージ
        [SerializeField]
        List<GameObject> coverBleckImageList = new List<GameObject>();

        private void Awake()
        {
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;// QuestionGameControllerのイベントを購読
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;//  GameManagerのイベントを購読
        }
        private void OnDestroy()
        {
            //解除
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        //GameState.Playingの時、初期状態にするメソッド
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
        /// 正解するごとにアイコンを覆うイメージを非表示にするメソッド
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
