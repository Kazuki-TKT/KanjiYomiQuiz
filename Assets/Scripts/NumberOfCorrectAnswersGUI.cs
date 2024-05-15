using System.Collections.Generic;
using UnityEngine;

namespace KanjiYomi
{
    /// <summary>
    /// ��ʍ���̃v���C���[�̐�������\������N���X
    /// </summary>
    public class NumberOfCorrectAnswersGUI : MonoBehaviour
    {
        [SerializeField]
        QuestionGameController questionGameController;

        //�eUI�𕢂������J�o�[�C���[�W
        [SerializeField]
        List<GameObject> coverBleckImageList = new List<GameObject>();

        private void Awake()
        {
            questionGameController.OnJudgeStateChanged += HandleJudgeStateChanged;// QuestionGameController�̃C�x���g���w��
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;//  GameManager�̃C�x���g���w��
        }
        private void OnDestroy()
        {
            //����
            questionGameController.OnJudgeStateChanged -= HandleJudgeStateChanged;
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        //GameState.Playing�̎��A������Ԃɂ��郁�\�b�h
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
        /// �������邲�ƂɃA�C�R���𕢂��C���[�W���\���ɂ��郁�\�b�h
        /// </summary>
        /// <param name="newJudge"></param>
        private void HandleJudgeStateChanged(QuestionGameController.Judge newJudge)
        {
            // newJudge�̒l�ɉ����ď����𕪊�
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
