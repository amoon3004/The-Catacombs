using UnityEngine;
using UnityEngine.Rendering.Universal;

// Controls player's torch for the power up ability
[RequireComponent(typeof(Light2D))]
public class TorchLight : MonoBehaviour
{
    public float boostedOuterRadius = 6f;
    public float changeSpeed = 4f;

    Light2D torch;
    float normalOuterRadius;
    float boostTimeLeft;

    void Awake()
    {
        torch = GetComponent<Light2D>();
        normalOuterRadius = torch.pointLightOuterRadius;
    }

    // Called by power up. Picking up a second one restarts the timer
    public void Boost(float seconds)
    {
        boostTimeLeft = seconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (boostTimeLeft > 0f) boostTimeLeft -= Time.deltaTime;

        // Head toward big radius while boosted, otherwise back to normal
        float target = boostTimeLeft > 0f ? boostedOuterRadius : normalOuterRadius;
        torch.pointLightOuterRadius = Mathf.MoveTowards(torch.pointLightOuterRadius, target, changeSpeed * Time.deltaTime);
    }
}
