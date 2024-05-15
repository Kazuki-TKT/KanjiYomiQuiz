using UnityEngine;
using DG.Tweening;
using TMPro;


namespace KanjiYomi
{
    /// <summary>
    /// ���̓�Փx���ς�����ۂɃe�L�X�g��\������N���X
    /// </summary>
    public class LevelTextGUI : MonoBehaviour
    {
        //�e���x�����Ƃ�TMP
        public TextMeshProUGUI level1Text, level2Text, level3Text, level4Text;
        
        //����
        public float duration;
        
        //���ʉ�
        public AudioClip onTextClip;

        /// <summary>
        /// ���x�����Ƃ̃e�L�X�g��\�����郁�\�b�h
        /// </summary>
        /// <param name="count">������</param>
        public void OnDisplayLevelText(int count)
        {
            switch (count)
            {
                case 0://���x��1
                    Initialize(level1Text);
                    TextAnimation(level1Text);
                    break;
                case 3://���x��2
                    Initialize(level2Text);
                    TextAnimation(level2Text);
                    break;
                case 6://���x��3
                    Initialize(level3Text);
                    TextAnimation(level3Text);
                    break;
                case 9://���x��4(�ŏI���)
                    Initialize(level4Text);
                    TextAnimation(level4Text);
                    break;
            }
            AuidoManager.Instance.PlaySound_SE(onTextClip);
        }

        //���x���e�L�X�g�̃A�j���[�V����
        void TextAnimation(TextMeshProUGUI tmpro)
        {
            Debug.Log("Level");
            // �����Ԋu���J����
            DOTween.To(() => tmpro.characterSpacing, value => tmpro.characterSpacing = value, 10, duration)
                .SetEase(Ease.OutQuart);

            // �t�F�[�h
            DOTween.Sequence()
                .Append(tmpro.DOFade(1, duration / 4))
                .AppendInterval(duration / 2)
                .Append(tmpro.DOFade(0, duration / 4));
        }

        //�e�L�X�g�̏�����
        private void Initialize(TextMeshProUGUI tmpro)
        {
            tmpro.DOFade(0, 0);//������
            tmpro.characterSpacing = -50;//�X�y�[�X���k�߂�
        }
    }
}
