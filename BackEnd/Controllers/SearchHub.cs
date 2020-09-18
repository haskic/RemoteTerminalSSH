using BackEnd.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BackEnd.Controllers
{
    public class SearchHub:Hub
    {
        private UserDbContext db;
        public SearchHub(UserDbContext context)
        {
            db = context;
        }
        public async Task GetSearchResult(string searchString)
        {
            
            var result = db.UserInfos.Where(value => value.NickName.StartsWith(searchString)).Select(v=> new { v.NickName, v.UserId}).Take(5);
            await this.Clients.All.SendAsync("GetSearchResult", JsonSerializer.Serialize(result));
        }
    }
}
