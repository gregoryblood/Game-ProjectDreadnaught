using UnityEngine;
using System.Collections.Generic;
public class BeamCannon : WeaponScript
{
    [SerializeField] float damage = 7;
    [SerializeField] int bounces = 5;
    [SerializeField] float bounceRange = 5f;
    LineRenderer beam;
    public LayerMask finalMask;
    List<Collider2D> prevTargets = new List<Collider2D>();
    bool hitActualTarget = false;
    public override void Prep()
    {
        beam = GetComponentsInChildren<LineRenderer>(true)[1];
        beam.startColor = ship.shipColor;
        beam.endColor = ship.shipColor;
        finalMask |= frigateLayer;
        finalMask |= shipLayer;
    }
    public override void Fire()
    {
        beam.positionCount = 1;
        prevTargets.Clear();
        //Setting data
        beam.enabled = true;
        beam.SetPosition(0, transform.position);
        Invoke("CancelBeam", 0.1f);
        BeamFire(currentTarget.transform.position, 1);
    }
    void BeamFire(Vector3 pos, int bouncesLeft)
    {
        if (bouncesLeft == bounces) return;
        else
        {
            Collider2D t = null;
            if (!hitActualTarget)
                t = currentTargetCol;
            else
            {
                //Look for big ships
                foreach (Collider2D target in Physics2D.OverlapCircleAll(pos, bounceRange, finalMask))
                {
                    if (!prevTargets.Contains(target))
                    {
                        t = target;
                        break;

                    }
                }
            }
            if (t)
            {
                if (shipMaster.DamageShip(t.GetInstanceID(), damage, t.transform.position))
                {
                    beam.positionCount += 1;
                    prevTargets.Add(t);
                    beam.SetPosition(bouncesLeft, t.attachedRigidbody.position);
                    BeamFire(t.attachedRigidbody.position, ++bouncesLeft);
                }
            }

        }
    }
    void CancelBeam()
    {
        beam.enabled = false;
    }
}
