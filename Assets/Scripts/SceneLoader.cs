using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KanjiYomi
{
    public class SceneLoader : MonoBehaviour
    {
        //public CanvasGroup fadeCanvasGroup; // �t�F�[�h�p��CanvasGroup
        //public TextMeshProUGUI loadingText; // ���[�f�B���O�e�L�X�g�\���p
        //public Button[] sceneChangeButton;
        //public string sceneName;
        //public GameState gameChangeState;

        //private void Start()
        //{
        //    // ������ԂŃt�F�[�h�L�����o�X�𓧖��ɐݒ�
        //    fadeCanvasGroup.alpha = 0;
        //    foreach(Button button in sceneChangeButton)
        //    {
        //        button.onClick.AddListener(() => LoadSceneAsync(sceneName).Forget());
        //    }
        //}

        //public async UniTaskVoid LoadSceneAsync(string sceneName)
        //{
        //    await FadeOutAsync(); // �t�F�[�h�A�E�g
        //    loadingText.text = "Loading..."; // ���[�f�B���O�e�L�X�g�\��
        //                                     // �V�[����񓯊��Ń��[�h
        //    var sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);

        //    // �V�[���̃��[�h����������܂őҋ@
        //    await sceneLoadOperation;
        //    // �V�[���̃��[�h��̏���
        //    GameManager.Instance.UpdateGameState(gameChangeState);
        //    //await FadeInAsync(); // �t�F�[�h�C��
        //    //loadingText.text = ""; // ���[�f�B���O�e�L�X�g���N���A
        //}

        //private async UniTask FadeOutAsync()
        //{
        //    fadeCanvasGroup.blocksRaycasts = true; // �t�F�[�h����UI�̑�����u���b�N

        //    float fadeDuration = 1f; // �t�F�[�h�̎���
        //    float timer = 0f;

        //    while (timer <= fadeDuration)
        //    {
        //        timer += Time.deltaTime;
        //        fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
        //        await UniTask.Yield(PlayerLoopTiming.Update);
        //    }
        //}

        //private async UniTask FadeInAsync()
        //{
        //    float fadeDuration = 1f; // �t�F�[�h�̎���
        //    float timer = 0f;

        //    while (timer <= fadeDuration)
        //    {
        //        timer += Time.deltaTime;
        //        fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
        //        await UniTask.Yield(PlayerLoopTiming.Update);
        //    }

        //    fadeCanvasGroup.blocksRaycasts = false; // �t�F�[�h�������UI�̑��������
        //}
    }
}
