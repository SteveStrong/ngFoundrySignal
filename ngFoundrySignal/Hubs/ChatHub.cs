using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System;

// https://blogs.msdn.microsoft.com/webdev/2017/09/14/announcing-signalr-for-asp-net-core-2-0/
// https://damienbod.com/2017/09/12/getting-started-with-signalr-using-asp-net-core-and-angular/
// https://blog.3d-logic.com/2017/09/18/the-signalr-for-asp-net-core-javascript-client-part-1-web-applications/


namespace ngFoundrySignal
{

    //https://github.com/aspnet/SignalR/tree/dev/samples/SocialWeather
    //public class JsonStreamFormatter<T> : IStreamFormatter<T>
    //{
    //    private JsonSerializer _serializer = new JsonSerializer();

    //    public async Task<T> ReadAsync(Stream stream)
    //    {
    //        var reader = new JsonTextReader(new StreamReader(stream));
    //        // REVIEW: Task.Run()
    //        return await Task.Run(() => _serializer.Deserialize<T>(reader));
    //    }

    //    public Task WriteAsync(T value, Stream stream)
    //    {
    //        var writer = new JsonTextWriter(new StreamWriter(stream));
    //        _serializer.Serialize(writer, value);
    //        writer.Flush();
    //        return Task.FromResult(0);
    //    }
    //}

    public class ChatHub : Hub
    {

        public Task SayHello()
        {
            return Clients.All.SendAsync("hello from chathub");
        }

        public Task Send(object message)
        {
            return Clients.All.SendAsync("Send", message);
        }

        public Task SendFrom(string sender, string message)
        {
            // Call the broadcastMessage method to update clients.
            return Clients.All.SendAsync("ReceiveFrom", sender, message);
        }

        public Task Version()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString();
            //var buildDate = ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute))).Description;

            var message = $"{version}";  // {buildDate}";
            return Clients.All.SendAsync("Version", message);
        }

        public Task Broadcast(string channel, object message)
        {
            return Clients.All.SendAsync(channel, message);
        }

        //https://damienbod.com/2017/12/05/sending-direct-messages-using-signalr-with-asp-net-core-and-angular/
        public Task Command(string channel, object command, object data)
        {
            var id = Context.ConnectionId;
            var list = new string[1] { id };

            return Clients.AllExcept(list).SendAsync(channel, command, data);
        }

        //https://damienbod.com/2017/09/18/signalr-group-messages-with-ngrx-and-angular/
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("JoinGroup", groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Clients.Group(groupName).SendAsync("LeaveGroup", groupName);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
