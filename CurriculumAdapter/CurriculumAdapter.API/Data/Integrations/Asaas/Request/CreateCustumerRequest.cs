using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CurriculumAdapter.API.Data.Integrations.Asaas.Request
{
    public class CreateCustumerRequest
    {
        public string Name { get; set; }  

        public string CpfCnpj { get; set; }  

        public string Email { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MobilePhone { get; set; }  // Fone celular

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Address { get; set; }  // Logradouro

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AddressNumber { get; set; }  // Número do endereço

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Complement { get; set; }  // Complemento do endereço

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Province { get; set; }  // Bairro

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PostalCode { get; set; }  // CEP

        public Guid ExternalReference { get; set; }  // Identificador externo

        public bool? NotificationDisabled { get; set; }  // true para desabilitar notificações

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MunicipalInscription { get; set; }  // Inscrição municipal

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StateInscription { get; set; }  // Inscrição estadual

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Observations { get; set; }  // Observações adicionais

        public CreateCustumerRequest(
            string name,
            string cpfCnpj,
            string email,
            string? mobilePhone,
            string? address,
            string? addressNumber,
            string? complement,
            string? province, 
            string? postalCode,
            Guid externalReference,
            bool? notificationDisabled,
            string? municipalInscription,
            string? stateInscription,
            string? observations
            )
        {
            Name = name;
            CpfCnpj = cpfCnpj;
            Email = email;
            MobilePhone = mobilePhone;
            Address = address;
            AddressNumber = addressNumber;
            Complement = complement;
            Province = province;
            PostalCode = postalCode;
            ExternalReference = externalReference;
            NotificationDisabled = notificationDisabled;
            MunicipalInscription = municipalInscription;
            StateInscription = stateInscription;
            Observations = observations;
        }

        //public bool? ForeignCustomer { get; set; }  // true se for pagador estrangeiro


    }

    
}
