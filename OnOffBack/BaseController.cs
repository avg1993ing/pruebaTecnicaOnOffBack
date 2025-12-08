using AutoMapper;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OnOffBack
{
    public class BaseController<TEntity, TEntityDto> : ControllerBase where TEntity : class
    {
        protected readonly IMapper _mapper;
        protected readonly IBaseService<TEntity> _service;
        protected readonly IValidator<TEntity> _validator;
        protected TEntity _model;
        protected TEntityDto _dtoModel;

        public BaseController(IMapper mapper,
                              IBaseService<TEntity> service,
                              IValidator<TEntity> validator)
        {
            _validator = validator;
            _mapper = mapper;
            _service = service;
        }

        protected async Task MapToEntityAsync(TEntityDto dtoEntity)
        {
            _model = _mapper.Map<TEntity>(dtoEntity);
        }

        protected void MapToDto()
        {
            _dtoModel = _mapper.Map<TEntityDto>(_model);
        }

        protected async Task<ValidationResult> ValidateAsyncDto()
        {
            // Map FluentValidation.Results.ValidationResult to System.ComponentModel.DataAnnotations.ValidationResult
            FluentValidation.Results.ValidationResult validationResult = await _validator.ValidateAsync(_model);

            if (validationResult.IsValid)
            {
                return ValidationResult.Success; // Return a valid System.ComponentModel.DataAnnotations.ValidationResult
            }

            // Convert FluentValidation errors to System.ComponentModel.DataAnnotations.ValidationResult format
            var validationErrors = validationResult.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                .ToArray();

            return new ValidationResult(string.Join(", ", validationErrors));
        }
    }
}
