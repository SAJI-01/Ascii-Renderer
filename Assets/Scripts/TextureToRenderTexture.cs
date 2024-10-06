using UnityEngine;

public class TextureToRenderTexture : MonoBehaviour
{
    public Texture2D sourceTexture; // Drag your imported texture here
    public RenderTexture renderTexture; // Assign your RenderTexture here

    void Start()
    {
        // Copy the texture to the RenderTexture
        Graphics.Blit(sourceTexture, renderTexture);
    }
}