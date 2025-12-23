using UnityEngine;

public class LatheController : MonoBehaviour
{
    public Transform carriageZ;
    public Transform toolHolderX;
    public Transform spindle;
    public Renderer statusLight;
    public ParticleSystem chipsParticles; // Стружка

    public float scale = 0.001f; // Подбери под модель (мм → метры)

    private float spindleSpeed = 0;

    public void UpdateFromData(CNCData data)
    {
        // Z — продольная каретка
        carriageZ.localPosition = new Vector3(0, 0, data.axes.z.current_position * scale);

        // X — поперечный резец
        toolHolderX.localPosition = new Vector3(data.axes.x.current_position * scale, 0, 0);

        // C — поворот заготовки
        spindle.localEulerAngles = new Vector3(0, data.axes.c.current_position, 0); // Подбери ось

        spindleSpeed = data.spindle.speed;

        // Статус
        statusLight.material.color = data.status == "running" ? Color.green : Color.red;

        // Стружка при обработке
        if (spindleSpeed > 100) chipsParticles.Play(); else chipsParticles.Stop();
    }

    void Update()
    {
        if (spindleSpeed > 0)
        {
            float rot = spindleSpeed * 360f / 60f * Time.deltaTime;
            spindle.Rotate(Vector3.right, rot); // Горизонтальный шпиндель — подбери ось (right/forward)
        }
    }
}