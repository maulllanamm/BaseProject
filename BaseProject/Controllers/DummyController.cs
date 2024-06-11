using Bogus;
using Bogus.DataSets;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interface;
using StackExchange.Redis;

namespace BaseProject.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DummyController : ControllerBase
    {
        private readonly IDummyService _service;
        private readonly ICacheService _cacheService;
        public DummyController(IDummyService service, ICacheService cacheService)
        {
            _service = service;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<ActionResult> GenerateUser(int amount)
        {
            var testUsers = new Faker<DummyDTO>()
            // Use an enum outside scope.
            .RuleFor(u => u.Gender, f => f.PickRandom(new[] { "Male", "Female" }))

            // Basic rules using built-in generators
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName((Name.Gender)f.Random.Number(0, 1)))
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName((Name.Gender)f.Random.Number(0, 1)))
            .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))

            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())

            //Use a method outside scope.
            .RuleFor(u => u.CartId, f => Guid.NewGuid());

            var user = testUsers.Generate(amount);
            await _service.CreateBulk(user);

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult> GetDummyUsers()
        {
            var cacheData = _cacheService.GetData<IEnumerable<DummyDTO>>("dummys");
            if (cacheData != null && cacheData.Count() > 0)
            {
                return Ok(cacheData);
            }
            var users = await _service.GetAll();

            // set expire time
            var expiryTime = DateTime.Now.AddSeconds(10);
            _cacheService.SetData<IEnumerable<DummyDTO>>("dummys", users, expiryTime);
            return Ok(users);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBulk(List<DummyDTO> dummys)
        {
            var users = await _service.UpdateBulk(dummys);
            return Ok(users);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBulk(List<DummyDTO> dummys)
        {
            var users = await _service.DeleteBulk(dummys);
            return Ok(users);
        }
    }

    
}
