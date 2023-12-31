using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensEffectCameraScript : MonoBehaviour
{
    private LensEffect lensEffect;
    // Start is called before the first frame update
    void Start()
    {
        lensEffect = GetComponent<LensEffect>();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (lensEffect.desiredMode)
        {
            lensEffect.lensMaterial.SetTexture("_mainTex", source);
            Graphics.Blit(source, destination, lensEffect.lensMaterial); //applying post processing
        }
        else
        {
            Graphics.Blit(source, destination); //no post processing gets applied outside of the desired camera mode
        }
    }
}
