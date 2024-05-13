using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace KanjiYomi
{
    public class EnemySpawnEffect : MonoBehaviour
    {
        public float spawnEffectTime = 1;

        [SerializeField]
        ParticleSystem ringParticle;
        [SerializeField]
        Renderer _renderer;

        int shaderProperty;

        void Start()
        {
            shaderProperty = Shader.PropertyToID("_cutoff");
            //_renderer = GetComponent<Renderer>();

            //ringParticle = GetComponentInChildren<ParticleSystem>();
            //var main = ringParticle.main;
            //main.duration = spawnEffectTime;
        }
        private void OnEnable()
        {
            SpawnEffect();
        }

        void SpawnEffect()
        {
            ringParticle.Play();
            //_renderer.material.DOFloat(1, shaderProperty, spawnEffectTime).SetEase(Ease.Linear);
            DOTween.To(() => 0f, value => _renderer.material.SetFloat(shaderProperty, value), 1f, spawnEffectTime).SetEase(Ease.Linear);

        }

        public void DespawnEffect(GameObject monster)
        {
            DOTween.To(() => 1f, value => _renderer.material.SetFloat(shaderProperty, value), 0, spawnEffectTime).SetEase(Ease.Linear).OnComplete(()=>monster.SetActive(false));
        }
    }
}
