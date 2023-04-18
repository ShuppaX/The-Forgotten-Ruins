using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.Traps
{
    public class SpikeTrap : Trap
    {
        [SerializeField] private Transform trapDamagerObject;
        [SerializeField] private AnimationCurve activationCurve;
        private AnimationCurve deactivationCurve = new AnimationCurve();
        private float trapTimer = 0.0f;
        private int activationCurveLastIndex = 0;
        private int deactivationCurveLastIndex = 0;
        private Coroutine trapRoutine;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            // Get last point of the activationCurve.
            activationCurveLastIndex = (activationCurve.length - 1);

            SettingUpDeactivationCurve();
        }

        private void SettingUpDeactivationCurve()
        {
            // Ensure deactivationCurve is empty.
            if ( deactivationCurve.length > 0 )
            {
                for ( int i = (deactivationCurve.length - 1); i > -1; i-- )
                {
                    deactivationCurve.RemoveKey(i);
                }
            }

            // Adding keys from the code.
            deactivationCurve.AddKey(0.0f, activationCurve[activationCurveLastIndex].value);
            deactivationCurve.AddKey(GetCooldown, 0.0f);

            // Get last point of the deactivationCurve.
            deactivationCurveLastIndex = (deactivationCurve.length - 1);
        }

        private void OnDisable()
        {
            StopAndNullRoutine();
        }

        public override void ActivateTrap()
        {
            if ( trapRoutine == null )
            {
                trapRoutine = StartCoroutine(MoveSpikesRoutine());
            }
        }

        private IEnumerator MoveSpikesRoutine()
        {
            trapTimer = 0.0f;

            while ( trapTimer < activationCurve[activationCurveLastIndex].time )
            {
                trapTimer += Time.deltaTime;

                Vector3 newPosition = Vector3.zero;
                newPosition.Set(trapDamagerObject.transform.position.x,
                                activationCurve.Evaluate(trapTimer),
                                trapDamagerObject.transform.position.z);

                trapDamagerObject.transform.position = newPosition;
                yield return null;
            }

            if ( IsRepeatable )
            {
                trapRoutine = null;
                trapRoutine = StartCoroutine(ReactivatingTrap());
            }
        }

        private IEnumerator ReactivatingTrap()
        {
            trapTimer = 0.0f;

            while ( trapTimer < deactivationCurve[deactivationCurveLastIndex].time )
            {
                trapTimer += Time.deltaTime;

                Vector3 newPosition = Vector3.zero;
                newPosition.Set(trapDamagerObject.transform.position.x,
                                deactivationCurve.Evaluate(trapTimer),
                                trapDamagerObject.transform.position.z);
                trapDamagerObject.transform.position = newPosition;

                yield return null;
            }

            trapRoutine = null;
            IsTrapActivated = false;
        }

        private void StopAndNullRoutine()
        {
            if ( trapRoutine != null )
            {
                StopCoroutine(trapRoutine);
                trapRoutine = null;
            }
        }
    }
}
