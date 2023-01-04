using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class MAASLogin 
{
    public const string MAAS_TOKEN = "MAAS_TOKEN";
    public const string MAAS_REFRESH_TOKEN = "MAAS_REFRESH_TOKEN";
    public const string MAAS_TOKEN_EXPIRE_DATE = "MAAS_TOKEN_EXPIRE_DATE";
    public const string MAAS_GUEST_USER = "MAAS_GUEST_USER";

    public static MAASToken PostLogin(string url, MAASTokenRequest login, string refreshToken = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("client_id", login.client_id);
        if (refreshToken == null)
        {

            form.AddField("scope", login.scope);
            form.AddField("response_type", login.response_type);
            form.AddField("username", login.username);
            form.AddField("password", login.password);
            form.AddField("grant_type", login.grant_type);
        }
        else
        {
            form.AddField("resource", login.client_id);
            form.AddField("grant_type", "refresh_token");
            form.AddField("response_type", "id_token");
            form.AddField("refresh_token", refreshToken);
        }
        var request = UnityWebRequest.Post(url, form);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SendWebRequest();

        while (!request.isDone) { }


        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("<color=red>Error posting the object: </color>" + request.error);
            return null;
        }

        var token = DeserializeJson<MAASToken>(request);


        return token;
    }

    public static T DeserializeJson<T>(UnityWebRequest request)
    {
        T Returnobj;
        if (request.downloadHandler.data == null) return default(T);
        string reader = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);

        //TODO Need to remove anything that is sending an empty collection
        if (typeof(T) == typeof(string))
        {
            return (T)Convert.ChangeType(reader, typeof(T));
        }
        if (reader != @"[]" && reader != "")
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings() { Culture = new CultureInfo("en-US")};
            Returnobj = JsonConvert.DeserializeObject<T>(reader, jsonSettings);
        }
        else
            Returnobj = default;
        return Returnobj;
    }

}
