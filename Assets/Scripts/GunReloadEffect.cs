using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunReloadEffect : MonoBehaviour
{
    private Quaternion targetRotation;
    public Transform gunSprite;
    private bool isCharging = false;
    public void ChargeAnim(float time)
    {
        targetRotation = gunSprite.rotation;
        targetRotation *= Quaternion.AngleAxis(90, Vector3.up);
        gunSprite.rotation = Quaternion.Lerp(gunSprite.rotation, targetRotation, 10 * Time.deltaTime);

        Invoke("StopCharge", time-1f);
    }

    void StopCharge()
    {
        targetRotation *= Quaternion.AngleAxis(0, Vector3.up);
        gunSprite.rotation = Quaternion.Lerp(gunSprite.rotation, targetRotation, 1 * Time.deltaTime);
    }
    IEnumerator StopCharging(float time)
    {
        if (isCharging)
            yield break;
        isCharging = true;
        yield return new WaitForSeconds(time);
        targetRotation *= Quaternion.AngleAxis(0, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        isCharging = false;
    }
}
