using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioLoader : MonoBehaviour
{
    private AudioSource audioSource;
    public AssetReference objectToLoad;
    private AudioClip audioClip;
    private AsyncOperationHandle<AudioClip> handle;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    public void LoadAndPlay()
    {
        Addressables.LoadAssetAsync<AudioClip>(objectToLoad).Completed += ObjectLoadDone;
    }

    /// <summary>
    /// LoadAssetAsync loads an Addressable Asset into memory, but does nt instantiate it.
    /// InstantiateAsync loads and instantiates an Addressable Asset in a single step.
    /// </summary>
    /// <param name="obj"></param>
    private void ObjectLoadDone(AsyncOperationHandle<AudioClip> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            var loadedObject = obj.Result;
            Debug.Log("Successfully loaded object.");
            audioClip = loadedObject;
            handle = obj;
            audioSource.clip = audioClip;
            audioSource.Play();
            Debug.Log("Successfully instantiated object.");
        }
    }

    public void UnloadAndStop()
    {
        audioSource.Stop();
        audioSource.clip = null;
        // I don't know if it's the problem with audio clips only
        // but to actually release audio data from memory you need to UnloadAudioData
        // just Releasing the reference does not unload memory which is used by audio clip
        audioClip.UnloadAudioData();
        audioClip = null;
        objectToLoad.ReleaseAsset();
    }
}
