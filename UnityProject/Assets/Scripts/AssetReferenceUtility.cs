using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

public class AssetReferenceUtility : MonoBehaviour
{
    public AssetReference objectToLoad;
    public AssetReference accessoryObjectToLoad;
    private GameObject instantiatedObject;
    private GameObject instantiatedAccessoryObject;
    
    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log("Successfully instantiated object.");
            if (accessoryObjectToLoad != null)
            {
                accessoryObjectToLoad.InstantiateAsync(instantiatedObject.transform).Completed
                    += (op) =>
                    {
                        if (op.Status == AsyncOperationStatus.Succeeded)
                        {
                            instantiatedAccessoryObject = op.Result;
                            Debug.Log("Successfully loaded and instantiated accessory object.");
                        }
                    };
            }
        }
    }

    public void UnloadObject()
    {
        Destroy(instantiatedObject);
        Addressables.ReleaseInstance(instantiatedObject);
        Addressables.ReleaseInstance(instantiatedAccessoryObject);
    }

    public void LoadObject()
    {
        if (instantiatedObject != null) return;
        Addressables.LoadAssetAsync<GameObject>(objectToLoad).Completed += ObjectLoadDone;
    }
}
