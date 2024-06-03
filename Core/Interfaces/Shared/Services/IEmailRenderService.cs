namespace Core.Interfaces.Shared.Services
{
    public interface IEmailRenderService
    {
        string RenderLiquidTemplate<T>(string EmailFile, T model);
    }
}
