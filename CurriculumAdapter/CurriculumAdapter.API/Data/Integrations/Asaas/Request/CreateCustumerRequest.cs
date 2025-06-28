using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CurriculumAdapter.API.Data.Integrations.Asaas.Request
{
    public class CreateCustumerRequest
    {
        public string name { get; set; }  

        public string cpfCnpj { get; set; }  

        public string email { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? mobilePhone { get; set; }  // Fone celular

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? address { get; set; }  // Logradouro

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? addressNumber { get; set; }  // Número do endereço

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? complement { get; set; }  // Complemento do endereço

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? province { get; set; }  // Bairro

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? postalCode { get; set; }  // CEP

        public string externalReference { get; set; }  // Identificador externo

        public bool? notificationDisabled { get; set; }  // true para desabilitar notificações

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? municipalInscription { get; set; }  // Inscrição municipal

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? stateInscription { get; set; }  // Inscrição estadual

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? observations { get; set; }  // Observações adicionais

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
            string externalReference,
            bool? notificationDisabled,
            string? municipalInscription,
            string? stateInscription,
            string? observations
            )
        {
            this.name = name;
            this.cpfCnpj = cpfCnpj;
            this.email = email;
            this.mobilePhone = mobilePhone;
            this.address = address;
            this.addressNumber = addressNumber;
            this.complement = complement;
            this.province = province;
            this.postalCode = postalCode;
            this.externalReference = externalReference;
            this.notificationDisabled = notificationDisabled;
            this.municipalInscription = municipalInscription;
            this.stateInscription = stateInscription;
            this.observations = observations;
        }

        //public bool? ForeignCustomer { get; set; }  // true se for pagador estrangeiro


    }

    
}
