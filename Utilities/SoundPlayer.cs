using AnotherLib.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace AnotherLib.Utilities
{
    public class SoundPlayer
    {
        public static SoundEffect[] sounds;
        public static SoundEffect[] ambienceSounds;

        public const float ShortSoundLength = 1.5f;
        public static List<TrackedSoundEffectInstance> activeSoundEffects;

        public static void Initialize()
        {
            activeSoundEffects = new List<TrackedSoundEffectInstance>();
        }

        public static void UpdateActiveSounds()
        {
            TrackedSoundEffectInstance[] activeSoundsCopy = activeSoundEffects.ToArray();
            foreach (TrackedSoundEffectInstance activeSound in activeSoundsCopy)
            {
                if (activeSound.soundInstance.State == SoundState.Stopped)
                {
                    RemoveActiveSound(activeSound);
                    continue;
                }
                if (activeSound.hasSource)
                {
                    if (activeSound.soundSource == null)
                    {
                        RemoveActiveSound(activeSound);
                        continue;
                    }
                }

                activeSound.UpdateAudioInformation();
            }
        }

        /// <summary>
        /// Plays a sound with no effects applied to it. (Good for UI and non-ingame things)
        /// </summary>
        /// <param name="soundType">Type of sound. Sounds types are available in SoundPlayer.</param>
        /// <param name="volumeMult">The multiplier from 0f - 1f of the sound's volume. Multiplied by GameData.SoundEffectVolume.</param>
        /// <param name="pitch">The pitch of the sound.</param>
        /// <param name="pan">The pan of the sound (left) -1 to 1 (right)</param>
        public static void PlayLocalSound(int soundType, float volumeMult = 1f, float pitch = 0f, float pan = 0f)
        {
            float volume = GameData.SoundEffectVolume * volumeMult;
            if (volume > 0.01f)
                sounds[soundType].Play(volume, pitch, pan);
        }

        /// <summary>
        /// Plays a sound with volume drop-off and position-based panning. Doesn't do anything if player is too far from the sound source.
        /// </summary>
        /// <param name="soundType">Type of sound. Sounds types are available in SoundPlayer.</param>
        /// <param name="soundPosition">The sound's position.</param>
        /// <param name="soundTravelDistance">The distance the sound travels (in tiles).</param>
        /// <param name="soundVolumeMultiplier">The multiplier from 0f - 1f of the sound's volume. Multiplied by GameData.SoundEffectVolume.</param>
        /// <param name="soundPitch">The pitch of the sound.</param>
        public static void PlaySoundFromOtherSource(int soundType, Vector2 soundPosition, float soundTravelDistance = 10f, float soundVolumeMultiplier = 1f, float soundPitch = 0f)
        {
            if (GameData.SoundEffectVolume <= 0.01f)
                return;

            float travelDist = soundTravelDistance * 16;
            int volumeApexDistance = (int)travelDist / 2;       //So that if the player gets close enough the volume doesn't go higher but instead stays at that volume
            float distanceFromSource = MathHelper.Clamp(Vector2.Distance(GameData.AudioPosition, soundPosition) - volumeApexDistance, 0, travelDist);
            bool isShortSound = sounds[soundType].Duration.TotalSeconds < ShortSoundLength;

            if (isShortSound && distanceFromSource >= travelDist)
                return;

            float soundVolume = ((travelDist - distanceFromSource) / travelDist) * soundVolumeMultiplier * GameData.SoundEffectVolume;
            float soundPan = (soundPosition.X - GameData.AudioPosition.X) / travelDist;            //If Pan is not working, remember to set the audio to mono. (Audacity->Right-Click Stereo-> Split to Mono)
            soundVolume = Math.Clamp(soundVolume, 0f, 1f);
            soundPan = Math.Clamp(soundPan, -1, 1);
            if (distanceFromSource == 0f)
                soundPan = 0;

            if (isShortSound)
            {
                sounds[soundType].Play(soundVolume, soundPitch, soundPan);
            }
            else
            {
                TrackedSoundEffectInstance newInstance = TrackedSoundEffectInstance.CreateTrackedSound(sounds[soundType].CreateInstance(), soundPosition, false, soundTravelDistance);
                newInstance.soundVolumeMult = soundVolumeMultiplier;
                newInstance.soundInstance.Pitch = soundPitch;
                newInstance.soundInstance.Volume = soundVolume;
                newInstance.soundInstance.Pan = soundPan;
                newInstance.soundInstance.Play();
                activeSoundEffects.Add(newInstance);
            }
        }

        /// <summary>
        /// Plays a sound with volume drop-off and position-based panning. Doesn't do anything if player is too far from the sound source.
        /// </summary>
        /// <param name="soundType">Type of sound. Sounds types are available in SoundPlayer.</param>
        /// <param name="soundSource">The sound's source. This source is tracked and the position of the sound is updated along with the source.</param>
        /// <param name="soundTravelDistance">The distance the sound travels (in tiles).</param>
        /// <param name="soundVolumeMult">The multiplier from 0f - 1f of the sound's volume. Multiplied by GameData.SoundEffectVolume.</param>
        /// <param name="soundPitch">The pitch of the sound.</param>
        public static void PlaySoundFromOtherSource(int soundType, CollisionBody soundSource, float soundTravelDistance = 10f, float soundVolumeMult = 1f, float soundPitch = 0f)
        {
            if (GameData.SoundEffectVolume <= 0.01f)
                return;

            float travelDist = soundTravelDistance * 16;
            int volumeApexDistance = (int)travelDist / 2;       //So that if the player gets close enough the volume doesn't go higher but instead stays at that volume
            Vector2 soundPosition = soundSource.position;
            float distanceFromSource = MathHelper.Clamp(Vector2.Distance(GameData.AudioPosition, soundPosition) - volumeApexDistance, 0, travelDist);
            bool isShortSound = sounds[soundType].Duration.TotalSeconds < ShortSoundLength;

            if (isShortSound && distanceFromSource >= travelDist)
                return;

            float soundVolume = ((travelDist - distanceFromSource) / travelDist) * soundVolumeMult * GameData.SoundEffectVolume;
            float soundPan = ((soundPosition.X - GameData.AudioPosition.X) / 16f) / travelDist;
            soundVolume = Math.Clamp(soundVolume, 0f, 1f);
            soundPan = Math.Clamp(soundPan, -1, 1);
            if (distanceFromSource == 0f)
                soundPan = 0;

            if (isShortSound)
            {
                sounds[soundType].Play(soundVolume, soundPitch, soundPan);
            }
            else
            {
                TrackedSoundEffectInstance newInstance = TrackedSoundEffectInstance.CreateTrackedSound(sounds[soundType].CreateInstance(), soundSource, false, soundTravelDistance);
                newInstance.soundVolumeMult = soundVolumeMult;
                newInstance.soundInstance.Pitch = soundPitch;
                newInstance.soundInstance.Volume = soundVolume;
                newInstance.soundInstance.Pan = soundPan;
                newInstance.soundInstance.Play();
                activeSoundEffects.Add(newInstance);
            }
        }

        /// <summary>
        /// Plays a tracked sound with volume drop-off and position-based panning. Doesn't do anything if player is too far from the sound source. Forces a tracked sound instance.
        /// </summary>
        /// <param name="soundType">Type of sound. Sounds types are available in SoundPlayer.</param>
        /// <param name="soundPosition">The sound's position. Determines volume and pan based on the distance from the player.</param>
        /// <param name="preserveInstance">Whether or not to delete this TrackedSoundInstance after use.</param>
        /// <param name="soundTravelDistance">The distance the sound travels (in tiles).</param>
        /// <param name="soundPlayMult">The multiplier from 0f - 1f of the sound's volume. Multiplied by GameData.SoundEffectVolume.</param>
        /// <param name="soundPitch">The pitch of the sound.</param>
        public static TrackedSoundEffectInstance PlayTrackedSoundFromOtherSource(int soundType, Vector2 soundPosition, bool preserveInstance, float soundTravelDistance = 10f, float soundPlayMult = -1f, float soundPitch = 0f)
        {
            if (GameData.SoundEffectVolume <= 0.01f)
                return null;

            float travelDist = soundTravelDistance * 16;
            int volumeApexDistance = (int)travelDist / 2;       //So that if the player gets close enough the volume doesn't go higher but instead stays at that volume
            float distanceFromSource = MathHelper.Clamp(Vector2.Distance(GameData.AudioPosition, soundPosition) - volumeApexDistance, 0, travelDist);
            float soundVolume = ((travelDist - distanceFromSource) / travelDist) * soundPlayMult * GameData.SoundEffectVolume;
            float soundPan = ((soundPosition.X - GameData.AudioPosition.X) / 16f) / travelDist;
            soundVolume = Math.Clamp(soundVolume, 0f, 1f);
            soundPan = Math.Clamp(soundPan, -1, 1);
            if (distanceFromSource == 0f)
                soundPan = 0;

            TrackedSoundEffectInstance newInstance = TrackedSoundEffectInstance.CreateTrackedSound(sounds[soundType].CreateInstance(), soundPosition, preserveInstance, soundTravelDistance);
            newInstance.soundVolumeMult = soundPlayMult;
            newInstance.soundInstance.Pitch = soundPitch;
            newInstance.soundInstance.Volume = soundVolume;
            newInstance.soundInstance.Pan = soundPan;
            newInstance.soundInstance.Play();
            return newInstance;
        }

        /// <summary>
        /// Plays a tracked sound with volume drop-off and position-based panning. Doesn't do anything if player is too far from the sound source. Forces a tracked sound instance.
        /// </summary>
        /// <param name="soundType">Type of sound. Sounds types are available in SoundPlayer.</param>
        /// <param name="soundSource">The sound's source. This source is tracked and the position of the sound is updated along with the source.</param>
        /// <param name="preserveInstance">Whether or not to delete this TrackedSoundInstance after use.</param>
        /// <param name="soundTravelDistance">The distance the sound travels (in tiles).</param>
        /// <param name="soundVolumeMult">The multiplier from 0f - 1f of the sound's volume. Multiplied by GameData.SoundEffectVolume.</param>
        /// <param name="soundPitch">The pitch of the sound.</param>
        public static TrackedSoundEffectInstance PlayTrackedSoundFromOtherSource(int soundType, CollisionBody soundSource, bool preserveInstance, float soundTravelDistance = 10f, float soundVolumeMult = -1f, float soundPitch = 0f)
        {
            if (GameData.SoundEffectVolume <= 0.01f)
                return null;

            float travelDist = soundTravelDistance * 16;
            int volumeApexDistance = (int)travelDist / 2;       //So that if the player gets close enough the volume doesn't go higher but instead stays at that volume
            Vector2 soundPosition = soundSource.position;
            float distanceFromSource = MathHelper.Clamp(Vector2.Distance(GameData.AudioPosition, soundPosition) - volumeApexDistance, 0, travelDist);
            float soundVolume = ((travelDist - distanceFromSource) / travelDist) * soundVolumeMult * GameData.SoundEffectVolume;
            float soundPan = ((soundPosition.X - GameData.AudioPosition.X) / 16f) / travelDist;
            soundVolume = Math.Clamp(soundVolume, 0f, 1f);
            soundPan = Math.Clamp(soundPan, -1, 1);
            if (distanceFromSource == 0f)
                soundPan = 0;

            TrackedSoundEffectInstance newInstance = TrackedSoundEffectInstance.CreateTrackedSound(sounds[soundType].CreateInstance(), soundSource, preserveInstance, soundTravelDistance);
            newInstance.soundVolumeMult = soundVolumeMult;
            newInstance.soundInstance.Pitch = soundPitch;
            newInstance.soundInstance.Volume = soundVolume;
            newInstance.soundInstance.Pan = soundPan;
            newInstance.soundInstance.Play();
            return newInstance;
        }

        private static void RemoveActiveSound(TrackedSoundEffectInstance activeSound)
        {
            if (activeSound.preserveInstance)
                return;

            activeSound.soundInstance.Dispose();
            activeSoundEffects.Remove(activeSound);
        }

        /// <summary>
        /// Plays the selected ambience sound with volume drop-off and position-based panning. Doesn't do anything if player is too far from the sound source.
        /// </summary>
        /// <param name="soundType">Type of sound. Sounds types are available in SoundPlayer.</param>
        /// <param name="soundPosition">The sound's position.</param>
        /// <param name="soundTravelDistance">The distance the sound travels. (in tiles)</param>
        public static void PlayAmbienceSound(int soundType, Vector2 soundPosition, float soundTravelDistance = 20f)
        {
            int volumeApexDistance = 16 * 16;       //So that if the player gets close enough the volume doesn't go higher but instead stays at that volume
            float travelDist = soundTravelDistance * 16;
            float distanceFromSource = MathHelper.Clamp(Vector2.Distance(GameData.AudioPosition, soundPosition) - volumeApexDistance, 0, travelDist);

            if (distanceFromSource >= travelDist)
                return;

            float soundVolume = ((travelDist - distanceFromSource) / travelDist) * (GameData.SoundEffectVolume * 0.5f);
            if (soundVolume <= 0.01f)
                return;

            float soundPan = (soundPosition.X - GameData.AudioPosition.X) / travelDist;
            soundPan = Math.Clamp(soundPan, -1, 1);

            ambienceSounds[soundType].Play(soundVolume, 0f, soundPan);
        }

        /// <summary>
        /// Plays the dungeon ambience and randomly plays ambinece sounds.
        /// </summary>
        public static void ManageAmbience()
        {
            if (GameData.random.Next(1, 650 + 1) == 1)
                PlayRandomAmbienceSound(GameData.AudioPosition);
        }

        /// <summary>
        /// Plays a random ambience sound from a specified list of random ambience sounds.
        /// </summary>
        /// <param name="center">The center point of where the sound should play. Gets translated to multiple tiles away.</param>
        public static void PlayRandomAmbienceSound(Vector2 center)
        {
            int ambienceSound = GameData.random.Next(0, ambienceSounds.Length);
            Vector2 randomOffset = new Vector2(GameData.random.Next(-5, 5 + 1), GameData.random.Next(-5, 5 + 1));
            randomOffset.Normalize();
            randomOffset *= 23.9f * 16f;
            Vector2 ambiencePosition = center + randomOffset;
            PlayAmbienceSound(ambienceSound, ambiencePosition, 24f);
        }
    }
}
