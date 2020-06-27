using FluentValidation;
using GerenciadorDeCervejas.Dominio.Cervejas.DTOs;

namespace GerenciadorDeCervejas.Dominio.Cervejas.Validators
{
    public class CervejaValidator : AbstractValidator<CervejaDTO>
    {
        public CervejaValidator()
        {
            RuleFor(r => r.Nome)
                .Cascade(CascadeMode.Continue)
                .NotNull().NotEmpty().WithMessage("O campo \"Nome\" é obrigatório.")
                .MaximumLength(50).WithMessage("O campo \"Nome\" não pode ter mais que 50 caracteres.");
            RuleFor(r => r.Descricao)
                .Cascade(CascadeMode.Continue)
                .NotNull().NotEmpty().WithMessage("O campo \"Descrição\" é obrigatório.")
                .MaximumLength(1000).WithMessage("O campo \"Descrição\" não pode ter mais que 1000 caracteres.");
            RuleFor(r => r.Harmonizacao)
                .Cascade(CascadeMode.Continue)
                .NotNull().NotEmpty().WithMessage("O campo \"Harmonização\" é obrigatório.")
                .MaximumLength(500).WithMessage("O campo \"Harmonização\" não pode ter mais que 500 caracteres.");
            RuleFor(r => r.Cor)
                .Cascade(CascadeMode.Continue)
                .NotNull().NotEmpty().WithMessage("O campo \"Cor\" é obrigatório.")
                .MaximumLength(100).WithMessage("O campo \"Cor\" não pode ter mais que 100 caracteres.");
            RuleFor(r => r.TeorAlcoolico)
                .Cascade(CascadeMode.Continue)
                .NotNull().NotEmpty().WithMessage("O campo \"Teor alcoólico\" é obrigatório.")
                .MaximumLength(10).WithMessage("O campo \"Teor alcoólico\" não pode ter mais que 10 caracteres.");
            RuleFor(r => r.Temperatura)
                .Cascade(CascadeMode.Continue)
                .NotNull().NotEmpty().WithMessage("O campo \"Temperatura\" é obrigatório.")
                .MaximumLength(10).WithMessage("O campo \"Temperatura\" não pode ter mais que 10 caracteres.");
            RuleFor(r => r.Ingredientes)
                .Cascade(CascadeMode.Continue)
                .NotNull().NotEmpty().WithMessage("O campo \"Ingredientes\" é obrigatório.")
                .MaximumLength(1000).WithMessage("O campo \"Ingredientes\" não pode ter mais que 10 caracteres.");
        }
    }
}
