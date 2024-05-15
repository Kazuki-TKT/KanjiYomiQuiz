using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KanjiYomi
{
    /// <summary>
    /// ��萧��p�̃X�N���^�u���I�u�W�F�N�g
    /// </summary>
    [CreateAssetMenu(fileName = "QuestionCollectioData", menuName = "QuestionCollectionAsset")]
    public class QuestionCollectionAsset : ScriptableObject
    {
        public List<QuestionData> questionData = new List<QuestionData>();
    }

    /// <summary>
    /// ��Փx
    /// </summary>
    public enum Difficulty
    {
        Easy,//�ȒP
        Normal,//����
        Hard,//���
        Hell//�n����
    }

    [System.Serializable]
    public class QuestionData
    {
        /// <summary>
        /// ���̌ŗLID
        /// </summary>
        [SerializeField]
        int questionID;

        public int QuestionID { get => questionID; }

        /// <summary>
        /// ���̓�Փx
        /// </summary>
        public Difficulty questionDifficulty;

        public string AdjectiveString { get => adjectiveString; }

        /// <summary>
        /// ���ƂȂ銿��
        /// </summary>
        [SerializeField]
        string question;
        public string Question { get => question; }

        /// <summary>
        /// ���̐���
        /// </summary>
        [SerializeField]
        string[] correct;
        public string[] Correct { get => correct; }

        /// <summary>
        /// �`�e�����ǂ����̔���
        /// </summary>
        public bool adjective;

        [SerializeField]
        string adjectiveString;
        /// <summary>
        /// �𓚂̐���
        /// </summary>
        [SerializeField, TextArea]
        string description;
        public string Description { get => description; }

    }


}
