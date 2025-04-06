using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight; 
    [SerializeField] private Material skyboxMaterial; 
    
    [SerializeField] private float nightLightIntensity = 0.2f; 
    [SerializeField] private float nightSkyboxIntensity = 0.3f; 
    [SerializeField] private float transitionDuration = 3.0f; 
    
    private float originalLightIntensity;
    private float originalSkyboxIntensity;


    private void Start()
    {

        if (directionalLight == null)
        {
            Debug.LogError("No directional light assigned!");
            return;
        }
        
  
        if (skyboxMaterial == null)
        {
            skyboxMaterial = RenderSettings.skybox;
        }
        

        originalLightIntensity = directionalLight.intensity;
        originalSkyboxIntensity = skyboxMaterial.GetFloat("_Exposure");
        
        
    }

public void MakeNightTime(bool instant = false)
    {
        if (instant)
        {
            ApplyNightSettings();
        }
        else
        {
            StartCoroutine(TransitionToNight());
        }
    }
    
    private IEnumerator TransitionToNight()
    {
        float elapsedTime = 0;
        
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            
            directionalLight.intensity = Mathf.Lerp(originalLightIntensity, nightLightIntensity, t);
            
            skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(originalSkyboxIntensity, nightSkyboxIntensity, t));
            
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ApplyNightSettings();
    }
    
    private void ApplyNightSettings()
    {
        RenderSettings.ambientIntensity = 0f; 
        RenderSettings.reflectionIntensity = 0f; 
        directionalLight.intensity = nightLightIntensity;
        skyboxMaterial.SetFloat("_Exposure", nightSkyboxIntensity);
    }
}