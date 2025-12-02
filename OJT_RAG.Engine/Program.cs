using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RagDemo
{
    class Program
    {
        private static readonly HttpClient httpClient = new HttpClient();

        static async Task Main()
        {
            // Configuration
            string projectId = "reflecting-surf-477600-p4"; // Matches service account JSON
            string region = "asia-southeast1"; // Fallback: change to "us-central1" if 404 persists
            string serviceAccountPath = @"D:\Project\CapStone\OJT_RAG_CSharp\OJT_RAG.Engine\rag-service-account.json";
            string bucketUri = "gs://run-sources-reflecting-surf-477600-p4-us-central1\r\n"; // Confirm bucket/file exist
            string baseUrl = $"https://{region}-aiplatform.googleapis.com/v1";

            try
            {
                // Step 1: Authenticate and get access token
                var credential = GoogleCredential.FromFile(serviceAccountPath)
                    .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
                string accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync(
                    "https://www.googleapis.com/auth/cloud-platform",
                    CancellationToken.None);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                Console.WriteLine("Access token obtained successfully!");

                // Step 2: Create RAG Corpus
                string parent = $"projects/{projectId}/locations/{region}/ragCorpora";
                var corpusRequestBody = new
                {
                    displayName = "ProductDocumentation",
                    description = "Knowledge base for product documentation"
                };
                var corpusJson = JsonConvert.SerializeObject(corpusRequestBody);
                var corpusContent = new StringContent(corpusJson, Encoding.UTF8, "application/json");
                var corpusResponse = await httpClient.PostAsync($"{baseUrl}/{parent}", corpusContent);
                if (!corpusResponse.IsSuccessStatusCode)
                {
                    string errorContent = await corpusResponse.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to create corpus: {corpusResponse.StatusCode} - {errorContent}");
                }
                string corpusResponseJson = await corpusResponse.Content.ReadAsStringAsync();
                dynamic corpusOperationData = JsonConvert.DeserializeObject(corpusResponseJson);
                string operationName = corpusOperationData.name; // Operation ID
                Console.WriteLine($"Started corpus creation operation: {operationName}");

                // Poll corpus creation operation
                string corpusName = null;
                bool isDone = false;
                int maxPolls = 60; // Max 5 minutes
                int pollCount = 0;
                while (!isDone && pollCount < maxPolls)
                {
                    var operationResponse = await httpClient.GetAsync($"{baseUrl}/{operationName}");
                    if (!operationResponse.IsSuccessStatusCode)
                    {
                        string errorContent = await operationResponse.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Failed to poll corpus operation: {operationResponse.StatusCode} - {errorContent}");
                    }
                    string operationJson = await operationResponse.Content.ReadAsStringAsync();
                    dynamic operationData = JsonConvert.DeserializeObject(operationJson);
                    isDone = operationData.done ?? false;
                    if (isDone)
                    {
                        corpusName = operationData.response.name; // Actual corpus resource name
                        Console.WriteLine($"Created corpus: {corpusName}");
                    }
                    else
                    {
                        Console.WriteLine("Corpus creation still running...");
                        await Task.Delay(5000);
                        pollCount++;
                    }
                }
                if (!isDone)
                {
                    throw new Exception("Corpus creation timed out. Check the operation in Google Cloud Console.");
                }

                // Step 3: Test GCS access
                var storageResponse = await httpClient.GetAsync($"https://storage.googleapis.com/storage/v1/b/cloud-ai-platform-2b8ffe9f-38d5-43c4-b812-fc8cebcc659f/o/Session%201.pdf");
                if (!storageResponse.IsSuccessStatusCode)
                {
                    string errorContent = await storageResponse.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to access GCS file: {storageResponse.StatusCode} - {errorContent}");
                }
                Console.WriteLine("GCS file access verified successfully!");

                // Step 4: Import data from GCS
                string importParent = $"{corpusName}:importFiles";
                var importRequestBody = new
                {
                    importRagFilesConfigs = new[]
                    {
                        new
                        {
                            gcsSource = new { uris = new[] { bucketUri } },
                            ragFileChunkingConfig = new { chunkSize = 1024, chunkOverlap = 128 },
                            maxEmbeddingRequestsPerMin = 600
                        }
                    }
                };
                var importJson = JsonConvert.SerializeObject(importRequestBody);
                var importContent = new StringContent(importJson, Encoding.UTF8, "application/json");
                var importResponse = await httpClient.PostAsync($"{baseUrl}/{importParent}", importContent);
                if (!importResponse.IsSuccessStatusCode)
                {
                    string errorContent = await importResponse.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to import files: {importResponse.StatusCode} - {errorContent}");
                }
                string importResponseJson = await importResponse.Content.ReadAsStringAsync();
                dynamic importData = JsonConvert.DeserializeObject(importResponseJson);
                operationName = importData.name;
                Console.WriteLine($"Started import operation: {operationName}");

                // Step 5: Poll the import operation
                isDone = false;
                pollCount = 0;
                while (!isDone && pollCount < maxPolls)
                {
                    var operationResponse = await httpClient.GetAsync($"{baseUrl}/{operationName}");
                    if (!operationResponse.IsSuccessStatusCode)
                    {
                        string errorContent = await operationResponse.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Failed to poll import operation: {operationResponse.StatusCode} - {errorContent}");
                    }
                    string operationJson = await operationResponse.Content.ReadAsStringAsync();
                    dynamic operationData = JsonConvert.DeserializeObject(operationJson);
                    isDone = operationData.done ?? false;
                    if (isDone)
                    {
                        dynamic responseData = operationData.response;
                        int importedFileCount = responseData?.importedRagFileCount ?? 0;
                        Console.WriteLine($"Imported {importedFileCount} file(s) to corpus.");
                    }
                    else
                    {
                        Console.WriteLine("Import operation still running...");
                        await Task.Delay(5000);
                        pollCount++;
                    }
                }
                if (!isDone)
                {
                    Console.WriteLine("Import timed out. Check the operation in Google Cloud Console.");
                }

                // Step 6: Interactive Q&A
                Console.WriteLine("Enter your questions (type 'exit' to quit):");
                while (true)
                {
                    Console.Write("Question: ");
                    string userQuery = Console.ReadLine();
                    if (userQuery.ToLower() == "exit") break;

                    string model = "gemini-1.5-flash-001";
                    string generateUrl = $"{baseUrl}/projects/{projectId}/locations/{region}/publishers/google/models/{model}:generateContent";
                    var generateRequestBody = new
                    {
                        contents = new[] { new { role = "user", parts = new[] { new { text = userQuery } } } },
                        generationConfig = new { temperature = 0.7, topP = 0.95, maxOutputTokens = 1024 },
                        tools = new[] { new { vertexRagStore = new { ragResources = new[] { new { ragCorpus = corpusName } } } } }
                    };
                    var generateJson = JsonConvert.SerializeObject(generateRequestBody);
                    var generateContent = new StringContent(generateJson, Encoding.UTF8, "application/json");
                    var generateResponse = await httpClient.PostAsync(generateUrl, generateContent);
                    if (!generateResponse.IsSuccessStatusCode)
                    {
                        string errorContent = await generateResponse.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Failed to generate content: {generateResponse.StatusCode} - {errorContent}");
                    }
                    string generateResponseJson = await generateResponse.Content.ReadAsStringAsync();
                    dynamic generateData = JsonConvert.DeserializeObject(generateResponseJson);
                    string answer = generateData.candidates[0].content.parts[0].text;
                    Console.WriteLine($"Answer: {answer}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}