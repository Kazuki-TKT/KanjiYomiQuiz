using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace KanjiYomi
{
    /// <summary>
    /// IME入力を管理するクラス
    /// 参考URL（https://nekojara.city/unity-input-system-text）
    /// </summary>
    public class IMEExample : MonoBehaviour
    {
        //インプット音
        public AudioSource inputSource;
        private void OnEnable()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // キーボードの入力を受け取る
            keyboard.onTextInput += OnTextInput;
            keyboard.onIMECompositionChange += OnIMECompositionChange;
        }


        private void OnDisable()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // キーボード入力の受取り解除
            keyboard.onTextInput -= OnTextInput;
            keyboard.onIMECompositionChange -= OnIMECompositionChange;
        }

        // 入力された文字を文字コード（16進数）と共に表示
        private void OnTextInput(char ch)
        {
            if(GameManager.Instance.currentGameState==GameState.Playing)inputSource.Play();
        }

        // IMEの入力中の文字列を受け取る
        private void OnIMECompositionChange(IMECompositionString str)
        {
            if (GameManager.Instance.currentGameState == GameState.Playing) inputSource.Play();
        }
    }
}
