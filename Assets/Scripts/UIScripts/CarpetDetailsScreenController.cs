using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Globalization;

public class CarpetDetailsScreenController : MonoBehaviour
{
    [SerializeField] private GameObject RequestServerManagerPrefab;
    [SerializeField] private GameObject LoadingScreenGo;
    [SerializeField] private Text carpetTypeTxt;
    [SerializeField] private Text carpetNameTxt;
    [SerializeField] private Text carpetPriceTxt;
    [SerializeField] private Text carpetDensityTxt;
    [SerializeField] private Text carpetBaseTxt;
    [SerializeField] private Text carpetPileHTxt;
    [SerializeField] private Text carpetPileYTxt;
    [SerializeField] private Text carpetYarnCTxt;
    [SerializeField] private Text carpetWeightTxt;
    [SerializeField] private Text carpetEdgingTxt;
    [SerializeField] private Image carpetImage;
    [SerializeField] private Button buyButton;

    private string TextureURL = "";
    public Carpet Carpet { get; set; }
    public CarpetDetails CarpetDetails { get; set; }
    public Collection Collection { get; set; }
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
        Carpet = CustomCoreRAM.selectedCarpet;
        Collection = CustomCoreRAM.selectedCollection;
        LoadCarpetDetailsFromServer();

    }


    private async void LoadCarpetDetailsFromServer()
    {
        GameObject requestServerManagerGo = Instantiate(RequestServerManagerPrefab);
        GetRequestToServer getRequestToServer = requestServerManagerGo.GetComponent<GetRequestToServer>();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        if (Carpet.id != "0" || !string.IsNullOrEmpty(Carpet.id))
            getParams.Add(new KeyValuePair<string, string>("carpet_id", Carpet.id));
        getRequestToServer.RequestToServerAPI("get-carpet-details", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<CarpetDetails> tmpCarpetDetails = new List<CarpetDetails>();
        tmpCarpetDetails.AddRange(JsonHelper.FromJson<CarpetDetails>(getRequestToServer.Response));
        CarpetDetails = tmpCarpetDetails[0];
        CustomCoreRAM.selectedCarpetDetails = CarpetDetails;
        getRequestToServer.Response = null;


        

        carpetNameTxt.text = CarpetDetails.serial_number;
        carpetTypeTxt.text = CarpetDetails.collection_name;
        carpetPriceTxt.text = CarpetDetails.price.ToString("N", CultureInfo.CreateSpecificCulture("sv-SE"));
        carpetDensityTxt.text = CarpetDetails.density;
        carpetBaseTxt.text = CarpetDetails.collection_base;
        carpetPileHTxt.text = CarpetDetails.pile_height;
        carpetPileYTxt.text = CarpetDetails.pile_yarn;
        carpetYarnCTxt.text = CarpetDetails.yarn_composition;
        carpetWeightTxt.text = CarpetDetails.weight;
        carpetEdgingTxt.text = CarpetDetails.edging;

        if (!string.IsNullOrEmpty(CarpetDetails.carpet_order_url))
            buyButton.interactable = true;

        if (CarpetDetails.avatar_url != null)
            TextureURL = HostConfig.MainHostUrl + CarpetDetails.avatar_url;
        //TextureURL = HostConfig.HostUrl + "images/abc.png";
        StartCoroutine(DownloadImage(TextureURL));
        Debug.Log(TextureURL);
    }

    public void OpenCarpetInAR()
    {
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
        if(!string.IsNullOrEmpty(Carpet.carpet_order_url))
            Application.OpenURL(Carpet.carpet_order_url);
    }

}
