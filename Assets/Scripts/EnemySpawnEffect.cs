using UnityEngine;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// �����X�^�[��\�����鎞�̉��o�Ȃǂ��s���N���X
    /// </summary>
    public class EnemySpawnEffect : MonoBehaviour
    {
        //�����G�t�F�N�g�̎���
        public float spawnEffectTime = 1;

        //�����G�t�F�N�g
        [SerializeField]
        ParticleSystem ringParticle;
        //�����X�^�[�̃����_�[
        [SerializeField]
        Renderer _renderer;

        //�V�F�[�_�[�v���p�e�B��ID���i�[
        int shaderProperty;

        void Start()
        {
            shaderProperty = Shader.PropertyToID("_cutoff");//CutOff�̃v���p�e�B��ID���擾
        }
        private void OnEnable()
        {
            SpawnEffect();
        }

        /// <summary>
        /// �����X�^�[�������o
        /// </summary>
        void SpawnEffect()
        {
            ringParticle.Play();//�����O�G�t�F�N�g
            DOTween.To(() => 0f, value => _renderer.material.SetFloat(shaderProperty, value), 1f, spawnEffectTime+0.5f).SetEase(Ease.Linear);//�����X�^�[�̃����_�[��CutOff�̒l�����X�ɏグ��

        }
        /// <summary>
        /// �����X�^�[��\�����o
        /// </summary>
        public void DespawnEffect(GameObject monster)
        {
            DOTween.To(() => 1f, value => _renderer.material.SetFloat(shaderProperty, value), 0, spawnEffectTime).SetEase(Ease.Linear).OnComplete(()=>monster.SetActive(false));
            //�����X�^�[�̃����_�[��CutOff�����X��0�ɂ�����A��\���ɂ���
        }
    }
}
