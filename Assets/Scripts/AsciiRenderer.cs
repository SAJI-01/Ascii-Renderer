using TMPro;
using UnityEngine;

public class AsciiRenderer : MonoBehaviour
{
    private const string Density = " `.-':_,^=;><+!rc*/z?sLTv)J7(|Fi{C}fI31tlu[neoZ5Yxjya]2ESwqkP6h9d4VpOGbUAKXHm8RD#$Bg0MNWQ%&@";

    public Texture2D image;
    public TMP_FontAsset font;
    public Transform parentAsciiObject;
    public float updateSpeed = 0.05f;
    public float sizeF = 1.2f;
    public float dontCreateTextBrightVal = 0.3f;

    private float _timer;
    private float _charWidth;
    private float _charHeight;
    
    private int _startIndex;
    private int _charIndex;

    public enum TextColorType
    {
        Colored,
        Grayscale
    }
    public TextColorType textType;

    

    private void Start()
    {
        //Application.targetFrameRate = 10;
        var cameraMain = Camera.main;
        if (cameraMain != null)
        {
            cameraMain.orthographicSize = Screen.height / 2f;
            cameraMain.transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, -10f);
        }

        _timer = 0f;
        CreateAsciiArt();
    }

    private void Update()
    {
        
        _timer += Time.deltaTime;
        if (_timer >= updateSpeed)
        {
            CreateAsciiArt();
            _timer = 0f;
        }

    }

    private void CreateAsciiArt()
    {
        DestroyText();
        AdjustTextCharacterWidthHeight();
        
        _charIndex = _startIndex;
        var pixels = image.GetPixels();
        for (var y = 0; y < image.height; y++)
        {
            for (var x = 0; x < image.width; x++)
            {
                
                var pixelIndex = pixels[x + y * image.width];
                var grayscale = (pixelIndex.r + pixelIndex.g + pixelIndex.b) / 3.0f;

                if (grayscale < dontCreateTextBrightVal) continue;
                var textObject = new GameObject("ASCII_" + x + "_" + y);
                textObject.transform.SetParent(parentAsciiObject);
                var textMesh = textObject.AddComponent<TextMeshPro>();
                textMesh.font = font;
                textMesh.fontSize = (int)(_charHeight * sizeF);
                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.color = textType == TextColorType.Colored ? pixelIndex : 
                                new Color(grayscale, grayscale, grayscale);
                textMesh.text = Density[(_charIndex + x + y * image.width) % Density.Length].ToString();
                textObject.transform.localPosition = new Vector3(x * _charWidth, y * _charHeight, 0);
            }
        }
        
        _startIndex++;
    }

    private void AdjustTextCharacterWidthHeight()
    {
        var imageAspectRatio = (float)image.width / image.height;
        var screenAspectRatio = (float)Screen.width / Screen.height;
        if (imageAspectRatio > screenAspectRatio)
        {
            _charWidth = (float)Screen.width / image.width;
            _charHeight = _charWidth / imageAspectRatio;
        }
        else
        {
            _charHeight = (float)Screen.height / image.height;
            _charWidth = _charHeight * imageAspectRatio;
        }
    }

    private void DestroyText()
    {
        for (var i = 0; i < parentAsciiObject.childCount; i++)
        {
            var child = parentAsciiObject.GetChild(i);
            Destroy(child.gameObject);
        }
    }
    
}