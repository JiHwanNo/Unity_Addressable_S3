using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;
public class S3_Addressable : MonoBehaviour
{
    public AssetLabelReference assetLabel;
    IList<IResourceLocation> _loaction;
    List<GameObject> gameObjects = new List<GameObject>();
    public Button instant_Btn;
    private void Start()
    {
        GetLocations();
        instant_Btn.onClick.AddListener(Instantiate);
    }
    void GetLocations()
    {
        Addressables.GetDownloadSizeAsync(assetLabel.labelString).Completed +=
            (handle) =>
            { Debug.Log("size : " + handle.Result); };
        Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                _loaction = handle.Result;
            };
    }

    public void Instantiate()
    {

        var location = _loaction[UnityEngine.Random.Range(0, _loaction.Count)];
        Debug.Log(location);
        try
        {

            Addressables.InstantiateAsync(location, Vector3.one, Quaternion.identity).Completed +=
                (handle) =>
                {
                    gameObjects.Add(handle.Result);
                    Invoke("Destroy", 5f);
                };
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }

    public void Destroy()
    {
        if (gameObjects.Count == 0)
            return;

        var index = gameObjects.Count - 1;
        Addressables.ReleaseInstance(gameObjects[index]);
        gameObjects.RemoveAt(index);
    }

}
