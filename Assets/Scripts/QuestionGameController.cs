using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using TMPro;
namespace KanjiYomi
{
    /// <summary>
    /// �������ăQ�[�����R���g���[������N���X
    /// </summary>
    public class QuestionGameController : MonoBehaviour
    {
        /// <summary>
        /// ����̗񋓌^
        /// </summary>
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

        //�Q�[���J�n�O��3,2,1,GO!�p�̃J�E���g�_�E�����s���N���X
        [SerializeField]
        StartCountDown startCountDown;

        //����\������e�L�X�g�̃N���X
        [SerializeField]
        QuestionTextGUI questionTextGUI;

        // ���̍ő�̉񓚎���(10�b)
        const float MAX_ANSWER_TIME = 15.0f;

        // �v���C���[�̓����Btrue:���� false�s����
        public bool playerAnswer;

        //���݂̐���
        int correctAnswerCount = 0;
        public int CorrectAnswerCount { get => correctAnswerCount; }

        //�������̉�
        public AudioClip correctSE;

        //�e���̃��x�����s��ꂽ���ǂ������Ǘ�����t���O
        public bool level1Flag, level2Flag, level3Flag, level4Flag;

        //�e���̃��x���p�̃e�L�X�g��\��������N���X
        [SerializeField] LevelTextGUI levelTextGUI;

        //�J�E���g�_�E���A�j���[�V����
        public Animator countDownAnimation;

        //�G�����X�^�[�𐧌䂷��N���X
        [SerializeField] EnemyController enemyController;

        //�L�����Z���g�[�N��
        CancellationTokenSource cts;

