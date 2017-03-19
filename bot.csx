#r "Newtonsoft.Json"
#load "EchoDialog.csx"

using System;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    // Initialize the azure bot
    using (BotService.Initialize())
    {
        // Deserialize the incoming activity
        string jsonContent = await req.Content.ReadAsStringAsync();
        var activity = JsonConvert.DeserializeObject<Activity>(jsonContent);
        
        
         var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                Activity reply = activity.CreateReply();
                
                var attachment = new
            {
                type = "template",
                payload = new
                {
                    template_type = "button",
                    text = "something...",
                    buttons = new[]
                    {
                        new {
                            type = "web_url",
                            url = "https://webviewpage.azurewebsites.net",
//url = "https://app.powerbi.com/view?r=eyJrIjoiOTBhODY4ODUtMWU3Ny00NGRjLTlhYjItMWM3ZjUyMzA5ZjY1IiwidCI6IjU3NGMzZTU2LTQ5MjQtNDAwNC1hZDFhLWQ4NDI3ZTdkYjI0MSIsImMiOjZ9",
                            title = "click me",
                            webview_height_ratio= "compact"
                            ,messenger_extensions = true
                            
                        }
                    }
                }
            };

            reply.ChannelData = JObject.FromObject(new { attachment });
                

await connector.Conversations.ReplyToActivityAsync(reply);
        
                        Activity replyToConversation = activity.CreateReply("Quick Replies");

dynamic quickReplies = new JObject();;

dynamic fbQRButtonRed = new JObject();
fbQRButtonRed.content_type = "text";
fbQRButtonRed.title = "red";
fbQRButtonRed.payload = "DEVELOPER_DEFINED_PAYLOAD_FOR_PICKING_RED";
fbQRButtonRed.image_url = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/07/Button_Icon_Red.svg/300px-Button_Icon_Red.svg.png";

dynamic fbQRButtonBlue = new JObject();
fbQRButtonBlue.content_type = "text";
fbQRButtonBlue.title = "blue";
fbQRButtonBlue.payload = "DEVELOPER_DEFINED_PAYLOAD_FOR_PICKING_BLUE";
fbQRButtonBlue.image_url = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Button_Icon_Blue.svg/768px-Button_Icon_Blue.svg.png";

dynamic fbQRButtonLocation = new JObject();
fbQRButtonLocation.content_type = "location";

dynamic buttonWebview = new JObject();
buttonWebview.content_type = "web_url";;
buttonWebview.url = "http://www.google.com";
buttonWebview.title = "webview";
//buttonWebview.webview_height_ratio = "compact";


quickReplies.quick_replies = new JArray(fbQRButtonRed, fbQRButtonBlue, fbQRButtonLocation);

replyToConversation.ChannelData = quickReplies;

await connector.Conversations.ReplyToActivityAsync(replyToConversation);
        
        
        
        // authenticate incoming request and add activity.ServiceUrl to MicrosoftAppCredentials.TrustedHostNames
        // if request is authenticated
       // if (!await BotService.Authenticator.TryAuthenticateAsync(req, new [] {activity}, CancellationToken.None))
       // {
       //     return BotAuthenticator.GenerateUnauthorizedResponse(req);
       // }
        
        
        return req.CreateResponse(HttpStatusCode.Accepted);
    }    
}
