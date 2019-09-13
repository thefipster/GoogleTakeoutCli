using System.IO;
using TheFipster.GoogleTakeout.MapsTimeline.Tasks;

namespace TheFipster.GoogleTakeout.Cli.Timeline
{
    public class SliceAction
    {
        private readonly SliceOptions _options;

        public SliceAction(SliceOptions options)
        {
            _options = options;
        }

        public int Execute()
        {
            var tempFile = Path.GetTempFileName();

            new RemoveOuterObjectTask(_options.InputFile, tempFile).Run();
            new SliceByDayTask(tempFile, _options.OutputDirectory).Run();

            return 0;
        }
    }
}
