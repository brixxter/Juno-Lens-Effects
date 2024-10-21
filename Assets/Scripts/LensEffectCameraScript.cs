using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensEffectCameraScript : MonoBehaviour
{
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (LensEffects.Instance.desiredMode)
        {
            LensEffects.Instance.lensMaterial.SetTexture("_mainTex", source);
            Graphics.Blit(source, destination, LensEffects.Instance.lensMaterial); //applying post processing
        }
        else
        {
            Graphics.Blit(source, destination); //no post processing gets applied outside of the desired camera mode
        }
    }
}
