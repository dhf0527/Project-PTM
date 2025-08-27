using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchManager : MonoBehaviour
{
    public static SearchManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Search(SearchKey searchKey)
    {
        foreach (var item in FindObjectsOfType<SearchObject>())
        {
            if (item.searchKey == searchKey)
                return item.gameObject;
        }

        Debug.LogError("오브젝트 검색 실패");
        return null;
    }
}
