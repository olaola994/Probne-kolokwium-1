namespace Probne_kolokwium.DTOs;

public class AddAnimalDTO
{
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime AdmissionDate  { get; set; }
    public int Owner_ID { get; set; }
    public List<AddProcedureDTO>? procedures { get; set; }
}