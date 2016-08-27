// Date   : 27.08.2016 23:22
// Project: LD36
// Author : bradur

using UnityEngine;

[RequireComponent(typeof(Light))]
public class RandomFlicker : MonoBehaviour {

    float random;
    [SerializeField]
    [Range(0f, 2f)]
    float minIntensity;

    [SerializeField]
    [Range(2f, 8f)]
    float maxIntensity;

    [SerializeField]
    private Light lightObject;

    void Start () {
        random = Random.Range(0f, 1000f);
    }

    void Update () {
        float noise = Mathf.PerlinNoise(random, Time.time);
        lightObject.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }


}
