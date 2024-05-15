using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// �G�����X�^�[�̓������Ǘ�����N���X
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        //�����X�^�[�̃f�[�^
        [SerializeField]
        List<MonsterData> monsterDatas;

        //���݂̃����X�^�[�������f�[�^
        MonsterData currentMonsterData;
        public MonsterData CurrentMonsterData { get => currentMonsterData; }

        //��ʌ���
        public PostProcessVolume postProcessVolume;
        ChromaticAberration chromaticAberrationLayer;

        //�����G�t�F�N�g�ƃV���b�N�E�F�[�u�G�t�F�N�g
        public ParticleSystem particleExprosion, shockWave;
        public ParticleSystem[] particleTinyExprosion;

        //Animator��property
        const string ATTACK = "Attack";
        const string DIE = "Die";

        //�������Ɣ�������SE
        public AudioClip summonsClip, explosionClip;

        private void Start()
        {
            postProcessVolume.profile.TryGetSettings(out chromaticAberrationLayer);
        }

        /// <summary>
        /// �����X�^�[������������U�����s�����\�b�h
        /// </summary>
        /// <param name="difficulty">���̓�Փx</param>
        public void MonsterSpawnAndAttack(Difficulty difficulty, CancellationToken token)
        {
            currentMonsterData = GetMonster(difficulty);//���݂̃����X�^�[���擾�����
            SpawnAndAttackAnimation(currentMonsterData.enemySpawn.spawnEffectTime + 0.5f, token).Forget();
        }

        /// <summary>
        /// �����X�^�[���S���ɍs�����\�b�h
        /// </summary>
        public void MonsterDie(CancellationToken token)
        {
            DieAnimation(currentMonsterData.enemySpawn.spawnEffectTime, token).Forget();
        }

        /// <summary>
        /// �����X�^�[���P�ނ������ɍs�����\�b�h
        /// </summary>
        public void MonsterEscape()
        {
            currentMonsterData.enemySpawn.DespawnEffect(currentMonsterData?.monsterObject);
        }

        /// <summary>
        /// �����X�^�[���������A�U�����s���A�j���[�V�������\�b�h
        /// </summary>
        /// <returns></returns>
        async UniTask SpawnAndAttackAnimation(float time, CancellationToken token)
        {
            try
            {
                currentMonsterData.monsterObject.SetActive(true);//�G��\��
                AuidoManager.Instance.PlaySound_SE(summonsClip);//����SE

                //--�\���A�j���[�V�������ԕ��ҋ@
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);

                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(ATTACK);//�A�^�b�N�U��
                shockWave.Play();//�V���b�N�E�F�[�u�̃p�[�e�B�N��
                foreach (AudioClip clip in currentMonsterData.screamClip)//SE��炷
                {
                    AuidoManager.Instance.PlaySound_SE(clip);
                }
                _ = DOTween.To(() => chromaticAberrationLayer.intensity.value, x => chromaticAberrationLayer.intensity.value = x, 1, 0.5f).SetEase(Ease.InSine);//ChromaticAberrationLayer���g�p���Ĕ��͂��o��

                //--1�b�ҋ@
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);

                _ = DOTween.To(() => chromaticAberrationLayer.intensity.value, x => chromaticAberrationLayer.intensity.value = x, 0, 0.5f)
            .SetEase(Ease.Linear);//ChromaticAberrationLayer�̒l�����ɖ߂�
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("�G�̍s�����L�����Z������܂����B");
            }
            catch (Exception ex)
            {
                Debug.LogError($"�G���[���������܂���: {ex.Message}");
            }
        }

        async UniTask DieAnimation(float time, CancellationToken token)
        {
            try
            {
                PlayerController.Instance.PlayerAttack(1);//�v���C���[�̍U��
                foreach (ParticleSystem particle in particleTinyExprosion)//�U�����q�b�g���鉉�o
                {
                    particle.Play();
                }

                //--1�b�ҋ@
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

                AuidoManager.Instance.PlaySound_SE(explosionClip);//������
                particleExprosion.Play();//�����G�t�F�N�g
                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(DIE);//�����X�^�[���|���A�j���[�V����

                //--�|���܂Ŏw��b�ҋ@
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);

                currentMonsterData.enemySpawn.DespawnEffect(currentMonsterData?.monsterObject);//�G���\��
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("�G�̍s�����L�����Z������܂����B");
            }
            catch (Exception ex)
            {
                Debug.LogError($"�G���[���������܂���: {ex.Message}");
            }
        }

        //MonstaerData����I�񂾃����X�^�[��Ԃ����\�b�h
        MonsterData GetMonster(Difficulty difficulty)
        {
            var monsterData = monsterDatas.Find(monsterData => monsterData.monsterDifficulty == difficulty);
            return monsterData;
        }


        /// <summary>
        /// �����X�^�[�̏����܂Ƃ߂��N���X
        /// </summary>
        [System.Serializable]
        public class MonsterData
        {
            public Difficulty monsterDifficulty;//�����X�^�[�̓�Փx
            public GameObject monsterObject;//�����X�^�[�̃I�u�W�F�N�g
            public EnemySpawnEffect enemySpawn;//�����X�^�[�̏����G�t�F�N�g���i��N���X
            public AudioClip[] screamClip;//�����X�^�[���o����
        }
    }
}