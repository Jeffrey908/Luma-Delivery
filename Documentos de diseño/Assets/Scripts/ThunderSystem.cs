using UnityEngine;
using System.Collections;

public class ThunderSystem : MonoBehaviour
{
    public Light lightningLight;
    public AudioSource thunderAudio;

    public float minTime = 5f;
    public float maxTime = 15f;

    void Start()
    {
        StartCoroutine(ThunderLoop());
    }

    IEnumerator ThunderLoop()
    {
        while (true)
        {
            float wait = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(wait);

            StartCoroutine(DoThunder());
        }
    }

    IEnumerator DoThunder()
    {
        int flashes = Random.Range(1, 4);

        for (int i = 0; i < flashes; i++)
        {
            lightningLight.enabled = true;
            lightningLight.intensity = Random.Range(2f, 5f);

            yield return new WaitForSeconds(0.1f);

            lightningLight.enabled = false;

            yield return new WaitForSeconds(0.1f);
        }

        // Decide si el sonido va sincronizado o no para darle mas realismo
        bool sync = Random.value > 0.5f;

        if (sync)
        {
            // Sonido inmediato (coordinado con el rayo)
            thunderAudio.Play();
        }
        else
        {
            // Delay realista (desfasado)
            float delay = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(delay);

            thunderAudio.Play();
        }
    }
}