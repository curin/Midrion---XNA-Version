using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace com.dbtgaming.Library.Audio
{
    public class AudioRegistry
    {
        public SoundEffectInstance this[string st]
        {
            get
            {
                return _soundInstances[st];
            }
            set
            {
                _soundInstances[st] = value;
            }
        }

        private static Dictionary<string, SoundEffectInstance> _soundInstances = new Dictionary<string, SoundEffectInstance>();
        private static Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

        public static Dictionary<string, SoundEffect> SoundEffects
        {
            get
            {
                return _soundEffects;
            }
            set
            {
                _soundEffects = value;
            }
        }

        public static void PlayInstance(string ID)
        {
            _soundInstances[ID].Play();
        }

        public static void LoopInstance(string ID)
        {
            _soundInstances[ID].IsLooped = true;
            _soundInstances[ID].Play();
        }

        public static void StopInstance(string ID)
        {
            _soundInstances[ID].Stop();
        }

        public static void PauseInstance(string ID)
        {
            _soundInstances[ID].Pause();
        }

        public static void EditInstance(string ID, int Volume, int Pan, int Pitch)
        {
            _soundInstances[ID].Pan = Pan;
            _soundInstances[ID].Volume = Volume;
            _soundInstances[ID].Pitch = Pitch;
        }

        public static void Apply3DToInstance(string ID, AudioListener[] listeners, AudioEmitter emitter)
        {
            _soundInstances[ID].Apply3D(listeners, emitter);
        }

        public static void Apply3DToInstance(string ID, AudioListener listener, AudioEmitter emitter)
        {
            _soundInstances[ID].Apply3D(listener, emitter);
        }

        public static void Apply3DToInstance(string ID, Vector3 listpos, Vector3 listforward, Vector3 listup, Vector3 listvelocity, Vector3 emitpos, Vector3 emitforward, Vector3 emitup, Vector3 emitvelocity, float dopplerscale)
        {
            AudioListener listener = new AudioListener();
            listener.Position = listpos;
            listener.Forward = listforward;
            listener.Up = listup;
            listener.Velocity = listvelocity;
            AudioEmitter emitter = new AudioEmitter();
            emitter.DopplerScale = dopplerscale;
            emitter.Forward = emitforward;
            emitter.Position = emitpos;
            emitter.Up = emitup;
            emitter.Velocity = emitvelocity;
            _soundInstances[ID].Apply3D(listener, emitter);
        }

        public static void addSoundInstance(string SoundID, string InstanceID)
        {
            _soundInstances.Add(InstanceID, _soundEffects[SoundID].CreateInstance());
        }

        public static void addSoundEffect(string ID, SoundEffect effect)
        {
            _soundEffects.Add(ID, effect);
        }
    }
}
