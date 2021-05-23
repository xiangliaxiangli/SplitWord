using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http.Cors;
using System.Data.Entity;
namespace TestWebAPI.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private SplitWordDB db = new SplitWordDB();

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        #region--用户登录验证
        [HttpPost]
        public string Login(string userName, string password)
        {

            User tempUser = null;
            tempUser = db.User.FirstOrDefault(x => x.userName.Equals(userName) && x.password.Equals(password));
            if (tempUser != null)
            {
                return "success|" + tempUser.userType;
            }
            return "fail";
        }
        #endregion

        #region--用户注册
        [HttpPost]
        public string Register(string userName, string password, string sex)
        {
            User tempUser = new User();
            tempUser = db.User.FirstOrDefault(x => x.userName.Equals(userName));
            if (tempUser != null)
            {//改用户已存在  注册失败
                return "fail";
            }
            else
            {
                tempUser = new User();
                tempUser.userName = userName;
                tempUser.password = password;
                tempUser.userSex = sex;
                //默认普通用户
                tempUser.userType = "normal";

                try
                {
                    db.User.Add(tempUser);
                    db.SaveChanges();
                    return "success";
                }
                catch (Exception e)
                {
                    return "fail";
                }
            }
        }
        #endregion

        #region--获取所有用户信息
        [HttpPost]
        public IHttpActionResult GetAllUserInfo()
        {
            List<User> userList = new List<User>();
            #region
            //测试数据
            User temp = new User();
            temp.userName = "guyao";
            temp.userSex = "男";
            temp.userType = "noraml";
            temp.UserID = 1;
            temp.password = "1234567";
            userList.Add(temp);

            User temp1 = new User();

            temp1.userName = "guyao1";
            temp1.userSex = "女";
            temp1.userType = "noraml";
            temp1.UserID = 11;
            temp1.password = "12345671";
            userList.Add(temp);

            return Json<List<User>>(userList);
            #endregion
            //return Json<List<User>>(db.User.ToList());
        }
        #endregion

        #region--修改用户信息
        public string ModifyUserInfo(int userId, string userName, string password, string sex, string userType)
        {
            User tempUser = null;
            tempUser = db.User.Find(userId);
            if (tempUser != null)
            {
                try
                {
                    tempUser.userName = userName;
                    tempUser.password = password;
                    tempUser.userSex = sex;
                    tempUser.userType = userType;

                    db.Entry(tempUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return "success";
                }
                catch (Exception e)
                {
                    return "fail";
                }
            }
            return "fail";
        }
        #endregion

        #region--通过userID删除用户---不删除文章
        [HttpPost]
        public string DeleteUserById(int userID)
        {
            try
            {
                User tempuser = db.User.Find(userID);
                if (tempuser != null)
                {
                    db.User.Remove(tempuser);
                    db.SaveChanges();
                    return "success";
                }
                else
                {
                    return "fail";
                }

            }
            catch (Exception e)
            {
                return "fail";
            }
        }
        #endregion

        #region--通过userName删除用户---不删除文章
        [HttpPost]
        public string DeleteUserByName(string userName)
        {
            try
            {
                User tempuser = db.User.FirstOrDefault(x => x.userName.Equals(userName));
                if (tempuser != null)
                {
                    db.User.Remove(tempuser);
                    db.SaveChanges();
                    return "success";
                }
                else
                {
                    return "fail";
                }

            }
            catch (Exception e)
            {
                return "fail";
            }
        }
        #endregion
    }
}