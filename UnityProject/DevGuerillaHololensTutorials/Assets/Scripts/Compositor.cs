using System;
using UnityEngine;

public class Compositor : MonoBehaviour
{
    // Buffer to hold processed data
    byte[] processedPixelsBuffer;
    // Buffer to hold raw data
    byte[] rawPixelBufer;
    // Texture for processed data
    Texture2D processedTexture;
   
    bool processingReady = false;
    bool shouldUpdateFrame = false;

    public MeshRenderer visualizeProcessing;
    MaterialPropertyBlock prop;

    bool CreateTexture()
    {
        if (processedTexture)
        {
            return true;
        }
        if (CameraController.Instance && CameraController.Instance.IsPlaying)
        {
            var cc = CameraController.Instance;

            // Prepare texture and buffer
            processedTexture = new Texture2D(cc.Width, cc.Height, TextureFormat.Alpha8, false);
            processedPixelsBuffer = new byte[cc.Width * cc.Height];
            rawPixelBufer = new byte[cc.Width * cc.Height * 4];
            processedTexture.Apply();
            if (visualizeProcessing)
            {
                // Set texture to visualize processed data
                prop = new MaterialPropertyBlock();
                prop.SetTexture("_MainTex", processedTexture);
                visualizeProcessing.SetPropertyBlock(prop);
            }
            return true;
        }
        return false;
    }

    void Update()
    {
        if (processingReady)
        {
            if (shouldUpdateFrame)
            {
                shouldUpdateFrame = false;
                ProcessUpdateFrame();
            }
        }
        else
        {
            if (processingReady = CreateTexture())
            {
                shouldUpdateFrame = true;
            }
        }
    }

    private void ProcessUpdateFrame()
    {
        var cc = CameraController.Instance;
        if (!cc || !cc.IsPlaying)
        {
            Debug.LogError("Something went wrong, CameraController is not valid");
            return;
        }
        Debug.Log("Updating frame");
        var pix = cc.Pixels;
        for (int i = 0; i < pix.Length; i++)
        {
            rawPixelBufer[i * 4] = pix[i].r;
            rawPixelBufer[i * 4 + 1] = pix[i].g;
            rawPixelBufer[i * 4 + 2] = pix[i].b;
            rawPixelBufer[i * 4 + 3] = pix[i].a;
        }
        try
        {
            FrameData fd = new FrameData()
            {
                width = cc.Width,
                height = cc.Height,
                channels = 4, // Color32 struct has 4 channels (r,g,b,a)
                pixels = rawPixelBufer
            };
            // Call plugin function
            CVPlugin.ProcessFrame(ref fd, processedPixelsBuffer);
            // Update texture
            processedTexture.LoadRawTextureData(processedPixelsBuffer);
            processedTexture.Apply();
            shouldUpdateFrame = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error during processing");
            Debug.LogError(e.Message);
        }
        
    }
}
