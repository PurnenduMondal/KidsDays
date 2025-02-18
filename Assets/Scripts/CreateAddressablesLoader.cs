﻿using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class CreateAddressablesLoader
{
     public static async Task InitByNameOrLabel<T>(string assetNameOrLabel, List<T> createdObjs)
        where T : Object
    {
        //var locations = await Addressables.LoadResourceLocationsAsync(assetNameOrLabel).Task;

        foreach (var location in Addressables.LoadResourceLocationsAsync(assetNameOrLabel).Result)
        {
            if(location.PrimaryKey.Contains(".png"))
            createdObjs.Add(await Addressables.LoadAssetAsync<Sprite>(location.PrimaryKey).Task as T);
        }
        
        //await CreateAssetsThenUpdateCollection(locations, createdObjs);
    }
    
    public static async Task IniByLoadedAddress<T>(IList<IResourceLocation> loadedLocations, List<T> createdObjs)
    where T : Object
    {
        await CreateAssetsThenUpdateCollection(loadedLocations, createdObjs);
    }

    private static async Task CreateAssetsThenUpdateCollection<T>(IList<IResourceLocation> locations, List<T> createdObjs)
        where T: Object
    {
        foreach (var location in locations)
        {
            if(location.PrimaryKey.Contains(".png"))
            createdObjs.Add(await Addressables.LoadAssetAsync<Sprite>(location.PrimaryKey).Task as T);
        }
            
    }

   
}




















































