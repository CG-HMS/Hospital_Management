using Hms.MVC.Models.Room;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize(Roles = "admin,nurse,physician")]
public class RoomController : Controller
{
    private readonly IApiService _api;

    public RoomController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _api.GetAsync<List<RoomViewModel>>("rooms");
        return View(rooms ?? new List<RoomViewModel>());
    }

    [HttpGet("Available")]
    public async Task<IActionResult> Available()
    {
        var rooms = await _api.GetAsync<List<RoomViewModel>>("rooms/available");
        return View("Index", rooms ?? new List<RoomViewModel>());
    }
}
