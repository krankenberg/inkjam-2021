using UnityEngine;

public class Torch : MonoBehaviour
{
    public float FlickerAmount = 0.2F;
    public float MinTime = 0.1F;
    public float MaxTime = 0.3F;

    private Light _light;
    private float _startIntensity;

    private float _timeUntilNextChange = 0.1F;

    private void Start()
    {
        _light = GetComponent<Light>();
        _startIntensity = _light.intensity;
    }

    private void Update()
    {
        if (_timeUntilNextChange <= 0)
        {
            _light.intensity = Random.Range(_startIntensity - FlickerAmount, _startIntensity + FlickerAmount);
            _timeUntilNextChange += Random.Range(MinTime, MaxTime);
        }
        else
        {
            _timeUntilNextChange -= Time.deltaTime;
        }
    }
}
