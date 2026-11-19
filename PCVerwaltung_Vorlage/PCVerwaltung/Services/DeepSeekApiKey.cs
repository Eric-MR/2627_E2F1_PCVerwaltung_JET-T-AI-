using System;
using System.IO;

namespace PCVerwaltung.Services
{
    internal static class DeepSeekApiKey
    {
        private const string EnvironmentVariableName = "DEEPSEEK_API_KEY";

        public static string GetApiKey()
        {
            string? apiKey = Environment.GetEnvironmentVariable(EnvironmentVariableName);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = ReadApiKeyFromEnvironmentFile(FindEnvironmentFile());
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    $"DeepSeek-API-Schlüssel fehlt. Lege eine .env-Datei mit '{EnvironmentVariableName}=dein-api-schlüssel' im Projekt- oder Anwendungsverzeichnis an.");
            }

            return apiKey.Trim();
        }

        private static string? FindEnvironmentFile()
        {
            string[] startDirectories =
            {
                AppContext.BaseDirectory,
                Environment.CurrentDirectory
            };

            foreach (string startDirectory in startDirectories)
            {
                DirectoryInfo? directory = new(startDirectory);
                while (directory is not null)
                {
                    string environmentFilePath = Path.Combine(directory.FullName, ".env");
                    if (File.Exists(environmentFilePath))
                    {
                        return environmentFilePath;
                    }

                    directory = directory.Parent;
                }
            }

            return null;
        }

        private static string? ReadApiKeyFromEnvironmentFile(string? environmentFilePath)
        {
            if (environmentFilePath is null)
            {
                return null;
            }

            foreach (string line in File.ReadLines(environmentFilePath))
            {
                string entry = line.Trim();
                if (entry.Length == 0 || entry.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                if (entry.StartsWith("export ", StringComparison.Ordinal))
                {
                    entry = entry.Substring("export ".Length).TrimStart();
                }

                int separatorIndex = entry.IndexOf('=');
                if (separatorIndex < 0
                    || !string.Equals(
                        entry.Substring(0, separatorIndex).Trim(),
                        EnvironmentVariableName,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                string value = entry.Substring(separatorIndex + 1).Trim();
                if (value.Length >= 2
                    && ((value[0] == '"' && value[^1] == '"')
                        || (value[0] == '\'' && value[^1] == '\'')))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                return value;
            }

            return null;
        }
    }
}
