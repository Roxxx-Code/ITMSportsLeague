using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;


namespace SportsLeague.API.Controllers;


[ApiController]

[Route("api/[controller]")]

public class MatchController : ControllerBase

{

    private readonly IMatchService _matchService;

    private readonly IMapper _mapper;
    private readonly IMatchLineupService _lineupService;


    public MatchController(IMatchService matchService, IMatchLineupService lineupService, IMapper mapper)
    {
        _matchService = matchService;
        _lineupService = lineupService;
        _mapper = mapper;
    }


    [HttpGet("tournament/{tournamentId}")]

    public async Task<ActionResult<IEnumerable<MatchResponseDTO>>> GetByTournament(

    int tournamentId)

    {

        try

        {

            var matches = await _matchService.GetAllByTournamentAsync(tournamentId);

            return Ok(_mapper.Map<IEnumerable<MatchResponseDTO>>(matches));

        }

        catch (KeyNotFoundException ex)

        {

            return NotFound(new { message = ex.Message });

        }

    }

    // Lineup endpoints
    [HttpPost("{id}/lineup")]
    public async Task<ActionResult<MatchLineupDto>> AddToLineup(int id, CreateMatchLineupDto dto)
    {
        try
        {
            var entity = _mapper.Map<MatchLineup>(dto);
            var created = await _lineupService.RegisterLineupAsync(id, entity);
            var resultDto = _mapper.Map<MatchLineupDto>(created);
            return CreatedAtAction(nameof(GetLineup), new { id = id }, resultDto);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpGet("{id}/lineup")]
    public async Task<ActionResult<IEnumerable<MatchLineupDto>>> GetLineup(int id)
    {
        try
        {
            var list = await _lineupService.GetLineupsByMatchAsync(id);
            return Ok(_mapper.Map<IEnumerable<MatchLineupDto>>(list));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("{id}/lineup/team/{teamId}")]
    public async Task<ActionResult<IEnumerable<MatchLineupDto>>> GetLineupByTeam(int id, int teamId)
    {
        try
        {
            var list = await _lineupService.GetLineupsByMatchAsync(id);
            var filtered = list.Where(ml => ml.Player.TeamId == teamId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupDto>>(filtered));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpDelete("{id}/lineup/{lineupId}")]
    public async Task<ActionResult> DeleteLineup(int id, int lineupId)
    {
        try
        {
            await _lineupService.DeleteLineupAsync(lineupId);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }


    [HttpGet("{id}")]

    public async Task<ActionResult<MatchResponseDTO>> GetById(int id)

    {

        var match = await _matchService.GetByIdAsync(id);

        if (match == null)

            return NotFound(new { message = $"Partido con ID {id} no encontrado" });

        return Ok(_mapper.Map<MatchResponseDTO>(match));

    }


    [HttpPost]

    public async Task<ActionResult<MatchResponseDTO>> Create(MatchRequestDTO dto)

    {

        try

        {

            var match = _mapper.Map<Match>(dto);

            var created = await _matchService.CreateAsync(match);

            var matchWithDetails = await _matchService.GetByIdAsync(created.Id);

            var responseDto = _mapper.Map<MatchResponseDTO>(matchWithDetails);

            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);

        }

        catch (KeyNotFoundException ex)

        {

            return NotFound(new { message = ex.Message });

        }

        catch (InvalidOperationException ex)

        {

            return Conflict(new { message = ex.Message });

        }

    }


    [HttpPut("{id}")]

    public async Task<ActionResult> Update(int id, MatchRequestDTO dto)

    {

        try

        {

            var match = _mapper.Map<Match>(dto);

            await _matchService.UpdateAsync(id, match);

            return NoContent();

        }

        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }

        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }

    }


    [HttpDelete("{id}")]

    public async Task<ActionResult> Delete(int id)

    {

        try

        {

            await _matchService.DeleteAsync(id);

            return NoContent();

        }

        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }

        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }

    }


    [HttpPatch("{id}/status")]

    public async Task<ActionResult> UpdateStatus(int id, UpdateMatchStatusDTO dto)

    {

        try

        {

            await _matchService.UpdateStatusAsync(id, dto.Status);

            return NoContent();

        }

        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }

        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }

    }

}