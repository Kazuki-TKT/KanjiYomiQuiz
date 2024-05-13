using UnityEditor;
using UnityEngine;

namespace KanjiYomi
{
    [CustomEditor(typeof(PlayerMissAnswerAnimation))]
    public class PlayerMissAnswerAnimationEditor : Editor
    {
        /// <summary>
        /// InspectorのGUIを更新
        /// </summary>
        public override void OnInspectorGUI()
        {
            //元のInspector部分を表示
            base.OnInspectorGUI();

            PlayerMissAnswerAnimation playerMissAnswerAnimation = target as PlayerMissAnswerAnimation;

            //ボタンを表示
            if (GUILayout.Button("アニメーションの実行"))
            {
                playerMissAnswerAnimation.PlayMissAnimation();
            }
        }
    }
}
