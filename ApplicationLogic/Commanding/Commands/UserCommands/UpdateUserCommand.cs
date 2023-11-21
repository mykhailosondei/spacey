using ApplicationCommon.DTOs.User;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.UserCommands;

public record UpdateUserCommand(Guid Id,UserUpdateDTO User) : IRequest, ICommand;