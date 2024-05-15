using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// �V�[���ړ��p�̃V���O���g���p�^�[���̃N���X
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;

        // �t�F�[�h�p��CanvasGroup
        public CanvasGroup fadeCanvasGroup;

        // ���[�f�B���O�e�L�X�g�\���p
        TextMeshProUGUI loadingText;

        //Fade���I��������ǂ����̐^�U�n
        public bool fadeOut;

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

        private void Start()
        {
            //������
            fadeCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
            fadeCanvasGroup.alpha = 0;
            loadingText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        public async UniTaskVoid LoadSceneAsync(string sceneName,GameState gameState)
        {
            
            await FadeOutAsync(1); // �t�F�[�h�A�E�g
            loadingText.text = "Loading..."; // ���[�f�B���O�e�L�X�g�\��
            var sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);// �V�[����񓯊��Ń��[�h
            await sceneLoadOperation;// �V�[���̃��[�h����������܂őҋ@
            await FadeInAsync(1);// �t�F�[�h�C��
            GameManager.Instance.UpdateGameState(gameState);// �w��̃X�e�[�g�Ƀ`�F���W
        }

        private async UniTask FadeOutAsync(float duration)
        {
            fadeCanvasGroup.blocksRaycasts = true; // �t�F�[�h����UI�̑�����u���b�N

            //--FadeOut����������܂őҋ@
            await fadeCanvasGroup.DOFade(1, duration).SetEase(Ease.Linear);
        }

        private async UniTask FadeInAsync(float duration)
        {
            //--FadeIn����������܂őҋ@
            await fadeCanvasGroup.DOFade(0, duration).SetEase(Ease.Linear);

            fadeCanvasGroup.blocksRaycasts = false; // �t�F�[�h�������UI�̑��������

        }
    }
}
