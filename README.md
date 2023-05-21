# RuData API
RuData API - .NET-библиотека, построенная поверх RUDATA API, разработанным Interfax ([подробнее](https://rudata.info/rd-api)). 
RuData API предназначена для взаимодействия с сервером EFIR.DataHub от Interfax (RuData) и позволяет получить справочные и исторические данные по различным финансовым инструментам.

## Зависимости

- Microsoft.AspNet.WebApi.Client
- Efir.DataHub.Models ([подробнее](https://developer.efir-net.ru/NuGetFeed))

`Efir.DataHub.Models` является внешним NuGet-пакетом, поэтому для разрешения проблем с зависимостями проекта проще использовать .NET CLI: 
```powershell
dotnet add package Microsoft.AspNet.WebApi.Client
dotnet add package Efir.DataHub.Models --source http://developer.efir-net.ru/nuget
```

## Использование

```csharp
using RuDataAPI;
using RuDataAPI.Extensions;

// reading credentials from json file and instantiate EFIR client
var credentials = EfirClient.GetCredentialsFromFile("MyCredentils.json");
using EfirClient efir = new EfirClient(credentials);

// logging to EFIR server
// consider call this method before fetching any data from EFIR server
await efir.LoginAsync();

// obtaining bond-issue static parameters
var secinfo = await efir.GetSecurityData("RU000A100EF5");
Console.WriteLine(secinfo.nickname);
Console.WriteLine(secinfo.endmtydate);
Console.WriteLine(secinfo.fintoolid);

// obtaining 3-month MOEX G-Curve rate known as of 19 May 2023
double rate3m = efir.CalculateGcurveForDate(new DateTime(2023, 5, 19), .25);
```

## Файл с параметрами авторизации

Для хранения данных с параметрами авторизации используется json-файл со следующим содержанием:
```json
{
  "Url": "https://dh2.efir-net.ru/v2",
  "Login": "my-login",
  "Password": "my-password"
}
```