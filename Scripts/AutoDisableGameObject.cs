using UnityEngine;

public class AutoDisableGameObject : MonoBehaviour
{
    public float disableTime = 1f;

    protected float timer;

    protected void Update()
    {
        timer += Time.deltaTime;
        if (timer > disableTime)
            gameObject.SetActive(false);
    }

    protected void OnEnable()
    {
        timer = 0f;
    }
}
