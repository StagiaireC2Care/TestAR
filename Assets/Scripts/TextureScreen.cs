using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScreen : MonoBehaviour
{
    public int mWidth = 256;
    public int mHeight = 256;

    private Camera mCamera;
    private Texture2D cubText;
    public Material cubMaterial;
    // Start is called before the first frame update
    void Start()
    {
        mCamera = Camera.main;
    }

    private void Update()
    {
        cubMaterial.SetTexture("_MainTex", RTImage());
    }

    private Texture2D RTImage()
    {
        Rect rect = new Rect(0, 0, mWidth, mHeight);
        RenderTexture renderTexture = new RenderTexture(mWidth, mHeight, 24);
        Texture2D screenShot = new Texture2D(mWidth, mHeight, TextureFormat.RGBA32, false);

        mCamera.targetTexture = renderTexture;
        mCamera.Render();

        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        mCamera.targetTexture = null;
        RenderTexture.active = null;

        Destroy(renderTexture);
        renderTexture = null;
        return screenShot;
    }
}
