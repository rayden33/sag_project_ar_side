using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CarpetController : MonoBehaviour
{
    [SerializeField]
    private Image ContentImage;
    [SerializeField]
    private Text ContentText;

    public GameObject LoadingScreen;
    public GameObject CarpetDetailsGo;

    public Carpet Carpet { get; private set; }


    private string TextureURL = "";

    public void FillContent(Carpet carpet)
    {
        Carpet = carpet;
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 278);

        ContentText.text = carpet.serial_number;
        if (carpet.avatar_url != null)
            TextureURL = HostConfig.MainHostUrl + carpet.avatar_url;
        //TextureURL = HostConfig.HostUrl + "images/abc.png";
        StartCoroutine(DownloadImage(TextureURL));
        Debug.Log(TextureURL);
    }
    public void OpenCarpetDetails()
    {
        CarpetDetailsScreenController cdsc = CarpetDetailsGo.GetComponent<CarpetDetailsScreenController>();
        cdsc.Carpet = Carpet;
        CarpetDetailsGo.SetActive(true);
    }

    IEnumerator DownloadImage(string uri)
    {
        Debug.Log(uri);
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Texture2D webTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture as Texture2D;
                    Sprite webSprite = SpriteFromTexture2D(webTexture);
                    ContentImage.sprite = webSprite;
                    break;
            }
            LoadingScreen.SetActive(false);
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

}
