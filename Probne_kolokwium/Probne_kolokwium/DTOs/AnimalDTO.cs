namespace Probne_kolokwium.DTOs;

public class AnimalDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime AdmissionDate  { get; set; }
    public OwnerDTO Owner { get; set; }
    public List<ProcedureDTO>? procedures { get; set; }
}