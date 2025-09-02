using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// This script is used to load addresables at runtime and instantiate them as children of a specified parent transform.
/// </summary>
public class AddresablesManagerSCRIPT : MonoBehaviour
{
    [SerializeField] Transform parentTransform;
    [SerializeField] RectTransform[] rectPlaceToSpawnTransform;

    [SerializeField] List<AssetReference> gameObjToLoadAsync;
    [SerializeField] List<GameObject> AllLoadedGameObjectsList = new List<GameObject>(2);

    private void Start()
    {
        StartCoroutine(LoadAddresablesRoutine());
    }

    private IEnumerator LoadAddresablesRoutine()
    {
        for (int i = 0; i < gameObjToLoadAsync.Count; i++)
        {
            Debug.Log("Loading " + gameObjToLoadAsync[i].RuntimeKey.ToString());

            AsyncOperationHandle<GameObject> handler = gameObjToLoadAsync[i].InstantiateAsync(parent: rectPlaceToSpawnTransform[i]);
            yield return handler;

            if (handler.Status == AsyncOperationStatus.Succeeded)
            {
                AllLoadedGameObjectsList.Add(handler.Result);
                // AllLoadedGameObjectsList[i].GetComponent<RectTransform>().SetAsLastSibling();
            }
            else
            {
                Debug.Log("Addresables not loaded" + gameObjToLoadAsync[i].RuntimeKey.ToString());
            }
        }

        Debug.Log("ALL Addresables Loaded");
    }
}
