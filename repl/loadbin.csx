#r "./dll/System.ComponentModel.Annotations.dll"
#r "./dll/Newtonsoft.Json.dll"
#r "./dll/Efir.DataHub.Models.dll"
#r "./dll/RuDataAPI.dll"


using RuDataAPI;
using System;

EfirCredentials creds = new EfirCredentials() 
{
    Url = "https://dh2.efir-net.ru/v2",
    Login = Args[0],
    Password = Args[1]
};

Console.WriteLine($"Efir credentials created for ({creds.Login}, {creds.Password}). See 'creds' variable (availavle in REPL context).");