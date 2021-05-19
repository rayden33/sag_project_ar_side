using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetRequestToServer : MonoBehaviour
{
    public string Response { get; set; }

    void Start()
    {
        //StartCoroutine(GetRequest("http://ethereal/site/get-categories?category_type=room"));
    }

    public void RequestToServerAPI(string apiRequestName, List<KeyValuePair<string,string>> getParams)
    {
        string requestUrl = HostConfig.HostUrl + apiRequestName;
        if (getParams.Count > 0)
            requestUrl += '?';
        for(int i = 0; i < getParams.Count; i++)
        {
            requestUrl += getParams[i].Key + '=' + getParams[i].Value;
            if (i < getParams.Count - 1)
                requestUrl += '&';
        }
        Debug.Log(requestUrl);
        if (requestUrl != "")
            StartCoroutine(GetRequest(requestUrl));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
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
                    Response = webRequest.error;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    Response = webRequest.error;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    Response = webRequest.downloadHandler.text;
                    break;
            }
        }
    }
}
