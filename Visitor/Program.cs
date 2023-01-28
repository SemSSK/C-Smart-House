using System;
using System.Net;
using Interfaces;
using Newtonsoft.Json;
using Serverito;

namespace Visitor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var server = new ServeritoListener();
            server.AddView(new URL("/",HttpMethods.OPTIONS,UrlMatchingType.StartsWith), context =>
            {
                AllowCors(context.Context);
                Utils.WriteToResponse(context.Context,"");
            });
            server.AddView(new URL("/lock",HttpMethods.GET), context =>
            {
                AllowCors(context.Context);
                Utils.WriteToResponse(context.Context,"Hey");
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
            server.AddView(new URL("/opendoor",HttpMethods.POST),context =>
            {
                AllowCors(context.Context);
                var username = ReadAuthenticationData(context.Context);
                Console.WriteLine(username);
                CheckLoggedInAndDo(context,serveritoContext => 
                    Utils
                        .WriteToResponse(context.Context,new RequestService()
                        .AskForDoorToOpen(username)));
            } );
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
            server.Start();
            Console.WriteLine("Visitor Server ready");
        }
        private static void AllowCors(HttpListenerContext ctx)
        {
            ctx.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            ctx.Response.Headers.Add("Access-Control-Allow-Methods","GET,PUT,POST,DELETE,PATCH,OPTIONS");
            ctx.Response.Headers.Add("Access-Control-Allow-Origin","*");
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