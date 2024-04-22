using NSC.Utils;
using UnityEngine;
using UnityEngine.Audio;
using VInspector;

namespace NSC.Audio
{
    public class AudioManager : PassiveSingleton<AudioManager>
    {
        [field: SerializeField] public AudioSource musicSource { get; private set; }
        [field: SerializeField] public AudioSource SFXSource { get; private set; }

        [field: SerializeField, Header("Audio Assets")] public AudioAsset SummonSFX { get; private set; }
        [field: SerializeField] public AudioAsset ProduceSFX { get; private set; }
        [field: SerializeField] public AudioAsset DamageSFX { get; private set; }
        [field: SerializeField] public AudioAsset PurchaseSFX { get; private set; }
        [field: SerializeField] public AudioAsset ClickSFX { get; private set; }
        [field: SerializeField] public AudioAsset GameOverSFX { get; private set; }

        private void Awake()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void Start()
        {
            //musicSource.clip = background;
            //musicSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;

            SFXSource.PlayOneShot(clip);
        }

        public void PlayRandomSFX(AudioAsset asset, Transform source = null)
        {
            if (asset == null) return;

            float volume = 1;
            if (source != null)
            {
                Vector2 viewPos = Camera.main.WorldToViewportPoint(source.position);
                // decrease volume if outside screen
                if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
                {
                    volume = 1 / (viewPos - Vector2.one * 0.5f).sqrMagnitude / 4;
                }
            }

            SFXSource.PlayOneShot(asset.RandomClip, volume);
        }

        public void PlayButtonClick()
        {
            //PlayRandomSFX(ClickSFX);
        }
    }
}