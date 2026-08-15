using Microsoft.AspNetCore.Mvc;
using BioServerAPI.Models;
using BioServerAPI.Services;

namespace BioServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BioServerController : ControllerBase
{
    private readonly BioServerService _service;
    private readonly ILogger<BioServerController> _logger;

    public BioServerController(BioServerService service, ILogger<BioServerController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // ============================================================
    // ENDPOINTS (Todos los métodos del WS original)
    // ============================================================

    [HttpPost("send-to-server")]
    public async Task<IActionResult> SendToServer([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.SendToServerAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en SendToServer");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("server-find")]
    public async Task<IActionResult> ServerFind([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.ServerFindAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en ServerFind");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("find-finger")]
    public async Task<IActionResult> FindFinger([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.FindFingerAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en FindFinger");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("find-palm")]
    public async Task<IActionResult> FindPalm([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.FindPalmAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en FindPalm");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("find-face")]
    public async Task<IActionResult> FindFace([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.FindFaceAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en FindFace");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("find-iris")]
    public async Task<IActionResult> FindIris([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.FindIrisAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en FindIris");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("find-voice")]
    public async Task<IActionResult> FindVoice([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.FindVoiceAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en FindVoice");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("server-save")]
    public async Task<IActionResult> ServerSave([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.ServerSaveAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en ServerSave");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("server-flush")]
    public async Task<IActionResult> ServerFlush([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.ServerFlushAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en ServerFlush");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("get-bio-key")]
    public async Task<IActionResult> GetBioKey([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.GetBioKeyAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetBioKey");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("get-app-key")]
    public async Task<IActionResult> GetAppKey([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.GetAppKeyAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetAppKey");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("get-data-bio-key")]
    public async Task<IActionResult> GetDataBioKey([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.GetDataBioKeyAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetDataBioKey");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("get-data-map-bio-key")]
    public async Task<IActionResult> GetDataMapBioKey([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.GetDataMapBioKeyAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetDataMapBioKey");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("get-data-server")]
    public async Task<IActionResult> GetDataServer([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.GetDataServerAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetDataServer");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("get-data-map-server")]
    public async Task<IActionResult> GetDataMapServer([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.GetDataMapServerAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetDataMapServer");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("server-delete")]
    public async Task<IActionResult> ServerDelete([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.ServerDeleteAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en ServerDelete");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("server-fuse")]
    public async Task<IActionResult> ServerFuse([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.ServerFuseAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en ServerFuse");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("special")]
    public async Task<IActionResult> Special([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.SpecialAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Special");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }

    [HttpPost("server-compare")]
    public async Task<IActionResult> ServerCompare([FromBody] BioServerRequest request)
    {
        try
        {
            var result = await _service.ServerCompareAsync(request.Id, request.Secret, request.Payload);
            return Ok(new BioServerResponse
            {
                Success = true,
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en ServerCompare");
            return StatusCode(500, new BioServerResponse
            {
                Success = false,
                Error = ex.Message,
                ErrorDetail = ex.ToString()
            });
        }
    }
}