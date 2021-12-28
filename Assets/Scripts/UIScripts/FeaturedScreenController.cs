using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FeaturedScreenController : MonoBehaviour
{
    [SerializeField] private GameObject RequestServerManagerPrefab;
    [SerializeField] private GameObject CarpetPrefab;
    [SerializeField] private GameObject CarpetListParentViewPortGo;
    [SerializeField] private GameObject CollectionPrefab;
    [SerializeField] private GameObject CollectionParentViewPortGo;
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
        loadCollectionList();
        loadCarpetList();
    }

    private void loadCollectionList()
    {
        if (CollectionParentViewPortGo.transform.childCount > 0)
            return;
        LoadingScreenGo.SetActive(true);
        LoadMainCollectionsFromServer();
    }

    private void loadCarpetList()
    {
        if (CarpetListParentViewPortGo.transform.childCount > 0)
            return;
        LoadingScreenGo.SetActive(true);
        LoadMainCarpetsFromServer();
    }

    private async void LoadMainCarpetsFromServer()
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
        GenerateCarpetList(carpetBasicInfos);
        getRequestToServer.Response = null;
    }

    private void GenerateCarpetList(List<CarpetBasicInfo> carpetBasicInfos)
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


    private async void LoadMainCollectionsFromServer()
    {
        GameObject requestServerManagerGo = Instantiate(RequestServerManagerPrefab);
        GetRequestToServer getRequestToServer = requestServerManagerGo.GetComponent<GetRequestToServer>();
        //GetRequestToServer getRequestToServer = new GetRequestToServer();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        getRequestToServer.RequestToServerAPI("get-collections", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<CollectionBasicInfo> collectionBasicInfos = new List<CollectionBasicInfo>();
        collectionBasicInfos.AddRange(JsonHelper.FromJson<CollectionBasicInfo>(getRequestToServer.Response));

        GenerateCollectionList(collectionBasicInfos);
        //LoadingScreenGo.SetActive(false);
        getRequestToServer.Response = null;
    }


    private void GenerateCollectionList(List<CollectionBasicInfo> collectionBasicInfos)
    {
        int activeCollectionCount = 0;
        GameObject tmpCollectionGo;
        /// Destroy all children
        foreach (Transform child in CollectionParentViewPortGo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (CollectionBasicInfo collectionBasicInfo in collectionBasicInfos)
        {
            if (collectionBasicInfo.status == 0)
                continue;
            tmpCollectionGo = Instantiate(CollectionPrefab, CollectionParentViewPortGo.transform);
            CollectionController collectionController = tmpCollectionGo.GetComponent<CollectionController>();
            collectionController.LoadingScreen = LoadingScreenGo;
            collectionController.CarpetListGo = CarpetListGo;
            collectionController.FillContent(collectionBasicInfo);
            activeCollectionCount++;
        }
        if (activeCollectionCount == 0)
            LoadingScreenGo.SetActive(false);
    }

    
}
