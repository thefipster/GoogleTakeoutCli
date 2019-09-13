using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TheFipster.GoogleTakeout.MapsTimeline.Tasks
{
    public class SliceByDayTask
    {
        private readonly JsonSerializer _serializer;
        private readonly string _inputFile;
        private readonly string _outputPath;

        private DateTime? _outputDate;
        private DateTime _entryDate;
        private JArray _outputData;
        private JsonReader _jsonReader;
        private StreamReader _streamReader;
        private FileStream _fileStream;

        private bool DateChanged => _outputDate.HasValue && _outputDate != _entryDate.Date;
        private bool NewEntryStarts => _jsonReader.TokenType == JsonToken.StartObject;

        public SliceByDayTask(string inputFile, string outputPath)
        {
            _serializer = new JsonSerializer();
            _outputData = new JArray();
            _inputFile = inputFile;
            _outputPath = outputPath;
        }

        public void Run()
        {
            using (_fileStream = File.Open(_inputFile, FileMode.Open))
            using (_streamReader = new StreamReader(_fileStream))
            using (_jsonReader = new JsonTextReader(_streamReader))
            {
                readFile();
            }
        }

        private void readFile()
        {
            while (_jsonReader.Read())
            {
                if (NewEntryStarts)
                    addEntryToOutput();

                if (DateChanged)
                    flushOutput();
            }
        }

        private void flushOutput()
        {
            writeOutput(_outputData, _outputDate.Value);
            resetOutput();
        }

        private void addEntryToOutput()
        {
            var entry = _serializer.Deserialize<JToken>(_jsonReader);
            _entryDate = getTimestampFromJsonToken(entry);
            _outputData.Add(entry);
            setOutputDateIfNull();
        }

        private void resetOutput()
        {
            _outputData = new JArray();
            _outputDate = _entryDate.Date;
        }

        private void setOutputDateIfNull()
        {
            if (_outputDate == null)
                _outputDate = _entryDate.Date;
        }

        private void writeOutput(JArray array, DateTime date)
        {
            var outputPath = getOutputPath(date);
            var json = array.ToString();
            File.WriteAllText(outputPath, json);
        }

        private string getOutputPath(DateTime date)
        {
            var filename = string.Concat(
                "gmaps-timeline-", 
                date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 
                ".json");

            return Path.Combine(_outputPath, filename);
        }

        private DateTime getTimestampFromJsonToken(JToken token)
        {
            var msSinceEpoch = token.Value<long>("timestampMs");
            var epochStart = new DateTime(
                1970, 1, 1, 0, 0, 0, 0, 
                DateTimeKind.Unspecified);

            return epochStart.AddMilliseconds(msSinceEpoch);
        }
    }
}
