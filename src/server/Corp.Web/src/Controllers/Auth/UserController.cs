using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Corp.Controllers.Auth;

[Route("api/[area]/[controller]")]
[Area(nameof(Auth))]
[Tags(nameof(Auth))]
[Authorize(Policy = AppPolicy.RequireUserRole)]
[ApiController]
public class UserController(
    ILogger<UserController> logger,
    UserManager<IdentityUser> userManager) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    /// <summary>
    /// Returns the current user.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A user object.</returns>
    [HttpGet]
    [Produces(typeof(IdentityUserModel))]
    [EndpointDescription("Returns the user object for the logged-in user.")]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        string id = User.GetNameIdentifier();

        IdentityUser? result = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
        if (result is null)
            return NotFound();

        return Ok(result.ToModel());
    }

    /// <summary>
    /// Updates the user object.
    /// </summary>
    /// <param name="model">The user object.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The updated user object.</returns>
    [HttpPut]
    [Produces(typeof(IdentityUserModel))]
    public async Task<ActionResult> Put(IdentityUserModel model, CancellationToken cancellationToken)
    {
        string id = User.GetNameIdentifier();

        IdentityUser? record = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
        if (record is null)
            return BadRequest();

        record.UpdateFrom(model);
        await _userManager.UpdateAsync(record);

        IdentityUser? result = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
        if (result is null)
            return BadRequest();

        return Ok(result.ToModel());
    }
}
