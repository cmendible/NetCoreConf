using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET;
using Alexa.NET.Response;
using System.Collections.Generic;
using System.Xml;
using Microsoft.SyndicationFeed.Rss;
using System.Linq;
using System;
using Alexa.NET.LocaleSpeech;

namespace AlexaSkill
{
    public static class Alexa
    {
        [FunctionName("Alexa")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            //this is the language used to invoke the skill
            string language = skillRequest.Request.Locale;

            bool isValid = await ValidateRequest(req, log, skillRequest);
            if (!isValid)
            {
                return new BadRequestResult();
            }

            var requestType = skillRequest.GetRequestType();
            var locale = SetupLanguages(skillRequest);

            SkillResponse response = null;

            if (requestType == typeof(LaunchRequest))
            {
                var message = await locale.Get("Welcome", null);
                response = ResponseBuilder.Tell(message);
                response.Response.ShouldEndSession = false;
            }
            else if (requestType == typeof(IntentRequest))
            {
                var intentRequest = skillRequest.Request as IntentRequest;

                if (intentRequest.Intent.Name == "victoria")
                {
                    var message = await locale.Get("event", new string[] { });

                    response = ResponseBuilder.Tell(message);
                }
                else if (intentRequest.Intent.Name == "AMAZON.CancelIntent")
                {
                    var message = await locale.Get("Cancel", null);
                    response = ResponseBuilder.Tell(message);
                }
                else if (intentRequest.Intent.Name == "AMAZON.HelpIntent")
                {
                    var message = await locale.Get("Help", null);
                    response = ResponseBuilder.Tell(message);
                    response.Response.ShouldEndSession = false;
                }
                else if (intentRequest.Intent.Name == "AMAZON.StopIntent")
                {
                    var message = await locale.Get("Stop", null);
                    response = ResponseBuilder.Tell(message);
                }
            }
            else if (requestType == typeof(SessionEndedRequest))
            {
                log.LogInformation("Session ended");
                response = ResponseBuilder.Empty();
                response.Response.ShouldEndSession = true;
            }

            return new OkObjectResult(response);
        }

        private static async Task<bool> ValidateRequest(HttpRequest request, ILogger log, SkillRequest skillRequest)
        {
            request.Headers.TryGetValue("SignatureCertChainUrl", out var signatureChainUrl);
            if (string.IsNullOrWhiteSpace(signatureChainUrl))
            {
                log.LogError("Validation failed. Empty SignatureCertChainUrl header");
                return false;
            }

            Uri certUrl;
            try
            {
                certUrl = new Uri(signatureChainUrl);
            }
            catch
            {
                log.LogError($"Validation failed. SignatureChainUrl not valid: {signatureChainUrl}");
                return false;
            }

            request.Headers.TryGetValue("Signature", out var signature);
            if (string.IsNullOrWhiteSpace(signature))
            {
                log.LogError("Validation failed - Empty Signature header");
                return false;
            }

            request.Body.Position = 0;
            var body = await request.ReadAsStringAsync();
            request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
            {
                log.LogError("Validation failed - the JSON is empty");
                return false;
            }

            // bool isTimestampValid = RequestVerification.RequestTimestampWithinTolerance(skillRequest);
            // bool valid = await RequestVerification.Verify(signature, certUrl, body);

            // if (!valid || !isTimestampValid)
            // {
            //     log.LogError("Validation failed - RequestVerification failed");
            //     return false;
            // }
            // else
            // {
            return true;
            // }
        }

        public static ILocaleSpeech SetupLanguages(SkillRequest skillRequest)
        {
            var store = new DictionaryLocaleSpeechStore();
            store.AddLanguage("en", new Dictionary<string, object>
            {
                { "Welcome", "Welcome to the NetCoreConf skill! I'm sorry to inform you that Carlos does not speak catalan" },
                { "event", "OK I'm sending the event to Event Grid" },
                { "Cancel", "I'm cancelling the request..." },
                { "Help", "Sorry you are on your own." },
                { "Stop", "Bye!" }
            });

            var localeSpeechFactory = new LocaleSpeechFactory(store);
            var locale = localeSpeechFactory.Create(skillRequest);

            return locale;
        }
    }
}