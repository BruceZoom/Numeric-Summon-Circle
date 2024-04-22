using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NSC.Audio
{
    [CreateAssetMenu(menuName = "Audio/Audio Asset")]
    public class AudioAsset : ScriptableObject
    {
        [SerializeField] private List<AudioClip> _audioClips;

        public AudioClip RandomClip => _audioClips[UnityEngine.Random.Range(0, _audioClips.Count - 1)];

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Generate Audio Asset", priority = -50)]
        public static void CreateAudioAsset(UnityEditor.MenuCommand menuCommand)
        {
            // skip other objects to execute only once
            if (menuCommand.context != null && menuCommand.context != UnityEditor.Selection.activeObject)
            {
                return;
            }

            var clips = UnityEditor.Selection.objects.Select(o => o as AudioClip).Where(c => c != null);
            if (clips.Count() <= 0)
            {
                Debug.LogWarning("No audio clip selected.");
                return;
            }

            AudioAsset asset = ScriptableObject.CreateInstance<AudioAsset>();
            // assign clips
            asset._audioClips = new List<AudioClip>();
            foreach (var clip in clips)
            {
                asset._audioClips.Add(clip);
            }
            
            string name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Scriptable Objects/Audio/NewAudioAsset.asset");
            UnityEditor.AssetDatabase.CreateAsset(asset, name);
            UnityEditor.AssetDatabase.SaveAssets();

            UnityEditor.EditorUtility.FocusProjectWindow();

            UnityEditor.Selection.activeObject = asset;
        }
#endif
    }
}