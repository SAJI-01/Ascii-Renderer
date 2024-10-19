
using System.Text;
using Unity.VisualScripting;
using UnityEngine;


public class ASCIIRendererGUI : MonoBehaviour
{
    private const string density = " `.-':_,^=;><+!rc*/z?sLTv)J7(|Fi{C}fI31tlu[neoZ5Yxjya]2ESwqkP6h9d4VpOGbUAKXHm8RD#$Bg0MNWQ%&@";
    
    [Range(10,60)] [SerializeField]
    private int frameRate = 10;
    [SerializeField] private Font font;
    
    public Texture2D image;
    private float charWidth;
    private float charHeight;
    private int startIndex;
    private int charIndex;
    public float sizeF = 1.2f;
    public float dontCreateTextBrightVal = 0.3f;
    [SerializeField] private bool Colored;
    private float imageAspectRatio;
    private float screenAspectRatio;
    private static readonly Vector3 brightnessVector = new Vector3(0.2126f, 0.7152f, 0.0722f);
    private float brightness;
    private Color32[] pixels;
    private Color32 pixel;


    private void Start()
    {
        Application.targetFrameRate = frameRate;
        CalculateCharacterDimensions();
        pixels = image.GetPixels32();
    }

    private void CalculateCharacterDimensions()
    {
        imageAspectRatio = (float)image.width / image.height;
        screenAspectRatio = (float)Screen.width / Screen.height;
        if (imageAspectRatio > screenAspectRatio)
        {
            charWidth = (float)Screen.width / image.width;
            charHeight = charWidth / imageAspectRatio;
        }
        else
        {
            charHeight = (float)Screen.height / image.height;
            charWidth = charHeight * imageAspectRatio;
        }
        PrecomputeCharacterDensityMapping();
    }

    private void PrecomputeCharacterDensityMapping()
    {
        // Precompute and cache character density mapping based on brightness levels
        // for optimized performance.
        // Add implementation here.
        
        
    }

    private GUIStyle style = new GUIStyle();
    void OnGUI()
    {
        
        GUI.backgroundColor = Color.black;
        charIndex = startIndex;

        style.font = font;
        style.fontSize = Mathf.CeilToInt(charWidth * sizeF);
        style.alignment = TextAnchor.MiddleCenter;
       
    
        for (int y = 0; y < image.height; y++)
        {
            var flipY = image.height - y - 1; 
            for (var x = 0; x < image.width; x++)
            {
                pixel = pixels[x + flipY * image.width];
                brightness = Vector3.Dot(new Vector3(pixel.r, pixel.g, pixel.b), brightnessVector);
                if (brightness < dontCreateTextBrightVal)
                {
                    continue;
                }

                style.normal.textColor = Colored ? pixel: new Color(brightness, brightness, brightness);

                var rect = new Rect(x * charWidth, y * charHeight, charWidth, charHeight);
                var charToDisplay = density[charIndex % density.Length];
                GUI.Label(rect, charToDisplay.ToString(), style);
                charIndex++;
            }
        }

        startIndex++; 
    }

}
