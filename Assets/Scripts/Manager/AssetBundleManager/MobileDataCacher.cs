/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate void CacheFileLoaded(string cachedFilePath);

public class MobileDataCacher
{
    private static MobileDataCacher instance;
    public static MobileDataCacher Instance
    {
        get
        {
            if (instance == null)
                instance = new MobileDataCacher();
            return instance;
        }
    }
    private string cacheListFilePath = string.Empty;
    private string cacheDirectory = string.Empty;

    public Dictionary<string, CacheFile> cacheList;
    public Dictionary<string, int> versionList;

    public string versionTableUrl = string.Empty;
    public event CacheFileLoaded CacheFileLoaded;

    public bool makedCacheList = false;

    public WWW download = null;

    private MobileDataCacher()
    {

        cacheListFilePath = MakeSaveDataPath("cacheList.txt");


        cacheList = new Dictionary<string, CacheFile>();
        versionList = new Dictionary<string, int>();


        LoadCacheList();
    }
    ~MobileDataCacher()
    {
        FlushCacheList();
    }
    private void FlushCacheList()
    {
        Stream stream = File.Open(cacheListFilePath, FileMode.Create);
        BinaryWriter binWriter = new BinaryWriter(stream, System.Text.Encoding.Unicode);
        //for(int i = 0 ; i < cacheList.Count ; ++i)
        {
            foreach (string key in cacheList.Keys)
            {
                CacheFile cacheFile = null;
                if (cacheList.TryGetValue(key, out cacheFile))
                {
                    binWriter.Write(key);
                    Debug.LogWarning(key);
                    binWriter.Write(cacheFile.Url);
                    Debug.LogWarning(cacheFile.Url);
                    binWriter.Write(cacheFile.Version);
                    Debug.LogWarning(cacheFile.Version);
                }
            }
        }
        binWriter.Close();
        stream.Close();
        stream.Dispose();
        Debug.Log("cache database flushed.");

    }
    private void LoadCacheList()
    {
        cacheList.Clear();
        if (File.Exists(cacheListFilePath))
        {
            Stream stream = File.Open(cacheListFilePath, FileMode.Open);
            BinaryReader binReader = new BinaryReader(stream, System.Text.Encoding.Unicode);
            while (binReader.PeekChar() != -1)
            {
                string sourceURL = binReader.ReadString();
                string cachePath = binReader.ReadString();
                int version = binReader.ReadInt32();
                cacheList.Add(sourceURL, new CacheFile(cachePath, version));
            }
            binReader.Close();
            stream.Close();
            stream.Dispose();
        }
        Debug.Log("cache database loaded.");
    }
    public IEnumerator DownloadVersionTable()
    {
        Debug.Log("downloading version table.");

        WWW download = new WWW(versionTableUrl);
        Debug.Log(versionTableUrl);

        yield return download;

        Debug.Log("version table downloaded.");

        Stream stream = new MemoryStream(download.bytes);
        StreamReader binReader = new StreamReader(stream, System.Text.Encoding.Unicode);

        versionList.Clear();

        while (!binReader.EndOfStream)
        {
            string url = binReader.ReadLine();
            int version = int.Parse(binReader.ReadLine());
            versionList.Add(url, version);
            Debug.Log("url : " + url);
            Debug.Log("version : " + version);
        }
        binReader.Close();
        stream.Close();
        binReader.Dispose();
        stream.Dispose();
        download.Dispose();

        makedCacheList = true;
    }
    public void ClearCachedFiles()
    {
        cacheList.Clear();
        FlushCacheList();
        if (Directory.Exists(cacheDirectory))
        {
            string[] files = Directory.GetFiles(cacheDirectory);
            if (files != null)
                for (int i = 0; i < files.Length; ++i)
                    File.Delete(files[i]);
        }
        Debug.Log("cleared cached files.");
    }
    public IEnumerator DownloadOrLoadFromCache(string sourceURL, string filename)
    {
        CacheFile cachedFile = null;
        bool downloadLatest = false;
        int latestVersion = -1;
        //CHB : determining either to download new file or use cached. - begin

        //if(cacheList.TryGetValue(sourceURL, out cachedFile))
        if (cacheList.TryGetValue(filename, out cachedFile))
        {
            if (versionList.TryGetValue(filename, out latestVersion))
            {
                if (cachedFile.Version < latestVersion)
                {
                    Debug.Log("latest file found.");
                    downloadLatest = true;
                }
            }
        }
        else
            downloadLatest = true;
        //CHB : determining either to download new file or use cached. - end

        if (downloadLatest)
        {
            Debug.Log("download file from web.");
            //string[] urlParts = sourceURL.Split('/');
            //string fileName = urlParts[urlParts.Length - 1];

            //string destinationPath = cacheDirectory +fileName;
            string destinationPath = MakeSaveDataPath(filename);

            download = new WWW(sourceURL);
            yield return download;

            Debug.Log("download completed.");

            FileStream stream = new FileStream(destinationPath, FileMode.Create);
            stream.Write(download.bytes, 0, download.bytes.Length);
            stream.Close();
            stream.Dispose();
            download.Dispose();

            Debug.Log("downloaded file cached.");


            if (cachedFile == null)
            {
                cachedFile = new CacheFile(filename, versionList[filename]);
                cacheList.Add(filename, cachedFile);
            }
            else
                cachedFile.Version = latestVersion;



            FlushCacheList();

            if (CacheFileLoaded != null)
                CacheFileLoaded(destinationPath);
            Debug.Log("newly downloaded/cached file was loaded.");
        }
        else if (CacheFileLoaded != null)
        {
            CacheFileLoaded(cachedFile.Url);
            Debug.Log("already cached file was loaded.");
        }
    }

    public string MakeSaveDataPath(string filename)
    {
        string savetargetpath = string.Empty;
        if (Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            savetargetpath = Application.dataPath + "/" + filename;

        }
        else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            savetargetpath = Application.dataPath + "/" + filename;

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            savetargetpath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
            savetargetpath = savetargetpath.Substring(0, savetargetpath.LastIndexOf('/'));
            savetargetpath += "/Documents/" + filename;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            savetargetpath = "/mnt/sdcard/assetbundle";

            if (!System.IO.Directory.Exists(savetargetpath))
            {
                System.IO.Directory.CreateDirectory(savetargetpath);
            }

            savetargetpath += "/" + filename;
        }
        return savetargetpath;
    }


}
*/