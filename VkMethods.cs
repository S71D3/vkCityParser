//using Microsoft.Analytics.Interfaces;
//using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;

using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace CSharpParser
{
    class VkMethods
    {
        VkApi api;
        City city;
        Country country;
        DbMethods db;

        public VkMethods()
        {
            this.api = new VkApi();
            this.db = new DbMethods();
        }

        public void TokenInit()
        {
            try
            {
                this.api.Authorize(new ApiAuthParams
                {
                    ApplicationId = 123456,
                    Login = "Login",
                    Password = "Password",
                    Settings = Settings.All
                }); //Api.Token содержит токен

                //Console.WriteLine(Api.Token);
                //var res = Api.Groups.Get(new GroupsGetParams());

                //Console.WriteLine(res.TotalCount);

                //Console.ReadLine();
            }
            catch {/*обработка, если не получилось создать токен*/}
        }

        public void SetCityId()
        {
            if (this.api != null)
            {
                this.country = new Country();
                country.Id = 1; //установка страны, по умолчанию Россия

                ReadOnlyCollection<City> cities = this.api.Database.GetCities(new VkNet.Model.RequestParams.Database.GetCitiesParams {
                    CountryId = Convert.ToInt32(this.country.Id),
                    Query = "Пермь", //здесь установка города, берем Пермь
                    NeedAll = false,
                    Count = 1,
                    Offset = 0
                });

                this.city = cities[0];
            }
            else {/*обработка пустого апи*/}
        }

        public void ParsePeoples()
        {
            //заполнение БД людей из города, подумать над оптимизацией
            //парсить можно только 1к людей за раз, парсим по пачкам
            //Добавить многопоточку
            ReadOnlyCollection<User> users;
            uint count = 0; //возможно нужно поменять на BigInteger, если нужно человек больше, чем 4кк
            do
            {
                users = this.api.Users.Search(new UserSearchParams
                {
                    City = Convert.ToInt32(city.Id),
                    Offset = count
                });

                //Добавляем id и кол-во в БД
                this.db.AddUser(users[0].Id, users[0].FriendLists.Count());

                count++;
            }
            while (users != null);
        }
    }
}