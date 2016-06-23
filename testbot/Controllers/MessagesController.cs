using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;

namespace testbot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                string QuestionString;
                TestLUIS testLUIS = await GetEntityFromLUIS(message.Text);
                if (testLUIS.intents.Count() > 0)
                {
                    switch (testLUIS.intents[0].intent)
                    {
                        case "ExistingFeature":
                            QuestionString = await GetFeature(testLUIS.entities[0].entity);
                            break;                        
                        default:
                            QuestionString = "Sorry, I am not getting you...";
                            break;
                    }
                }
                else
                {
                    QuestionString = "Sorry, I am not getting you...";
                }
                // return our reply to the user
                return message.CreateReplyMessage(QuestionString);
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }
        private static async Task<TestLUIS> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            TestLUIS Data = new TestLUIS();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://api.projectoxford.ai/luis/v1/application?id=8c964792-9689-4666-99bc-aa098ed9c12d&subscription-key=196293091bb94400a95df61554a4abc3&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<TestLUIS>(JsonDataResponse);
                }
            }
            return Data;
        }
        private async Task<string> GetFeature(string RandFeature)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 5);
            switch (randomNumber)
            {
                case 0:
                    return string.Format("BotFramework is very convinient");                    
                case 1:
                    return string.Format("You can make a new bot and connect it to different channels");                    
                case 2:
                    return string.Format("You can directly publish your bot to Azure with MS Visual Studio");                    
                case 3:
                    return string.Format("You can add LUIS integration to your bot in a few simple steps");
                case 4:
                    return string.Format("You can add your already existing bot to framework if you use its API");
                default:
                    return string.Format("BotFramework is very convenient");                    
            }
           
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
                return message.CreateReplyMessage($"Hi. I'm testbot. I'm made for checking out various features of the bot framework");
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }

    }
}