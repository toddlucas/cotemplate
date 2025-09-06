using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Corp.Pagination;

namespace Corp.Controllers.Identity;

[Route("api/[area]/[controller]")]
[Area(nameof(Identity))]
[Tags(nameof(Identity))]
[Authorize(Policy = AppPolicy.RequireAdminRole)]
[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public class UserController(
    ILogger<UserController> logger,
    UserManager<IdentityUser> userManager) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    /// <summary>
    /// Returns a user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A user object.</returns>
    [HttpGet]
    // [EndpointSummary("/api/demographics/profile")]
    [EndpointDescription("Returns a user details object.")]
    public async Task<ActionResult> Get(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        // TODO: Map to IdentityUserModel.
        IdentityUser? result = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
        if (result is null)
            return NotFound();

        return Ok(result.ToModel());
    }

    /// <summary>
    /// Returns a list of users.
    /// </summary>
    /// <param name="query">Pagination query parameters.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of users.</returns>
    [HttpGet("list")]
    [EndpointDescription("Returns a paginated list of users.")]
    public async Task<ActionResult> List([FromQuery] PagedQuery query, CancellationToken cancellationToken)
    {
        IQueryable<IdentityUser> queryable = _userManager.Users;

        query.Search((term) =>
        {
            queryable = queryable
                .Where(c => c.Email!.ToLower().Contains(term));
        });

        IdentityUser[] records = await queryable
            .OrderByPage(query, nameof(IdentityUser.Id))
            .Paginate(query, out int count)
            .ToArrayAsync(cancellationToken);

        return Ok(PagedResult.Create(records.ToModels(), count, (string?)null));
    }
}
