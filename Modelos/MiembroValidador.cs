namespace TACOS.Modelos;
using FluentValidation;
using System.Text.RegularExpressions;
using TACOS.Modelos.Interfaces;

/// <summary>
/// Valida que todos los campos de un miembro o staff sean correctos previo al registro en la
/// base de datos.
/// </summary>
public class MiembroValidador : AbstractValidator<IAsociado>
{
    [GeneratedRegex(
        "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)"
        +"*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", 
        RegexOptions.CultureInvariant) ]
    private partial Regex regexEmail();

    [GeneratedRegex(
        "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$",
        RegexOptions.CultureInvariant)]
    private partial Regex regexContrasena();

    [GeneratedRegex(
        @"^(\d{3}[- ]?){2}\d{4}$",
        RegexOptions.CultureInvariant)]
    private partial Regex regexTelefono();

    [GeneratedRegex(
        "^[a-zA-ZáéíóúñÑ][a-zA-ZáéíóúñÑ0-9]*$",
        RegexOptions.CultureInvariant)]
    private partial Regex regexAlfanumerico();

    /// <summary>
    /// Especifica las reglas que debe seguir el Miembro para ser admitido a la
    /// base de datos.
    /// </summary>
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
            .Matches(this.regexEmail())
                .WithMessage(formatoEmail);
        RuleFor(IAsociado => IAsociado.Contrasena)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexContrasena())
                .WithMessage(formatoContrasena);
        RuleFor(IAsociado => IAsociado.Persona.Nombre)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexAlfanumerico())
                .WithMessage(alfanumerico);
        RuleFor(IAsociado => IAsociado.Persona.ApellidoPaterno)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexAlfanumerico())
                .WithMessage(alfanumerico);
        RuleFor(IAsociado => IAsociado.Persona.ApellidoMaterno)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(regexAlfanumerico).WithMessage(alfanumerico);
        RuleFor(IAsociado => IAsociado.Persona.Direccion)
            .NotEmpty().WithMessage(obligatorio);
        RuleFor(IAsociado => IAsociado.Persona.Telefono)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexTelefono())
                .WithMessage(formatoTelefono);
    }   
}
