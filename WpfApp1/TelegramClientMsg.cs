using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WpfApp1
{
    class TelegramMessageClient
    {
        private MainWindow w;

        private TelegramBotClient bot;
        public ObservableCollection<MessageLog> BotMessageLog { get; set; }

        private void MessageListener(object sender, MessageEventArgs e)
        {
           
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";

            Debug.WriteLine($"{text} TypeMessage: {e.Message.Type.ToString()}");

            

            var messageText = e.Message.Text;
            var MsgType = e.Message.Type;
            var chatId = e.Message.Chat.Id;
            var msg = e.Message;
            if(messageText == "/start")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Hi!");
            }
            if(e.Message.Type == MessageType.Text && messageText != "/start") bot.SendTextMessageAsync(e.Message.Chat.Id, "Send me your file");
            if(e.Message.Type == MessageType.Document)
            {
                bot.SendTextMessageAsync(chatId, "Download file...");
                Download(e.Message.Document.FileId, e.Message.Document.FileName,msg);
            }
            if(e.Message.Type == MessageType.Photo)
            {
                bot.SendTextMessageAsync(chatId, "Download photo...");
                Download(e.Message.Photo[e.Message.Photo.Length - 1].FileId.ToString(), $"{e.Message.MessageId.ToString()}.jpg",msg);
            }
            if(e.Message.Type == MessageType.Sticker)
            {
                bot.SendTextMessageAsync(chatId: e.Message.Chat, "I got a sticker. Here is a sticker for you");
                bot.SendStickerAsync(chatId: e.Message.Chat, sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-fred.webp");
            }
            if(e.Message.Type == MessageType.Video)
            {
                bot.SendTextMessageAsync(chatId, "Download video...");
                Download(e.Message.Video.FileId, e.Message.Video.FileName,msg);
            }
            w.Dispatcher.Invoke(() =>
            {
                BotMessageLog.Add(
                new MessageLog(
                    DateTime.Now.ToLongTimeString(), messageText, e.Message.Chat.FirstName, e.Message.Chat.Id));
            });
        }

        public TelegramMessageClient(MainWindow W)
        {
            this.BotMessageLog = new ObservableCollection<MessageLog>();
            this.w = W;

            bot = new TelegramBotClient("5231816245:AAE06GBsraf_BUDeLeNelCUKXpRdBIJdzWE");

            bot.OnMessage += MessageListener;

            bot.StartReceiving();
        }

        public void SendMessage(string Text, string Id)
        {
            long id = Convert.ToInt64(Id);
            bot.SendTextMessageAsync(id, Text);
        }
        public async void Download(string fileId, string path, Message msg)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(@"E:\TeleFiles\" + $"{msg.Chat.Id}");
                if (!dir.Exists)
                {
                    dir.Create();
                }
                var file = await bot.GetFileAsync(fileId);
                FileStream fs = new FileStream($@"E:\TeleFiles\{msg.Chat.Id}\" + $"_{path}", FileMode.Create);
                await bot.DownloadFileAsync(file.FilePath, fs);
                fs.Close();

                fs.Dispose();
                await bot.SendTextMessageAsync(msg.Chat.Id, "Установка успешно завершена");
            }
            catch (Exception ex)
            {
                await bot.SendTextMessageAsync(msg.Chat.Id, "Ошибка\n" +
                   $"{ex.Message}");
                Console.WriteLine("Error downloading: " + ex.Message);

            }
        }
        public void SaveHistory()
        {
            string json = JsonConvert.SerializeObject(BotMessageLog);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "bot_data";
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON file (.json)|*.json";
            if(dlg.ShowDialog() == true)
            {
                string fileName = dlg.FileName;
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine(json);
                }
            }
            MessageBox.Show("Data is saved", "Save data", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void Clear()
        {
            BotMessageLog.Clear();
               
        }
    }
}
