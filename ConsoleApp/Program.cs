
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;




    var apiKey = "sk-proj-0R7RbnYExDbcVpbtVatGT3BlbkFJwTXnGYOC92mFHjdBiW30";
    var prompt = "Hello, ChatGPT! How can I integrate you into my C# application?";

    var response = await GetChatGPTResponse(apiKey, prompt);
    Console.WriteLine(response);


static async Task<string> GetChatGPTResponse(string apiKey, string prompt)
{
    HttpClient client = new HttpClient();
    var requestUri = "https://api.openai.com/v1/chat/completions";
    var requestBody = new
    {
        model = "gpt-3.5-turbo-16k-0613",
        messages = "",
        temperature = 1,
        max_tokens = 256,
        top_p = 1,
        frequency_penalty = 0,
        presence_penalty = 0
    };

    var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

    var response = await client.PostAsync(requestUri, content);
    var responseContent = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception($"API request failed with status code: {response.StatusCode}, response: {responseContent}");
    }

    try
    {
        var jsonResponse = JObject.Parse(responseContent);
        var result = jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString();

        if (result == null)
        {
            throw new Exception("Response JSON structure is not as expected.");
        }

        return result.Trim();
    }
    catch (JsonException ex)
    {
        throw new Exception($"Failed to parse JSON response: {ex.Message}");
    }
}
