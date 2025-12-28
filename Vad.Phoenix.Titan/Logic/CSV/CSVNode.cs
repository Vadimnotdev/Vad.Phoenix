using Vad.Phoenix.Titan.Logic.Debug;

namespace Vad.Phoenix.Titan.Logic.CSV
{
    public class CSVNode
    {
        private string _name;
        private CSVTable _table;

        public CSVNode(string[] lines, string fileName)
        {
            _name = fileName;

            Load(lines);
        }

        public void Load(string[] lines)
        {
            _table = new CSVTable(this, lines.Length);

            if (lines.Length >= 2)
            {
                List<string> columnNames = ParseLine(lines[0]);
                List<string> columnTypes = ParseLine(lines[1]);

                for (int i = 0; i < columnNames.Count; i++) _table.AddColumn(columnNames[i]);

                for (int i = 0; i < columnTypes.Count; i++)
                {
                    string type = columnTypes[i];
                    int columnType = -1;

                    if (!string.IsNullOrEmpty(type))
                    {
                        if (string.Equals(type, "string")) columnType = 0;
                        else if (string.Equals(type, "int")) columnType = 1;
                        else if (string.Equals(type, "boolean")) columnType = 2;

                        else
                        {
                            Debugger.Error($"Invalid column type '{columnTypes[i]}', column name {columnNames[i]}, file {_name}. Expectied: int/string/boolean.");
                        }
                    }
                    _table.AddColumnType(columnType);
                }
                _table.ValidateColumnTypes();

                if (lines.Length > 2)
                {
                    for (int i = 2; i < lines.Length; i++)
                    {
                        List<string> values = ParseLine(lines[i]);

                        if (values.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(values[0])) _table.CreateRow();

                            for (int j = 0; j < values.Count; j++) _table.AddAndConvertValue(values[j], j);
                        }
                    }
                }
            }
        }

        public List<string> ParseLine(string line)
        {
            bool inQuote = false;
            string readField = string.Empty;

            List<string> fields = [];

            for (int i = 0; i < line.Length; i++)
            {
                char currentChar = line[i];

                if (currentChar == '"')
                {
                    if (inQuote)
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"') readField += currentChar;
                        else inQuote = false;
                    }
                    else inQuote = true;
                }
                else if (currentChar == ',' && !inQuote)
                {
                    fields.Add(readField);
                    readField = "";
                }
                else readField += currentChar;
            }
            fields.Add(readField);

            return fields;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public CSVTable GetTable()
        {
            return _table;
        }
    }
}