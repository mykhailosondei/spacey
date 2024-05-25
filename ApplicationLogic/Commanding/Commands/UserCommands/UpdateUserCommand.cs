using ApplicationCommon.DTOs.User;
using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.UserCommands;

public record UpdateUserCommand(Guid Id,UserUpdateDTO User) : IRequest, ICommand;