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
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        List<MonsterData> monsterDatas;

        //���݂̃����X�^�[�������f�[�^
        MonsterData currentMonsterData;
        public MonsterData CurrentMonsterData { get => currentMonsterData; }

        //��ʌ���
        public PostProcessVolume postProcessVolume;
        ChromaticAberration chromaticAberrationLayer;

        //�����G�t�F�N�g
        public ParticleSystem particleExprosion,shockWave;
        public ParticleSystem[] particleTinyExprosion;

        //Animator��property
        const string ATTACK = "Attack";
        const string DIE = "Die";

        public AudioClip summonsClip, explosionClip;
        private void Start()
        {
            postProcessVolume.profile.TryGetSettings(out chromaticAberrationLayer);
        }
        public void MonsterSpawnAndAttackAnimation(Difficulty difficulty, CancellationToken token)
        {
            currentMonsterData = GetMonster(difficulty);
            SpawnAndAttackAnimation(currentMonsterData.enemySpawn.spawnEffectTime+0.5f, token).Forget();
        }
        public void MonsterDie(CancellationToken token)
        {
            DieAnimation(currentMonsterData.enemySpawn.spawnEffectTime, token).Forget();
        }

        public void MonsterEscape()
        {
            currentMonsterData.enemySpawn.DespawnEffect(currentMonsterData?.monsterObject);
        }

        async UniTask SpawnAndAttackAnimation(float time,  CancellationToken token)
        {
            try
            {
                currentMonsterData.monsterObject.SetActive(true);//�G��\��
                AuidoManager.Instance.PlaySound_SE(summonsClip);
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);//�\���A�j���[�V�������ԕ��ҋ@
                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(ATTACK);//�A�^�b�N�U��
                shockWave.Play();
                foreach (AudioClip clip in currentMonsterData.screamClip)//SE��炷
                {
                    AuidoManager.Instance.PlaySound_SE(clip);
                }
                DOTween.To(() => chromaticAberrationLayer.intensity.value, x => chromaticAberrationLayer.intensity.value = x, 1, 0.5f).SetEase(Ease.InSine);//�|�X�g�v���Z�X���g�p���Ĕ��͂��o��
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
                DOTween.To(() => chromaticAberrationLayer.intensity.value, x => chromaticAberrationLayer.intensity.value = x, 0, 0.5f)
            .SetEase(Ease.Linear);
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
                foreach(ParticleSystem particle in particleTinyExprosion)
                {
                    particle.Play();
                }

                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                AuidoManager.Instance.PlaySound_SE(explosionClip);//������
                particleExprosion.Play();//����
                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(DIE);//�|���
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


        [System.Serializable]
    public class MonsterData
    {
        public Difficulty monsterDifficulty;
        public GameObject monsterObject;
        public EnemySpawnEffect enemySpawn;
            public AudioClip[] screamClip;
    }
    }
}