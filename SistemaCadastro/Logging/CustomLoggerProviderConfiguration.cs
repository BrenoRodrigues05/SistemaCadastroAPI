namespace SistemaCadastro.Logging
{
    public class CustomLoggerProviderConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public string LogFilePath { get; set; } = "Logs/sistema-cadastro.log";

        public bool LogToFile { get; set; } = true;
        public bool LogToDatabase { get; set; } = true;

    }
}
