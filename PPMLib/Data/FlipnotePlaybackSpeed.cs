using PPMLib.Attributes;

namespace PPMLib.Data
{
    public enum FlipnotePlaybackSpeed
    {
        [Display("0.5 FPS")]
        [PlaybackSpeedDuration(120)]
        Speed1 = 1,

        [Display("1 FPS")]
        [PlaybackSpeedDuration(60)]
        Speed2 = 2,

        [Display("2 FPS")]
        [PlaybackSpeedDuration(30)]
        Speed3 = 3,

        [Display("4 FPS")]
        [PlaybackSpeedDuration(15)]
        Speed4 = 4,

        [Display("6 FPS")]
        [PlaybackSpeedDuration(10)]
        Speed5 = 5,


        [Display("12 FPS")]
        [PlaybackSpeedDuration(5)]
        Speed6 = 6,

        [Display("20 FPS")]
        [PlaybackSpeedDuration(3)]
        Speed7 = 7,

        [Display("30 FPS")]
        [PlaybackSpeedDuration(2)]
        Speed8 = 8,
    }
}
