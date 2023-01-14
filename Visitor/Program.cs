using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Dapper;
using Interfaces;
using Newtonsoft.Json;
using Serverito;

namespace Visitor
{
    internal class Program
    {
        private static Dictionary<string, User> loggedInUsers = new Dictionary<string, User>();
        public static void Main(string[] args)
        {
            var server = new ServeritoListener();
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
            server.AddView(new URL("/opendoor",HttpMethods.POST),context =>
            {
                CheckLoggedInAndDo(context,serveritoContext => 
                    Utils
                        .WriteToResponse(context.Context,new RequestService()
                        .AskForDoorToOpen()));
            } );
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
            Console.WriteLine("Visitor Server ready");
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
            return loggedInUsers.ContainsKey(hash);
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