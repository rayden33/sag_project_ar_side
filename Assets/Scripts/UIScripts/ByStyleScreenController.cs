using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ByStyleScreenController : MonoBehaviour
{
    [SerializeField] private GameObject RequestServerManagerGo;
    [SerializeField] private GameObject StyleCategoryPrefab;
    [SerializeField] private GameObject ParentViewPortGo;
    [SerializeField] private GameObject LoadingScreenGo;
    [SerializeField] private GameObject CollectionListGo;
    void Start()
    {

    }
    private void OnEnable()
    {
        if (ParentViewPortGo.transform.childCount > 0)
            return;
        LoadingScreenGo.SetActive(true);
        LoadCategoriesFromServer();
    }

    private async void LoadCategoriesFromServer()
    {
        GetRequestToServer getRequestToServer = RequestServerManagerGo.GetComponent<GetRequestToServer>();
        List<KeyValuePair<string, string>> getParams = new List<KeyValuePair<string, string>>();
        getParams.Add(new KeyValuePair<string, string>("category_type", "style"));
        getRequestToServer.RequestToServerAPI("get-categories", getParams);
        while (getRequestToServer.Response == null)
            await Task.Yield();
        Debug.Log(getRequestToServer.Response);

        List<Category> categories = new List<Category>();
        categories.AddRange(JsonHelper.FromJson<Category>(getRequestToServer.Response));
        /*List<Category> categories1 = new List<Category>();
        categories1.AddRange(JsonHelper.FromJson<Category>(getRequestToServer.Response));*/
        GenerateCategoryList(categories);
        //LoadingScreenGo.SetActive(false);
        getRequestToServer.Response = null;
    }


    private void GenerateCategoryList(List<Category> categories)
    {
        GameObject tmpStyleCatGo;
        /// Destroy all children
        foreach (Transform child in ParentViewPortGo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Category category in categories)
        {
            tmpStyleCatGo = Instantiate(StyleCategoryPrefab, ParentViewPortGo.transform);
            StyleCategoryController styleCategoryController = tmpStyleCatGo.GetComponent<StyleCategoryController>();
            styleCategoryController.LoadingScreen = LoadingScreenGo;
            styleCategoryController.CollectionListGo = CollectionListGo;
            styleCategoryController.FillContent(category);
        }
    }
}
