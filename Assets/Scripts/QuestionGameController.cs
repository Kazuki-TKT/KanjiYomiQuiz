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
            Miss,//不正解
            Initial//初期
        }

        //現在のJudgeの状態
        private Judge currentJudgeState;

        // Judgeが変化したときに発行されるイベント
        public event Action<Judge> OnJudgeStateChanged;

        //◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆/◆
        //
        [SerializeField]
        StartCountDown startCountDown;
        [SerializeField]
        QuestionTextGUI questionTextGUI;


        // 一問の最大の回答時間(10秒)
        const float MAX_ANSWER_TIME = 15.0f;
        // プレイヤーの答え。true:正解 false不正解
        public bool playerAnswer;

        //現在の正解数
        int correctAnswerCount = 0;
        public int CorrectAnswerCount { get => correctAnswerCount; }
        public AudioClip correctSE;
        public bool level1Flag, level2Flag, level3Flag, level4Flag;

        //カウントダウンアニメーション
        public Animator countDownAnimation;

        //敵モンスターを制御する
        [SerializeField] EnemyController enemyController;
        [SerializeField] LevelTextGUI levelTextGUI;

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

        private void OnEnable()
        {
            StartGame();
        }

        void GameManagerOnGameStateChanged(GameState state)
        {
            //if (state == GameState.Playing) StartGame();
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
            GameManager.Instance.UpdateGameState(GameState.Playing);
            PlayerController.Instance.SetDefaultPlayerLife();
            SetGameState(Judge.Initial);
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
            level1Flag = level2Flag = level3Flag = level4Flag = false;
            QuestionManager.Instance.CreateQuestionData();
            //カウントダウン開始
            StartCoroutine(startCountDown.StartTextCountdown());
            await UniTask.WaitUntil(() => startCountDown.IsCountdownFinished);
            try
            {
                while (PlayerController.Instance.PlayerLife > 0
                    && GameManager.Instance.gameState == GameState.Playing
                    && correctAnswerCount < 10)
                {
                    //初期化
                    playerAnswer = false;
                    SetGameState(Judge.Initial);

                    //----待機処理----//
                    switch (correctAnswerCount)
                    {
                        case 0:
                            if (!level1Flag)
                            {
                                levelTextGUI.OnDisplayLevelText(correctAnswerCount);
                                await UniTask.Delay(TimeSpan.FromSeconds(levelTextGUI.duration + 0.5f), cancellationToken: token);
                                PlayerController.Instance.PlayCameraMove(PlayerController.Instance.timelineAssets[0]);
                                await UniTask.WaitUntil(() => PlayerController.Instance.DirectorStop, cancellationToken: token);
                                level1Flag = true;
                            }
                            break;
                        case 3:
                            if (!level2Flag)
                            {
                                levelTextGUI.OnDisplayLevelText(correctAnswerCount);
                                await UniTask.Delay(TimeSpan.FromSeconds(levelTextGUI.duration + 0.5f), cancellationToken: token);
                                PlayerController.Instance.PlayCameraMove(PlayerController.Instance.timelineAssets[1]);
                                await UniTask.WaitUntil(() => PlayerController.Instance.DirectorStop, cancellationToken: token);
                                level2Flag = true;
                            }
                            break;
                        case 6:
                            if (!level3Flag)
                            {
                                levelTextGUI.OnDisplayLevelText(correctAnswerCount);
                                await UniTask.Delay(TimeSpan.FromSeconds(levelTextGUI.duration + 0.5f), cancellationToken: token);
                                PlayerController.Instance.PlayCameraMove(PlayerController.Instance.timelineAssets[2]);
                                await UniTask.WaitUntil(() => PlayerController.Instance.DirectorStop, cancellationToken: token);
                                level3Flag = true;
                            }
                            break;
                        case 9:
                            if (!level4Flag)
                            {
                                levelTextGUI.OnDisplayLevelText(correctAnswerCount);
                                await UniTask.Delay(TimeSpan.FromSeconds(levelTextGUI.duration + 0.5f), cancellationToken: token);
                                AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.LastBattle);
                                PlayerController.Instance.PlayCameraMove(PlayerController.Instance.timelineAssets[3]);
                                await UniTask.WaitUntil(() => PlayerController.Instance.DirectorStop, cancellationToken: token);
                                level4Flag = true;
                            }
                            break;
                    }

                    //問題を取得
                    QuestionManager.Instance.GetQuestionData(correctAnswerCount);

                    //敵を召喚し、攻撃を行う
                    enemyController.MonsterSpawnAndAttackAnimation(QuestionManager.Instance.CurrentData.questionDifficulty, token);

                    //----待機処理----//
                    await UniTask.Delay(TimeSpan.FromSeconds(enemyController.CurrentMonsterData.enemySpawn.spawnEffectTime + 1f), cancellationToken: token);

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
                        KanjiYomi.AuidoManager.Instance.PlaySound_SE(correctSE);//正解音
                        SetGameState(Judge.Correct);//判定の変更
                        correctAnswerCount++;//正解数を1カウント追加
                        enemyController.MonsterDie(token);//敵倒れる
                        countDownAnimation.SetTrigger("stop");//カウントダウンアニメーションを止める
                        //----待機処理----//
                        await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: token);
                    }
                    else
                    {
                        Debug.Log($"不正解:{QuestionManager.Instance.CurrentData.Correct}");
                        SetGameState(Judge.Miss);//判定の変更
                        enemyController.MonsterEscape();//敵逃げる
                        countDownAnimation.SetTrigger("stop");//カウントダウンアニメーションを止める

                        //----待機処理----//
                        bool result = await PlayerController.Instance.answerMissAnimation.MissAnimation(token);
                        await UniTask.WaitUntil(() => result);
                        //----待機処理----//
                        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: token);// キーボードのスペースが押されるまで
                        if (PlayerController.Instance.PlayerLife > 0) PlayerController.Instance.DecreasePlayerLife();//プレイヤーのライフを削る

                    }
                    //----待機処理----//
                    await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
                }
                if (CorrectAnswerCount == 10)
                {
                    //ゲームクリア
                    GameManager.Instance.UpdateGameState(GameState.GameClear);
                }
                else
                {
                    //ゲームオーバー
                    GameManager.Instance.UpdateGameState(GameState.GameOver);
                }

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
