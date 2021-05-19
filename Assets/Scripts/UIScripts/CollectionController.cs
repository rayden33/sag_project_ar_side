using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CollectionController : MonoBehaviour
{
    [SerializeField]
    private Image ContentImage;
    [SerializeField]
    private Text ContentText;

    public GameObject LoadingScreen;
    public GameObject CarpetListGo;

    public string RoomCategoryId { get; private set; }
    public string StyleCategoryId { get; private set; }
    public Collection Collection { get; private set; }


    private string TextureURL = "";

    public void FillContent(string roomCategoryId, string styleCategoryId, Collection collection)
    {
        RoomCategoryId = roomCategoryId;
        StyleCategoryId = styleCategoryId;
        Collection = collection;
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 620);

        ContentText.text = collection.name_en;
        if (collection.avatar != null)
            TextureURL = HostConfig.MainHostUrl + collection.avatar;
        //TextureURL = HostConfig.HostUrl + "images/abc.png";
        StartCoroutine(DownloadImage(TextureURL));
        Debug.Log(TextureURL);
    }

    public void OpenCarpetList()
    {
        CarpetListScreenController clsc = CarpetListGo.GetComponent<CarpetListScreenController>();
        clsc.RoomCategoryId = RoomCategoryId;
        clsc.StyleCategoryId = StyleCategoryId;
        clsc.CollectionId = Collection.collection_id.ToString();
        CarpetListGo.SetActive(true);
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
