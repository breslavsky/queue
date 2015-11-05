using Queue.Sounds;
using System.IO;
using System.Media;

namespace Queue.Notification
{
    public static class SoundPlayerExtensions
    {
        public static void PlayNumber(this SoundPlayer player, int number)
        {
            foreach (var sound in Speaker.GetSounds(number))
            {
                using (var stream = new MemoryStream(sound))
                {
                    player.Stream = stream;
                    player.PlaySync();
                }
            }
        }

        public static void PlayStream(this SoundPlayer player, UnmanagedMemoryStream stream)
        {
            player.Stream = stream;
            player.PlaySync();
        }
    }
}