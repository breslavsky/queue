using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Queue.Sounds
{
    public static class Speaker
    {
        private const string Thousands = "thousands";
        private const string Thousand = "thousand";
        private const string Thousandi = "thousandi";

        private static Dictionary<string, Stream> template = new Dictionary<string, Stream>(){
            { Thousands, Digits.thousands },
            { Thousand, Digits.thousand },
            { Thousandi, Digits.thousandi },

            { "1f", Digits._1f },
            { "2f", Digits._2f },

            // normal

            { "0", Digits._000 },
            { "1", Digits._001 },
            { "2", Digits._002 },
            { "3", Digits._003 },
            { "4", Digits._004 },
            { "5", Digits._005 },
            { "6", Digits._006 },
            { "7", Digits._007 },
            { "8", Digits._008 },
            { "9", Digits._009 },
            { "11", Digits._011 },
            { "12", Digits._012 },
            { "13", Digits._013 },
            { "14", Digits._014 },
            { "15", Digits._015 },
            { "16", Digits._016 },
            { "17", Digits._017 },
            { "18", Digits._018 },
            { "19", Digits._019 },
            { "10", Digits._010 },
            { "20", Digits._020 },
            { "30", Digits._030 },
            { "40", Digits._040 },
            { "50", Digits._050 },
            { "60", Digits._060 },
            { "70", Digits._070 },
            { "80", Digits._080 },
            { "90", Digits._090 },
            { "100", Digits._100 },
            { "200", Digits._200 },
            { "300", Digits._300 },
            { "400", Digits._400 },
            { "500", Digits._500 },
            { "600", Digits._600 },
            { "700", Digits._700 },
            { "800", Digits._800 },
            { "900", Digits._900 },

            // postfix

            { "1_", Digits._001_ },
            { "2_", Digits._002_ },
            { "3_", Digits._003_ },
            { "4_", Digits._004_ },
            { "5_", Digits._005_ },
            { "6_", Digits._006_ },
            { "7_", Digits._007_ },
            { "8_", Digits._008_ },
            { "9_", Digits._009_ },
            { "11_", Digits._011_ },
            { "12_", Digits._012_ },
            { "13_", Digits._013_ },
            { "14_", Digits._014_ },
            { "15_", Digits._015_ },
            { "16_", Digits._016_ },
            { "17_", Digits._017_ },
            { "18_", Digits._018_ },
            { "19_", Digits._019_ },
            { "10_", Digits._010_ },
            { "20_", Digits._020_ },
            { "30_", Digits._030_ },
            { "40_", Digits._040_ },
            { "50_", Digits._050_ },
            { "60_", Digits._060_ },
            { "70_", Digits._070_ },
            { "80_", Digits._080_ },
            { "90_", Digits._090_ },
            { "100_", Digits._100_ },
            { "200_", Digits._200_ },
            { "300_", Digits._300_ },
            { "400_", Digits._400_ },
            { "500_", Digits._500_ },
            { "600_", Digits._600_ },
            { "700_", Digits._700_ },
            { "800_", Digits._800_ },
            { "900_", Digits._900_ }
        };

        public static byte[][] GetSounds(int number)
        {
            var sounds = new List<byte[]>();

            var soundIds = GetSoundsIds(number, null);

            var lastIndex = soundIds.Length - 1;

            var postfix = string.Format("{0}_", soundIds[lastIndex]);
            if (template.ContainsKey(postfix))
            {
                soundIds[lastIndex] = postfix;
            }

            foreach (var soundId in soundIds)
            {
                var sound = template[soundId];

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    sound.CopyTo(memoryStream);
                    sound.Position = 0;
                    sounds.Add(memoryStream.ToArray());
                }
            }

            return sounds.ToArray();
        }

        private static string[] GetSoundsIds(int number, string modificator)
        {
            List<string> soundIds = new List<string>();

            if (number >= 1000000)
            {
                new NotSupportedException();
            }

            if (number >= 1000)
            {
                int next = number / 1000;
                soundIds = soundIds.Concat(GetSoundsIds(next, "f")).ToList();

                string soundId = Thousands;

                if (next % 100 < 10 || next % 100 > 20)
                {
                    string tail = next.ToString();
                    int digit = int.Parse(tail[tail.Length - 1].ToString());

                    if (digit == 1)
                    {
                        soundId = Thousand;
                    }
                    else
                    {
                        if (digit < 5)
                        {
                            soundId = Thousandi;
                        }
                    }
                }

                soundIds.Add(soundId);
                number -= next * 1000;
            }

            if (number > 10 && number < 20)
            {
                soundIds.Add(number.ToString());
            }
            else
            {
                string Digits = number.ToString();
                int power = Digits.Length - 1;

                char digit = Digits[0];

                int current = (int)(int.Parse(digit.ToString()) * Math.Pow((double)10, (double)power));

                if (current != 0)
                {
                    string soundId = current.ToString();

                    if ((current == 1 || current == 2) && modificator != null)
                    {
                        soundId += modificator;
                    }

                    soundIds.Add(soundId);

                    int next = number - (int)current;

                    if (next > 0)
                    {
                        soundIds = soundIds.Concat(GetSoundsIds(next, modificator)).ToList();
                    }
                }
            }

            return soundIds.ToArray();
        }
    }
}