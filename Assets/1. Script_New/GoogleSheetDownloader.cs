#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using Unity.EditorCoroutines.Editor;

//GPT사용
public class GoogleSheetDownloader : EditorWindow
{
    private string sheetUrl = "https://docs.google.com/spreadsheets/d/117v_ABS7di3oVKaRIfUqTNK4F-nM-TAOKf2jEISA2Ks/edit?gid=0#gid=0/export?format=csv";
    private string fileName = "1-1";
    private string saveFolder = "Assets/Resources/CSV/";

    [MenuItem("Tools/Google Sheet CSV Downloader")]
    public static void ShowWindow()
    {
        GetWindow<GoogleSheetDownloader>("Google Sheet Downloader");
    }

    // 자동으로 URL 생성
    private string GetCsvUrlFromSheetUrl(string url)
    {
        // ID 추출
        string sheetId = url.Split('/')[5]; // URL의 5번째 부분이 ID
        // gid 추출
        string gid = GetGidFromUrl(url);
        return $"https://docs.google.com/spreadsheets/d/{sheetId}/export?format=csv&gid={gid}";
    }

    private string GetGidFromUrl(string url)
    {
        // 정규 표현식을 사용하여 gid 값 추출
        var match = System.Text.RegularExpressions.Regex.Match(url, @"[?&]gid=(\d+)");

        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        return string.Empty;  // gid가 없으면 빈 문자열 반환
    }

    void OnGUI()
    {
        GUILayout.Label("Google Sheet CSV 다운로드", EditorStyles.boldLabel);
        sheetUrl = GetCsvUrlFromSheetUrl(EditorGUILayout.TextField("Google Sheet URL", sheetUrl));
        fileName = EditorGUILayout.TextField("CSV 파일 이름", fileName);

        if (GUILayout.Button("CSV 다운로드 및 저장"))
        {
            EditorCoroutineUtility.StartCoroutine(DownloadCSV(sheetUrl, fileName + ".csv"), this);
        }
    }

    IEnumerator DownloadCSV(string url, string fileName)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            File.WriteAllText(saveFolder + fileName, www.downloadHandler.text);
            Debug.Log($"CSV 저장 완료: {saveFolder + fileName}");
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("CSV 다운로드 실패: " + www.error);
        }
    }
}
#endif