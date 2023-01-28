using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Interfaces;
using Newtonsoft.Json;
using Serverito;

namespace AdminServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var server = new ServeritoListener("http://localhost:8001/");
            var channel = new TcpChannel(Ports.REQPORT);
            ChannelServices.RegisterChannel(channel);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RequestManager), Names.REQNAME,
                WellKnownObjectMode.Singleton);
            Console.WriteLine("Admin Server ready listening on http://localhost:8001/");
            server.AddView(new URL("/",HttpMethods.OPTIONS,UrlMatchingType.StartsWith), context =>
            {
                AllowCors(context.Context);
                Utils.WriteToResponse(context.Context,"");
            });
            server.AddView(new URL("/temperature",HttpMethods.GET), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context,serveritoContext => 
                    Utils
                    .WriteToResponse(context.Context,new TemperatureViewer().GetTemperature()
                    .Value
                    .ToString()));
                ;
            } );
            server.AddView(new URL("/humidity",HttpMethods.GET), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context,serveritoContext => 
                    Utils
                        .WriteToResponse(context.Context,new HumidityViewer()
                            .GetHumidity()
                            .Value
                            .ToString()));
            } );
            server.AddView(new URL("/alert",HttpMethods.GET), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    Utils
                        .WriteToResponse(serveritoContext.Context,
                            System.Text.Json.JsonSerializer.Serialize(new MovementCapture().GetCaptures()));
                });
            });
            
            
            //Door controller
            server.AddView(new URL("/door",HttpMethods.GET), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    var req = RequestManager.getRequest();
                    var json = System.Text.Json.JsonSerializer.Serialize(req);
                    Utils
                        .WriteToResponse(serveritoContext.Context,json);
                });
            });
            server.AddView(new URL("/lock",HttpMethods.GET), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    var state =  new
                    {
                        state = new DoorService().GetLockState()
                    };
                    Utils.WriteToResponse(serveritoContext.Context,
                        System.Text.Json.JsonSerializer.Serialize(state));
                });
            });
            server.AddView(new URL("/door/open",HttpMethods.POST), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    new DoorService().TurnOn();
                    Utils.WriteToResponse(serveritoContext.Context,"");
                });
            });
            server.AddView(new URL("/door/close",HttpMethods.POST), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    new DoorService().TurnOff();
                    Utils.WriteToResponse(serveritoContext.Context,"");
                });
            });
            
            //Users Controller
            server.AddView(new URL("/user",HttpMethods.GET), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(new ConnexionService().GetUsers());
                    Utils.WriteToResponse(serveritoContext.Context,json);
                });
            });
            server.AddView(new URL("/user",HttpMethods.POST), context =>
            {
                AllowCors(context.Context);
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
            server.AddView(new URL("/user",HttpMethods.PUT), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    var user = JsonConvert.DeserializeObject<User>(ReadBodyToString(serveritoContext.Context));
                    if (user != null)
                    {
                        Console.WriteLine("updating user");
                        new ConnexionService().UpdateUser(user);
                    }
                    Utils.WriteToResponse(serveritoContext.Context,"");
                });
            });
            server.AddView(new URL("/user",HttpMethods.DELETE), context =>
            {
                AllowCors(context.Context);
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
                AllowCors(context.Context);
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
                    Utils.WriteToResponse(context.Context,System.Text.Json.JsonSerializer.Serialize(user));     
                }
            });
            server.AddView(new URL("/logout", HttpMethods.POST), context =>
            {
                AllowCors(context.Context);
                CheckLoggedInAndDo(context, serveritoContext =>
                {
                    new ConnexionService().Logout(ReadAuthenticationData(serveritoContext.Context));
                    Utils
                        .WriteToResponse(serveritoContext.Context,"");
                });
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

        private static void AllowCors(HttpListenerContext ctx)
        {
            ctx.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            ctx.Response.Headers.Add("Access-Control-Allow-Methods","GET,PUT,POST,DELETE,PATCH,OPTIONS");
            ctx.Response.Headers.Add("Access-Control-Allow-Origin","*");
        }
        private static bool CheckIfLoggedIn(string hash)
        {
            if (hash is null)
            {
                return false;
            }

            return new ConnexionService().CheckIfLoggedIn(hash);
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