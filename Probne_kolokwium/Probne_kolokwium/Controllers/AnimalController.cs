using Microsoft.AspNetCore.Mvc;
using Probne_kolokwium.DTOs;
using Probne_kolokwium.Repositories;

namespace Probne_kolokwium.Controllers;
[ApiController]
[Route("/api/animal")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    public AnimalController(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    [HttpGet]
    public async Task<IActionResult> getInformationAboutAnimal(int id)
    {
        AnimalDTO animal = await _animalRepository.InformationAboutAnimal(id);
        if (animal != null) return Ok(animal);
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAnimalAndProcedures([FromBody] AddAnimalDTO request)
    {
        bool doesOwnerExist = await _animalRepository.DoesOwnerExist(request.Owner_ID);
        if (!doesOwnerExist)
        {
            return BadRequest("Owner doesn't exist");
        }

        if (request.procedures != null)
        {
            foreach (var procedureDto in request.procedures)
            {
                bool doesProcedureExist = await _animalRepository.IsProcedureExist(procedureDto.ProcedureId);
                if (!doesProcedureExist)
                {
                    return BadRequest($"Procedure with ID {procedureDto.ProcedureId} doesn't exist");
                }
            }
        }
    
        // If all procedures exist, continue with adding the animal and its procedures
        int animalId = await _animalRepository.AddAnimal(request);
        foreach (var procedureDto in request.procedures)
        {
            await _animalRepository.AddProcedure(procedureDto, animalId);
        }
        return Ok();
    }
    
    
}