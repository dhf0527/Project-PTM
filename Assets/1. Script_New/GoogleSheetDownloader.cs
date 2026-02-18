#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using Unity.EditorCoroutines.Editor;

public class GoogleSheetDownloader : EditorWindow
{
    // 탭 구분을 위한 Enum 추가
    public enum FileTag { None, WorldMap, Victory }
    private FileTag selectedTag = FileTag.None;

    private string rawSheetUrl = "https://docs.google.com/spreadsheets/d/117v_ABS7di3oVKaRIfUqTNK4F-nM-TAOKf2jEISA2Ks/edit#gid=0";
    private string fileName = "1-1";
    private string saveFolder = "Assets/Resources/CSV/";
    private bool isDownloading = false;

    [MenuItem("Tools/Google Sheet CSV Downloader")]
    public static void ShowWindow()
    {
        GetWindow<GoogleSheetDownloader>("Google Sheet Downloader");
    }

    private string ConvertToCsvUrl(string url)
    {
        try
        {
            string[] splitUrl = url.Split('/');
            if (splitUrl.Length < 6) return url;

            string sheetId = splitUrl[5];
            var match = System.Text.RegularExpressions.Regex.Match(url, @"gid=(\d+)");
            string gid = match.Success ? match.Groups[1].Value : "0";

            return $"https://docs.google.com/spreadsheets/d/{sheetId}/export?format=csv&gid={gid}";
        }
        catch
        {
            return url;
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Google Sheet CSV 다운로드 설정", EditorStyles.boldLabel);

        rawSheetUrl = EditorGUILayout.TextField("Google Sheet URL", rawSheetUrl);
        fileName = EditorGUILayout.TextField("기본 파일 이름", fileName);

        // --- 드롭다운(탭) 추가 ---
        selectedTag = (FileTag)EditorGUILayout.EnumPopup("저장 카테고리 (태그)", selectedTag);

        // 미리보기 이름 표시 (사용자 확인용)
        string previewName = selectedTag == FileTag.None ? $"{fileName}.csv" : $"{fileName}{selectedTag}.csv";
        EditorGUILayout.HelpBox($"저장될 파일명: {previewName}", MessageType.Info);
        // -----------------------

        EditorGUILayout.Space();

        GUI.enabled = !isDownloading;
        if (GUILayout.Button(isDownloading ? "다운로드 중..." : "CSV 다운로드 및 저장"))
        {
            string finalUrl = ConvertToCsvUrl(rawSheetUrl);
            // 파일명 뒤에 선택된 태그 문자열을 붙여서 전달
            EditorCoroutineUtility.StartCoroutine(DownloadCSV(finalUrl, previewName), this);
        }
        GUI.enabled = true;
    }

    IEnumerator DownloadCSV(string url, string fullFileName)
    {
        isDownloading = true;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            File.WriteAllText(saveFolder + fullFileName, www.downloadHandler.text);
            Debug.Log($"<color=cyan><b>[알림]</b></color> {fullFileName} 저장 완료!");
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError($"다운로드 실패: {www.error}");
        }

        isDownloading = false;
    }
}
#endif