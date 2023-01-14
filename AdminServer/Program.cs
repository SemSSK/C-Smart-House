using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Interfaces;
using Newtonsoft.Json;
using Serverito;
using System.Text.Json;

namespace AdminServer
{
    internal class Program
    {
        
        private static Dictionary<string, User> loggedInUsers = new Dictionary<string, User>();
        
        public static void Main(string[] args)
        {
            var channel = new TcpChannel(Ports.REQPORT);
            ChannelServices.RegisterChannel(channel);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(RequestManager),Names.REQNAME,WellKnownObjectMode.Singleton);

            var server = new ServeritoListener("http://localhost:8001/");
            Console.WriteLine("Admin Server ready listening on http://localhost:8001/");
            new ConnexionService().GetUsers().ForEach(user =>
            {
                Console.WriteLine(user.username);
            });
            server.AddView(new URL("/temperature",HttpMethods.GET), context =>
            {
                CheckLoggedInAndDo(context,serveritoContext => 
                    Utils
                    .WriteToResponse(context.Context,new TemperatureViewer().GetTemperature()
                    .Value
                    .ToString()));
                ;
            } );
            server.AddView(new URL("/humidity",HttpMethods.GET), context =>
            {
                CheckLoggedInAndDo(context,serveritoContext => 
                    Utils
                        .WriteToResponse(context.Context,new HumidityViewer()
                            .GetHumidity()
                            .Value
                            .ToString()));
            } );
            server.AddView(new URL("/alert",HttpMethods.GET), context =>
            {
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    Utils
                        .WriteToResponse(serveritoContext.Context,
                            JsonConvert.SerializeObject(new MovementCapture().GetCaptures()));
                });
            });
            
            
            //Door controller
            server.AddView(new URL("/door",HttpMethods.GET), context =>
            {
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    var req = RequestManager.getRequest();
                    Utils
                        .WriteToResponse(serveritoContext.Context,
                            req ?? "");
                });
            });
            server.AddView(new URL("/door/open",HttpMethods.POST), context =>
            {
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    new DoorService().TurnOn();
                    Utils.WriteToResponse(serveritoContext.Context,"");
                });
            });
            server.AddView(new URL("/door/close",HttpMethods.POST), context =>
            {
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    new DoorService().TurnOff();
                    Utils.WriteToResponse(serveritoContext.Context,"");
                });
            });
            
            //Users Controller
            server.AddView(new URL("/user",HttpMethods.GET), context =>
            {
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(new ConnexionService().GetUsers());
                    Utils.WriteToResponse(serveritoContext.Context,json);
                });
            });
            server.AddView(new URL("/user",HttpMethods.POST), context =>
            {
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    var user = JsonConvert.DeserializeObject<User>(ReadBodyToString(serveritoContext.Context));
                    if (user != null)
                    {
                        new ConnexionService().AddUser(user.username,user.password);
                    }
                    Utils.WriteToResponse(serveritoContext.Context,"");
                });
            });
            server.AddView(new URL("/user",HttpMethods.DELETE), context =>
            {
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    var user = JsonConvert.DeserializeObject<User>(ReadBodyToString(serveritoContext.Context));
                    if (user != null && user.Id != null && user.Id != 0)
                    {
                        new ConnexionService().DeleteUser(user.Id);
                    }
                    Utils.WriteToResponse(serveritoContext.Context,"");
                });
            });
           
            //Login controller
            server.AddView(new URL("/login",HttpMethods.POST), context =>
            {
                var maybeUsr = JsonConvert.DeserializeObject<User>(ReadBodyToString(context.Context));
                if (maybeUsr.username is null || maybeUsr.password is null)
                {
                    context.Context.Response.StatusCode = 403;
                    Utils.WriteToResponse(context.Context,"Wrong credentials");
                }
                var user = new ConnexionService().Login(maybeUsr.username, maybeUsr.password);
                if (user is null)
                {
                    context.Context.Response.StatusCode = 403;
                    Utils.WriteToResponse(context.Context,"Wrong credentials");
                }
                else
                {
                    loggedInUsers.Add(user.username,user);
                    Utils.WriteToResponse(context.Context,user.username);     
                } 
            });
            
            server.Start();
        }
        
        private static string ReadBodyToString(HttpListenerContext ctx)
        {
            return Utils.ReadRequestInput(ctx);
        }
        

        private static string ReadAuthenticationData(HttpListenerContext ctx)
        {
            var header = ctx.Request.Headers;
            var key = header.Get("Auth");
            return key;
        }

        private static bool CheckIfLoggedIn(string hash)
        {
            if (hash is null)
            {
                return false;
            }
            return loggedInUsers.ContainsKey(hash) && loggedInUsers[hash].isAdmin;
        }

        private static void CheckLoggedInAndDo(ServeritoContext context,ViewFunction func)
        {
            var ctx = context.Context;
            var loggedIn = CheckIfLoggedIn(ReadAuthenticationData(ctx));
            if (loggedIn)
            {
                func(context);
            }
            else
            {
                ctx.Response.StatusCode = 403;
                Utils.WriteToResponse(ctx,"Not logged in");
            }
        }
    }
}