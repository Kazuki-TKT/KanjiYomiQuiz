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
            Wrong,//�s����
            Initial//����
        }

        //���݂�Judge�̏��
        private Judge currentJudgeState;

        // Judge���ω������Ƃ��ɔ��s�����C�x���g
        public event Action<Judge> OnJudgeStateChanged;

        //��/��/��/��/��/��/��/��/��/��/��/��/��/��/��/��
        //
        [SerializeField]
        QuestionTextGUI questionTextGUI;

        // ���̍ő�̉񓚎���(10�b)
        const float MAX_ANSWER_TIME = 15.0f;
        // �v���C���[�̓����Btrue:���� false�s����
        public bool playerAnswer;

        //���݂̐���
        int correctAnswerCount=0;

        //�J�E���g�_�E���A�j���[�V����
        public Animator countDownAnimation;

        //�G�����X�^�[�𐧌䂷��
        [SerializeField] EnemyController enemyController;

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

        void GameManagerOnGameStateChanged(GameState state)
        {
            if (state == GameState.Playing) Debug.Log("Playing");
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
            QuestionManager.Instance.CreateQuestionData();
            try
            {
                while(PlayerController.Instance.PlayerLife>0
                    &&GameManager.Instance.gameState==GameState.Playing
                    &&correctAnswerCount<10)
                {
                    //�����擾
                    QuestionManager.Instance.GetQuestionData(correctAnswerCount);
                    //������
                    playerAnswer = false;
                    SetGameState(Judge.Initial);
                    //�G���������A�U�����s��
                    enemyController.MonsterSpawnAndAttackAnimation(QuestionManager.Instance.CurrentData.questionDifficulty,token);

                    //----�ҋ@����----//
                    await UniTask.Delay(TimeSpan.FromSeconds(enemyController.CurrentMonsterData.enemySpawn.spawnEffectTime+1f), cancellationToken: token);

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
                        SetGameState(Judge.Correct);//����̕ύX
                        correctAnswerCount++;//���𐔂�1�J�E���g�ǉ�
                        enemyController.MonsterDie(token);//�G�|���
                        countDownAnimation.SetTrigger("stop");//�J�E���g�_�E���A�j���[�V�������~�߂�
                        //----�ҋ@����----//
                        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
                    }
                    else
                    {
                        Debug.Log($"�s����:{QuestionManager.Instance.CurrentData.Correct}");
                        SetGameState(Judge.Wrong);//����̕ύX
                        enemyController.MonsterEscape();//�G������
                        countDownAnimation.SetTrigger("stop");//�J�E���g�_�E���A�j���[�V�������~�߂�
                        if (PlayerController.Instance.PlayerLife > 0) PlayerController.Instance.DecreasePlayerLife();//�v���C���[�̃��C�t�����
                        //----�ҋ@����----//
                        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: token);// �L�[�{�[�h�̃X�y�[�X���������܂�
                    }
                    //----�ҋ@����----//
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                }

                //�Q�[���N���A
                ClearGame();
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
