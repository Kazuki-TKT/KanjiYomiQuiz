using UnityEditor;
using UnityEngine;

namespace KanjiYomi
{
    [CustomEditor(typeof(PlayerMissAnswerAnimation))]
    public class PlayerMissAnswerAnimationEditor : Editor
    {
        /// <summary>
        /// Inspector��GUI���X�V
        /// </summary>
        public override void OnInspectorGUI()
        {
            //����Inspector������\��
            base.OnInspectorGUI();

            PlayerMissAnswerAnimation playerMissAnswerAnimation = target as PlayerMissAnswerAnimation;

            //�{�^����\��
            if (GUILayout.Button("�A�j���[�V�����̎��s"))
            {
                playerMissAnswerAnimation.PlayMissAnimation();
            }
        }
    }
}
