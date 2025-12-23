using UnityEngine;

public class ArmTrigger : MonoBehaviour
{
    [Header("Настройки анимации")]
    public Animator armAnimator;
    public string triggerName = "ActivateRobot"; // Имя твоего Trigger в Animator

    [Header("Настройки тележки")]
    public string trolleyNameContains = "uploads_files_4270070_Trolley-shared";
    public string trolleyTag = "Trolley"; // Рекомендую использовать тег

    private void OnTriggerEnter(Collider other)
    {
        bool isTrolley = other.gameObject.name.Contains(trolleyNameContains) ||
                         other.CompareTag(trolleyTag);

        if (isTrolley)
        {
            Debug.Log("Тележка заехала — запускаем анимацию робот-руки!");

            if (armAnimator != null)
            {
                // Запускаем триггер каждый раз при въезде
                armAnimator.SetTrigger(triggerName);
            }
        }
    }

    // Опционально: сброс триггера при выезде (чтобы анимация могла прерваться и запуститься заново)
    private void OnTriggerExit(Collider other)
    {
        bool isTrolley = other.gameObject.name.Contains(trolleyNameContains) ||
                         other.CompareTag(trolleyTag);

        if (isTrolley && armAnimator != null)
        {
            // Сбрасываем триггер — это позволяет запустить анимацию снова при следующем въезде
            armAnimator.ResetTrigger(triggerName);
        }
    }
}