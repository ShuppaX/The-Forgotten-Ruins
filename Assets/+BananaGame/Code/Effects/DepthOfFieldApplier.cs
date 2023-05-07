using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace BananaSoup.Effects
{
    public class DepthOfFieldApplier : MonoBehaviour
    {
        [SerializeField] private float updateTimer = 0.1f;
        [SerializeField] private float gameplayBlur = 10.0f;
        private Camera cam;
        private DepthOfField depthOfField;
        private Volume volume;
        private Coroutine updateRoutine;
        private float maxFarBlurRadius;

        public float SetUpdateTimer { set => updateTimer = value; }
        public float GetGameplayBlur => gameplayBlur;
        public float SetBlur
        {
            get
            {
                return maxFarBlurRadius;
            }
            set
            {
                maxFarBlurRadius = value;
                depthOfField.farMaxBlur = value;
            }
        }

        private void Start()
        {
            Setup();

            updateRoutine = StartCoroutine(UpdateDepthOfField());
        }

        private void OnDisable()
        {
            StopUpdateRoutine();
        }

        private void Setup()
        {
            cam = Camera.main;
            if ( cam == null )
            {
                Debug.LogError(name + " couldn't get a reference to a Main Camera!");
            }

            volume = GetComponent<Volume>();
            if ( volume == null )
            {
                Debug.LogError(name + " couldn't get a reference to a Volume!");
            }

            if ( volume.profile.TryGet(out DepthOfField dof) )
            {
                depthOfField = dof;
            }
            else
            {
                Debug.LogError(name + " couldn't get a reference to a Depth of Field!");
            }
        }

        public void StopUpdateRoutine()
        {
            if ( updateRoutine != null )
            {
                StopCoroutine(updateRoutine);
                updateRoutine = null;
            }
        }

        private IEnumerator UpdateDepthOfField()
        {
            yield return new WaitForSeconds(updateTimer);

            float distance = Vector3.Distance(cam.transform.position, PlayerBase.Instance.transform.position);
            depthOfField.focusDistance.value = distance;

            updateRoutine = null;

            updateRoutine = StartCoroutine(UpdateDepthOfField());
        }

        public void StartUpdating()
        {
            if ( updateRoutine == null )
            {
                updateRoutine = StartCoroutine(UpdateDepthOfField());
            }
        }
    }
}
