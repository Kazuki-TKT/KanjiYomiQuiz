using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
namespace KanjiYomi
{
    public class QuestionManager : MonoBehaviour
    {

        public static QuestionManager Instance;

        /// <summary>
        /// ���W�̃X�N���^�u���I�u�W�F�N�g
        /// </summary>
        [SerializeField]
        [Header("���W�̃X�N���^�u���I�u�W�F�N�g")]
        QuestionCollectionAsset quesitonCollectionAsset;

        /// <summary>
        /// 1�Q�[���Ŏg�p������̃��X�g
        /// </summary>
        public List<QuestionData> questionDatas = new List<QuestionData>();

        List<QuestionData> randomDatas = new List<QuestionData>();

        public
        List<QuestionData> randomEasyDatas = new List<QuestionData>();
        List<QuestionData> randomNormalDatas = new List<QuestionData>();
        List<QuestionData> randomHardDatas = new List<QuestionData>();
        List<QuestionData> randomHellDatas = new List<QuestionData>();

        QuestionData currentData;
        public QuestionData CurrentData { get => currentData; set => currentData = value; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// �����擾���郁�\�b�h
        /// </summary>
        public void GetQuestionData(int count)
        {
            switch (count)
            {
                case 0:
                case 1:
                case 2:
                    GetRandomData(randomEasyDatas, Difficulty.Easy);
                    break;
                case 3:
                case 4:
                case 5:
                    GetRandomData(randomNormalDatas, Difficulty.Normal);
                    break;
                case 6:
                case 7:
                case 8:
                    GetRandomData(randomHardDatas, Difficulty.Hard);
                    break;
                case 9:
                    GetRandomData(randomHellDatas, Difficulty.Hell);
                    break;
            }
        }
        private void GetRandomData(List<QuestionData> dataList, Difficulty difficulty)
        {
                currentData = dataList.First();
                dataList.RemoveAt(0);
            if (dataList.Count == 0)
            {
                SortDifficultyData(difficulty);
            }
        }

        public void CreateQuestionData()
        {
            SortDifficultyData(Difficulty.Easy);
            SortDifficultyData(Difficulty.Normal);
            SortDifficultyData(Difficulty.Hard);
            SortDifficultyData(Difficulty.Hell);
        }

        void SortDifficultyData(Difficulty difficulty)
        {
            randomDatas.Clear();
            foreach (QuestionData questionData in quesitonCollectionAsset.questionData)
            {
                if (questionData.questionDifficulty != difficulty) continue;
                randomDatas.Add(questionData);
            }
            // ���X�g�������_���ɕ��בւ��āA�擪����w��J�E���g�܂�questionDatas�ɒǉ�
            randomDatas = randomDatas.OrderBy(a => Guid.NewGuid()).ToList();
            switch (difficulty)
            {
                case Difficulty.Easy:
                    randomEasyDatas = randomDatas.OrderBy(a => Guid.NewGuid()).ToList();
                    break;
                case Difficulty.Normal:
                    randomNormalDatas = randomDatas.OrderBy(a => Guid.NewGuid()).ToList();
                    break;
                case Difficulty.Hard:
                    randomHardDatas = randomDatas.OrderBy(a => Guid.NewGuid()).ToList();
                    break;
                case Difficulty.Hell:
                    randomHellDatas = randomDatas.OrderBy(a => Guid.NewGuid()).ToList();
                    break;
            }
        }

        /// <summary>
        /// ���͒l���������ǂ������肵�Abool�^�ŕԂ�
        /// </summary>
        public bool CheckQuestion(QuestionData questionData, TMP_InputField inputField)
        {
            if (questionData.Correct == inputField.text)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
