using System.Runtime.InteropServices;

// Create struct layout to match memory layout of struct in plugin
[StructLayout(LayoutKind.Sequential), System.Serializable]
public struct FrameData
{
    public int width;
    public int height;
    public int channels;
    public byte[] pixels;
}

public static class CVPlugin
{
    // Import function call from our plugin
    [DllImport("DevGuerillaCVPlugin.dll", EntryPoint = "processFrameCanny")]
    public static extern void ProcessFrame(ref FrameData fd, byte[] outData);
}
