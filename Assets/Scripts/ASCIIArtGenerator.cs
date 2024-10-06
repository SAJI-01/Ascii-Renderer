using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use this if you are using TextMeshPro
using System.Text;

public class ASCIIArtGenerator : MonoBehaviour
{
    public TextMeshProUGUI asciiText; // For TextMeshPro
    // or 
    // public Text asciiText; // For Unity's default Text
    private WebCamTexture webCamTexture;
    private const string density = "Ñ@#W$9876543210?!abc;:+=-,._          "; // Define your density string
    private int width = 64; // Width of the ASCII image
    private int height = 48; // Height of the ASCII image
    private float frameInterval = 0.1f; // Process every 0.1 seconds
    private float nextFrameTime = 0;

    void Start()
    {
        webCamTexture = new WebCamTexture();
        asciiText.text = ""; // Initialize the text
        webCamTexture.Play();
    }

    void Update()
    {
        if (webCamTexture.width > 100) 
        {
            if (Time.time >= nextFrameTime) 
            {
                nextFrameTime = Time.time + frameInterval;
                ProcessFrame();
            }
        }
    }

    void ProcessFrame()
    {
        Color32[] pixels = webCamTexture.GetPixels32();
        StringBuilder asciiImage = new StringBuilder();

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Color32 pixelColor = pixels[i + j * webCamTexture.width];
                float avg = (pixelColor.r + pixelColor.g + pixelColor.b) / 3f;
                int charIndex = Mathf.FloorToInt(Mathf.Lerp(0, density.Length, avg / 255f));
                char c = density[charIndex]; // Get the character from the density string
                if (c == ' ') 
                {
                    asciiImage.Append("&nbsp;"); // Append non-breaking space for Unity
                } 
                else 
                {
                    asciiImage.Append(c); // Append the character
                }

            }
            asciiImage.AppendLine();
        }

        asciiText.text = asciiImage.ToString(); // Update text
        //asciiText.ForceMeshUpdate(); // Force an update of the mesh to reflect changes
    }

}