using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class UsersTest : ControllerTest
    {
        public UsersTest(BALoginWebHost factory) : base(factory, "api/Users") { }

        [Fact]
        public async void Option_Ok()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "");
            var resp = await Client.SendAsync(req);
            Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
        }

        [Theory]
        [InlineData("DisplayName", "asc")]
        [InlineData("UserName", "asc")]
        [InlineData("RegisterTime", "asc")]
        [InlineData("ApprovedTime", "asc")]
        [InlineData("ApprovedBy", "asc")]
        [InlineData("DisplayName", "desc")]
        [InlineData("UserName", "desc")]
        [InlineData("RegisterTime", "desc")]
        [InlineData("ApprovedTime", "desc")]
        [InlineData("ApprovedBy", "desc")]
        public async void Get_Ok(string query, string order)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetAsJsonAsync<QueryData<object>>($"?sort={query}&order={order}&offset=0&limit=20&name=Admin&displayName=Administrator&_=1547628247338");
            Assert.Single(qd.rows);
        }

        [Theory()]
        [InlineData("Administrator")]
        [InlineData("Admin")]
        [InlineData("系统默认创建")]
        public async void Search_Ok(string search)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetAsJsonAsync<QueryData<object>>($"?search={search}&sort=&order=&offset=0&limit=20&category=&name=&define=0&_=1547608210979");
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            UserHelper.Delete(UserHelper.Retrieves().Where(usr => usr.UserName == "UnitTest_Delete").Select(usr => usr.Id));

            var nusr = new User { UserName = "UnitTest_Delete", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            var resp = await Client.PostAsJsonAsync<User, bool>("", nusr);
            Assert.True(resp);

            nusr.Id = UserHelper.Retrieves().First(u => u.UserName == nusr.UserName).Id;
            resp = await Client.PostAsJsonAsync<User, bool>("", nusr);
            Assert.True(resp);

            var ids = UserHelper.Retrieves().Where(d => d.UserName == nusr.UserName).Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("", ids));
        }

        [Fact]
        public async void PostById_Ok()
        {
            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;

            var ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{rid}?type=role", string.Empty);
            Assert.NotNull(ret);

            var gid = GroupHelper.Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{gid}?type=group", string.Empty);
            Assert.NotNull(ret);

            // 创建用户
            var nusr = new User { UserName = "UnitTest_Reset", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            UserHelper.Save(nusr);

            // 申请重置
            UserHelper.ForgotPassword(new ResetUser() { DisplayName = nusr.DisplayName, Reason = "UnitTest", ResetTime = DateTime.Now, UserName = nusr.UserName });

            // 重置操作
            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{nusr.UserName}?type=reset", string.Empty);
            Assert.NotNull(ret);

            UserHelper.Delete(UserHelper.Retrieves().Where(usr => usr.UserName == nusr.UserName).Select(usr => usr.Id));
        }

        [Fact]
        public async void PutById_Ok()
        {
            var ids = UserHelper.Retrieves().Where(u => u.UserName == "Admin").Select(u => u.Id);
            var gid = GroupHelper.Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{gid}?type=group", ids);
            Assert.True(ret);

            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{rid}?type=role", ids);
            Assert.True(ret);
        }
    }
}
