using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CollectionScreenController : MonoBehaviour
{
    [SerializeField] private GameObject RequestServerManagerGo;
    [SerializeField] private GameObject CollectionPrefab;
    [SerializeField] private GameObject ParentViewPortGo;
    [SerializeField] private GameObject LoadingScreenGo;
    [SerializeField] private GameObject CarpetListGo;
    public string RoomCategoryId = "0";
    public string StyleCategoryId = "0";
    void Start()
    {

    }
    private void OnEnable()
    {
        /*if (ParentViewPortGo.transform.childCount > 0)
            return;*/
        LoadingScreenGo.SetActive(true);
        LoadCollectionsFromServer();
    }

    private async void LoadCollectionsFromServer()
    {
        GetRequestToServer getRequestToServer = RequestServerManagerGo.GetComponent<GetRequestToServer>();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        if (RoomCategoryId != "0")
            getParams.Add(new KeyValuePair<string, string>("room_cat_id", RoomCategoryId));
        if (StyleCategoryId != "0")
            getParams.Add(new KeyValuePair<string, string>("style_cat_id", StyleCategoryId));
        getRequestToServer.RequestToServerAPI("get-collections", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<Collection> collections = new List<Collection>();
        collections.AddRange(JsonHelper.FromJson<Collection>(getRequestToServer.Response));
        /*List<Category> categories1 = new List<Category>();
        categories1.AddRange(JsonHelper.FromJson<Category>(getRequestToServer.Response));*/
        GenerateCollectionList(collections);
        //LoadingScreenGo.SetActive(false);
        getRequestToServer.Response = null;
    }


    private void GenerateCollectionList(List<Collection> collections)
    {
        GameObject tmpCollectionGo;
        /// Destroy all children
        foreach (Transform child in ParentViewPortGo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Collection collection  in collections)
        {
            tmpCollectionGo = Instantiate(CollectionPrefab, ParentViewPortGo.transform);
            CollectionController collectionController = tmpCollectionGo.GetComponent<CollectionController>();
            collectionController.LoadingScreen = LoadingScreenGo;
            collectionController.CarpetListGo = CarpetListGo;
            collectionController.FillContent(RoomCategoryId, StyleCategoryId, collection);
        }
    }
}
