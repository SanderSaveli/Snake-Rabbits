using UnityEngine;

namespace SanderSaveli.Snake
{
    public class CarrotView : MonoBehaviour
    {
        [SerializeField] private Carrot _carrot;
        [SerializeField] private ParticleSystem _particleSystem;

        private void OnEnable()
        {
            _carrot.OnHealthChange += HandleTakeDamage;
            _carrot.OnKill += HandleKill;
        }

        private void OnDisable()
        {
            _carrot.OnHealthChange -= HandleTakeDamage;
            _carrot.OnKill -= HandleKill;
        }

        private void HandleTakeDamage(int currentHealth)
        {
            _particleSystem.Play();
        }

        private void HandleKill()
        {
            Destroy(gameObject);
        }
    }
}
