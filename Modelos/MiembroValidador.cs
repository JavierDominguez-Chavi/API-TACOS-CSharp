namespace TACOS.Modelos;
using FluentValidation;
using System.Text.RegularExpressions;
using TACOS.Modelos.Interfaces;

public class MiembroValidador : AbstractValidator<IAsociado>
{
    public MiembroValidador()
    {
        string obligatorio = "Todos los campos son obligatorios";
        string alfanumerico = "Los nombres sólo pueden contener letras y números";
        string formatoContrasena = "La contraseña debe tener al menos 8 caracteres: " +
            "al menos una letra minúscula; al menos una mayúscula y al menos un número";
        string formatoEmail = "El email no tiene el formato correcto.";
        string formatoTelefono = "El telefono no tiene el formato correcto.";
        Regex regexAlfanumerico = new Regex("^[a-zA-ZáéíóúñÑ][a-zA-ZáéíóúñÑ0-9]*$");
        Regex regexContrasena = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");
        Regex regexEmail = new Regex(
            "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)"
           +"*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$"
        );
        Regex regexTelefono = new Regex(@"^(\d{3}[- ]?){2}\d{4}$");
        RuleFor(IAsociado => IAsociado.Persona.Email)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(regexEmail)
                .WithMessage(formatoEmail);
        RuleFor(IAsociado => IAsociado.Contrasena)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(regexContrasena)
                .WithMessage(formatoContrasena);
        RuleFor(IAsociado => IAsociado.Persona.Nombre)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(regexAlfanumerico)
                .WithMessage(alfanumerico);
        RuleFor(IAsociado => IAsociado.Persona.ApellidoPaterno)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(regexAlfanumerico)
                .WithMessage(alfanumerico);
        RuleFor(IAsociado => IAsociado.Persona.ApellidoMaterno)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(regexAlfanumerico).WithMessage(alfanumerico);
        RuleFor(IAsociado => IAsociado.Persona.Direccion)
            .NotEmpty().WithMessage(obligatorio);
        RuleFor(IAsociado => IAsociado.Persona.Telefono)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(regexTelefono)
                .WithMessage(formatoTelefono);
    }   
}
