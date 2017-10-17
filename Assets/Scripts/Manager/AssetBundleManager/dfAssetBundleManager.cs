using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class dfAssetBundleManager : msMonoSingleton<dfAssetBundleManager>
{
    const string _AssetBundlesPath = "/AssetBundles/";

    public string _DownloadUrl = "http://kowon.dongseo.ac.kr/~savrang/";    


    class AssetBundleInfo
    {
        public string _bundleName;
        public string _objectName;
        public string _objectType;
        public int _version;
    }
    List<AssetBundleInfo> _assetBundelInfoList = new List<AssetBundleInfo>();



    public dfAssetBundleManager()
    {
        string platformFolderForAssetBundles = 
        #if UNITY_EDITOR
                    GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
        #else
			        GetPlatformFolderForAssetBundles(Application.platform);
        #endif

        // Set base downloading url.
        //string relativePath = GetRelativePath();
        //_DownloadUrl = relativePath + _AssetBundlesPath + platformFolderForAssetBundles + "/";
    }

    public string GetRelativePath()
    {
        if (Application.isEditor)
            return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/"); // Use the build output folder directly.
        //else if (Application.isWebPlayer)
        //    return System.IO.Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/StreamingAssets";
        else if (Application.isMobilePlatform || Application.isConsolePlatform)
            return Application.streamingAssetsPath;
        else // For standalone player.
            return "file://" + Application.streamingAssetsPath;
    }


#if UNITY_EDITOR
    public static string GetPlatformFolderForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            //case BuildTarget.WebPlayer:
            //    return "WebPlayer";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
            default:
                return null;
        }
    }
#endif

    static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            //case RuntimePlatform.WindowsWebPlayer:
            //case RuntimePlatform.OSXWebPlayer:
            //    return "WebPlayer";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            // Add more build platform for your own.
            // If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
            default:
                return null;
        }
    }

    //-------------------------------------------------------------------------------------------------------------
    //
    //-------------------------------------------------------------------------------------------------------------
    public void CheckPatchInfo()
    {
        string url = _DownloadUrl + "assetbundles.xml";

        StartCoroutine(LoadAssetPatchInfo(url));   
    }

    
    //-------------------------------------------------------------------------------------------------------------
    //
    //-------------------------------------------------------------------------------------------------------------
    IEnumerator LoadAssetPatchInfo(string url)
    {
        WWW www = new WWW(url);

        // Wait for download to complete
        yield return www;

        if(www.size == 0)
        {
            dfInterfaceManager.Instance.UpdatePatchStateString("패치정보를 찾을 수 없습니다.");

            dfGameManager.Instance.PatchEnd();

            yield break;
        }


        XMLInStream inStream = new XMLInStream(www.text);

        int assetBundleCount = 0;
        inStream.List("assetbundleinfo", delegate(XMLInStream countStream)
        {
            countStream.Content("count", out assetBundleCount);
        });

        inStream.List("assetbundle", delegate(XMLInStream bundleInfoStream)
        {
            AssetBundleInfo bundleInfo = new AssetBundleInfo();

            bundleInfoStream.Content("bundlename", out bundleInfo._bundleName);
            bundleInfoStream.Content("objectname", out bundleInfo._objectName);
            bundleInfoStream.Content("objecttype", out bundleInfo._objectType);
            bundleInfoStream.Content("version", out bundleInfo._version);

            
            _assetBundelInfoList.Add(bundleInfo);
        });
        
        if(assetBundleCount != _assetBundelInfoList.Count)
        {
            //출력 - 패치정보가 올바르지 않습니다.

            yield break;
        }

        if (_assetBundelInfoList.Count > 0)
            StartCoroutine(LoadBundleCoroutine());
    }
    


    //-------------------------------------------------------------------------------------------------------------
    //
    //-------------------------------------------------------------------------------------------------------------
    public void LoadBundle()
    {
        string url = "http://kowon.dongseo.ac.kr/~savrang/cube.unity3d";
        string name = "cube";


        //coroutine.StartCoroutine(LoadBundleCoroutine(url, name));
    }

    //-------------------------------------------------------------------------------------------------------------
    //
    //-------------------------------------------------------------------------------------------------------------
    IEnumerator LoadBundleCoroutine()
    {
        foreach (AssetBundleInfo bundleInfo in _assetBundelInfoList)
        {
            string url = _DownloadUrl + bundleInfo._bundleName;

            // Start a download of the given URL
            WWW www = WWW.LoadFromCacheOrDownload(url, bundleInfo._version);

            // Wait for download to complete
            yield return www;

            // Load and retrieve the AssetBundle
            AssetBundle bundle = www.assetBundle;

            // Load the object asynchronously
            AssetBundleRequest request = null;
            
            switch(bundleInfo._objectType.ToLower())
            {
                case "gameobject":
                    request = bundle.LoadAssetAsync(bundleInfo._objectName, typeof(GameObject));
                    break;
                default:
                    //출력 - 지원하는 패치파일이 아닙니다.
                    break;
            }

            if (request != null)
            {
                // Wait for completion
                yield return request;

                // Get the reference to the loaded object
                GameObject obj = request.asset as GameObject;

                dfAssetManager.Instance.CreateAssetBundleGameObject(bundleInfo._objectName, obj);

                // Unload the AssetBundles compressed contents to conserve memory
                bundle.Unload(false);
            }

            // Frees the memory from the web stream
            www.Dispose();
        }

        dfGameManager.Instance.PatchEnd();
        yield break;
    }
}
