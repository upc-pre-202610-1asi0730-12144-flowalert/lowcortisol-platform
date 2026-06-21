using LowCortisol.Platform.API.DeviceControl.Application.Acl;
using LowCortisol.Platform.API.Workplace.Application.CommandServices;
using LowCortisol.Platform.API.Workplace.Application.QueryServices;
using LowCortisol.Platform.API.Workplace.Domain.Model.Queries;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1/sites")]
public sealed class SitesController : ControllerBase
{
    private readonly ISiteCommandService _siteCommandService;
    private readonly ISiteQueryService _siteQueryService;
    private readonly IDeviceControlContextFacade _deviceControlContextFacade;

    public SitesController(
        ISiteCommandService siteCommandService,
        ISiteQueryService siteQueryService,
        IDeviceControlContextFacade deviceControlContextFacade)
    {
        _siteCommandService = siteCommandService;
        _siteQueryService = siteQueryService;
        _deviceControlContextFacade = deviceControlContextFacade;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SiteResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSites(CancellationToken cancellationToken)
    {
        var sites = await _siteQueryService.Handle(new GetAllSitesQuery(), cancellationToken);
        var resources = sites.Select(SiteResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("{siteId:guid}")]
    [ProducesResponseType(typeof(SiteResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSiteById(Guid siteId, CancellationToken cancellationToken)
    {
        var site = await _siteQueryService.Handle(new GetSiteByIdQuery(siteId), cancellationToken);

        return site is null ? NotFound() : Ok(SiteResourceFromEntityAssembler.ToResourceFromEntity(site));
    }

    [HttpPost]
    [ProducesResponseType(typeof(SiteResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateSite(
        CreateSiteResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateSiteCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _siteCommandService.Handle(command, cancellationToken);

        return WorkplaceActionResultAssembler.ToActionResult(
            this,
            result,
            SiteResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpGet("{siteId:guid}/rooms")]
    [ProducesResponseType(typeof(IEnumerable<RoomResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoomsBySiteId(Guid siteId, CancellationToken cancellationToken)
    {
        var rooms = await _siteQueryService.Handle(new GetRoomsBySiteIdQuery(siteId), cancellationToken);
        var resources = rooms.Select(RoomResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpPost("{siteId:guid}/rooms")]
    [ProducesResponseType(typeof(RoomResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRoom(
        Guid siteId,
        CreateRoomResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateRoomCommandFromResourceAssembler.ToCommandFromResource(siteId, resource);
        var result = await _siteCommandService.Handle(command, cancellationToken);

        return WorkplaceActionResultAssembler.ToActionResult(
            this,
            result,
            RoomResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpGet("{siteId:guid}/physical-model")]
    [ProducesResponseType(typeof(SitePhysicalModelResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPhysicalModel(Guid siteId, CancellationToken cancellationToken)
    {
        var site = await _siteQueryService.Handle(new GetSitePhysicalModelByIdQuery(siteId), cancellationToken);
        if (site is null) return NotFound();

        var deviceGroupIds = site.Rooms.SelectMany(room => room.DeviceGroups).Select(group => group.Id).ToList();
        var inventories = await _deviceControlContextFacade.GetInventoryByDeviceGroupIdsAsync(
            deviceGroupIds,
            cancellationToken);

        return Ok(SitePhysicalModelResourceFromEntityAssembler.ToResourceFromEntity(site, inventories));
    }
}
