using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using TMPro;
namespace KanjiYomi
{
    /// <summary>
    /// 漢字当てゲームをコントロールするクラス
    /// </summary>
    public class QuestionGameController : MonoBehaviour
    {
        /// <summary>
        /// 判定の列挙型
        /// </summary>
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

        //ゲーム開始前の3,2,1,GO!用のカウントダウンを行うクラス
        [SerializeField]
        StartCountDown startCountDown;

        //問題を表示するテキストのクラス
        [SerializeField]
        QuestionTextGUI questionTextGUI;

        // 一問の最大の回答時間(10秒)
        const float MAX_ANSWER_TIME = 15.0f;

        // プレイヤーの答え。true:正解 false不正解
        public bool playerAnswer;

        //現在の正解数
        int correctAnswerCount = 0;
        public int CorrectAnswerCount { get => correctAnswerCount; }

        //正解時の音
        public AudioClip correctSE;

        //各問題のレベルが行われたかどうかを管理するフラグ
        public bool level1Flag, level2Flag, level3Flag, level4Flag;

        //各問題のレベル用のテキストを表示させるクラス
        [SerializeField] LevelTextGUI levelTextGUI;

        //カウントダウンアニメーション
        public Animator countDownAnimation;

        //敵モンスターを制御するクラス
        [SerializeField] EnemyController enemyController;

        //キャンセルトークン
        CancellationTokenSource cts;

