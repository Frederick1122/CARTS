using UnityEngine;

namespace OldCode
{
    public class CarVFXController : MonoBehaviour
    {
        [Space(10)] public bool useEffects = false;

        public ParticleSystem RLWParticleSystem;
        public ParticleSystem RRWParticleSystem;

        [Space(10)] public TrailRenderer RLWTireSkid;
        public TrailRenderer RRWTireSkid;

        private CarController carController;

        private void Start()
        {
            carController = GetComponent<CarController>();

            if (!useEffects)
            {
                if (RLWParticleSystem != null)
                    RLWParticleSystem.Stop();

                if (RRWParticleSystem != null)
                    RRWParticleSystem.Stop();

                if (RLWTireSkid != null)
                    RLWTireSkid.emitting = false;

                if (RRWTireSkid != null)
                    RRWTireSkid.emitting = false;
            }
        }

        private void FixedUpdate() =>
            DriftCarPS();

        public void DriftCarPS()
        {
            if (useEffects)
            {
                if (carController.IsDrifting)
                {
                    RLWParticleSystem.Play();
                    RRWParticleSystem.Play();
                }
                else if (!carController.IsDrifting)
                {
                    RLWParticleSystem.Stop();
                    RRWParticleSystem.Stop();
                }

                if ((carController.IsTractionLocked || Mathf.Abs(carController.LocalVelocityX) > 5f) &&
                    Mathf.Abs(carController.CarSpeed) > 12f)
                {
                    RLWTireSkid.emitting = true;
                    RRWTireSkid.emitting = true;
                }
                else
                {
                    RLWTireSkid.emitting = false;
                    RRWTireSkid.emitting = false;
                }
            }
            else if (!useEffects)
            {
                if (RLWParticleSystem != null)
                    RLWParticleSystem.Stop();

                if (RRWParticleSystem != null)
                    RRWParticleSystem.Stop();

                if (RLWTireSkid != null)
                    RLWTireSkid.emitting = false;

                if (RRWTireSkid != null)
                    RRWTireSkid.emitting = false;
            }
        }
    }
}