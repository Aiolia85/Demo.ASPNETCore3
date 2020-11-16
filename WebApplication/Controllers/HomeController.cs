using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entity;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        public HomeController(ILogger<HomeController> logger, IOptions<AppSetting> option, IUserInfoRepository repository, IMapper mapper)
        {
            this._logger = logger;
            this._option = option;
            this._repository = repository;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {

            UserInfo ui = new UserInfo();
            ui.CreateDate = DateTime.Now;
            ui.Username = "testName";
            ui.Password = "password";

            this._repository.Insert(ui);
            this._repository.Save();

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
