using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace TheFipster.GoogleTakeout.Cli.Timeline
{
    [Verb("slice", HelpText = "Slices the whole takeout timeline file by days.")]
    public class SliceOptions
    {
        [Option('i', "input", Required = true, HelpText = "Takeout timeline file")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output directory for daily files. If empty, directory of input is used.")]
        public string OutputDirectory { get; set; }
    }
}
