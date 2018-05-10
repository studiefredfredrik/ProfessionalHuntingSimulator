using Raven.Client.Documents;

namespace HighScoreApi
{
    public static class RavenDbConfiguration
    {
        public static IDocumentStore Configure(RavenDbSettings settings)
        {
            var documentStore = new DocumentStore
            {
                Urls = settings.Urls,
                Database = settings.DatabaseName,
            };

            documentStore.Initialize();

            return documentStore;
        }
    }

    public class RavenDbSettings
    {
        public string[] Urls { get; set; }
        public string DatabaseName { get; set; }
    }
}
