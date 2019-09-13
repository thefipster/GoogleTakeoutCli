using System.IO;

namespace TheFipster.GoogleTakeout.MapsTimeline.Tasks
{
    public class RemoveOuterObjectTask
    {
        private const char ArrayStart = '[';

        private bool firstArrayTokenFound;
        private string line;

        private bool arrayNotBeginning => !firstArrayTokenFound && !line.Contains(ArrayStart);
        private bool arrayBeginning => !firstArrayTokenFound && line.Contains(ArrayStart);

        public RemoveOuterObjectTask(string inputFile, string outputFile)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
        }

        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }

        public void Run()
        {
            using (var writer = new StreamWriter(OutputFile))
            using (var reader = new StreamReader(InputFile))
            {
                trimOuterObjectFromInputFile(writer, reader);
            }
        }

        private void trimOuterObjectFromInputFile(StreamWriter writer, StreamReader reader)
        {
            do
            {
                line = reader.ReadLine();

                if (arrayNotBeginning)
                    continue;

                cutBegin();
                writeToOutput(writer, reader);

            } while (line != null);
        }

        private void writeToOutput(StreamWriter writer, StreamReader reader)
        {
            if (!reader.EndOfStream)
            {
                writer.WriteLine(line);
            }
        }

        private void cutBegin()
        {
            if (arrayBeginning)
            {
                firstArrayTokenFound = true;
                line = line.Substring(line.IndexOf(ArrayStart));
            }
        }
    }
}
