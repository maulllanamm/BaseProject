﻿using DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace BaseProject.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult> GetByListId([FromQuery] List<Guid> listId)
        {
            var users = await _userService.GetByListId(listId);
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult> GetByPage([FromQuery] int page)
        {
            var users = await _userService.GetAll(page);
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] Guid id)
        {
            var users = await _userService.GetById(id);
            return Ok(users);
        }


        [HttpPut]
        public async Task<ActionResult> Update(UserDTO userDto)
        {
            var users = await _userService.Update(userDto);
            return Ok(users);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBulk(List<UserDTO> userDto)
        {
            var users = await _userService.UpdateBulk(userDto);
            return Ok(users);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            var users = await _userService.Delete(id);
            return Ok(users);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteBulk(List<UserDTO> userDto)
        {
            var users = await _userService.DeleteBulk(userDto);
            return Ok(users);
        }
        [HttpDelete]
        public async Task<ActionResult> SoftDelete(Guid id)
        {
            var users = await _userService.SoftDelete(id);
            return Ok(users);
        }
        [HttpDelete]
        public async Task<ActionResult> SoftDeleteBulk(List<Guid> listId)
        {
            var users = await _userService.SoftDeleteBulk(listId);
            return Ok(users);
        }
    }
}
