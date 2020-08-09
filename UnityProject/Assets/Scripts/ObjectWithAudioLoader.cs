using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

public class ObjectWithAudioLoader : MonoBehaviour
{
    public AssetReference objectToLoad;
    private GameObject instantiatedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadObject()
    {
        if (instantiatedObject != null) return;
        Addressables.LoadAssetAsync<GameObject>(objectToLoad).Completed += ObjectLoadDone;
    }
    
    /// <summary>
    /// LoadAssetAsync loads an Addressable Asset into memory, but does nt instantiate it.
    /// InstantiateAsync loads and instantiates an Addressable Asset in a single step.
    /// </summary>
    /// <param name="obj"></param>
    private void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedObject = obj.Result;
            Debug.Log("Successfully loaded object.");
            instantiatedObject = Instantiate(loadedObject);
            loadedObject.GetComponent<AudioSource>().Play();
            Debug.Log("Successfully instantiated object.");
        }
    }

    public void UnloadObject()
    {
        if (instantiatedObject == null) return;
        Destroy(instantiatedObject);
        Addressables.ReleaseInstance(instantiatedObject);
    }
}
