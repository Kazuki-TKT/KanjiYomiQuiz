using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
using TMPro;
using DG.Tweening;
using System.Threading;

namespace KanjiYomi
{
    public class QuestionGameController : MonoBehaviour
    {
        public enum Judge
        {
            Correct,//正解
            Wrong,//不正解
            Initial//初期
        }

        //現在のJudgeの状態
        private Judge currentJudgeState;

        // Judgeが変化したときに発行されるイベント
        public event Action<Judge> OnJudgeStateChanged;

        //◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆
        //
        [SerializeField]
        QuestionTextGUI questionTextGUI;

        // 一問の最大の回答時間(10秒)
        const float MAX_ANSWER_TIME = 15.0f;
        // プレイヤーの答え。true:正解 false不正解
        public bool playerAnswer;

        //現在の正解数
        int correctAnswerCount=0;

        //カウントダウンアニメーション
        public Animator countDownAnimation;

        //敵モンスターを制御する
        [SerializeField] EnemyController enemyController;

        //キャンセルトークン
        CancellationTokenSource cts;

        private void Awake()
        {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        void Start()
        {
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(1)) CancelGame();
            if (Input.GetKeyDown(KeyCode.Alpha1)) StartGame();
        }

        void GameManagerOnGameStateChanged(GameState state)
        {
            if (state == GameState.Playing) Debug.Log("Playing");
        }

        // Judgeの状態を設定し、必要に応じてイベントを発行するメソッド
        public void SetGameState(Judge newJudge)
        {
            // 現在の状態と新しい状態が異なる場合にのみ処理を実行
            if (currentJudgeState != newJudge)
            {
                currentJudgeState = newJudge;
                // イベントを発行
                OnJudgeStateChanged?.Invoke(currentJudgeState);
            }
        }

        // ゲームを始めるメソッド
        public void StartGame()
        {
            // 新しい CancellationTokenSource を作成
            cts = new CancellationTokenSource();

            // CancellationToken の取得
            CancellationToken token = cts.Token;

            // ゲームを開始
            PlayGame(token).Forget();
        }

        // ゲームをキャンセルするメソッド
        public void CancelGame()
        {
            // キャンセル要求を発行
            cts?.Cancel();
        }


        /// <summary>
        /// ゲームを行う関数
        /// </summary>
        async UniTask PlayGame(CancellationToken token)
        {
            correctAnswerCount = 0;
            QuestionManager.Instance.CreateQuestionData();
            try
            {
                while(PlayerController.Instance.PlayerLife>0
                    &&GameManager.Instance.gameState==GameState.Playing
                    &&correctAnswerCount<10)
                {
                    //問題を取得
                    QuestionManager.Instance.GetQuestionData(correctAnswerCount);
                    //初期化
                    playerAnswer = false;
                    SetGameState(Judge.Initial);
                    //敵を召喚し、攻撃を行う
                    enemyController.MonsterSpawnAndAttackAnimation(QuestionManager.Instance.CurrentData.questionDifficulty,token);

                    //----待機処理----//
                    await UniTask.Delay(TimeSpan.FromSeconds(enemyController.CurrentMonsterData.enemySpawn.spawnEffectTime+1f), cancellationToken: token);

                    //問題を表示する
                    questionTextGUI.SetQuesitionText(MAX_ANSWER_TIME, QuestionManager.Instance.CurrentData);

                    countDownAnimation.SetTrigger("play");
                    //プレイヤーの回答時間の動き
                    float elapsedTime = 0f;
                    while (elapsedTime < MAX_ANSWER_TIME && !playerAnswer)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: token);
                        elapsedTime += 0.1f;
                        if (playerAnswer) break;
                    }

                    //正解不正解で分岐
                    if (playerAnswer)
                    {
                        Debug.Log($"正解:{QuestionManager.Instance.CurrentData.Correct}");
                        SetGameState(Judge.Correct);//判定の変更
                        correctAnswerCount++;//正解数を1カウント追加
                        enemyController.MonsterDie(token);//敵倒れる
                        countDownAnimation.SetTrigger("stop");//カウントダウンアニメーションを止める
                        //----待機処理----//
                        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
                    }
                    else
                    {
                        Debug.Log($"不正解:{QuestionManager.Instance.CurrentData.Correct}");
                        SetGameState(Judge.Wrong);//判定の変更
                        enemyController.MonsterEscape();//敵逃げる
                        countDownAnimation.SetTrigger("stop");//カウントダウンアニメーションを止める
                        if (PlayerController.Instance.PlayerLife > 0) PlayerController.Instance.DecreasePlayerLife();//プレイヤーのライフを削る
                        //----待機処理----//
                        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: token);// キーボードのスペースが押されるまで
                    }
                    //----待機処理----//
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                }

                //ゲームクリア
                ClearGame();
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("ゲームがキャンセルされました。");
            }
            catch (Exception ex)
            {
                Debug.LogError($"エラーが発生しました: {ex.Message}");
            }
        }

        void ClearGame()
        {
            Debug.Log("クリア");
        }

        
    }
}
