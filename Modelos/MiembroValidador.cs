namespace TACOS.Modelos;
using FluentValidation;
using System.Text.RegularExpressions;

/// <summary>
/// Valida que todos los campos de un miembro sean correctos previo al registro en la
/// base de datos.
/// </summary>
public partial class MiembroValidador : AbstractValidator<Miembro>
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
        this.RuleFor(miembro => miembro.Persona!.Email)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexEmail())
                .WithMessage(formatoEmail);
        this.RuleFor(miembro => miembro.Contrasena)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexContrasena())
                .WithMessage(formatoContrasena);
        this.RuleFor(miembro => miembro.Persona!.Nombre)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexAlfanumerico())
                .WithMessage(alfanumerico);
        this.RuleFor(miembro => miembro.Persona!.ApellidoPaterno)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexAlfanumerico())
                .WithMessage(alfanumerico);
        this.RuleFor(miembro => miembro.Persona!.ApellidoMaterno)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexAlfanumerico()).WithMessage(alfanumerico);
        this.RuleFor(miembro => miembro.Persona!.Direccion)
            .NotEmpty().WithMessage(obligatorio);
        this.RuleFor(miembro => miembro.Persona!.Telefono)
            .NotEmpty().WithMessage(obligatorio)
            .Matches(this.regexTelefono())
                .WithMessage(formatoTelefono);
    }   
}
