using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace KanjiYomi
{
    public class IMEExample : MonoBehaviour
    {
        public AudioSource inputSource;
        private void OnEnable()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // �L�[�{�[�h�̓��͂��󂯎��
            keyboard.onTextInput += OnTextInput;
            keyboard.onIMECompositionChange += OnIMECompositionChange;
        }


        private void OnDisable()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // �L�[�{�[�h���͂̎������
            keyboard.onTextInput -= OnTextInput;
            keyboard.onIMECompositionChange -= OnIMECompositionChange;
        }

        // ���͂��ꂽ�����𕶎��R�[�h�i16�i���j�Ƌ��ɕ\��
        private void OnTextInput(char ch)
        {
            // print($"OnTextInput: {ch}({(int)ch:X02})");
            inputSource.Play();
        }

        // IME�̓��͒��̕�������󂯎��
        private void OnIMECompositionChange(IMECompositionString str)
        {
            // print($"OnIMECompositionChange: {str}");
            inputSource.Play();
        }
    }
}
