using System.Linq;
using CommandLine;

namespace TheFipster.GoogleTakeout.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<TimelineOptions>(args)
                .MapResult(
                    (TimelineOptions options) => new TimelineCommand(options).Run(args.Skip(1).ToArray()),
                    errs => 1);
        }
    }
}
