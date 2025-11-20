using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using System.Collections;

public class photograph : MonoBehaviour
{
    public Camera renderCamera;
    public int width = 1920;
    public int height = 1080;

    public string fileName = "RenderedImage";
    public string outputFileName = "RenderedImage.png";
    int imageNum = 0;

    void Start()
    {
        StartCoroutine(screen());
        //RenderAndSave();
    }

    IEnumerator screen()
    {
        yield return new WaitForSeconds(0.1f);
        RenderAndSave();
        StartCoroutine(screen());
    }

    void RenderAndSave()
    {
        // Crea RenderTexture
        RenderTexture rt = new RenderTexture(width, height, 24);
        renderCamera.targetTexture = rt;

        // Crea Texture2D
        Texture2D image = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Renderizza la camera
        renderCamera.Render();

        // Legge i pixel dalla RenderTexture
        RenderTexture.active = rt;
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        // Pulisce
        renderCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Salva come PNG
        byte[] bytes = image.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, "frames/" + fileName + imageNum + ".png");
        File.WriteAllBytes(path, bytes);
        imageNum++;
        Debug.Log("Immagine salvata in: " + path);
    }
}
