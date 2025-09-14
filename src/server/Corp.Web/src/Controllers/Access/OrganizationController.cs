using Microsoft.AspNetCore.Authorization;

using Corp.Access;

namespace Corp.Controllers.Access;

[Route("api/[area]/[controller]")]
[Area(nameof(Access))]
[Tags(nameof(Access))]
[Authorize(Policy = AppPolicy.RequireUserRole)]
[ApiController]
public class OrganizationController(
    ILogger<OrganizationController> logger,
    OrganizationService organizationService) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly OrganizationService _organizationService = organizationService;

    /// <summary>
    /// Returns an organization.
    /// </summary>
    /// <param name="id">The ID of the organization.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An organization object.</returns>
    [HttpGet("{id:long}")]
    [TenantRead]
    [Produces(typeof(OrganizationModel))]
    [EndpointDescription("Returns an organization object.")]
    public async Task<ActionResult> Get(long id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        OrganizationModel? result = await _organizationService.ReadOrDefaultAsync(id, cancellationToken);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Returns an organization with related entities.
    /// </summary>
    /// <param name="id">The ID of the organization.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An organization details object.</returns>
    [HttpGet("{id:long}/detail")]
    [TenantRead]
    [Produces(typeof(OrganizationDetailModel))]
    [EndpointDescription("Returns an organization details object with related entities.")]
    public async Task<ActionResult> GetDetail(long id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        OrganizationDetailModel? result = await _organizationService.ReadDetailOrDefaultAsync(id, cancellationToken);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Returns a list of organizations.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of organizations.</returns>
    [HttpGet]
    [TenantRead]
    [Produces("application/json", Type = typeof(OrganizationModel[]))]
    [EndpointDescription("Returns a list of organizations.")]
    public async Task<ActionResult> List(CancellationToken cancellationToken)
    {
        OrganizationModel[] results = await _organizationService.ListAsync(cancellationToken);
        return Ok(results);
    }

    /// <summary>
    /// Creates an organization.
    /// </summary>
    /// <param name="model">The organization object.</param>
    /// <returns>The organization object.</returns>
    [HttpPost]
    [TenantWrite]
    [Produces(typeof(OrganizationModel))]
    [EndpointDescription("Creates an organization.")]
    public async Task<ActionResult> Post(OrganizationModel model)
    {
        if (model is null)
            return BadRequest();

        OrganizationModel result = await _organizationService.CreateAsync(model);
        return Ok(result);
    }

    /// <summary>
    /// Updates an organization.
    /// </summary>
    /// <param name="model">The organization object.</param>
    /// <returns>The organization object.</returns>
    [HttpPut]
    [TenantWrite]
    [Produces(typeof(OrganizationModel))]
    [EndpointDescription("Updates an organization.")]
    public async Task<ActionResult> Put(OrganizationModel model)
    {
        if (model is null)
            return BadRequest();

        OrganizationModel? result = await _organizationService.UpdateAsync(model);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Deletes an organization.
    /// </summary>
    /// <param name="id">The ID of the organization.</param>
    [HttpDelete("{id:long}")]
    [TenantWrite]
    [Authorize(Policy = AppPolicy.RequireResellerRole)]
    [EndpointDescription("Deletes an organization.")]
    public async Task<ActionResult> Delete(long id)
    {
        if (id <= 0)
            return BadRequest();

        bool succeeded = await _organizationService.DeleteAsync(id);
        if (!succeeded)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Finds organizations by name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of matching organizations.</returns>
    [HttpGet("search")]
    [TenantRead]
    [Produces("application/json", Type = typeof(OrganizationModel[]))]
    [EndpointDescription("Finds organizations by name.")]
    public async Task<ActionResult> SearchByName([FromQuery] string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest();

        OrganizationModel[] results = await _organizationService.FindByNameAsync(name, cancellationToken);
        return Ok(results);
    }

    /// <summary>
    /// Finds an organization by code.
    /// </summary>
    /// <param name="code">The code to search for.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The organization or null if not found.</returns>
    [HttpGet("search/code")]
    [TenantRead]
    [Produces(typeof(OrganizationModel))]
    [EndpointDescription("Finds an organization by code.")]
    public async Task<ActionResult> SearchByCode([FromQuery] string code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest();

        OrganizationModel? result = await _organizationService.FindByCodeAsync(code, cancellationToken);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Finds organizations by status.
    /// </summary>
    /// <param name="status">The status to filter by.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of organizations with the specified status.</returns>
    [HttpGet("search/status")]
    [TenantRead]
    [Produces("application/json", Type = typeof(OrganizationModel[]))]
    [EndpointDescription("Finds organizations by status.")]
    public async Task<ActionResult> SearchByStatus([FromQuery] string status, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(status))
            return BadRequest();

        OrganizationModel[] results = await _organizationService.FindByStatusAsync(status, cancellationToken);
        return Ok(results);
    }

    /// <summary>
    /// Returns root organizations (organizations without a parent).
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of root organizations.</returns>
    [HttpGet("root")]
    [TenantRead]
    [Produces("application/json", Type = typeof(OrganizationModel[]))]
    [EndpointDescription("Returns root organizations.")]
    public async Task<ActionResult> GetRoot(CancellationToken cancellationToken)
    {
        OrganizationModel[] results = await _organizationService.FindRootOrganizationsAsync(cancellationToken);
        return Ok(results);
    }

    /// <summary>
    /// Returns child organizations of a parent organization.
    /// </summary>
    /// <param name="id">The parent organization ID.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of child organizations.</returns>
    [HttpGet("{id:long}/children")]
    [TenantRead]
    [Produces("application/json", Type = typeof(OrganizationModel[]))]
    [EndpointDescription("Returns child organizations of a parent organization.")]
    public async Task<ActionResult> GetChildren(long id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        OrganizationModel[] results = await _organizationService.FindByParentAsync(id, cancellationToken);
        return Ok(results);
    }
}
