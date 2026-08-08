using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DummyCanvas : MonoBehaviour
{
    public Slider healthMeterSlider;
    public Slider staggerMeterSlider;
    public float damage;
    private void Start()
    {
        staggerMeterSlider.value = 0;
        healthMeterSlider.value = healthMeterSlider.maxValue;
    }

    public void onHit()
    {
        healthMeterSlider.value -= damage;
        staggerMeterSlider.value += Random.Range(5f,16f);

        if (staggerMeterSlider.value >= staggerMeterSlider.maxValue)
        {
            staggerMeterSlider.value = staggerMeterSlider.maxValue;
        }


        if (healthMeterSlider.value <= 0f)
        {
            StartCoroutine("ResetStats");
        }

    }

    private IEnumerator ResetStats()
    {
        staggerMeterSlider.value -= 0.001f;
        healthMeterSlider.value +=Time.deltaTime * 2f;
        staggerMeterSlider.value = 0;
        yield return new WaitForSeconds(1.5f);
        healthMeterSlider.value = healthMeterSlider.maxValue;
    }

    private void Update()
    {
        //staggerMeterSlider.value -= 0.001f;

        if (staggerMeterSlider.value  <= 0f)
        {
            staggerMeterSlider.value = 0;
        }

    }
}
