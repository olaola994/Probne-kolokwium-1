using System.Data;
using System.Data.SqlClient;
using Probne_kolokwium.DTOs;

namespace Probne_kolokwium.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;
    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<AnimalDTO> InformationAboutAnimal(int id)
    {
        var query = "SELECT A.ID, A.Name, A.Type, A.AdmissionDate, " +
                    "A.Owner_ID, O.FirstName, O.LastName, P.Name, P.Description," +
                    " PA.Date FROM Animal A LEFT JOIN Owner O on A.Owner_ID = O.ID " +
                    "LEFT JOIN Procedure_Animal PA on A.ID = PA.Animal_ID " +
                    "LEFT JOIN [Procedure] P on PA.Procedure_ID = P.ID WHERE A.ID = @id";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        await connection.OpenAsync();

        var result = command.ExecuteReader();
        var Animal = new AnimalDTO();
        Animal.procedures = new List<ProcedureDTO>(); // Initialize procedures list
        while (result.Read())
        {
            Animal.Id = result.GetInt32(0);
            Animal.Name = result.GetString(1);
            Animal.Type = result.GetString(2);
            Animal.AdmissionDate = result.GetDateTime(3);
            Animal.Owner = new OwnerDTO()
            {
                Id = result.GetInt32(4),
                FirstName = result.GetString(5),
                LastName = result.GetString(6)
            };
            if (!result.IsDBNull(7))
            {
                Animal.procedures.Add(new ProcedureDTO
                {
                    Name = result.GetString(7),
                    Description = result.GetString(8),
                    Date = result.GetDateTime(9)
                });
            }
        }

        return Animal;
    }

    public async Task<int> AddAnimal(AddAnimalDTO request)
    {
        var query = @"INSERT INTO Animal (Name, Type, AdmissionDate, Owner_ID) 
                  VALUES (@Name, @Type, @AdmissionDate, @Owner_ID);
                    SELECT SCOPE_IDENTITY();";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@Name", request.Name);
        command.Parameters.AddWithValue("@Type", request.Type);
        command.Parameters.AddWithValue("@AdmissionDate", request.AdmissionDate);
        command.Parameters.AddWithValue("@Owner_ID", request.Owner_ID);
        await connection.OpenAsync();
        int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
        
        return insertedId;
    }
    

    public async Task<int> AddProcedure(AddProcedureDTO request, int AnimalID)
    {
        var query = @"INSERT INTO Procedure_Animal (Procedure_ID, Animal_ID, Date) 
                  VALUES (@Procedure_ID, @Animal_ID, @Date)";
            
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@Procedure_ID", request.ProcedureId);
        command.Parameters.AddWithValue("@Animal_ID", AnimalID);
        command.Parameters.AddWithValue("@Date", request.Date);
        await connection.OpenAsync();
        int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
        
        return insertedId;
    }

    public async Task<bool> IsProcedureExist(int id)
    {
        var query = @"SELECT COUNT(*) FROM [Procedure] where @id = ID";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        int orderCount = Convert.ToInt32(result);

        return orderCount > 0;
    }

    public async Task<bool> DoesOwnerExist(int OwnerId)
    {
        var query = @"SELECT COUNT(*) FROM Owner Where Owner.ID = @OwnerId";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@OwnerId", OwnerId);
        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        int orderCount = Convert.ToInt32(result);

        return orderCount > 0;
    }
}