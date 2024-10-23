  using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Assets.Scripts;
    using Assets.Scripts.Scenes;
    using ModApi.Levels;
    using BeautifyEffect;
    using Assets.Scripts.Flight;
[ExecuteInEditMode]
public class CameraTest : MonoBehaviour {

    //SOLELY FOR TESTING IN THE EDITOR

    private Material material;
    public float vignetteRadius;
    public float vignetteFeather;
    public int vignetteMode;
    public float aberrationStrength;
  

    void Awake ()
    {   
        material = new Material(Shader.Find("LensShader"));
    }
    
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {       
        material.SetTexture("_mainTex",source);
        material.SetFloat("_vignetteRadius", vignetteRadius);
        material.SetFloat("_vignetteFeather", vignetteFeather);
        material.SetFloat("_aberrationStrength", aberrationStrength*0.01f);
        material.SetInt("_useNoise", 1);
        material.SetInt("_offset", 500*(int)UnityEngine.Random.Range(0, 10000));
        
        if (vignetteMode == 0)
        {
            material.SetFloat("_aspectRatio", 1);
        }
        else if (vignetteMode > 0)
        {
            material.SetFloat("_aspectRatio", gameObject.GetComponent<Camera>().aspect);
        }

        material.SetInt("_vignetteMode", vignetteMode);
        Graphics.Blit (source, destination, material);
    }
}