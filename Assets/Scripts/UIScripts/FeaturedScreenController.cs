using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FeaturedScreenController : MonoBehaviour
{
    [SerializeField] private GameObject RequestServerManagerPrefab;
    [SerializeField] private GameObject CarpetPrefab;
    [SerializeField] private GameObject CarpetListParentViewPortGo;
    [SerializeField] private GameObject TopCarpetPrefab;
    [SerializeField] private GameObject TopCarpetParentViewPortGo;
    [SerializeField] private GameObject CarpetListGo;
    [SerializeField] private GameObject LoadingScreenGo;
    [SerializeField] private GameObject CarpetDetailsGo;

    void Start()
    {
        if(CustomCoreRAM.isReturnFromAR)
        {
            CarpetDetailsScreenController cdsc = CarpetDetailsGo.GetComponent<CarpetDetailsScreenController>();
            CarpetDetailsGo.SetActive(true);
        }
        else
            customSystemInit();
    }

    private void customSystemInit()
    {
        CustomCoreRAM.selectedCarpet = new Carpet();
        CustomCoreRAM.selectedCategory = new Category();
        CustomCoreRAM.selectedCollection = new Collection();
    }

    private void OnEnable()
    {
        loadTopCarpetList();
        loadRandomCarpetList();
    }

    private void loadTopCarpetList()
    {
        if (TopCarpetParentViewPortGo.transform.childCount > 0)
            return;
        LoadingScreenGo.SetActive(true);
        LoadTopCarpetsFromServer();
    }

    private void loadRandomCarpetList()
    {
        if (CarpetListParentViewPortGo.transform.childCount > 0)
            return;
        LoadingScreenGo.SetActive(true);
        LoadRandomCarpetsFromServer();
    }

    private async void LoadTopCarpetsFromServer()
    {
        GameObject requestServerManagerGo = Instantiate(RequestServerManagerPrefab);
        GetRequestToServer getRequestToServer = requestServerManagerGo.GetComponent<GetRequestToServer>();
        //GetRequestToServer getRequestToServer = new GetRequestToServer();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        getRequestToServer.RequestToServerAPI("get-main-carpets", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<CarpetBasicInfo> carpetBasicInfos = new List<CarpetBasicInfo>();
        carpetBasicInfos.AddRange(JsonHelper.FromJson<CarpetBasicInfo>(getRequestToServer.Response));
        GenerateTopCarpetList(carpetBasicInfos);
        getRequestToServer.Response = null;
    }


    private void GenerateTopCarpetList(List<CarpetBasicInfo> carpetBasicInfos)
    {
        Debug.Log("Hello");
        GameObject tmpCarpetGo;
        /// Destroy all children
        foreach (Transform child in TopCarpetParentViewPortGo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (CarpetBasicInfo carpetBasicInfo in carpetBasicInfos)
        {
            tmpCarpetGo = Instantiate(TopCarpetPrefab, TopCarpetParentViewPortGo.transform);
            TopCarpetController carpetController = tmpCarpetGo.GetComponent<TopCarpetController>();
            carpetController.LoadingScreen = LoadingScreenGo;
            carpetController.CarpetDetailsGo = CarpetDetailsGo;
            Debug.Log(carpetBasicInfo.id + "Hellkoooo2");
            carpetController.FillContent(carpetBasicInfo);
        }
    }

    private async void LoadRandomCarpetsFromServer()
    {
        GameObject requestServerManagerGo = Instantiate(RequestServerManagerPrefab);
        GetRequestToServer getRequestToServer = requestServerManagerGo.GetComponent<GetRequestToServer>();
        //GetRequestToServer getRequestToServer = new GetRequestToServer();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        getRequestToServer.RequestToServerAPI("get-random-carpets", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<CarpetBasicInfo> carpetBasicInfos = new List<CarpetBasicInfo>();
        carpetBasicInfos.AddRange(JsonHelper.FromJson<CarpetBasicInfo>(getRequestToServer.Response));
        GenerateRandomCarpetList(carpetBasicInfos);
        getRequestToServer.Response = null;
    }

    private void GenerateRandomCarpetList(List<CarpetBasicInfo> carpetBasicInfos)
    {
        Debug.Log("Hello");
        GameObject tmpCarpetGo;
        /// Destroy all children
        foreach (Transform child in CarpetListParentViewPortGo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (CarpetBasicInfo carpetBasicInfo in carpetBasicInfos)
        {
            tmpCarpetGo = Instantiate(CarpetPrefab, CarpetListParentViewPortGo.transform);
            CarpetController carpetController = tmpCarpetGo.GetComponent<CarpetController>();
            carpetController.LoadingScreen = LoadingScreenGo;
            carpetController.CarpetDetailsGo = CarpetDetailsGo;
            Debug.Log(carpetBasicInfo.id + "Hellkoooo");
            carpetController.FillContent(carpetBasicInfo);
        }
    }


    

    
}
