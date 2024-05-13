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
        /// 問題集のスクリタブルオブジェクト
        /// </summary>
        [SerializeField]
        [Header("問題集のスクリタブルオブジェクト")]
        QuestionCollectionAsset quesitonCollectionAsset;

        /// <summary>
        /// 1ゲームで使用する問題のリスト
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
        /// 問題を取得するメソッド
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
            // リストをランダムに並べ替えて、先頭から指定カウントまでquestionDatasに追加
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
        /// 入力値が正解かどうか判定し、bool型で返す
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
