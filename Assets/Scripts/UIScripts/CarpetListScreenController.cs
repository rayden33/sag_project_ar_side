using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CarpetListScreenController : MonoBehaviour
{
    [SerializeField] private GameObject RequestServerManagerPrefab;
    [SerializeField] private GameObject CarpetPrefab;
    [SerializeField] private GameObject ParentViewPortGo;
    [SerializeField] private GameObject LoadingScreenGo;
    [SerializeField] private GameObject CarpetDetailsGo;
    public string RoomCategoryId = "0";
    public string StyleCategoryId = "0";
    public string CollectionId;
    public Collection Collection { get; private set; }
    void Start()
    {

    }
    private void OnEnable()
    {
        /*if (ParentViewPortGo.transform.childCount > 0)
            return;*/
        LoadingScreenGo.SetActive(true);
        LoadCarpetsFromServer();
    }


   

    private async void LoadCarpetsFromServer()
    {
        GameObject requestServerManagerGo = Instantiate(RequestServerManagerPrefab);
        GetRequestToServer getRequestToServer = requestServerManagerGo.GetComponent<GetRequestToServer>();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        if (RoomCategoryId != "0")
            getParams.Add(new KeyValuePair<string, string>("room_cat_id", RoomCategoryId));
        if (StyleCategoryId != "0")
            getParams.Add(new KeyValuePair<string, string>("style_cat_id", StyleCategoryId));
        getParams.Add(new KeyValuePair<string, string>("collection_id", CollectionId));
        getRequestToServer.RequestToServerAPI("get-carpets", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<CarpetBasicInfo> carpetBasicInfos = new List<CarpetBasicInfo>();
        carpetBasicInfos.AddRange(JsonHelper.FromJson<CarpetBasicInfo>(getRequestToServer.Response));
        /*List<Category> categories1 = new List<Category>();
        categories1.AddRange(JsonHelper.FromJson<Category>(getRequestToServer.Response));*/
        GenerateCarpetList(carpetBasicInfos);
        //LoadingScreenGo.SetActive(false);
        getRequestToServer.Response = null;
    }


    private void GenerateCarpetList(List<CarpetBasicInfo> carpetBasicInfos)
    {
        Debug.Log("Hekllo");
        GameObject tmpCarpetGo;
        /// Destroy all children
        foreach (Transform child in ParentViewPortGo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (CarpetBasicInfo carpetBasicInfo in carpetBasicInfos)
        {
            tmpCarpetGo = Instantiate(CarpetPrefab, ParentViewPortGo.transform);
            CarpetController carpetController = tmpCarpetGo.GetComponent<CarpetController>();
            carpetController.LoadingScreen = LoadingScreenGo;
            carpetController.CarpetDetailsGo = CarpetDetailsGo;
            Debug.Log("Hekllo2");
            carpetController.FillContent(carpetBasicInfo);
        }
    }
}