        public TextMeshProUGUI count;
        private void OnEnable()
        {
            StartGame();
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
            if (GameManager.Instance.currentGameState != GameState.Playing) GameManager.Instance.UpdateGameState(GameState.Playing);//ゲームプレイングじゃない場合ゲームプレイングに変更
            PlayerController.Instance.SetDefaultPlayerLife(); //プレイヤーのライフを初期値に設定
            cts = new CancellationTokenSource();// 新しい CancellationTokenSource を作成
            CancellationToken token = cts.Token;// CancellationToken の取得
            PlayGame(token).Forget();// ゲームを開始
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
            QuestionManager.Instance.CreateQuestionData();//問題の作成

            //--Fade用のキャンバスグループのblocksRaycastsがオフになるまで待機
            await UniTask.WaitUntil(() => !SceneLoader.Instance.fadeCanvasGroup.blocksRaycasts, cancellationToken: token);

            correctAnswerCount = 0;//現在の正解数を0にする
            level1Flag = level2Flag = level3Flag = level4Flag = false;//各レベルのフラグをfalseにする
            StartCoroutine(startCountDown.StartTextCountdown());//カウントダウン開始

            //--3,2,1,GOのカウンタダウンが終了するまで待機
            await UniTask.WaitUntil(() => startCountDown.IsCountdownFinished);

            try
            {
                while (PlayerController.Instance.PlayerLife > 0//プレイヤーのライフが0より上
                    && GameManager.Instance.currentGameState == GameState.Playing//ステートがPlaying
                    && correctAnswerCount < 10)//正解数が10より下
                {
                    //初期化
                    playerAnswer = false;
                    SetGameState(Judge.Initial);//判定のステートを初期に
                    float elapsedTime = 0f;//経過時間を0にリセット
                    int countTime = 0;

                    //正解数により処理を行う
                    switch (correctAnswerCount)
                    {
                        case 0:
                            if (!level1Flag)
                            {
                                bool level1 = await ChangeDependingOnFlags(0, token);//レベル1の最初の1回だけ行う
                                await UniTask.WaitUntil(() => level1);
                            }
                            level1Flag = true;
                            break;
                        case 3:
                            if (!level2Flag)
                            {
                                bool level2 = await ChangeDependingOnFlags(1, token);//レベル2の最初の1回だけ行う
                                await UniTask.WaitUntil(() => level2);
                            }
                            level2Flag = true;
                            break;
                        case 6:
                            if (!level3Flag)
                            {
                                bool level3 = await ChangeDependingOnFlags(2, token);//レベル3の最初の1回だけ行う
                                await UniTask.WaitUntil(() => level3);
                            }
                            level3Flag = true;
                            break;
                        case 9:
                            if (!level4Flag)
                            {
                                AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.LastBattle);//BGM変更
                                bool level4 = await ChangeDependingOnFlags(3, token);//レベル4の最初の1回だけ行う
                                await UniTask.WaitUntil(() => level4);
                            }
                            level4Flag = true;
                            break;
                        default:

                            break;
                    }
                    QuestionManager.Instance.GetQuestionData(correctAnswerCount);//問題を取得
                    enemyController.MonsterSpawnAndAttack(QuestionManager.Instance.CurrentData.questionDifficulty, token);//敵を召喚し、攻撃を行う

                    //--敵のアニメーション時間+1秒待機
                    await UniTask.Delay(TimeSpan.FromSeconds(enemyController.CurrentMonsterData.enemySpawn.spawnEffectTime + 1f), cancellationToken: token);

                    questionTextGUI.SetQuesitionText(MAX_ANSWER_TIME, QuestionManager.Instance.CurrentData);//問題を表示する

                    while (elapsedTime < MAX_ANSWER_TIME && !playerAnswer) //制限時間内に正解するまでWhile
                    {
                        if (playerAnswer) break;
                        if (countTime == 8) countDownAnimation.SetBool("play", true);//カウントダウンアニメを始める

                        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
                        elapsedTime += 1f;
                        countTime++;
                        count.text = countTime.ToString();
                    }
                    countDownAnimation.SetBool("play", false);//カウントダウンアニメーションを止める
                    //正解不正解で分岐
                    if (playerAnswer)
                    {
                        //Debug.Log($"正解:{QuestionManager.Instance.CurrentData.Correct[0]}");
                        AuidoManager.Instance.PlaySound_SE(correctSE);//正解音
                        SetGameState(Judge.Correct);//判定の変更
                        correctAnswerCount++;//正解数を1カウント追加
                        enemyController.MonsterDie(token);//敵倒れる
                        
                        //--3秒待機
                        await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: token);
                    }
                    else
                    {
                        //Debug.Log($"不正解:{QuestionManager.Instance.CurrentData.Correct}");
                        SetGameState(Judge.Miss);//判定の変更
                        enemyController.MonsterEscape();//敵逃げる
                        
                        
                        //--MissAnimationgaが終了するまで待機
                        bool result = await PlayerController.Instance.answerMissAnimation.MissAnimation(token);

                        //--resultがtrueになるまで
                        await UniTask.WaitUntil(() => result);

                        //--スペースキーを押すまで待機
                        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: token);// キーボードのスペースが押されるまで
                        if (PlayerController.Instance.PlayerLife > 0) PlayerController.Instance.DecreasePlayerLife();//プレイヤーのライフを削る

                    }

                    //--1秒待機
                    await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);

                }
                if (CorrectAnswerCount == 10)//ゲームクリア
                {
                    GameManager.Instance.UpdateGameState(GameState.GameClear);
                }
                else//ゲームオーバー
                {
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

        /// <summary>
        /// 各レベルの最初に行うメソッド
        /// </summary>
        async 　UniTask<bool> ChangeDependingOnFlags(int timelineNum,CancellationToken token)
        {
                levelTextGUI.OnDisplayLevelText(correctAnswerCount);//各レベルのテキストアニメーション

                //--テキストアニメーションの時間+0.5f秒待機
                await UniTask.Delay(TimeSpan.FromSeconds(levelTextGUI.duration + 0.5f), cancellationToken: token);

                PlayerController.Instance.PlayCameraMove(PlayerController.Instance.timelineAssets[timelineNum]);//各レベルのカメラアニメーション

                //--カメラアニメーションが終わるまで待機
                await UniTask.WaitUntil(() => PlayerController.Instance.DirectorStop, cancellationToken: token);

                return true;//trueを返す
        }
    }
}
