using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    WebCamTexture webcamTexture;
    // color buffer to reduce GC overhead
    Color32[] buffer = null;


    MaterialPropertyBlock prop;
    public MeshRenderer visualizeCapture;

    // Basic getters, should be self explanatory
    public bool IsPlaying
    {
        get
        {
            return webcamTexture && webcamTexture.isPlaying;
        }
    }

    public int Width
    {
        get
        {
            return 896;
        }
    }

    public int Height
    {
        get
        {
            return 504;
        }
    }

    public Color32[] Pixels {
        get
        {
            if (webcamTexture)
                webcamTexture.GetPixels32(buffer);
            return buffer;
        }
    }

    public static CameraController Instance 
    {
        get; private set;
    }

    void CreateTexture()
    {
        // Create Unity webcam texture
        webcamTexture = new WebCamTexture(Width, Height, 30);
        // Init buffer
        buffer = new Color32[Width * Height];
        // Start camera stream
        webcamTexture.Play();
        if (visualizeCapture)
        {
            prop = new MaterialPropertyBlock();
            prop.SetTexture("_MainTex", webcamTexture);
            visualizeCapture.SetPropertyBlock(prop);
        }
    }

    IEnumerator Start()
    {
        if (Instance)
        {
            Destroy(this);
            yield break;
        }
        Instance = this;
        FindWebCams();

        // Let user agree to use the webcam first
        // This call 
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("webcam found");
            CreateTexture();
        }
        else
        {
            Debug.Log("webcam not found");
        }
    }

    void FindWebCams()
    {
        // Iterate over available devices
        foreach (var device in WebCamTexture.devices)
        {
            Debug.Log("Name: " + device.name);
        }
    }

    private void OnDestroy()
    {
        if (webcamTexture)
        {
            webcamTexture.Stop();
            Destroy(webcamTexture);
            buffer = null;
            Instance = null;
        }
    }
}
