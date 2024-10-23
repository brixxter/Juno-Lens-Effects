using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class LensEffectCameraScript : MonoBehaviour
{
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {     
            LensEffects.Instance.lensMaterial.SetTexture("_mainTex", source);
            LensEffects.Instance.lensMaterial.SetInt("_offset", 500*(int)UnityEngine.Random.Range(0, 10000));
            Graphics.Blit(source, destination, LensEffects.Instance.lensMaterial); //applying post processing
    }
}
