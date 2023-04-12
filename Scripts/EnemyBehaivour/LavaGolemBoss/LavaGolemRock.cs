using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class LavaGolemRock : MonoBehaviour
{
    public float Speed;
    EnemyHitCenter EHC;
    private void Start()
    {
        EHC = gameObject.GetComponent<EnemyHitCenter>();
        this.transform.LookAt(FPSController.PlayerObject.transform);
    }
    void Update()
    {
        try
        {
            if (EHC == null)
            {
                EHC = gameObject.GetComponent<EnemyHitCenter>();
            }
            // this.transform.LookAt(FPSController.FocusTransform.transform);
            //this.transform.LookAt(FPSController.FocusTransform.position);
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(FPSController.PlayerObject.transform);
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, 2.5f * Time.deltaTime);
            this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            // this.transform.Translate(this.transform.forward * Speed * Time.deltaTime, Space.World);
        }
        catch
        {
            Debug.LogError("Error Found Destroying Corrupted Object");
            Destroy(this.gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            EffectManager UIS = PostProcessingManager.instance.gameObject.GetComponent<EffectManager>();
            UIS.CreateEnemyHit(this.transform.position, true, this.transform.GetComponent<Renderer>().materials[0], this.transform.GetComponent<Renderer>().materials[0]);
            UISfxManager U = PostProcessingManager.instance.gameObject.GetComponent<UISfxManager>();
            U.PlayRockHitSound();
            CameraShaker.Instance.ShakeOnce(2, 2, 0, 1);
            if (collision.transform.tag == "Player")
            {
                EHC.HitPlayer();
            }
            if (collision.transform.tag == "EnemyMob")
            {
                if (collision.gameObject.GetComponent<LavaGolemMain>() != null)
                {
                    collision.gameObject.GetComponent<LavaGolemMain>().gameObject.GetComponent<EnemyMainStat>().MobHP += 1000;
                }
            }
            if (collision.transform.tag == "DestructableObject")
            {
                DestructableObject DO = collision.transform.GetComponent<DestructableObject>();
                int HitPoint = DO.GetHitPoint();
                if (HitPoint == 1)
                {
                    DO.Explode();
                }
                else
                {
                    DO.DecreaseHitPoint(1);
                }
            }
            Destroy(this.gameObject);
        }
        catch
        {
            Debug.LogError("Error Found Destroying Corrupted Object");
            Destroy(this.gameObject);
        }
    }
}
