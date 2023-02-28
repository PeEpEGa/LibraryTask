using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

public class BaseController : ControllerBase
{
    protected async Task<IActionResult> SafeExecute(Func<Task<IActionResult>> action,
        CancellationToken cancellationToken)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}