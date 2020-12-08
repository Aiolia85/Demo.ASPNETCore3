using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reoository.EF;
using Repository;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<AppSetting> _option;
        private readonly IUserInfoRepository _repository;
        private readonly IMapper _mapper;
        private readonly LocalDBContext1 _dbContext1;
        private readonly LocalDBContext _dbContext2;

        public HomeController(ILogger<HomeController> logger, IOptions<AppSetting> option, IUserInfoRepository repository, IMapper mapper, LocalDBContext1 dbContext1,LocalDBContext dbContext2)
        {
            this._logger = logger;
            this._option = option;
            this._repository = repository;
            this._mapper = mapper;
            this._dbContext1 = dbContext1;
            this._dbContext2 = dbContext2;
        }
        public IActionResult Index()
        {
            var allUser=this._repository.GetAll();
            foreach (var user in allUser)
            {
                this._repository.Delete(user);
            }
            this._repository.Save();
            using (TransactionScope ts = new TransactionScope())
            {
                UserInfo ui = new UserInfo();
                ui.CreateDate = DateTime.Now;
                ui.Username = "testName";
                ui.Password = "password";

                _dbContext1.UserInfo.Add(ui);
                _dbContext1.SaveChanges();

                TestTable tt = new TestTable();
                tt.StrCol = "abc";
                _dbContext2.TestTable.Add(tt);
                _dbContext2.SaveChanges();

                ts.Complete();
            }


            //this._repository.Insert(ui);
            //this._repository.Save();

            _logger.LogInformation("adsbv");

            List<UserInfo> users = this._repository.GetAll().ToList();
            IEnumerable<UserDTO> sendMsgViewModels = _mapper.Map<List<UserInfo>, IEnumerable<UserDTO>>(users);


            throw new Exception("ddsdfsdfsd");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
