using System.Collections;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [Header("Night Mode Settings")]
    [Tooltip("The main directional light (usually the sun)")]
    public Light directionalLight;
    
    [Tooltip("Target light intensity for night mode (0 = completely dark)")]
    [Range(0f, 1f)]
    public float nightLightIntensity = 0.2f;
    
    [Tooltip("Color for the directional light at night (usually bluish)")]
    public Color nightLightColor = new Color(0.1f, 0.1f, 0.3f);
    
    [Tooltip("Transition duration in seconds")]
    public float transitionDuration = 1.5f;
    
    [Tooltip("Animation curve for the transition")]
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Environment Settings")]
    [Tooltip("Target ambient light color for night")]
    public Color nightAmbientColor = new Color(0.05f, 0.05f, 0.1f);
    
    [Tooltip("Night sky color")]
    public Color nightSkyColor = new Color(0.02f, 0.02f, 0.05f);
    
    [Tooltip("Night fog color")]
    public Color nightFogColor = new Color(0.02f, 0.02f, 0.05f);
    
    [Tooltip("Increase fog density at night")]
    public bool increaseFogAtNight = true;
    
    [Tooltip("Night fog density")]
    [Range(0f, 0.1f)]
    public float nightFogDensity = 0.03f;
    
    // Original values storage
    private float originalLightIntensity;
    private Color originalLightColor;
    private Color originalAmbientColor;
    private Color originalSkyColor;
    private Color originalFogColor;
    private float originalFogDensity;
    private bool isTransitioning = false;
    private bool isNightMode = false;
    
    private void Awake()
    {
        // Find directional light if not assigned
        if (directionalLight == null)
        {
            // Try to find the main directional light in the scene
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    break;
                }
            }
            
            if (directionalLight == null)
            {
                Debug.LogWarning("LightManager: No directional light assigned or found in the scene.");
            }
        }
        
        // Store original values
        if (directionalLight != null)
        {
            originalLightIntensity = directionalLight.intensity;
            originalLightColor = directionalLight.color;
        }
        
        originalAmbientColor = RenderSettings.ambientLight;
        originalSkyColor = RenderSettings.skybox != null ? RenderSettings.skybox.GetColor("_SkyTint") : Color.blue;
        originalFogColor = RenderSettings.fogColor;
        originalFogDensity = RenderSettings.fogDensity;
    }
    
    /// <summary>
    /// Switch to night mode by dimming the directional light and adjusting environment settings
    /// </summary>
    public void DimLights()
    {
        if (!isTransitioning && !isNightMode)
        {
            StartCoroutine(TransitionToNight());
        }
    }
    
    /// <summary>
    /// Switch back to day mode by restoring original lighting
    /// </summary>
    public void RestoreLights()
    {
        if (!isTransitioning && isNightMode)
        {
            StartCoroutine(TransitionToDay());
        }
    }
    
    /// <summary>
    /// Toggle between night and day modes
    /// </summary>
    public void ToggleLights()
    {
        if (!isTransitioning)
        {
            if (isNightMode)
                RestoreLights();
            else
                DimLights();
        }
    }
    
    private IEnumerator TransitionToNight()
    {
        isTransitioning = true;
        float elapsedTime = 0f;
        
        // Starting values
        float startLightIntensity = directionalLight != null ? directionalLight.intensity : 0f;
        Color startLightColor = directionalLight != null ? directionalLight.color : Color.white;
        Color startAmbientColor = RenderSettings.ambientLight;
        Color startSkyColor = RenderSettings.skybox != null ? RenderSettings.skybox.GetColor("_SkyTint") : Color.blue;
        Color startFogColor = RenderSettings.fogColor;
        float startFogDensity = RenderSettings.fogDensity;
        
        // Remember original fog state
        bool fogWasEnabled = RenderSettings.fog;
        
        // Enable fog if it should be increased at night
        if (increaseFogAtNight)
        {
            RenderSettings.fog = true;
        }
        
        // Gradual transition
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float curveValue = transitionCurve.Evaluate(t);
            
            // Update directional light
            if (directionalLight != null)
            {
                directionalLight.intensity = Mathf.Lerp(startLightIntensity, nightLightIntensity, curveValue);
                directionalLight.color = Color.Lerp(startLightColor, nightLightColor, curveValue);
            }
            
            // Update environment settings
            RenderSettings.ambientLight = Color.Lerp(startAmbientColor, nightAmbientColor, curveValue);
            
            if (RenderSettings.skybox != null)
            {
                RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(startSkyColor, nightSkyColor, curveValue));
            }
            
            RenderSettings.fogColor = Color.Lerp(startFogColor, nightFogColor, curveValue);
            
            if (increaseFogAtNight)
            {
                RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, nightFogDensity, curveValue);
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure final values are set precisely
        if (directionalLight != null)
        {
            directionalLight.intensity = nightLightIntensity;
            directionalLight.color = nightLightColor;
        }
        
        RenderSettings.ambientLight = nightAmbientColor;
        
        if (RenderSettings.skybox != null)
        {
            RenderSettings.skybox.SetColor("_SkyTint", nightSkyColor);
        }
        
        RenderSettings.fogColor = nightFogColor;
        
        if (increaseFogAtNight)
        {
            RenderSettings.fogDensity = nightFogDensity;
        }
        
        isTransitioning = false;
        isNightMode = true;
    }
    
    private IEnumerator TransitionToDay()
    {
        isTransitioning = true;
        float elapsedTime = 0f;
        
        // Starting values
        float startLightIntensity = directionalLight != null ? directionalLight.intensity : 0f;
        Color startLightColor = directionalLight != null ? directionalLight.color : Color.white;
        Color startAmbientColor = RenderSettings.ambientLight;
        Color startSkyColor = RenderSettings.skybox != null ? RenderSettings.skybox.GetColor("_SkyTint") : Color.blue;
        Color startFogColor = RenderSettings.fogColor;
        float startFogDensity = RenderSettings.fogDensity;
        
        // Gradual transition
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float curveValue = transitionCurve.Evaluate(t);
            
            // Update directional light
            if (directionalLight != null)
            {
                directionalLight.intensity = Mathf.Lerp(startLightIntensity, originalLightIntensity, curveValue);
                directionalLight.color = Color.Lerp(startLightColor, originalLightColor, curveValue);
            }
            
            // Update environment settings
            RenderSettings.ambientLight = Color.Lerp(startAmbientColor, originalAmbientColor, curveValue);
            
            if (RenderSettings.skybox != null)
            {
                RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(startSkyColor, originalSkyColor, curveValue));
            }
            
            RenderSettings.fogColor = Color.Lerp(startFogColor, originalFogColor, curveValue);
            RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, originalFogDensity, curveValue);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure final values are set precisely
        if (directionalLight != null)
        {
            directionalLight.intensity = originalLightIntensity;
            directionalLight.color = originalLightColor;
        }
        
        RenderSettings.ambientLight = originalAmbientColor;
        
        if (RenderSettings.skybox != null)
        {
            RenderSettings.skybox.SetColor("_SkyTint", originalSkyColor);
        }
        
        RenderSettings.fogColor = originalFogColor;
        RenderSettings.fogDensity = originalFogDensity;
        
        isTransitioning = false;
        isNightMode = false;
    }
}