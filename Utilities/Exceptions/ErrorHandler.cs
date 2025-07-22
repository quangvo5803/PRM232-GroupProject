using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Utilities.Exceptions
{
    public static class ErrorHandler
    {
        public static async Task HandleValidationErrorAsync(
            HttpResponseMessage response,
            ITempDataDictionary tempData
        )
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var errorObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                if (errorObj != null && errorObj.ContainsKey("errors"))
                {
                    var errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(
                        errorObj["errors"].ToString()
                    );

                    var errorMessages = new List<string>();
                    foreach (var kvp in errors)
                    {
                        foreach (var error in kvp.Value)
                        {
                            errorMessages.Add(error);
                        }
                    }

                    tempData["error"] = string.Join("<br/>", errorMessages);
                }
                else if (errorObj != null && errorObj.ContainsKey("message"))
                {
                    tempData["error"] = errorObj["message"].ToString();
                }
                else
                {
                    tempData["error"] = "An unknown error occurred.";
                }
            }
            catch
            {
                tempData["error"] = "An unknown error occurred.";
            }
        }
    }
}
