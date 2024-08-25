using CondigiBack.Models;
using CondigiBack.Modules.Contracts.DTOs;
using Microsoft.IdentityModel.Tokens;
using static CondigiBack.Modules.Contracts.DTOs.ContractAIDTO.CreateReceiverCompany;

namespace CondigiBack.Libs.Services
{
    public class GeneratePrompt
    {
        public string CompanyToCompany(Company receiverCompany, Company senderCompany, ContractType contractType,
            ContractAIDTO.ContractAIGeneralDto contract)
        {
            var prompt = $@"
            Genera un contrato de tipo {contractType.Name} el cual se enfoca en {contractType.Description} 
            debes hacerlo de forma que sea de una empresa a otra empresa con la siguiente información:

            Empresa Remitente:
            - Nombre: {senderCompany.Name}
            - Dirección: {senderCompany.Address}
            - Email: {senderCompany.Email}
            - Teléfono: {senderCompany.Phone}
            - RUC: {senderCompany.RUC}

            Empresa Receptora:
            - Nombre: {receiverCompany.Name}
            - Dirección: {receiverCompany.Address}
            - Email: {receiverCompany.Email}
            - Teléfono: {receiverCompany.Phone}
            - RUC: {receiverCompany.RUC}

            En la redaccion del contrato se deben incluir los siguientes detalles:

            Detalles del Contrato:
            - Tipo de Contrato: {contractType.Name}
            - Detalle del tipo de contrato: {contractType.Description}
            - Fecha de Inicio: {contract.StartDate:yyyy-MM-dd}
            - Fecha de Fin: {contract.EndDate:yyyy-MM-dd}
            
            El numero exacto de clausulas que debes generar dentro del contrato es de {contract.NumClauses} clausulas.
            "
                ;

            if (contract.PaymentAmount.HasValue)
            {
                prompt +=
                    $"Incluye este monto de Pago: ${contract.PaymentAmount.Value} dentro del contrato el cual es el valor acordado\n";
            }

            if (contract.PaymentFrequency != null)
            {
                prompt +=
                    $"La Frecuencia de Pago con la cual se debe pagar el monto es de: {contract.PaymentFrequency} debes divir el monto total por la frecuencia para que puedas especificar cuanto debera pagar en cada cuota.\n";
            }

            prompt += $@"
            Adicionalmente debes incluir estos Detalles dentro de la redaccion los cuales son utiles para que sepas que debes escribir y controlar dentro de la redaccion del contrato:
            - Estos seran los detalles que debes contemplar dentro de la redaccion del contrato: {contract.ContractDetails}
            - Estos son los objetivos que se pretenden alcanzar o cumplir con este contrato: {contract.ContractObjects} , debes usarlos de forma intrinsica dentro de la redaccion
            - Y esto es lo que debes redactar en el contrato en cuanto a la confidencialidad del contrato: {contract.ContractConfidentiality}
    
            Eso es todo, recuerda que debes generar todo en español, adicional genera tambien las firmas de las 2 partes interesadas, no respondas nada mas que no sea unicamente el contenido del contrato que debes generar.
            ";

            return prompt;
        }

        public string GenerateContent(Company? receiverCompany, Company? senderCompany, Person? receiverPerson,
            Person? senderPerson, ContractType contractType,
            ContractAIDTO.ContractAIGeneralDto contract)
        {
            var prompt = $@"
            Genera un contrato de tipo {contractType.Name}, el cual se enfoca en {contractType.Description}.
            Debes hacerlo de forma que el que envia es {contract.SenderType} y el que recibe es {contract.ReceiverType}, con la siguiente información:
            ";

            if (senderCompany != null)
            {
                prompt += $@"
                Empresa Remitente:
                - Nombre: {senderCompany.Name}
                - Dirección: {senderCompany.Address}
                - Email: {senderCompany.Email}
                - Teléfono: {senderCompany.Phone}
                - RUC: {senderCompany.RUC}
                ";
            }
            else if (senderPerson != null)
            {
                prompt += $@"
                Persona Remitente:
                - Nombre: {senderPerson.FirstName} {senderPerson.LastName}
                - Dirección: {senderPerson.Address}
                - Teléfono: {senderPerson.Phone}
                - Identificación: {senderPerson.Identification}
                ";
            }

            if (receiverCompany != null)
            {
                prompt += $@"
                Empresa Receptora:
                - Nombre: {receiverCompany.Name}
                - Dirección: {receiverCompany.Address}
                - Email: {receiverCompany.Email}
                - Teléfono: {receiverCompany.Phone}
                - RUC: {receiverCompany.RUC}
                ";
            }
            else if (receiverPerson != null)
            {
                prompt += $@"
                Persona Receptora:
                - Nombre: {receiverPerson.FirstName} {receiverPerson.LastName}
                - Dirección: {receiverPerson.Address}
                - Teléfono: {receiverPerson.Phone}
                - Identificación: {receiverPerson.Identification}
                ";
            }

            prompt += $@" 
                En la redacción del contrato se deben incluir los siguientes detalles:

                Detalles del Contrato:
            - Tipo de Contrato: {contractType.Name}
            - Detalle del tipo de contrato: {contractType.Description}
            - Fecha de Inicio: {contract.StartDate:yyyy-MM-dd}
            - Fecha de Fin: {contract.EndDate:yyyy-MM-dd}
            
            El número exacto de clausulas que debes generar dentro del contrato es de {contract.NumClauses} clausulas.
            ";

            if (contract.PaymentAmount.HasValue)
            {
                prompt += $@"
                    Incluye este monto de pago: ${contract.PaymentAmount.Value} dentro del contrato el cual es el valor acordado";
            }

            if (contract.PaymentFrequency != null)
            {
                prompt += $@"
                    La frecuencia de Pago con la cual se debe pagar el monto es de: {contract.PaymentFrequency} debes divir el monto total por la frecuencia para que puedas especificar cuanto debera pagar en cada cuota.";
            }

            prompt += $@"
            Adicionalmente debes incluir estos detalles dentro de la redacción los cuales son útiles para que sepas que debes escribir y controlar dentro de la redaccion del contrato:
            - Estos serán los detalles que debes contemplar dentro de la redacción del contrato: {contract.ContractDetails} ";
            if (!string.IsNullOrEmpty(contract.ContractObjects)){
                prompt += $@"
                    - Estos son los objetivos que se pretenden alcanzar o cumplir con este contrato: {contract.ContractObjects}. 
                    Debes usarlos de forma intrinsica dentro de la redaccion";
            }

            if (!string.IsNullOrEmpty(contract.ContractConfidentiality))
            {
                prompt += $@"
                    - Y esto es lo que debes redactar en el contrato en cuanto a la confidencialidad del contrato: {contract.ContractConfidentiality}";
            }

            prompt += @"
                Eso es todo, recuerda que debes generar todo en español, adicional genera el espacio para las firmas de las 2 partes interesadas. No respondas nada mas que no sea unicamente el contenido del contrato que debes generar.
            ";

            return prompt;
        }
    }
}