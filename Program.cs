
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ebo
{
    public class Program
    {
        private static string magicWordToStop = "";
        public static async Task Main(string[] args)
        {
            string userInput = "";
            string currentDirectory = Directory.GetCurrentDirectory();
            string magic = System.IO.Path.Combine(currentDirectory, "", ".magic");
            string openAiKeyPath = System.IO.Path.Combine(currentDirectory, "", ".openAiKey");

            TextReader trKey = new StreamReader(openAiKeyPath);
            string openAiKey = trKey.ReadLine();

            var openAiService = new OpenAIService(new OpenAiOptions() { ApiKey = openAiKey });

            TextReader tr = new StreamReader(magic);
            magicWordToStop = tr.ReadLine();

            Console.Write("This is EBO, the OpenAi chatbot, how can i help u? ");

            while (!userInput.Equals(magicWordToStop))
            {
                userInput = Console.ReadLine();
                var output = await OpenAiChatAsync(userInput, openAiService);
                Console.WriteLine(output);
            }

            Console.Write("Thank you for chating with EBO. ");
        }



        public static async Task<string> OpenAiChatAsync(string input,OpenAIService openAiService)
        {
            var completionResult = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = input,
                MaxTokens = 150,
                Temperature = (float?)0.9,
                TopP = 1,
                FrequencyPenalty = 0,
                PresencePenalty = (float?)0.6,
                Stop = magicWordToStop
            }, "text-davinci-003");

            if (completionResult.Successful)
            {
                return completionResult.Choices.FirstOrDefault().Text;
            }
            else
            {
                return "Something wrong has happened.Contact support";
            }

        }

    }

}
