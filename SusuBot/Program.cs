using System;
using System.Diagnostics;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using OpenCvSharp;

namespace SusuBot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("APIKEYHERE");

        private static Boolean _disconnectApp = false;
        private static Random rand = new Random();
        public static int i = 1;

        static void Main(string[] args)
        {
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            Bot.StartReceiving();
            while (!_disconnectApp)
            {

            }
            Bot.StopReceiving();
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
        }

        private static void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;
            if (message.Text.StartsWith("/help"))
            {
                var text = "Hello! I'm Susu.\n Here are some commands :)\n /GetPic - Get a random picture, of me of course!";
                await Bot.SendTextMessageAsync(message.Chat.Id, text);
            }
            else if (message.Text.StartsWith("/getpic"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, ":D");
                var files = Directory.GetFiles("c:\\susu", "*.jpg");
                FileInfo fileInfo = new FileInfo(files[rand.Next(files.Length)]);
                FileStream fs = fileInfo.OpenRead();
                FileToSend fileToSend = new FileToSend(fileInfo.FullName, fs);
                await Bot.SendPhotoAsync(message.Chat.Id, fileToSend);
            }
            else if (message.Text.StartsWith("/getvideo"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, ":D");
                var files = Directory.GetFiles("c:\\susu", "*.mp4");
                FileInfo fileInfo = new FileInfo(files[rand.Next(files.Length)]);
                FileStream fs = fileInfo.OpenRead();
                FileToSend fileToSend = new FileToSend(fileInfo.FullName, fs);
                await Bot.SendVideoAsync(message.Chat.Id, fileToSend);
            }
            else if (message.Text.StartsWith("/howareyou"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, "Meh, i will show you");
                using (CvCapture cap = CvCapture.FromCamera(2))
                {
                    using (IplImage img = cap.QueryFrame())
                    {
                        i = i + 1;
                        var file = string.Format("c:\\susu\\000{0}.jpg", i);
                        img.SaveImage(file);
                        FileInfo fileInfo = new FileInfo(file);
                        FileStream fs = fileInfo.OpenRead();
                        FileToSend fileToSend = new FileToSend(fileInfo.FullName, fs);
                        await Bot.SendPhotoAsync(message.Chat.Id, fileToSend);

                    }
                }
            }
            else if (message.Text.StartsWith("/Bye"))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, "GoodBye!!");
                _disconnectApp = true;
            }

        }

        private static void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
        }

    }
}
