using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FeaturedScreenController : MonoBehaviour
{
    [SerializeField] private GameObject RequestServerManagerGo;
    [SerializeField] private GameObject RequestServerManagerGo2;
    [SerializeField] private GameObject CarpetPrefab;
    [SerializeField] private GameObject CarpetListParentViewPortGo;
    [SerializeField] private GameObject CollectionPrefab;
    [SerializeField] private GameObject CollectionParentViewPortGo;
    [SerializeField] private GameObject CarpetListGo;
    [SerializeField] private GameObject LoadingScreenGo;
    [SerializeField] private GameObject CarpetDetailsGo;

    void Start()
    {
        
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
        GetRequestToServer getRequestToServer = RequestServerManagerGo.GetComponent<GetRequestToServer>();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        getRequestToServer.RequestToServerAPI("get-main-carpets", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<Carpet> carpets = new List<Carpet>();
        carpets.AddRange(JsonHelper.FromJson<Carpet>(getRequestToServer.Response));
        GenerateCarpetList(carpets);
        getRequestToServer.Response = null;
    }

    private void GenerateCarpetList(List<Carpet> carpets)
    {
        Debug.Log("Hello");
        GameObject tmpCarpetGo;
        /// Destroy all children
        foreach (Transform child in CarpetListParentViewPortGo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Carpet carpet in carpets)
        {
            tmpCarpetGo = Instantiate(CarpetPrefab, CarpetListParentViewPortGo.transform);
            CarpetController carpetController = tmpCarpetGo.GetComponent<CarpetController>();
            carpetController.LoadingScreen = LoadingScreenGo;
            carpetController.CarpetDetailsGo = CarpetDetailsGo;
            Debug.Log("Hekllo2");
            carpetController.FillContent(carpet);
        }
    }


    private async void LoadMainCollectionsFromServer()
    {
        GetRequestToServer getRequestToServer = RequestServerManagerGo2.GetComponent<GetRequestToServer>();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        getRequestToServer.RequestToServerAPI("get-collections", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<Collection> collections = new List<Collection>();
        collections.AddRange(JsonHelper.FromJson<Collection>(getRequestToServer.Response));

        GenerateCollectionList(collections);
        //LoadingScreenGo.SetActive(false);
        getRequestToServer.Response = null;
    }


    private void GenerateCollectionList(List<Collection> collections)
    {
        int activeCollectionCount = 0;
        GameObject tmpCollectionGo;
        /// Destroy all children
        foreach (Transform child in CollectionParentViewPortGo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Collection collection in collections)
        {
            if (collection.status == 0)
                continue;
            tmpCollectionGo = Instantiate(CollectionPrefab, CollectionParentViewPortGo.transform);
            CollectionController collectionController = tmpCollectionGo.GetComponent<CollectionController>();
            collectionController.LoadingScreen = LoadingScreenGo;
            collectionController.CarpetListGo = CarpetListGo;
            collectionController.FillContent(collection);
            activeCollectionCount++;
        }
        if (activeCollectionCount == 0)
            LoadingScreenGo.SetActive(false);
    }
}
