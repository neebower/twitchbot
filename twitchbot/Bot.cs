using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Models.v5.Channels;
using TwitchLib.Api.Models.v5.Users;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace twitchbot
{
    internal class Bot
    {
        readonly ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUserName, TwitchInfo.BotToken);
        TwitchClient client;

        TwitchAPI api;

        private int counter = 0;

        internal void Connect()
        {
            CommandsController cmd = new CommandsController(SendMessage, new MyBinarySerializer("commands.xml"));
            
           
            Console.WriteLine("Connecting..");
            client = new TwitchClient();

            client.OnLog += Client_OnLog;
            client.OnMessageReceived += Client_OnMessageReceived;
            
            client.Initialize(credentials, TwitchInfo.ChannelName);
            client.Connect();

            
            api = new TwitchAPI();
            api.Settings.ClientId = TwitchInfo.ClientId;
            api.Settings.AccessToken = TwitchInfo.BotTokenRefresh;

            Thread myThread1 = new Thread(new ParameterizedThreadStart(Spam));
            myThread1.Start(" https://site - подпишись на группу Вконтакте и будь в курсе последних событий");

            Thread myThread2 = new Thread(new ParameterizedThreadStart(Spam));
            myThread2.Start("Поддержи стримера. Подпишись на канал Kappa");
        }


        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            counter++;
            switch (e.ChatMessage.Message)
            {
                case "!vk":
                    {
                        SendMessage("Подписывайтесь на мою группу Вконтакте: https://site");
                        break;
                    }
                case "!pc":
                    {
                        SendMessage("Процессор: Intel Core i7 8700k; Материнка: ASUS ROG Strix z370 f-gaming; Видеокарта: GTX 1080 8gb; Оперативка: Corsair Vengeance RGB 16gb 3000mhz; БП: Corsair RM750i 750w; Корпус: Corsair 460x rgb; СЖО: Corsair h100i v2");
                        break;
                    }
                case "!sense":
                    {
                        SendMessage("Мышь Razer Basilisk. 700 dpi, sens 0.5");
                        break;
                    }
                case "!donate":
                    {
                        SendMessage("Поддержать меня вы можете по этой ссылке: https://site");
                        break;
                    }
                case "!youtube":
                    {
                        SendMessage("https://site");
                        break;
                    }
                case "!uptime":
                    {
                        var span = GetUptime();
                        SendMessage(span == null? "Стример спит или продает пирожки": $"Cтрим идет уже {DateFormatForAge(TimeSpan.FromTicks(GetUptime().Value.Ticks /*- TimeSpan.TicksPerHour * 3*/))}");
                        break;
                    }
                case "!followage":
                    {
                        if (e.ChatMessage.IsBroadcaster) break;
                        //TimeSpan? time = GetFollowAge(e.ChatMessage.UserId);
                        CultureInfo ci = new CultureInfo("ru-RU");

                        DateTimeFormatInfo dtfi = ci.DateTimeFormat;
                        
                        DateTime? time = GetFollowSince(e.ChatMessage.UserId);
                        SendMessage(time == null ? "Вы, Уважаемый, подписаться забыли." : $"{e.ChatMessage.DisplayName} подписан с {time.Value.Day} {dtfi.MonthGenitiveNames[time.Value.Month-1]} 2018г. ({ (int)TimeSpan.FromTicks(DateTime.Now.Ticks - time.Value.Ticks).TotalDays} дней)");
                        //SendMessage(time == null?  "Вы, Уважаемый, подписаться забыли." : $"{e.ChatMessage.DisplayName} с нами уже {DateFormatForAge(time)}");
                        break;
                    }
                case "!follows":
                    {
                        
                        SendMessage($"Нас уже {GetFollowers()?.Count()}, возрадуемся!");
                        break;
                    }
            }

           
        }

        private string DateFormatForAge(TimeSpan? span)
        {
            if(span == null)
            {
                return null;
            }

            Dictionary<string, int> date = new Dictionary<string, int>()
            {
                 {"year", 12*30*24*60*60 },
                 {"month", 30*24*60*60 },
                {"week", 24*60*60 * 7  },
                  {"day" , 24*60*60 },
                  {"hour", 60*60 },
                  {"minute", 60 },
                
            };
            

            StringBuilder builder = new StringBuilder();

            int seconds = (int)span.Value.TotalSeconds;
            foreach (var r in date)
            {
                if (seconds / r.Value == 0) continue;

                int except = seconds / r.Value;
                seconds -= except * r.Value;
                builder.Append(except + " ");
                //if(except)
                if(except / 10 == 1)
                {
                    switch (r.Key)
                    {
                        case "year":
                            {
                                builder.Append("лет");
                                break;
                            }
                        case "month":
                            {
                                builder.Append("месяцев");
                                break;
                            }
                        case "week":
                            {
                                builder.Append("недель");
                                break;
                            }
                        case "day":
                            {
                                builder.Append("дней");
                                break;
                            }
                        case "hour":
                            {
                                builder.Append("часов");
                                break;
                            }
                        case "minute":
                            {
                                builder.Append("минут");
                                break;
                            }
                        
                    }
                }
                else if (except % 10 == 1)
                {
                    switch (r.Key)
                    {
                        case "year":
                            {
                                builder.Append("год");
                                break;
                            }
                        case "month":
                            {
                                builder.Append("месяц");
                                break;
                            }
                        case "week":
                            {
                                builder.Append("неделю");
                                break;
                            }
                        case "day":
                            {
                                builder.Append("день");
                                break;
                            }
                        case "hour":
                            {
                                builder.Append("час");
                                break;
                            }
                        case "minute":
                            {
                                builder.Append("минуту");
                                break;
                            }
                        
                    }
                }
                else if (except % 10 > 1 && except % 10 < 5)
                {
                    
                    switch (r.Key)
                    {
                        case "year":
                            {
                                builder.Append("года");
                                break;
                            }
                        case "month":
                            {
                                builder.Append("месяца");
                                break;
                            }
                        case "week":
                            {
                                builder.Append("недели");
                                break;
                            }
                        case "day":
                            {
                                builder.Append("дня");
                                break;
                            }
                        case "hour":
                            {
                                builder.Append("часа");
                                break;
                            }
                        case "minute":
                            {
                                builder.Append("минуты");
                                break;
                            }
                        
                    }
                }
                else if (except % 10 >= 5)
                {
                    switch (r.Key)
                    {
                        case "year":
                            {
                                builder.Append("лет");
                                break;
                            }
                        case "month":
                            {
                                builder.Append("месяцев");
                                break;
                            }
                        case "week":
                            {
                                builder.Append("недель");
                                break;
                            }
                        case "day":
                            {
                                builder.Append("дней");
                                break;
                            }
                        case "hour":
                            {
                                builder.Append("часов");
                                break;
                            }
                        case "minute":
                            {
                                builder.Append("минут");
                                break;
                            }
                       
                    }
                }
                
                builder.Append(" ");
            }

            builder.Append(seconds + " ");

            if (seconds / 10 == 1) builder.Append("секунд");
            else if(seconds% 10 == 1) builder.Append("секунду");
            else if(seconds% 10 >1 && seconds% 10 < 5) builder.Append("секунды");
            else builder.Append("секунд");
            return builder.ToString();
        }
            

        private DateTime? GetFollowSince(string userid)
        {
            try
            {
                if (userid == null)
                {
                    return null;
                }
                IEnumerable<UserFollow> r;
                r = api.Users.v5.GetUserFollowsAsync(userid)?.Result.Follows;

                if (!r.Any())
                {
                    return null;
                }
                return r.Where(x => x.Channel.Name == TwitchInfo.ChannelName).First().CreatedAt;
            }
            catch
            {
                return null;
            }
         

        }

        private TimeSpan? GetFollowAge(string userid)
        {
            if (userid == null)
            {
                return null;
            }
           
            var r = api.Users.v5.GetUserFollowsAsync(userid)?.Result.Follows;

            if(!r.Any())
            {
                return null;
            }
        

            //if(createDate == null)
            //{
            //    return null;
            //}
            return TimeSpan.FromTicks(
                DateTime.Now.Ticks
                //- TimeSpan.TicksPerHour * 3 
                - r.Where(
                    x => x.Channel.Name == TwitchInfo.ChannelName )
                    .First().CreatedAt.Ticks);
        }

        private ChannelFollow[] GetFollowers()
        {
            try
            {
                string userId = GetUserId(TwitchInfo.ChannelName);
                if (userId == null)
                {
                    return null;
                }


                return api.Channels.v5.GetAllFollowersAsync(userId).Result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private TimeSpan? GetUptime()
        {
            try
            {


                string userId = GetUserId(TwitchInfo.ChannelName);
                if (userId == null)
                {
                    return null;
                }


                return api.Streams.v5.GetUptimeAsync(GetUserId(TwitchInfo.ChannelName)).Result;
            }
            catch
            {
                return null;
            }
        }


        private string GetUserId(string name)
        {
            try
            {

            
            User[] userlist = api.Users.v5.GetUserByNameAsync(name).Result.Matches;

            if(userlist == null || userlist.Length == 0)
            {
                return null;
            }

            return userlist[0].Id;
            }
            catch
            {
                return null;
            }
    }


        private void SendMessage(string text)
        {
            try
            {


                client.SendMessage(TwitchInfo.ChannelName, text);
            }
            catch
            {
                throw;
            }

        }


        private void Client_OnLog(object sender, OnLogArgs e)
        {

            Console.WriteLine(e.Data);
            Console.WriteLine("----------------------");
        }

        public void Spam(object x)
        {
            
            string str = x as string;
            
            //very bad)
            int mult = str.StartsWith("Поддер") ? 28 : 30;
            for (; ; )
            {
                if (counter > 2)
                {
                    Thread.Sleep(1000 * 60 * mult);
                    SendMessage(str);
                    counter = 0;
                }
                else
                {
                    Thread.Sleep(1000 * 60);
                }
                
            }
        }
    }
}
