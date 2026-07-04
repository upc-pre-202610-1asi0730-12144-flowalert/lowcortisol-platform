using LowCortisol.Platform.API.Shared.Interfaces.Rest.ProblemDetails;
using LowCortisol.Platform.API.Support.Application.CommandServices;
using LowCortisol.Platform.API.Support.Application.QueryServices;
using LowCortisol.Platform.API.Support.Domain.Model.Commands;
using LowCortisol.Platform.API.Support.Domain.Model.Queries;
using LowCortisol.Platform.API.Support.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Support.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Support.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1/support")]
[Produces("application/json")]
public sealed class SupportController : ControllerBase
{
    private readonly ISupportCommandService _commandService;
    private readonly ISupportQueryService _queryService;

    public SupportController(ISupportCommandService commandService, ISupportQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet("tickets")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SupportTicketResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTickets([FromQuery] string? userId, CancellationToken cancellationToken)
    {
        var tickets = await _queryService.Handle(new GetSupportTicketsQuery(userId), cancellationToken);
        return Ok(tickets.Select(SupportTicketResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("tickets")]
    [ProducesResponseType(typeof(SupportTicketResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTicket([FromBody] CreateSupportTicketResource resource, CancellationToken cancellationToken)
    {
        var result = await _commandService.Handle(
            new CreateSupportTicketCommand(resource.UserId, resource.SiteId, resource.Title, resource.Description, resource.Category),
            cancellationToken);

        return SupportActionResultAssembler.ToActionResult(
            this,
            result,
            SupportTicketResourceFromEntityAssembler.ToResourceFromEntity,
            ticket => CreatedAtAction(nameof(GetTickets), new { userId = ticket.UserId }, ticket));
    }

    [HttpPost("tickets/{ticketId:guid}/resolve")]
    [ProducesResponseType(typeof(SupportTicketResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResolveTicket(Guid ticketId, CancellationToken cancellationToken)
    {
        var result = await _commandService.Handle(new UpdateSupportTicketCommand(ticketId, "resolved"), cancellationToken);
        return SupportActionResultAssembler.ToActionResult(this, result, SupportTicketResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("tickets/{ticketId:guid}/close")]
    [ProducesResponseType(typeof(SupportTicketResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CloseTicket(Guid ticketId, CancellationToken cancellationToken)
    {
        var result = await _commandService.Handle(new UpdateSupportTicketCommand(ticketId, "closed"), cancellationToken);
        return SupportActionResultAssembler.ToActionResult(this, result, SupportTicketResourceFromEntityAssembler.ToResourceFromEntity);
    }

    [HttpPost("tickets/{ticketId:guid}/messages")]
    [ProducesResponseType(typeof(SupportMessageResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage(Guid ticketId, [FromBody] SendSupportMessageResource resource, CancellationToken cancellationToken)
    {
        var result = await _commandService.Handle(
            new SendSupportMessageCommand(ticketId, resource.SenderId, resource.SenderType, resource.Content),
            cancellationToken);

        return SupportActionResultAssembler.ToActionResult(
            this,
            result,
            SupportMessageResourceFromEntityAssembler.ToResourceFromEntity,
            message => CreatedAtAction(nameof(GetConversationByTicket), new { ticketId = message.TicketId }, message));
    }

    [HttpGet("agents")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SupportAgentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAgents(CancellationToken cancellationToken)
    {
        var agents = await _queryService.Handle(new GetSupportAgentsQuery(), cancellationToken);
        return Ok(agents.Select(SupportAgentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("articles")]
    [ProducesResponseType(typeof(IReadOnlyCollection<KnowledgeArticleResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticles(CancellationToken cancellationToken)
    {
        var articles = await _queryService.Handle(new GetKnowledgeArticlesQuery(), cancellationToken);
        return Ok(articles.Select(KnowledgeArticleResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("conversations")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SupportConversationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConversations(CancellationToken cancellationToken)
    {
        var conversations = await _queryService.Handle(new GetSupportConversationsQuery(), cancellationToken);
        return Ok(conversations.Select(SupportConversationResourceFromViewAssembler.ToResourceFromView));
    }

    [HttpGet("conversations/{ticketId:guid}")]
    [ProducesResponseType(typeof(SupportConversationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConversationByTicket(Guid ticketId, CancellationToken cancellationToken)
    {
        var conversation = await _queryService.Handle(new GetSupportConversationByTicketQuery(ticketId), cancellationToken);
        return conversation is null
            ? this.NotFoundProblem("Conversation not found.", "The requested support conversation does not exist.")
            : Ok(SupportConversationResourceFromViewAssembler.ToResourceFromView(conversation));
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(SupportSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await _queryService.Handle(new GetSupportSummaryQuery(), cancellationToken);
        return Ok(SupportSummaryResourceFromResultAssembler.ToResourceFromResult(summary));
    }
}
