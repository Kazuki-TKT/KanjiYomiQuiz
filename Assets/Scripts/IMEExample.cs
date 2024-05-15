using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace KanjiYomi
{
    /// <summary>
    /// IME���͂��Ǘ�����N���X
    /// �Q�lURL�ihttps://nekojara.city/unity-input-system-text�j
    /// </summary>
    public class IMEExample : MonoBehaviour
    {
        //�C���v�b�g��
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
            if(GameManager.Instance.currentGameState==GameState.Playing)inputSource.Play();
        }

        // IME�̓��͒��̕�������󂯎��
        private void OnIMECompositionChange(IMECompositionString str)
        {
            if (GameManager.Instance.currentGameState == GameState.Playing) inputSource.Play();
        }
    }
}
