using Probne_kolokwium.DTOs;

namespace Probne_kolokwium.Repositories;

public interface IAnimalRepository
{
    Task<AnimalDTO> InformationAboutAnimal(int id);
    Task<bool> DoesOwnerExist(int id);
    Task<bool> IsProcedureExist(int id);
    Task<int> AddAnimal(AddAnimalDTO request);
    Task<int> AddProcedure(AddProcedureDTO request, int AnimalID);
}