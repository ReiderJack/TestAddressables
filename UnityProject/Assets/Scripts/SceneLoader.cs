using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoader : MonoBehaviour
{
    public AssetReference scene;
    private bool isLoaded = false;

    // for loading scenes from JSON use string 
    //public string scene;
    private AsyncOperationHandle<SceneInstance> handle;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // This command works for either AssetReference or string
        //Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive).Completed += SceneLoadComplete;
        //scene.LoadSceneAsync().Completed += SceneLoadComplete; This one works for AssetReference.
    }

    public void LoadScene()
    {
        if (scene == null)
        {
            Debug.LogError("Scene reference is null!");
            return;
        }
        if (isLoaded) return;
        Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive).Completed += SceneLoadComplete;
    }
    
    private void SceneLoadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            // Set out reference to the AsyncOperationHandle (see next section)
            Debug.Log(obj.Result.Scene.name + "Successfully loaded.");
            // (optional) do more stuff
            handle = obj;
            isLoaded = true;
        }
    }

    public void UnloadScene()
    {
        if (isLoaded)
        {
            Addressables.UnloadSceneAsync(handle, true).Completed += SceneUnloadComplete;
        }
    }

    private void SceneUnloadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Successfully unloaded scene.");
            isLoaded = false;
        }
    }
}
