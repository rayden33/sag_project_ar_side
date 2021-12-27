using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetCarpetInfo : MonoBehaviour
{
    [SerializeField] private GameObject CarpetShadowGo;
    
    // Start is called before the first frame update
    public string TextureURL = "";

    // Start is called before the first frame update
    void Start()
    {
        //carpetInfoTxt.text = PlayerPrefs.GetString("carpet_name");
        TextureURL = HostConfig.MainHostUrl + PlayerPrefs.GetString("carpet_avatar");
        StartCoroutine(DownloadImage(TextureURL));
    }

    // Update is called once per frame
    private void Update()
    {
        
        // Make sure user is on Android platform
        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
        
    }

    IEnumerator DownloadImage(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            float rationX = 1.0f, rationZ = 1.0f;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    //Response = webRequest.error;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    //Response = webRequest.error;
                    break;
                case UnityWebRequest.Result.Success:
                    CarpetShadowGo.SetActive(true);
                    this.gameObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                    CarpetShadowGo.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                    rationX = (float)(((DownloadHandlerTexture)webRequest.downloadHandler).texture.width) / (float)(((DownloadHandlerTexture)webRequest.downloadHandler).texture.height);
                    //rationZ = ((DownloadHandlerTexture)webRequest.downloadHandler).texture.width / ((DownloadHandlerTexture)webRequest.downloadHandler).texture.width;
                    this.transform.parent.localScale = new Vector3(rationX, 1.0f, rationZ);
                    CarpetShadowGo.GetComponent<Renderer>().material.color = Color.black;
                    break;
            }
        }
    }
}
