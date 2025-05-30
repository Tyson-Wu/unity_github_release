using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AssetBundleManager : MonoBehaviour
{
    private string assetBundleURL = "https://github.com/Tyson-Wu/unity_github_release_ab_asset/releases/download/v1.0.0/t_0";
    private AssetBundle loadedAssetBundle;

    IEnumerator Start()
    {
        // 下载 AssetBundle
        UnityWebRequest request = UnityWebRequest.Get(assetBundleURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 下载成功，加载 AssetBundle
            loadedAssetBundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
            if (loadedAssetBundle != null)
            {
                Debug.Log("AssetBundle loaded successfully.");
                // 从 AssetBundle 中加载资源
                var asset = loadedAssetBundle.LoadAsset("Cube");
                Instantiate(asset);
            }
            else
            {
                Debug.LogError("Failed to load AssetBundle.");
            }
        }
        else
        {
            Debug.LogError("Failed to download AssetBundle: " + request.error);
        }
    }
}
