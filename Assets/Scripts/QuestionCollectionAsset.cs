using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiYomi
{
    /// <summary>
    /// 問題制作用のスクリタブルオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "QuestionCollectioData", menuName = "QuestionCollectionAsset")]
    public class QuestionCollectionAsset : ScriptableObject
    {
        public List<QuestionData> questionData = new List<QuestionData>();
    }

    /// <summary>
    /// 難易度
    /// </summary>
    public enum Difficulty
    {
        Easy,//簡単
        Normal,//普通
        Hard,//難しい
        Hell//地獄級
    }

    [System.Serializable]
    public class QuestionData
    {
        /// <summary>
        /// 問題の固有ID
        /// </summary>
        [SerializeField]
        int questionID;

        public int QuestionID { get => questionID; }

        /// <summary>
        /// 問題の難易度
        /// </summary>
        public Difficulty questionDifficulty;

        public string AdjectiveString { get => adjectiveString; }

        /// <summary>
        /// 問題となる漢字
        /// </summary>
        [SerializeField]
        string question;
        public string Question { get => question; }

        /// <summary>
        /// 問題の正解
        /// </summary>
        [SerializeField]
        string[] correct;
        public string[] Correct { get => correct; }

        /// <summary>
        /// 形容詞かどうかの判定
        /// </summary>
        public bool adjective;

        [SerializeField]
        string adjectiveString;
        /// <summary>
        /// 解答の説明
        /// </summary>
        [SerializeField, TextArea]
        string description;
        public string Description { get => description; }

    }


}
