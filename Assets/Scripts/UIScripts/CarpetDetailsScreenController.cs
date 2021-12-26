using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarpetDetailsScreenController : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreenGo;
    [SerializeField] private Text carpetNameTxt;
    [SerializeField] private Image carpetImage;
    [SerializeField]public GameObject CarpetOrderGo;

    private string TextureURL = "";
    public Carpet Carpet { get; set; }
    void Start()
    {

    }
    private void OnEnable()
    {
        /*if (ParentViewPortGo.transform.childCount > 0)
            return;*/
        LoadingScreenGo.SetActive(true);
        FillContent();
    }

    private void FillContent()
    {
        carpetNameTxt.text = Carpet.serial_number;

        
        if (Carpet.avatar_url != null)
            TextureURL = HostConfig.MainHostUrl + Carpet.avatar_url;
        //TextureURL = HostConfig.HostUrl + "images/abc.png";
        StartCoroutine(DownloadImage(TextureURL));
        Debug.Log(TextureURL);

    }

    public void OpenCarpetInAR()
    {
        PlayerPrefs.SetString("carpet_id", Carpet.id);
        PlayerPrefs.SetString("carpet_name", Carpet.serial_number);
        PlayerPrefs.SetString("carpet_avatar", Carpet.avatar_url);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
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
                    carpetImage.sprite = webSprite;
                    break;
            }
            LoadingScreenGo.SetActive(false);
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }


    public void OpenCarpetOrder()
    {
        CarpetOrderScreenController cosc = CarpetOrderGo.GetComponent<CarpetOrderScreenController>();
        cosc.Carpet = Carpet;
        CarpetOrderGo.SetActive(true);
        cosc.FillContent();
    }

}
