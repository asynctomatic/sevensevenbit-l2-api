namespace SevenSevenBit.Operator.Web.Controllers.Api.V1;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Infrastructure.Identity.Auth;
using SevenSevenBit.Operator.Web.Attributes.Routing;
using SevenSevenBit.Operator.Web.Models.Api;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Authorize]
[ApiRoute("users")]
[ApiVersion("1.0")]
public class UserController : ApiControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly IUsersService usersService;

    public UserController(
        ILogger<UserController> logger,
        IUsersService usersService)
    {
        this.logger = logger;
        this.usersService = usersService;
    }

    [HttpPost]
    [Authorize(Policies.WriteUsers)]
    [SwaggerOperation(
        Summary = "Register new User",
        Description = "This endpoint registers a user.",
        OperationId = "RegisterUser",
        Tags = new[] { "User" })]
    [SwaggerResponse(StatusCodes.Status201Created, "Returns the newly registered user.", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The user registration request was invalid.", typeof(ProblemDetails))]
    public async Task<ActionResult<UserDto>> Index(
        [FromBody, SwaggerRequestBody("The user registration request.", Required = true)] RegisterUserModel registerUserModel,
        CancellationToken cancellationToken)
    {
        var isStarkKeyUsed = await usersService.IsStarkKeyUsedAsync(
            registerUserModel.StarkKey,
            cancellationToken);

        if (isStarkKeyUsed)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "StarkKey Already In Use",
                Detail = $"StarkKey {registerUserModel.StarkKey} is already registered.",
                Status = StatusCodes.Status400BadRequest,
                Type = ((int)ErrorCodes.StarkKeyAlreadyInUse).ToString(),
                Instance = HttpContext.Request.Path,
            };

            logger.LogWarning("StarkKey {StarkKey} is already registered", registerUserModel.StarkKey);
            return BadRequest(problemDetails);
        }

        var newUser = await usersService.RegisterUserAsync(
            registerUserModel.StarkKey,
            cancellationToken);

        return ApiCreated($"users/{newUser.Id}", new UserDto(newUser));
    }

    [HttpGet]
    [Authorize(Policies.ReadUsers)]
    [SwaggerOperation(
        Summary = "Get All Users",
        Description = "This endpoint fetches all users.",
        OperationId = "GetAllUsers",
        Tags = new[] { "User" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns all users.", typeof(PaginatedResponseDto<UserDto>))]
    public async Task<ActionResult<PaginatedResponseDto<UserDto>>> Index(
        [FromQuery, SwaggerParameter("The pagination and filters for the user request.", Required = true)] GetUsersQueryModel queryModel,
        CancellationToken cancellationToken)
    {
        var users = await usersService.GetUsersAsync(
            new(queryModel.PageNumber, queryModel.PageSize),
            queryModel.CreationDate,
            queryModel.CreationDateFilterOption,
            queryModel.SortBy,
            cancellationToken);

        return Ok(users);
    }

    [HttpGet("{userId:guid:required}")]
    [Authorize(Policies.ReadUsers)]
    [SwaggerOperation(
        Summary = "Get User",
        Description = "This endpoint fetches a specific user by ID.",
        OperationId = "GetUser",
        Tags = new[] { "User" })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns an user.", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found.", typeof(ProblemDetails))]
    public async Task<ActionResult<UserDto>> Index(
        [FromRoute, SwaggerParameter("The user id.", Required = true)] Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await usersService.GetUserAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            ProblemDetails problemDetails = new()
            {
                Title = "User Not Found",
                Detail = $"User {userId} not found.",
                Status = StatusCodes.Status404NotFound,
                Type = ((int)ErrorCodes.UserIdNotFound).ToString(),
                Instance = HttpContext.Request.Path,
            };

            return NotFound(problemDetails);
        }

        return Ok(new UserDto(user));
    }
}