        public TextMeshProUGUI count;
        private void OnEnable()
        {
            StartGame();
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
            if (GameManager.Instance.currentGameState != GameState.Playing) GameManager.Instance.UpdateGameState(GameState.Playing);//�Q�[���v���C���O����Ȃ��ꍇ�Q�[���v���C���O�ɕύX
            PlayerController.Instance.SetDefaultPlayerLife(); //�v���C���[�̃��C�t�������l�ɐݒ�
            cts = new CancellationTokenSource();// �V���� CancellationTokenSource ���쐬
            CancellationToken token = cts.Token;// CancellationToken �̎擾
            PlayGame(token).Forget();// �Q�[�����J�n
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
            QuestionManager.Instance.CreateQuestionData();//���̍쐬

            //--Fade�p�̃L�����o�X�O���[�v��blocksRaycasts���I�t�ɂȂ�܂őҋ@
            await UniTask.WaitUntil(() => !SceneLoader.Instance.fadeCanvasGroup.blocksRaycasts, cancellationToken: token);

            correctAnswerCount = 0;//���݂̐��𐔂�0�ɂ���
            level1Flag = level2Flag = level3Flag = level4Flag = false;//�e���x���̃t���O��false�ɂ���
            StartCoroutine(startCountDown.StartTextCountdown());//�J�E���g�_�E���J�n

            //--3,2,1,GO�̃J�E���^�_�E�����I������܂őҋ@
            await UniTask.WaitUntil(() => startCountDown.IsCountdownFinished);

            try
            {
                while (PlayerController.Instance.PlayerLife > 0//�v���C���[�̃��C�t��0����
                    && GameManager.Instance.currentGameState == GameState.Playing//�X�e�[�g��Playing
                    && correctAnswerCount < 10)//���𐔂�10��艺
                {
                    //������
                    playerAnswer = false;
                    SetGameState(Judge.Initial);//����̃X�e�[�g��������
                    float elapsedTime = 0f;//�o�ߎ��Ԃ�0�Ƀ��Z�b�g
                    int countTime = 0;

                    //���𐔂ɂ�菈�����s��
                    switch (correctAnswerCount)
                    {
                        case 0:
                            if (!level1Flag)
                            {
                                bool level1 = await ChangeDependingOnFlags(0, token);//���x��1�̍ŏ���1�񂾂��s��
                                await UniTask.WaitUntil(() => level1);
                            }
                            level1Flag = true;
                            break;
                        case 3:
                            if (!level2Flag)
                            {
                                bool level2 = await ChangeDependingOnFlags(1, token);//���x��2�̍ŏ���1�񂾂��s��
                                await UniTask.WaitUntil(() => level2);
                            }
                            level2Flag = true;
                            break;
                        case 6:
                            if (!level3Flag)
                            {
                                bool level3 = await ChangeDependingOnFlags(2, token);//���x��3�̍ŏ���1�񂾂��s��
                                await UniTask.WaitUntil(() => level3);
                            }
                            level3Flag = true;
                            break;
                        case 9:
                            if (!level4Flag)
                            {
                                AuidoManager.Instance.PlaySound_BGM(BGMData.BGM.LastBattle);//BGM�ύX
                                bool level4 = await ChangeDependingOnFlags(3, token);//���x��4�̍ŏ���1�񂾂��s��
                                await UniTask.WaitUntil(() => level4);
                            }
                            level4Flag = true;
                            break;
                        default:

                            break;
                    }
                    QuestionManager.Instance.GetQuestionData(correctAnswerCount);//�����擾
                    enemyController.MonsterSpawnAndAttack(QuestionManager.Instance.CurrentData.questionDifficulty, token);//�G���������A�U�����s��

                    //--�G�̃A�j���[�V��������+1�b�ҋ@
                    await UniTask.Delay(TimeSpan.FromSeconds(enemyController.CurrentMonsterData.enemySpawn.spawnEffectTime + 1f), cancellationToken: token);

                    questionTextGUI.SetQuesitionText(MAX_ANSWER_TIME, QuestionManager.Instance.CurrentData);//����\������

                    while (elapsedTime < MAX_ANSWER_TIME && !playerAnswer) //�������ԓ��ɐ�������܂�While
                    {
                        if (playerAnswer) break;
                        if (countTime == 8) countDownAnimation.SetBool("play", true);//�J�E���g�_�E���A�j�����n�߂�

                        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
                        elapsedTime += 1f;
                        countTime++;
                        count.text = countTime.ToString();
                    }
                    countDownAnimation.SetBool("play", false);//�J�E���g�_�E���A�j���[�V�������~�߂�
                    //����s�����ŕ���
                    if (playerAnswer)
                    {
                        //Debug.Log($"����:{QuestionManager.Instance.CurrentData.Correct[0]}");
                        AuidoManager.Instance.PlaySound_SE(correctSE);//������
                        SetGameState(Judge.Correct);//����̕ύX
                        correctAnswerCount++;//���𐔂�1�J�E���g�ǉ�
                        enemyController.MonsterDie(token);//�G�|���
                        
                        //--3�b�ҋ@
                        await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: token);
                    }
                    else
                    {
                        //Debug.Log($"�s����:{QuestionManager.Instance.CurrentData.Correct}");
                        SetGameState(Judge.Miss);//����̕ύX
                        enemyController.MonsterEscape();//�G������
                        
                        
                        //--MissAnimationga���I������܂őҋ@
                        bool result = await PlayerController.Instance.answerMissAnimation.MissAnimation(token);

                        //--result��true�ɂȂ�܂�
                        await UniTask.WaitUntil(() => result);

                        //--�X�y�[�X�L�[�������܂őҋ@
                        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: token);// �L�[�{�[�h�̃X�y�[�X���������܂�
                        if (PlayerController.Instance.PlayerLife > 0) PlayerController.Instance.DecreasePlayerLife();//�v���C���[�̃��C�t�����

                    }

                    //--1�b�ҋ@
                    await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);

                }
                if (CorrectAnswerCount == 10)//�Q�[���N���A
                {
                    GameManager.Instance.UpdateGameState(GameState.GameClear);
                }
                else//�Q�[���I�[�o�[
                {
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

        /// <summary>
        /// �e���x���̍ŏ��ɍs�����\�b�h
        /// </summary>
        async �@UniTask<bool> ChangeDependingOnFlags(int timelineNum,CancellationToken token)
        {
                levelTextGUI.OnDisplayLevelText(correctAnswerCount);//�e���x���̃e�L�X�g�A�j���[�V����

                //--�e�L�X�g�A�j���[�V�����̎���+0.5f�b�ҋ@
                await UniTask.Delay(TimeSpan.FromSeconds(levelTextGUI.duration + 0.5f), cancellationToken: token);

                PlayerController.Instance.PlayCameraMove(PlayerController.Instance.timelineAssets[timelineNum]);//�e���x���̃J�����A�j���[�V����

                //--�J�����A�j���[�V�������I���܂őҋ@
                await UniTask.WaitUntil(() => PlayerController.Instance.DirectorStop, cancellationToken: token);

                return true;//true��Ԃ�
        }
    }
}
