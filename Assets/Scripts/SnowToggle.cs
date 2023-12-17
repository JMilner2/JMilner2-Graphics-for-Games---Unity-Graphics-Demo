using UnityEngine;

public class SnowToggle : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] ParticleSystem snowParticleSystem;
    [SerializeField] ParticleSystem rainParticleSystem;
    public float activationHeight = 500f;

    private bool isSnowActive = false;
    private bool isRainActive = false;

    private void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform not assigned in the inspector!");
            return;
        }

        if (snowParticleSystem == null)
        {
            Debug.LogError("Snow particle system not assigned in the inspector!");
            return;
        }

        snowParticleSystem.gameObject.SetActive(false);
        snowParticleSystem.Stop();
        rainParticleSystem.gameObject.SetActive(false);
        rainParticleSystem.Stop();
        snowParticleSystem.gameObject.SetActive(true);
        rainParticleSystem.gameObject.SetActive(true);
        CheckCameraHeight();
    }

    private void Update()
    {
        if (cameraTransform == null || snowParticleSystem == null || rainParticleSystem == null)
        {
            return;
        }

        CheckCameraHeight();
    }

    private void CheckCameraHeight()
    {
        if (cameraTransform.position.y > activationHeight && !isSnowActive && !isRainActive)
        {
            SetSnowActive(true);
            SetRainActive(false);
        }
        else if (cameraTransform.position.y <= activationHeight && isSnowActive && !isRainActive)
        {
            SetSnowActive(false);
            SetRainActive(true);
        }
    }

    private void SetSnowActive(bool active)
    {
        var emission = snowParticleSystem.emission;
        emission.enabled = active;
        isSnowActive = active;
    }

    private void SetRainActive(bool active)
    {
        var emission = rainParticleSystem.emission;
        emission.enabled = active;
        isRainActive = active;
    }
}
