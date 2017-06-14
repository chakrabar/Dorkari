using Dorkari.Helpers.Core.Extensions;

namespace Dorkari.Samples.Cmd.Examples
{
    class StringExamples
    {
        public static void Show()
        {
            var sentence01 = "   Oh my    god !  What a                shame!";
            var trimmed = sentence01.TrimExtraSpaces();

            var sentence02 = " Oh my god! What a shame!";
            var blanksafeEqual = sentence01.NullAndBlankSpaceSafeEquals(sentence02);

            var sentence03 = " Oh my god. What a _shame_";
            var baseEqual = sentence01.EqualsWithoutIgnoreChars(sentence03, ' ', '!', '.', '_');
        }
    }
}
