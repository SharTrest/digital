using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro; // Для TextMeshPro (3D)

[System.Serializable]
public class Axis { public float current_position; }
[System.Serializable]
public class Axes { public Axis x, y, z, a, c; }
[System.Serializable]
public class Spindle { public float speed; }
[System.Serializable]
public class CNCData { public string machine_id; public string status; public Axes axes; public Spindle spindle; }
[System.Serializable]
public class Wrapper { public CNCData cnc_machine; }

public class JSONFetcher : MonoBehaviour
{
    public string url = "https://karasevv.com/test/mt_data.json";
    public float updateInterval = 2f;

    [Header("3D Текст в мире")]
    public TextMeshPro worldText; // Перетащи сюда объект TextMeshPro (3D)

    void Start()
    {
        if (worldText == null)
        {
            Debug.LogWarning("TextMeshPro (3D) не назначен! Назначь в Inspector.");
        }
        StartCoroutine(GetDataRoutine());
    }

    IEnumerator GetDataRoutine()
    {
        while (true)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Wrapper wrapper = JsonUtility.FromJson<Wrapper>(www.downloadHandler.text);

                    if (wrapper?.cnc_machine != null)
                    {
                        CNCData cnc = wrapper.cnc_machine;

                        string display = $"<b>{cnc.machine_id}</b>\n" +
                                         $"Статус: {cnc.status}\n" +
                                         $"Шпиндель: {cnc.spindle.speed} об/мин\n" +
                                         $"X: {cnc.axes.x.current_position:F1} мм\n" +
                                         $"Y: {cnc.axes.y.current_position:F1} мм\n" +
                                         $"Z: {cnc.axes.z.current_position:F1} мм\n" +
                                         $"C: {cnc.axes.c.current_position:F1}°";

                        // Выводим прямо в свойство .text объекта
                        if (worldText != null)
                        {
                            worldText.text = display;
                        }
                    }
                }
                else
                {
                    if (worldText != null)
                    {
                        worldText.text = "Нет связи";
                    }
                }
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}