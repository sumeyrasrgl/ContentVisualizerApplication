using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class Asset
{
    public string assetName;
    public string downloadURL;
}

[System.Serializable]
public class AssetList
{
    public Asset[] asset;
}
public class LoadAssetBundle : MonoBehaviour
{
    [SerializeField] private List<string> downloadURLList = new List<string>();
    [SerializeField] private List<string> assetNameList = new List<string>();

    [SerializeField] private Button downloadButtonPrefab;
    [SerializeField] private GameObject uiButtonPrefab;
    [SerializeField] private LoadJSONFile loadJSONFile;
    [SerializeField] private string jsonURL;

    [SerializeField] private List<GameObject> downloadedAssetList = new List<GameObject>();
    [SerializeField] private List<Material> materialList = new List<Material>();


    private const string DOWNLOAD_BUTTON_TEXT = "Download List";
    private AssetBundle bundle;
    public GameObject assetbundles;

    private Canvas canvas;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        CreateDownloadButton(canvas);

    }

    void CreateDownloadButton(Canvas canvas)
    {
        Transform actualCanvasTransform = canvas.gameObject.transform;
        Button downloadListButton = Instantiate(downloadButtonPrefab, new Vector3(actualCanvasTransform.position.x + 110, actualCanvasTransform.position.y + 210, 0), Quaternion.identity);
        downloadListButton.transform.parent = actualCanvasTransform;
        downloadListButton.GetComponentInChildren<Text>().text = DOWNLOAD_BUTTON_TEXT;
        downloadListButton.onClick.AddListener(() => StartDownloadButtonCoroutine(downloadListButton));
    }

    void StartDownloadButtonCoroutine(Button downloadListButton)
    {
        if (jsonURL != null)
        {
            StartCoroutine(loadJSONFile.WebRequestJSONFile(jsonURL, (isSuccess, JSONData) => JSONToAssetList(isSuccess, JSONData)));
            downloadListButton.gameObject.SetActive(false);
        }
    }

    void JSONToAssetList(bool isSuccess, string json)
    {
        if (isSuccess)
        {
            var assetList = JsonUtility.FromJson<AssetList>(json);
            foreach (Asset item in assetList.asset)
            {
                assetNameList.Add(item.assetName);
                downloadURLList.Add(item.downloadURL);
            }

            CreateUIButton();
        }
        else
        {
            Debug.LogError(json);
        }
    }

    void CreateUIButton()
    {
        Transform actualCanvasTransform = canvas.gameObject.transform;

        for (int urlIndex = 0; urlIndex < downloadURLList.Count; urlIndex++)
        {
            GameObject assetButton = Instantiate(uiButtonPrefab, new Vector3(actualCanvasTransform.position.x - 135, actualCanvasTransform.position.y + 210 + (50 * -urlIndex), 0), Quaternion.identity);
            assetButton.transform.parent = actualCanvasTransform;
            assetButton.GetComponentInChildren<Text>().text = assetNameList[urlIndex];
            StartButtonCoroutine(assetButton, urlIndex);
        }
    }

    public void StartButtonCoroutine(GameObject assetButton, int assetIndex)
    {
        assetButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (assetbundles.transform.childCount > 0)
            {
                for (int index = 0; index < assetbundles.transform.childCount; index++)
                {
                    assetbundles.transform.GetChild(index).gameObject.SetActive(false);
                }
            }

            var asset = assetbundles.transform.Find(assetNameList[assetIndex]);
            if (asset == null)
            {
                StartCoroutine(WebRequest(downloadURLList[assetIndex], assetNameList[assetIndex]));
            }
            else
            {
                asset.gameObject.SetActive(true);
            }
        });
    }
    IEnumerator WebRequest(string url, string assetName)
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                bundle = DownloadHandlerAssetBundle.GetContent(www);
                GameObject obj = (GameObject)bundle.LoadAsset(assetName);
                GameObject gameObject = Instantiate(obj, new Vector3(0, 0, 0.5f), Quaternion.identity);
                gameObject.name = assetName;
                gameObject.transform.localScale = new Vector3(2, 2, 2);
                gameObject.transform.parent = assetbundles.transform;
                downloadedAssetList.Add(obj);
                bundle.Unload(false);
            }
        }
    }

    IEnumerator WebRequestTexture(string url, string assetName)
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                bundle = DownloadHandlerAssetBundle.GetContent(www);
                Material material = (Material)bundle.LoadAsset(assetName);
                materialList.Add(material);
            }
        }
    }
}
