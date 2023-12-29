
using System;
using System.Collections;
using System.Linq;
using System.Text;
using ModApi;
using ModApi.Mods;
using UnityEngine;
using Assets.Scripts;
using BeautifyEffect;
using Assets.Scripts.Flight.GameView.Cameras;

using Assets.Scripts.Flight;
using Object = System.Object;

public class LensEffect : MonoBehaviour
{
    private Beautify beautifyPlugin;
    public Material lensMaterial;
    private Texture2D dirtTex;
    private Texture2D vignetteTex;
    private ModSettings.vignettes currentVignette;
    public bool desiredMode;
    private Assets.Scripts.Flight.GameView.Cameras.CameraMode currentMode;
    private LensEffectCameraScript lensScript;

    void Awake()
    {
        ModSettings.Instance.Changed += SettingChanged;
        FlightSceneScript.Instance.ViewManager.GameView.CameraControllerManager.CameraModeChanged += OnCameraModeChanged;
        Game.Instance.FlightScene.ViewManager.MapViewManager.ForegroundStateChanged += OnMapViewChanged;

        beautifyPlugin = CameraManagerScript.Instance.ImageEffects.Beautify;

        lensMaterial = new Material(Mod.Instance.ResourceLoader.LoadAsset<Shader>("LensShader"));
        if (lensMaterial == null) Debug.LogError("Lens Shader failed to load");

        dirtTex = Mod.Instance.ResourceLoader.LoadAsset<Texture2D>("lensDirt");
        if (dirtTex == null) Debug.LogError("dirtTex failed to load");
    
        lensMaterial.SetTexture("_dirtTex", dirtTex);
       
        currentVignette = ModSettings.Instance.vignetteType;

        desiredMode = ModSettings.Instance.allViewModes;

        StartCoroutine(AddScriptDelay());

        ReloadSettings();
    }

    void ReloadSettings()
    {
        beautifyPlugin.lensDirtTexture = dirtTex;
        beautifyPlugin.lensDirtIntensity = ModSettings.Instance.lensDirtIntensity;
        beautifyPlugin.lensDirt = desiredMode;

        currentVignette = ModSettings.Instance.vignetteType;

        if (currentVignette == ModSettings.vignettes.ellipse)
        {
            vignetteTex = Mod.Instance.ResourceLoader.LoadAsset<Texture2D>("lensVignette1");
            if (vignetteTex == null) Debug.LogError("Ellipse vignette failed to load");
        }
        else if (currentVignette == ModSettings.vignettes.engineeringCam)
        {
            vignetteTex = Mod.Instance.ResourceLoader.LoadAsset<Texture2D>("lensVignette2");
            if (vignetteTex == null) Debug.LogError("Engineering cam vignette failed to load");
        }

        lensMaterial.SetTexture("_vignetteTex", vignetteTex);
        lensMaterial.SetFloat("_vignetteIntensity", ModSettings.Instance.vignetteIntensity);
        lensMaterial.SetFloat("_aberrationStrength", ModSettings.Instance.aberrationStrength*0.01f);
    }

    private void SettingChanged(Object sender, EventArgs e)
    {
        if (currentMode == null)
        {
            desiredMode = ModSettings.Instance.allViewModes;
        }
        else
        {
            modeCheck();
        }
        ReloadSettings();
    }

    private void OnCameraModeChanged(Assets.Scripts.Flight.GameView.Cameras.CameraMode newMode, Assets.Scripts.Flight.GameView.Cameras.CameraMode oldMode)
    {
        currentMode = newMode;
        modeCheck();
        CameraManagerScript.Instance.ImageEffects.Beautify.lensDirt = desiredMode;
        CameraManagerScript.Instance.ImageEffects.Beautify.lensDirtIntensity = ModSettings.Instance.lensDirtIntensity;
    }

    private void OnMapViewChanged(bool foreground) //prevents the dirt texture from resetting back to the default
    {
        if (!foreground) beautifyPlugin.lensDirtTexture = dirtTex;
    }

    private void modeCheck()
    {
        if (!ModSettings.Instance.allViewModes)
        {
            desiredMode = currentMode.Name == "Camera" && currentMode.NightVision.a == 1;
        }
        else
        {
            desiredMode = currentMode.NightVision.a == 1;
        }

        lensScript.enabled = desiredMode;
    }

    IEnumerator AddScriptDelay() //ensures the post processing script is added to the camera last
    {
        yield return new WaitForSeconds(0.1f);
        lensScript = this.gameObject.AddComponent<LensEffectCameraScript>();
    }
}