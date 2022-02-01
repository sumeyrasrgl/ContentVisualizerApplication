using System.IO;
using UnityEngine;

public class JSONWriter : MonoBehaviour
{
    private const string ASSET_INFORMATION_FILE_NAME= "/assetInformation.txt";

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

    public AssetList assetList = new AssetList();

    public void Start()
    {
        ConvertToJSONFile();
    }
    public void ConvertToJSONFile()
    {
        string strJSON = JsonUtility.ToJson(assetList);
        File.WriteAllText(Application.dataPath + ASSET_INFORMATION_FILE_NAME, strJSON);
    }
}
