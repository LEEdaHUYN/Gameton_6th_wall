using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager
{
    // 실제 로드한 리소스.
    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();


    // 임시 
    public void AddOnlineData<T>(string key, T data) where T : Object
    {
        _resources.Add(key, data);
    }


    #region 리소스 로드
    public T Load<T>(string key ,Action<T> callback = null) where T : Object
    {
        if (_resources.TryGetValue(key, out Object resource))
        {
            callback?.Invoke(resource as T);
            return resource as T;
        }

        //스프라이트 로드할때 항상 .sprite가 붙어 있어야하는데 데이터시트에 .sprite가 붙어있지 않은 데이터가 많음
        //임시로 붙임 -드래곤
        if (typeof(T) == typeof(Sprite))
        {
            key = key + ".sprite";
            if (_resources.TryGetValue(key, out Object temp))
            { 
                callback?.Invoke(temp as T);
                return temp as T;
            }
        }

        return null;
    }

    public void Instantiate(string key, Transform parent = null,Action<GameObject> callback = null)
    {
        
        Load<GameObject>(key, (prefab) =>
        {
            GameObject go = GameObject.Instantiate(prefab, parent);
            go.name = prefab.name;
            go.transform.localPosition = prefab.transform.position;
            callback?.Invoke(go);
        });

    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        Object.Destroy(go);
    }

    #endregion
    #region 어드레서블



    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (key.Contains(".multiplesprite"))
        {
            //멀티 스프라이트인 경우 하위객체를 배열 형태의 키값으로 추가시킴
            string parentKey = $"{key.Replace(".multiplesprite", "")}";
            var asyncOperation = Addressables.LoadAssetAsync<IList<T>>(key);
            asyncOperation.Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    IList<T> sprites = op.Result;
                    for(int i=0; i< sprites.Count; i++)
                    {
                        string keyName = $"{parentKey}[{i}]";
                        // 캐시 확인.
                        if (_resources.TryGetValue(keyName, out Object resource))
                        {
                            callback?.Invoke(sprites[i]);
                            return;
                        }

                        _resources.Add(keyName, sprites[i]);
                        
                    }
                    callback?.Invoke(sprites[sprites.Count-1]);
                }
            };
        }
        else
        {
            //스프라이트인 경우 하위객체의 찐이름으로 로드하면 스프라이트로 로딩이 됌
            string loadKey = key;
            if (typeof(T) == typeof(Sprite))
            {
                // 폴더로 등록할 경우 스프라이트 등록 처리 포함
                int startIdx = key.IndexOf('/') + 1;
                string fileName = key.Substring(startIdx, key.IndexOf('.')-startIdx);
                string ext = key.Substring(key.IndexOf('.'));
                loadKey = $"{key}[{fileName}]";
            }
            var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
            asyncOperation.Completed += (op) =>
            {
                string keyName = key;
                int folderIndex = key.IndexOf('/');
                if(folderIndex > 0)
                {
                    folderIndex += 1;
                    keyName = key.Substring(folderIndex, key.IndexOf('.') - folderIndex);
                }
                 

                // 캐시 확인.
                if (_resources.TryGetValue(keyName, out Object resource))
                {
                    callback?.Invoke(op.Result);
                    return;
                }

                _resources.Add(keyName, op.Result);
                callback?.Invoke(op.Result);
            };
        }
       


    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            
            int loadCount = 0;

            int totalCount = op.Result.Count;
            string[] spriteObjs = { ".sprite", ".multiplesprite", ".png", ".jpg",".jpeg" };


            foreach (var result in op.Result)
            {
                if (spriteObjs.Any(substring => result.PrimaryKey.Contains(substring)))
                {
                    LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
                else
                {
                    LoadAsync<T>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
            }
        };
    }



    #endregion
}
