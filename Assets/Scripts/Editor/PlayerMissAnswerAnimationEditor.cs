using System.Threading;
using UnityEditor;
using UnityEngine;

namespace KanjiYomi
{
    [CustomEditor(typeof(PlayerMissAnswerAnimation))]
    public class PlayerMissAnswerAnimationEditor : Editor
    {
        CancellationTokenSource cts;
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
                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                playerMissAnswerAnimation.PlayMissAnimation(token);
            }
        }
    }
}
