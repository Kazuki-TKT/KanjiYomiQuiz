using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using TMPro;
namespace KanjiYomi
{
    /// <summary>
    /// �����Ǘ�����V���O���g���p�^�[���̃N���X
    /// </summary>
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

        //�ꎞ�I�ɖ����i�[����List
        List<QuestionData> randomDatas = new List<QuestionData>();

        List<QuestionData> randomEasyDatas = new List<QuestionData>();//��Փx:Easy���i�[�����f�[�^���X�g
        List<QuestionData> randomNormalDatas = new List<QuestionData>();//��Փx:Normal���i�[�����f�[�^���X�g
        List<QuestionData> randomHardDatas = new List<QuestionData>();//��Փx:Hard���i�[�����f�[�^���X�g
        List<QuestionData> randomHellDatas = new List<QuestionData>();//��Փx:Hell���i�[�����f�[�^���X�g

        //���݂̖��̃f�[�^
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
            switch (count)//�������ɂ�蕪��
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

        /// <summary>
        /// ����̓�Փx�̃��X�g�̐擪���擾���郁�\�b�h
        /// ��肪�Ȃ��Ȃ����ꍇ�͂�����x��������
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="difficulty"></param>
        private void GetRandomData(List<QuestionData> dataList, Difficulty difficulty)
        {
                currentData = dataList.First();
                dataList.RemoveAt(0);
            if (dataList.Count == 0)
            {
                SortDifficultyData(difficulty);
            }
        }

        //�e��Փx�̖��𐶐����郁�\�b�h
        public void CreateQuestionData()
        {
            SortDifficultyData(Difficulty.Easy);
            SortDifficultyData(Difficulty.Normal);
            SortDifficultyData(Difficulty.Hard);
            SortDifficultyData(Difficulty.Hell);
        }

        /// <summary>
        /// �X�N���^�u���A�Z�b�g�̖��W���V���b�t�����e��Փx�̃��X�g�ɑ�����郁�\�b�h
        /// </summary>
        /// <param name="difficulty"></param>
        void SortDifficultyData(Difficulty difficulty)
        {
            randomDatas.Clear();//�����_���f�[�^���X�g����ɂ���
            foreach (QuestionData questionData in quesitonCollectionAsset.questionData)//�w��̓�Փx�̖�肾���������_���f�[�^���X�g�Ɋi�[
            {
                if (questionData.questionDifficulty != difficulty) continue;
                randomDatas.Add(questionData);
            }
            randomDatas = randomDatas.OrderBy(a => Guid.NewGuid()).ToList();// �����_�����X�g�������_���ɕ��בւ���
            switch (difficulty)//�w��̓�Փx�̃��X�g�Ƀ����_�����X�g�̒l����
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
            bool correct = false;
            foreach(string questionString in questionData.Correct)
            {
                if (questionString == inputField.text)
                {
                    correct =true;
                }
            }
            return correct;
            //if (questionData.Correct == inputField.text)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
