using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MultiCraft.Helpers;

[System.Serializable]
public class NexusMod
{
    public string tag_name;
}

public class UpdateHelper : MonoBehaviour
{
    private const string URL = "https://api.github.com/repos/xhexifx/multicraft/releases/latest";

    public static IEnumerator checkForUpdate()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            request.timeout = 10;
            
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Plugin.Logger.LogError($"Failed to update check MultiCraft: {request.error}");
                ErrorMessage.AddDebug($"Failed to update check MultiCraft: {request.error}");
                yield break;
            }
            
            string json = request.downloadHandler.text;

            NexusMod mod = JsonUtility.FromJson<NexusMod>(json);

            if (PluginInfo.PLUGIN_VERSION != mod.tag_name)
            {
                Plugin.Logger.LogInfo($"New version of MultiCraft is available: {mod.tag_name}");
                ErrorMessage.AddDebug($"New version of MultiCraft is available: {mod.tag_name} (Current: {PluginInfo.PLUGIN_VERSION})");
            }
        }
    }
}