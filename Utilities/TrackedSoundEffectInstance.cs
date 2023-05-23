using AnotherLib;
using AnotherLib.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AnotherLib.Utilities
{
    public class TrackedSoundEffectInstance
    {
        public Vector2 soundPosition;
        public SoundEffectInstance soundInstance;
        public CollisionBody soundSource;
        public float soundTravelDistance;
        public int soundVolumeApexDistance;
        public float soundVolumeMult;
        public bool hasSource;
        public bool preserveInstance;

        public static TrackedSoundEffectInstance CreateTrackedSound(SoundEffectInstance soundInstance, Vector2 soundPosition, bool preserveInstance, float soundTravelDistance = 10f, float soundVolumeMult = 1f)
        {
            TrackedSoundEffectInstance newInstance = new TrackedSoundEffectInstance();
            newInstance.soundInstance = soundInstance;
            newInstance.soundPosition = soundPosition;
            newInstance.soundTravelDistance = soundTravelDistance;
            newInstance.soundVolumeApexDistance = (int)newInstance.soundTravelDistance / 2;
            newInstance.soundVolumeMult = soundVolumeMult;
            newInstance.preserveInstance = preserveInstance;
            return newInstance;
        }

        public static TrackedSoundEffectInstance CreateTrackedSound(SoundEffectInstance soundInstance, CollisionBody soundSource, bool preserveInstance, float soundTravelDistance = 10f, float soundVolumeMult = 1f)
        {
            TrackedSoundEffectInstance newInstance = new TrackedSoundEffectInstance();
            newInstance.soundInstance = soundInstance;
            newInstance.soundSource = soundSource;
            newInstance.soundTravelDistance = soundTravelDistance;
            newInstance.soundVolumeApexDistance = (int)newInstance.soundTravelDistance / 2;
            newInstance.soundVolumeMult = soundVolumeMult;
            newInstance.preserveInstance = preserveInstance;
            newInstance.hasSource = true;

            return newInstance;
        }


        public void UpdateAudioInformation()
        {
            if (hasSource)
            {
                if (soundSource != null)
                    soundPosition = soundSource.position;
            }

            float distanceFromSource = MathHelper.Clamp(Vector2.Distance(GameData.AudioPosition, soundPosition) - soundVolumeApexDistance, 0, soundTravelDistance);
            if (distanceFromSource >= soundTravelDistance)
                return;

            float currentSoundVolume = ((soundTravelDistance - distanceFromSource) / soundTravelDistance) * soundVolumeMult * GameData.SoundEffectVolume;
            float currentSoundPan = (soundPosition.X - GameData.AudioPosition.X) / soundTravelDistance;
            currentSoundVolume = MathHelper.Clamp(currentSoundVolume, 0f, 1f);
            currentSoundPan = MathHelper.Clamp(currentSoundPan, 0f, 1f);
            if (distanceFromSource == 0f)
                currentSoundPan = 0;

            soundInstance.Volume = currentSoundVolume;
            soundInstance.Pan = currentSoundPan;
        }

        public void UpdateSourcePosition(Vector2 sourcePosition)
        {
            soundPosition = sourcePosition;
        }
    }
}
