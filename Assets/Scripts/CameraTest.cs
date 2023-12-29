  using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ModApi;
    using ModApi.Common;
    using ModApi.Mods;
    using ModApi.Scenes.Events;
    using UnityEngine;
    using Assets.Scripts;
    using Assets.Scripts.Scenes;
    using ModApi.Levels;
    using BeautifyEffect;
    using ModApi.Flight.GameView;
    using Assets.Scripts.Flight;
[ExecuteInEditMode]
public class CameraTest : MonoBehaviour {

    //SOLELY FOR TESTING IN THE EDITOR

    private Material material;
    private Texture2D vignetteTex;

    public float vignetteIntensity;
    public float distortionStrength;
    public float aberrationStrength;

    void Awake ()
    {   
        vignetteTex = Resources.Load<Texture2D>("Sprites/lensVignetteAlpha1");
        material = new Material(Shader.Find("LensShader"));
        material.SetTexture("_vignetteTex", vignetteTex);
    }
    
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {       
        material.SetTexture("_mainTex",source);
        material.SetFloat("_vignetteIntensity", vignetteIntensity);
        material.SetFloat("_distortionStrength",distortionStrength);
        material.SetFloat("_aberrationStrength", aberrationStrength*0.01f);
        Graphics.Blit (source, destination, material);
    }
}