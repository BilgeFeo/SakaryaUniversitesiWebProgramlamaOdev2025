using WebProgramlamaOdev.DTOs;

namespace WebProgramlamaOdev.Services.ServiceInterfaces
{
    public interface ISignUpService
    {

        Task<bool> RegisterMemberAsync(MemberRegisterRequestDto registerRequestDto);

    }
}
