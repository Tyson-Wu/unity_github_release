using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class VersionManager : MonoBehaviour
{
    // 
    // https://github.com/Tyson-Wu/unity_github_release_ab_asset/releases/download/v1.0.0/version.json
    private string versionURL = "https://github.com/Tyson-Wu/unity_github_release_ab_asset/releases/latest/download/version.json";
    VersionData versionData;
    IEnumerator Start()
    {
        // 下载版本信息
        UnityWebRequest versionRequest = UnityWebRequest.Get(versionURL);
        yield return versionRequest.SendWebRequest();

        if (versionRequest.result == UnityWebRequest.Result.Success)
        {
            string json = versionRequest.downloadHandler.text;
            versionData = JsonUtility.FromJson<VersionData>(json);
            int localVersionId = PlayerPrefs.GetInt("Version", -1);
            Debug.Log("[version info] \n" + json);
            Debug.Log("local verison id = " + localVersionId);
            // 检查本地版本和远程版本
            if (versionData.version > localVersionId)
            {
                Debug.Log("New version available, updating...");
                // 下载新的 AssetBundle
                StartCoroutine(DownloadAssetBundle(versionData.assetBundleURL));
            }
        }
        else
        {
            Debug.LogError("Failed to download version info.");
        }
    }

    IEnumerator DownloadAssetBundle(string assetBundleURL)
    {
        UnityWebRequest request = UnityWebRequest.Get(assetBundleURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            AssetBundle loadedAssetBundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
            if (loadedAssetBundle != null)
            {
                Debug.Log("AssetBundle updated.");
                PlayerPrefs.SetInt("Version", versionData.version); // 保存新版本号
            }
            else
            {
                Debug.LogError("Failed to load AssetBundle.");
            }
        }
    }

    [System.Serializable]
    public class VersionData
    {
        public int version;
        public string assetBundleURL;
    }
}

