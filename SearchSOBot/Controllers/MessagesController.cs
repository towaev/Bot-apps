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
using System.Net.Http.Headers;

namespace SearchSOBot
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
                StackBot stackRequest = await GetJsonFromStack(message.Text);
                if (stackRequest.items.Count() > 0)
                {
                    //multiple messages realisation. doesn't work atm. excplicit post required to catch messages from this one
                    //int n = stackRequest.items.Count();
                    //if (n > 5) n = 5;
                    //for (int i = 0; i < n; i++)
                    //{
                    //    QuestionString = await GetAnswers(stackRequest.items[i].question_id);                        
                    //    connector.Messages.SendMessage(message.CreateReplyMessage(QuestionString));
                    //}
                    // //return null;
                    // QuestionString = await GetAnswers(stackRequest.items[0].question_id);
                    // return null; // no reply

                    QuestionString = await GetAnswers(stackRequest.items[0].question_id);
                    return message.CreateReplyMessage(QuestionString);
                   
                }
                else
                {
                    QuestionString = "Sorry, I couldn't find anything, try a different question?";
                    return message.CreateReplyMessage(QuestionString);
                }
                // return our reply to the user                
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private static async Task<StackBot> GetJsonFromStack(string Query)
        {
            StackBot Data = new StackBot();
            Query = Uri.EscapeDataString(Query);
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var httpClient = new HttpClient(handler))
            {
                var apiUrl = ("http://api.stackexchange.com/2.2/search/excerpts?pagesize=5&order=desc&sort=relevance&q=" + Query + "&closed=True&site=stackoverflow");

                //setup HttpClient
                httpClient.BaseAddress = new Uri(apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //make request
                var response = await httpClient.GetStringAsync(apiUrl);

                Data = JsonConvert.DeserializeObject<StackBot>(response);

            }
            return Data;
        }


        private async Task<string> GetAnswers(int questionId)
        {
            string stringId = questionId.ToString();
            string response = "http://stackoverflow.com/questions/" + stringId;
            return response;
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
                return message.CreateReplyMessage($"Hi. I'll pull some answers from SO for you");
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