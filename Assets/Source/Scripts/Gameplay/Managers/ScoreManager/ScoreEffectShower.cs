using SanderSaveli.UDK.Tools;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ScoreEffectShower : MonoBehaviour, IScoreEffectShower
    {
        [Header("Prefab")]
        [SerializeField] private ScoreEffect _scorePrefab;

        [Header("Components")]
        [SerializeField] private Transform _scoreParent;

        private InjectionObjectPool<ScoreEffect> _pool;

        private DiContainer _container;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }

        private void Awake()
        {
            _pool = new InjectionObjectPool<ScoreEffect>(_container, _scorePrefab, _scoreParent);
        }

        public void ShowAndAddScore(int score, Vector3 position)
        {
            _pool.Get().ShowScore(score, position);
        }
    }
}
