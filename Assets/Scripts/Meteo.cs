using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Meteo : MonoBehaviour
{
    public HeaderMeteo headerMeteo;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("https://servizos.meteogalicia.gal/mgrss/predicion/jsonCPrazo.action?dia=-1&request_locale=es"));
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }

            //aquí hacemos que el json se carge en el objeto
            headerMeteo = JsonUtility.FromJson<HeaderMeteo>(webRequest.downloadHandler.text);
            Debug.Log("Comentario: " + headerMeteo.listaPredicions[0].comentario + "\n" + "DataActualizacion " + headerMeteo.listaPredicions[0].dataActualizacion+"\n"+"Titulo: "+headerMeteo.listaPredicions[0].titulo);
        }
    }
}
