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
            Correct,//����
            Miss,//�s����
            Initial//����
        }

        //���݂�Judge�̏��
        private Judge currentJudgeState;

        // Judge���ω������Ƃ��ɔ��s�����C�x���g
        public event Action<Judge> OnJudgeStateChanged;

        //��/��/��/��/��/��/��/��/��/��/��/��/��/��/��/��
        //
        [SerializeField]
        StartCountDown startCountDown;
        [SerializeField]
        QuestionTextGUI questionTextGUI;


        // ���̍ő�̉񓚎���(10�b)
        const float MAX_ANSWER_TIME = 15.0f;
        // �v���C���[�̓����Btrue:���� false�s����
        public bool playerAnswer;

        //���݂̐���
        int correctAnswerCount = 0;
        public int CorrectAnswerCount { get => correctAnswerCount; }
        public AudioClip correctSE;
        public bool level1Flag, level2Flag, level3Flag, level4Flag;

        //�J�E���g�_�E���A�j���[�V����
        public Animator countDownAnimation;

        //�G�����X�^�[�𐧌䂷��
        [SerializeField] EnemyController enemyController;
        [SerializeField] LevelTextGUI levelTextGUI;

        //�L�����Z���g�[�N��
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

        // Judge�̏�Ԃ�ݒ肵�A�K�v�ɉ����ăC�x���g�𔭍s���郁�\�b�h
        public void SetGameState(Judge newJudge)
        {
            // ���݂̏�ԂƐV������Ԃ��قȂ�ꍇ�ɂ̂ݏ��������s
            if (currentJudgeState != newJudge)
            {
                currentJudgeState = newJudge;
                // �C�x���g�𔭍s
                OnJudgeStateChanged?.Invoke(currentJudgeState);
            }
        }

        // �Q�[�����n�߂郁�\�b�h
        public void StartGame()
        {
            GameManager.Instance.UpdateGameState(GameState.Playing);
            PlayerController.Instance.SetDefaultPlayerLife();
            SetGameState(Judge.Initial);
            // �V���� CancellationTokenSource ���쐬
            cts = new CancellationTokenSource();

            // CancellationToken �̎擾
            CancellationToken token = cts.Token;
            // �Q�[�����J�n

            PlayGame(token).Forget();
        }

        // �Q�[�����L�����Z�����郁�\�b�h
        public void CancelGame()
        {
            // �L�����Z���v���𔭍s
            cts?.Cancel();
        }


        /// <summary>
        /// �Q�[�����s���֐�
        /// </summary>
        async UniTask PlayGame(CancellationToken token)
        {

            correctAnswerCount = 0;
            level1Flag = level2Flag = level3Flag = level4Flag = false;
            QuestionManager.Instance.CreateQuestionData();
            //�J�E���g�_�E���J�n
            StartCoroutine(startCountDown.StartTextCountdown());
            await UniTask.WaitUntil(() => startCountDown.IsCountdownFinished);
            try
            {
                while (PlayerController.Instance.PlayerLife > 0
                    && GameManager.Instance.gameState == GameState.Playing
                    && correctAnswerCount < 10)
                {
                    //������
                    playerAnswer = false;
                    SetGameState(Judge.Initial);

                    //----�ҋ@����----//
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

                    //�����擾
                    QuestionManager.Instance.GetQuestionData(correctAnswerCount);

                    //�G���������A�U�����s��
                    enemyController.MonsterSpawnAndAttackAnimation(QuestionManager.Instance.CurrentData.questionDifficulty, token);

                    //----�ҋ@����----//
                    await UniTask.Delay(TimeSpan.FromSeconds(enemyController.CurrentMonsterData.enemySpawn.spawnEffectTime + 1f), cancellationToken: token);

                    //����\������
                    questionTextGUI.SetQuesitionText(MAX_ANSWER_TIME, QuestionManager.Instance.CurrentData);

                    countDownAnimation.SetTrigger("play");
                    //�v���C���[�̉񓚎��Ԃ̓���
                    float elapsedTime = 0f;
                    while (elapsedTime < MAX_ANSWER_TIME && !playerAnswer)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: token);
                        elapsedTime += 0.1f;
                        if (playerAnswer) break;
                    }

                    //����s�����ŕ���
                    if (playerAnswer)
                    {
                        Debug.Log($"����:{QuestionManager.Instance.CurrentData.Correct}");
                        KanjiYomi.AuidoManager.Instance.PlaySound_SE(correctSE);//������
                        SetGameState(Judge.Correct);//����̕ύX
                        correctAnswerCount++;//���𐔂�1�J�E���g�ǉ�
                        enemyController.MonsterDie(token);//�G�|���
                        countDownAnimation.SetTrigger("stop");//�J�E���g�_�E���A�j���[�V�������~�߂�
                        //----�ҋ@����----//
                        await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: token);
                    }
                    else
                    {
                        Debug.Log($"�s����:{QuestionManager.Instance.CurrentData.Correct}");
                        SetGameState(Judge.Miss);//����̕ύX
                        enemyController.MonsterEscape();//�G������
                        countDownAnimation.SetTrigger("stop");//�J�E���g�_�E���A�j���[�V�������~�߂�

                        //----�ҋ@����----//
                        bool result = await PlayerController.Instance.answerMissAnimation.MissAnimation(token);
                        await UniTask.WaitUntil(() => result);
                        //----�ҋ@����----//
                        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: token);// �L�[�{�[�h�̃X�y�[�X���������܂�
                        if (PlayerController.Instance.PlayerLife > 0) PlayerController.Instance.DecreasePlayerLife();//�v���C���[�̃��C�t�����

                    }
                    //----�ҋ@����----//
                    await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
                }
                if (CorrectAnswerCount == 10)
                {
                    //�Q�[���N���A
                    GameManager.Instance.UpdateGameState(GameState.GameClear);
                }
                else
                {
                    //�Q�[���I�[�o�[
                    GameManager.Instance.UpdateGameState(GameState.GameOver);
                }

            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("�Q�[�����L�����Z������܂����B");
            }
            catch (Exception ex)
            {
                Debug.LogError($"�G���[���������܂���: {ex.Message}");
            }
        }

        void ClearGame()
        {
            Debug.Log("�N���A");
        }


    }
}
