using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ServicesAbstractions;

namespace API.Controllers;

[Authorize(Roles = "Student")]
[Route("api/[controller]")]
[ApiController]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpPost("toggle/{courseId}")]
    public async Task<IActionResult> ToggleWishlist(int courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _wishlistService.ToggleWishlistAsync(userId, courseId);

        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyWishlist()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var wishlist = await _wishlistService.GetMyWishlistAsync(userId);

        return Ok(wishlist);
    }
}