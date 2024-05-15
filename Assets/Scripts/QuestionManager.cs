using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using TMPro;
namespace KanjiYomi
{
    /// <summary>
    /// 問題を管理するシングルトンパターンのクラス
    /// </summary>
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

        //一時的に問題を格納するList
        List<QuestionData> randomDatas = new List<QuestionData>();

        List<QuestionData> randomEasyDatas = new List<QuestionData>();//難易度:Easyが格納されるデータリスト
        List<QuestionData> randomNormalDatas = new List<QuestionData>();//難易度:Normalが格納されるデータリスト
        List<QuestionData> randomHardDatas = new List<QuestionData>();//難易度:Hardが格納されるデータリスト
        List<QuestionData> randomHellDatas = new List<QuestionData>();//難易度:Hellが格納されるデータリスト

        //現在の問題のデータ
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
            switch (count)//正答数により分岐
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
        /// 特定の難易度のリストの先頭を取得するメソッド
        /// 問題がなくなった場合はもう一度生成する
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

        //各難易度の問題を生成するメソッド
        public void CreateQuestionData()
        {
            SortDifficultyData(Difficulty.Easy);
            SortDifficultyData(Difficulty.Normal);
            SortDifficultyData(Difficulty.Hard);
            SortDifficultyData(Difficulty.Hell);
        }

        /// <summary>
        /// スクリタブルアセットの問題集をシャッフルし各難易度のリストに代入するメソッド
        /// </summary>
        /// <param name="difficulty"></param>
        void SortDifficultyData(Difficulty difficulty)
        {
            randomDatas.Clear();//ランダムデータリストを空にする
            foreach (QuestionData questionData in quesitonCollectionAsset.questionData)//指定の難易度の問題だけをランダムデータリストに格納
            {
                if (questionData.questionDifficulty != difficulty) continue;
                randomDatas.Add(questionData);
            }
            randomDatas = randomDatas.OrderBy(a => Guid.NewGuid()).ToList();// ランダムリストをランダムに並べ替える
            switch (difficulty)//指定の難易度のリストにランダムリストの値を代入
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
