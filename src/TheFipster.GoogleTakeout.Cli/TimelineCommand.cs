using CommandLine;
using TheFipster.GoogleTakeout.Cli.Timeline;

namespace TheFipster.GoogleTakeout.Cli
{
    public class TimelineCommand
    {
        private readonly TimelineOptions _options;

        public TimelineCommand(TimelineOptions options)
        {
            _options = options;
        }

        public int Run(string[] args)
        {
            return Parser.Default.ParseArguments<SliceOptions>(args)
                .MapResult(
                    (SliceOptions options) => new SliceAction(options).Execute(), 
                    errs => 1);
        }
    }
}
