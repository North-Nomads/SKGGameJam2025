using System.Collections;
using HighVoltage.Infrastructure.Sentry;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.HighVoltage.Scripts.UI.Elements
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject healthOwner;
        [SerializeField] private Image fillBar;
        [SerializeField] private Color healthBarDefaultColor = Color.red;
        [SerializeField] private Color healthBarJustHitColor = Color.white;
        [SerializeField] private float animationTime = 0.2f;
        private Coroutine _currentAnimation;
        private float _targetFill;

        private void Awake()
        {
            IHealthOwner owner = healthOwner.GetComponent<IHealthOwner>();
            if (owner == null)
            {
                Debug.LogError($"{name} waited for {healthOwner.name} to have a IHealthOwner but owner is null");
                return;
            }

            owner.NotifyHealthBar += UpdateHealthBar;
        }

        private void UpdateHealthBar(object sender, float healthLeftFraction)
        {
            // Stop any existing animation to prevent overlapping
            if (_currentAnimation != null)
            {
                fillBar.fillAmount = _targetFill;
                StopCoroutine(_currentAnimation);
            }
        
            _currentAnimation = StartCoroutine(AnimateHealthBar(healthLeftFraction));
        }

        private IEnumerator AnimateHealthBar(float targetFill)
        {
            _targetFill = targetFill;
            float initialFill = fillBar.fillAmount;
            float elapsedTime = 0f;
        
            // Immediately set to "just hit" color at the start
            fillBar.color = healthBarJustHitColor;

            while (elapsedTime < animationTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / animationTime);
            
                // Lerp the fill amount
                fillBar.fillAmount = Mathf.Lerp(initialFill, targetFill, t);
            
                // Lerp the color
                fillBar.color = Color.Lerp(healthBarJustHitColor, healthBarDefaultColor, t);
            
                yield return null;
            }

            // Ensure final values are set exactly
            fillBar.fillAmount = targetFill;
            fillBar.color = healthBarDefaultColor;
        
            _currentAnimation = null;
        }
    }
}