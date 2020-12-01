using System;

namespace ClinicalTools.UI
{
    public class HexStringConverter
    {
        public string FloatToHex(float number) => ByteIntToHex(FloatToByteInt(number));
        public int FloatToByteInt(float number) => (int)(number * 255);
        public string ByteIntToHex(int number) => number.ToString("X").PadLeft(2, '0');
        public float HexToFloat(string hex) => Convert.ToInt32(hex, 16) / 255.0f;

        public bool IsHex(char ch) => char.IsDigit(ch) || IsAlphaHex(ch);

        private const char ALPHA_LOWER_MIN = 'a';
        private const char ALPHA_LOWER_MAX = 'f';
        private const char ALPHA_UPPER_MIN = 'A';
        private const char ALPHA_UPPER_MAX = 'F';
        private bool IsAlphaHex(char ch)
            => IsCharBetween(ch, ALPHA_LOWER_MIN, ALPHA_LOWER_MAX)
                || IsCharBetween(ch, ALPHA_UPPER_MIN, ALPHA_UPPER_MAX);

        private bool IsCharBetween(char ch, char minChar, char maxChar)
            => ch >= minChar && ch <= maxChar;
    }
}